using System;
using System.Collections.Generic;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Collision;
using FarseerPhysics.Collision.Shapes;
using FarseerPhysics.Common;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Contacts;
using FarseerPhysics.Dynamics.Joints;
using FarseerPhysics.Factories;
using StasisGame.Components;
using StasisGame.Systems;
using StasisCore;

namespace StasisGame
{
    public enum BudState
    {
        NODE,
        FLOWER,
        DORMANT,
        DEAD
    };

    public enum BudType
    {
        TERMINAL,
        LATERAL,
        NONE
    };

    public class Metamer
    {
        public const int MAX_FIXTURES_TO_TEST = 20;
        public float axis;
        public float currentAngle;
        public Tree tree;
        public Body body;
        public Vector2 position;
        public Vector2 oldPosition;
        public int ci;
        public int cj;
        private Vector2 force;
        public Vector2 externalForce;
        public BudType activeBud;
        public BudState terminalBudState;
        public BudState lateralBudState;
        public Metamer previousMetamer;
        public Metamer mainMetamer;
        public Metamer lateralMetamer;
        public float budQuality = 1f;
        private bool placeBudOnLeft;
        private List<MetamerMarker> associatedMarkers;
        private int localGridHalfWidth;
        private int localGridHalfHeight;
        private Vector2 optimalGrowthDirection;
        private AABB aabb;
        private float width;
        private int iterations;
        public bool isRoot;
        public bool isTail;
        public bool isBroken;
        public bool isTrunk;
        private bool doBreak;
        private float mass;
        public float inverseMass;
        public float inverseMassSq;
        private List<Metamer> constraintPoints;
        private List<Metamer> previousConstraintPoints;
        public int numFixturesToTest;
        public Fixture[] fixturesToTest;
        private Transform collisionXF;
        private Vector2[] collisionVertices;
        private Vector2[] collisionNormals;
        private FixedMouseJoint mouseJoint;
        private float textureWidth;
        public List<MetamerConstraint> constraints;
        public List<MetamerConstraint> relatedConstraints;
        private MetamerConstraint internodeConstraint;
        private bool constraintsCreated;
        private float currentTextureAngle;
        private Color textureColor = Color.White;
        private int timeToLive = 360;
        private VertexPositionColorTexture[] _vertices;
        private Texture2D leafTexture;
        private float _z;
        public int anchorCount;
        private PrimitiveRenderObject _renderObject;

        public VertexPositionColorTexture[] vertices { get { return _vertices; } }

        public Metamer(Tree tree, Metamer previousMetamer, BudType activeBud, BudState terminalBudState, BudState lateralBudState, float axis, bool placeBudOnLeft)
        {
            this.tree = tree;
            this.activeBud = activeBud;
            this.previousMetamer = previousMetamer;
            this.terminalBudState = terminalBudState;
            this.lateralBudState = lateralBudState;
            this.axis = axis;
            this.placeBudOnLeft = placeBudOnLeft;

            aabb = new AABB();
            isRoot = previousMetamer == null;
            constraintPoints = new List<Metamer>();
            previousConstraintPoints = new List<Metamer>();
            associatedMarkers = new List<MetamerMarker>();
            fixturesToTest = new Fixture[MAX_FIXTURES_TO_TEST];
            localGridHalfWidth = (int)Math.Floor(tree.perceptionRadius / TreeSystem.PLANT_CELL_SIZE) + 2;
            localGridHalfHeight = (int)Math.Floor(tree.perceptionRadius / TreeSystem.PLANT_CELL_SIZE) + 2;
            currentAngle = axis;
            currentTextureAngle = axis;
            collisionVertices = new Vector2[FarseerPhysics.Settings.MaxPolygonVertices];
            collisionNormals = new Vector2[FarseerPhysics.Settings.MaxPolygonVertices];
            constraints = new List<MetamerConstraint>();
            relatedConstraints = new List<MetamerConstraint>();
            _vertices = new VertexPositionColorTexture[2];

            // Mass
            mass = 1f;
            inverseMass = 1f / mass;

            // Calculate position
            position = (isRoot ? tree.position : previousMetamer.position + new Vector2((float)Math.Cos(axis), (float)Math.Sin(axis)) * tree.internodeLength);
            oldPosition = position;

            // Trunk constraints
            if (!isRoot)
            {
                if (isApex() && tree.iterations < 2)
                {
                    // Create anchor constraints
                    Vector2 relative = position - tree.rootPosition;
                    Vector2 anchorA = tree.rootPosition + tree.anchorNormal * 3f;
                    float distance = (anchorA - position).Length();
                    constraints.Add(new TrunkMetamerConstraint(this, anchorA, distance));

                    Vector2 anchorB = tree.rootPosition + tree.anchorNormal * -3f;
                    distance = (anchorB - position).Length();
                    constraints.Add(new TrunkMetamerConstraint(this, anchorB, distance));
                }

                // Create metamer-metamer links
                if (!previousMetamer.isBroken)
                    internodeConstraint = new DistanceMetamerConstraint(this, previousMetamer, tree.internodeLength * 0.8f, 0.5f);
            }

            // Put this metamer in a cell in the metamer grid
            ci = tree.treeSystem.getPlantGridX(position.X);
            cj = tree.treeSystem.getPlantGridY(position.Y);
            if (!tree.treeSystem.metamerGrid.ContainsKey(ci))
                tree.treeSystem.metamerGrid[ci] = new Dictionary<int, List<Metamer>>();
            if (!tree.treeSystem.metamerGrid[ci].ContainsKey(cj))
                tree.treeSystem.metamerGrid[ci][cj] = new List<Metamer>();
            tree.treeSystem.metamerGrid[ci][cj].Add(this);

            // Destroy markers in the occupied zone
            destroyMarkersInOccupiedZone();

            // Expand level boundary
            tree.treeSystem.levelSystem.expandBoundary(position);

            // Determine z-index
            _z = tree.layerDepth + StasisMathHelper.floatBetween(-0.02f, 0.01f, tree.random);
        }

        // isBranchingPoint
        public bool isBranchingPoint()
        {
            //return terminalBudState == BudState.NODE && lateralBudState == BudState.NODE;
            return mainMetamer != null && lateralMetamer != null;
        }

        // isApex
        public bool isApex()
        {
            return lateralMetamer == null && mainMetamer == null;
        }

        // createLimbBody
        public void createLimbBody()
        {
            if (body == null)
            {
                Fixture fixture;
                int entityId = tree.treeSystem.entityManager.createEntity(tree.levelUid);

                body = BodyFactory.CreateBody(tree.treeSystem.physicsSystem.getWorld(tree.levelUid), position, entityId);
                body.BodyType = BodyType.Dynamic;
                body.FixedRotation = true;
                body.LinearDamping = 0.5f;
                fixture = body.CreateFixture(new CircleShape(0.5f, 0.5f));
                fixture.IsSensor = true;

                mouseJoint = new FixedMouseJoint(body, body.Position);
                mouseJoint.MaxForce = 4000f;

                tree.treeSystem.physicsSystem.getWorld(tree.levelUid).AddJoint(mouseJoint);
                tree.treeSystem.entityManager.addComponent(tree.levelUid, entityId, new MetamerComponent(this));
                tree.treeSystem.entityManager.addComponent(tree.levelUid, entityId, new IgnoreRopeRaycastComponent());
            }
        }

        // destroyMarkersInOccupiedZone
        private void destroyMarkersInOccupiedZone()
        {
            int gridX = tree.treeSystem.getPlantGridX(position.X);
            int gridY = tree.treeSystem.getPlantGridY(position.Y);
            Dictionary<int, MarkerCell> gridRow;
            MarkerCell gridCell;

            for (int i = -localGridHalfWidth; i < localGridHalfWidth; i++)
            {
                for (int j = -localGridHalfHeight; j < localGridHalfHeight; j++)
                {
                    if (tree.treeSystem.markerGrid.TryGetValue(gridX + i, out gridRow) && gridRow.TryGetValue(gridY + j, out gridCell))
                    {
                        List<MetamerMarker> markersToDestroy = new List<MetamerMarker>();
                        for (int n = 0; n < gridCell.markers.Count; n++)
                        {
                            MetamerMarker marker = gridCell.markers[n];
                            if ((marker.point - position).Length() < tree.occupancyRadius)
                                markersToDestroy.Add(marker);
                        }
                        for (int n = 0; n < markersToDestroy.Count; n++)
                            gridCell.markers.Remove(markersToDestroy[n]);
                    }
                }
            }
        }

        // calculateLocalEnvironment
        public void calculateLocalEnvironment()
        {
            // Update iterations
            iterations++;

            if (isBroken)
                return;

            // Chain update
            if (mainMetamer != null)
                mainMetamer.calculateLocalEnvironment();
            if (lateralMetamer != null)
                lateralMetamer.calculateLocalEnvironment();

            if (activeBud != BudType.NONE)
            {
                // Initialize local environment
                int gridX = tree.treeSystem.getPlantGridX(position.X);
                int gridY = tree.treeSystem.getPlantGridY(position.Y);
                Dictionary<int, MarkerCell> gridRow;
                MarkerCell gridCell;
                for (int i = -localGridHalfWidth; i < localGridHalfWidth; i++)
                {
                    for (int j = -localGridHalfHeight; j < localGridHalfHeight; j++)
                    {
                        // Ensure grid cells exist
                        bool populateCell = false;
                        if (!tree.treeSystem.markerGrid.TryGetValue(gridX + i, out gridRow))
                            tree.treeSystem.markerGrid[gridX + i] = new Dictionary<int, MarkerCell>();
                        if (!tree.treeSystem.markerGrid[gridX + i].TryGetValue(gridY + j, out gridCell))
                        {
                            populateCell = true;
                            tree.treeSystem.markerGrid[gridX + i][gridY + j] = new MarkerCell(gridX + i, gridY + j);
                        }

                        if (populateCell)
                        {
                            // Populate local grid with marker points
                            for (int n = 0; n < TreeSystem.MARKERS_PER_CELL; n++)
                            {
                                MarkerCell cell = tree.treeSystem.markerGrid[gridX + i][gridY + j];
                                if (cell.markers.Count < TreeSystem.MARKERS_PER_CELL)     // enforce a maximum amount of markers per cell
                                {
                                    // Generate random point
                                    Vector2 random = new Vector2(gridX + i, gridY + j) * TreeSystem.PLANT_CELL_SIZE + new Vector2((float)tree.random.NextDouble(), (float)tree.random.NextDouble()) * TreeSystem.PLANT_CELL_SIZE;

                                    // Query the world before adding marker
                                    aabb.LowerBound = random;
                                    aabb.UpperBound = random;
                                    bool placeMarker = true;

                                    tree.treeSystem.physicsSystem.getWorld(tree.levelUid).QueryAABB((Fixture fixture) =>
                                    {
                                        int entityId = (int)fixture.Body.UserData;

                                        if (fixture.TestPoint(ref random, tree.internodeLength))
                                        {
                                            placeMarker = tree.treeSystem.entityManager.getComponent(tree.levelUid, entityId, ComponentType.BlockTreeLimbs) == null;
                                            return false;
                                        }
                                        else
                                        {
                                            return true;
                                        }
                                    },
                                        ref aabb);

                                    // Add marker
                                    if (placeMarker)
                                    {
                                        tree.treeSystem.markerGrid[gridX + i][gridY + j].markers.Add(new MetamerMarker(cell, random));
                                    }
                                }
                            }
                        }
                    }
                }

                // Find markers in the perception cone
                if (activeBud == BudType.TERMINAL)
                    findMarkersInPerceptionCone(activeBud, axis, tree.perceptionAngle);
                else if (activeBud == BudType.LATERAL)
                {
                    float newAxis = axis;
                    newAxis += placeBudOnLeft ? -tree.lateralAngle : tree.lateralAngle;
                    findMarkersInPerceptionCone(activeBud, newAxis, tree.perceptionAngle);
                }
            }

            // Calculate shadow values
            calculateShadowValues();
        }

        // calculateShadowValues
        private void calculateShadowValues()
        {
            Dictionary<int, MarkerCell> gridRow;
            MarkerCell gridCell;
            int gridX = tree.treeSystem.getPlantGridX(position.X);
            int gridY = tree.treeSystem.getPlantGridY(position.Y);

            for (int q = 0; q < Tree.SHADOW_DEPTH; q++)
            {
                for (int p = 0; p < q; p++)
                {
                    int affectedXLeft = gridX - p;
                    int affectedXRight = gridX + p;
                    int affectedY = gridY + q;
                    float deltaS = tree.penumbraA * (float)Math.Pow(tree.penumbraB, -q);

                    if (tree.treeSystem.markerGrid.TryGetValue(affectedXLeft, out gridRow) && gridRow.TryGetValue(affectedY, out gridCell))
                        gridCell.shadowValue += deltaS;
                    if (tree.treeSystem.markerGrid.TryGetValue(affectedXRight, out gridRow) && gridRow.TryGetValue(affectedY, out gridCell))
                        gridCell.shadowValue += deltaS;

                }
            }
        }

        // findMarkersInPerceptionCone
        private void findMarkersInPerceptionCone(BudType budType, float axis, float axisTolerance)
        {
            int gridX = tree.treeSystem.getPlantGridX(position.X);
            int gridY = tree.treeSystem.getPlantGridY(position.Y);
            Dictionary<int, MarkerCell> gridRow;
            MarkerCell gridCell;
            int gridRange = (int)Math.Floor(tree.perceptionRadius / TreeSystem.PLANT_CELL_SIZE) + 2;
            for (int i = -gridRange; i < gridRange; i++)
            {
                for (int j = -gridRange; j < gridRange; j++)
                {
                    if (tree.treeSystem.markerGrid.TryGetValue(gridX + i, out gridRow) && gridRow.TryGetValue(gridY + j, out gridCell))
                    {
                        for (int n = 0; n < gridCell.markers.Count; n++)
                        {
                            Vector2 relativePosition = (gridCell.markers[n].point - position);
                            float distanceToBud = relativePosition.Length();
                            Matrix rotationMatrix = Matrix.CreateRotationZ(-axis);
                            relativePosition = Vector2.Transform(relativePosition, rotationMatrix);
                            float markerAngle = (float)Math.Atan2(relativePosition.Y, relativePosition.X);
                            if (markerAngle <= axisTolerance && markerAngle >= -axisTolerance && distanceToBud <= tree.perceptionRadius)
                            {
                                // Add competition results to grid cell
                                gridCell.addMarkerCompetition(n, budType, distanceToBud, this);
                            }
                        }
                    }
                }
            }
        }

        // determineBudFate
        public void determineBudFate()
        {
            if (isBroken)
                return;

            // Chain up metamer calls
            if (mainMetamer != null)
                mainMetamer.determineBudFate();
            if (lateralMetamer != null)
                lateralMetamer.determineBudFate();

            // Find associated markers
            findAssociatedMarkers(activeBud);

            // Calculate bud quality from cell shadow value
            int gridX = tree.treeSystem.getPlantGridX(position.X);
            int gridY = tree.treeSystem.getPlantGridY(position.Y);
            budQuality = Math.Max(tree.fullExposure - tree.treeSystem.markerGrid[gridX][gridY].shadowValue, 0);
            //budQuality = Math.Max(tree.fullExposure - Main.markerGrid[gridX][gridY].shadowValue + tree.penumbraA, 0);

            if (activeBud != BudType.NONE)
            {
                // Set quality to zero if there is no room to grow
                if (associatedMarkers.Count == 0)
                    budQuality = 0;
                else
                {
                    MarkerCell cellWithLowestShadowValue = associatedMarkers[0].cell;
                    bool foundAShadow = false;
                    for (int i = 1; i < associatedMarkers.Count; i++)
                    {
                        MarkerCell cell = associatedMarkers[i].cell;
                        if (cell.shadowValue > 0)
                            foundAShadow = true;
                        if (cell.shadowValue < cellWithLowestShadowValue.shadowValue)
                            cellWithLowestShadowValue = cell;
                    }

                    // Calculate optimal growth direction
                    Vector2 normal = Vector2.Zero;
                    if (foundAShadow)
                    {
                        // Average marker directions in the cell with the lowest shadow value
                        for (int i = 0; i < cellWithLowestShadowValue.markers.Count; i++)
                        {
                            Vector2 relativePosition = (cellWithLowestShadowValue.markers[i].point - position);
                            relativePosition.Normalize();
                            normal += relativePosition / cellWithLowestShadowValue.markers.Count;
                        }
                        normal.Normalize();
                        optimalGrowthDirection = normal;
                    }
                    else
                    {
                        // Average all marker directions
                        for (int i = 0; i < associatedMarkers.Count; i++)
                        {
                            Vector2 relativePosition = (associatedMarkers[i].point - position);
                            relativePosition.Normalize();
                            normal += relativePosition / associatedMarkers.Count;
                        }
                        normal.Normalize();
                        optimalGrowthDirection = normal;
                    }
                }
            }
        }

        // findAssociatedMarkers
        private void findAssociatedMarkers(BudType budType)
        {
            int gridX = tree.treeSystem.getPlantGridX(position.X);
            int gridY = tree.treeSystem.getPlantGridY(position.Y);
            Dictionary<int, MarkerCell> gridRow;
            MarkerCell gridCell;
            int gridRange = (int)Math.Floor(tree.perceptionRadius / TreeSystem.PLANT_CELL_SIZE) + 2;
            associatedMarkers.Clear();
            for (int i = -gridRange; i < gridRange; i++)
            {
                for (int j = -gridRange; j < gridRange; j++)
                {
                    if (tree.treeSystem.markerGrid.TryGetValue(gridX + i, out gridRow) && gridRow.TryGetValue(gridY + j, out gridCell))
                    {
                        associatedMarkers.AddRange(gridCell.getAssociatedMarkers(budType, this));
                    }
                }
            }
        }

        // appendNewShoots
        public void appendNewShoots()
        {
            if (isBroken)
                return;

            // Chain metamer calls
            if (mainMetamer != null)
                mainMetamer.appendNewShoots();
            if (lateralMetamer != null)
                lateralMetamer.appendNewShoots();

            if (activeBud != BudType.NONE)
            {
                // Check bud quality
                if (budQuality > 0)
                {
                    // Create shoots...
                    Debug.Assert(activeBud != BudType.NONE);
                    if (activeBud == BudType.TERMINAL)
                    {
                        if (terminalBudState != BudState.DEAD)
                        {
                            mainMetamer = assembleShoot();
                            terminalBudState = mainMetamer == null ? BudState.DEAD : BudState.NODE;
                        }
                        activeBud = BudType.LATERAL;
                    }
                    else if (activeBud == BudType.LATERAL)
                    {
                        if (lateralBudState != BudState.DEAD)
                        {
                            lateralMetamer = assembleShoot();
                            lateralBudState = lateralMetamer == null ? BudState.DEAD : BudState.NODE;
                        }
                        activeBud = BudType.NONE;
                    }
                }
            }
        }

        // assembleShoot
        private Metamer assembleShoot()
        {
            // Initial metamer conditions for the head metamer
            BudType initialActiveBud = tree.maxShootLength > 1 ? BudType.LATERAL : BudType.TERMINAL;
            BudState initialTerminalBudState = tree.maxShootLength > 1 ? BudState.NODE : BudState.DORMANT;
            BudState initialLateralBudState = BudState.DORMANT;
            //float initialAxis = (float)Math.Atan2(optimalGrowthDirection.Y, optimalGrowthDirection.X);

            // Calculate the growth direction
            Vector2 newGrowthDirection = new Vector2((float)Math.Cos(axis), (float)Math.Sin(axis));
            newGrowthDirection += (optimalGrowthDirection * tree.optimalGrowthWeight);
            newGrowthDirection += (tree.tropism * tree.tropismWeight);
            newGrowthDirection.Normalize();
            float initialAxis = (float)Math.Atan2(newGrowthDirection.Y, newGrowthDirection.X);

            // Calculate the shoot length
            int shootLength = (int)(budQuality * tree.maxShootLength);
            bool recalculateDistance = false;
            Vector2 hitPoint = Vector2.Zero;
            tree.treeSystem.physicsSystem.getWorld(tree.levelUid).RayCast((Fixture fixture, Vector2 point, Vector2 normal, float fraction) =>
            {
                int entityId = (int)fixture.Body.UserData;
                if (tree.treeSystem.entityManager.getComponent(tree.levelUid, entityId, ComponentType.IgnoreTreeCollision) != null)
                    return fraction;

                recalculateDistance = true;
                hitPoint = point;
                return fraction;
            },
                position,
                position + newGrowthDirection * ((float)shootLength * tree.internodeLength));
            if (recalculateDistance)
                shootLength = (int)Math.Floor((hitPoint - position).Length());

            if (shootLength > 0)
            {
                Metamer head = new Metamer(tree, this, initialActiveBud, initialTerminalBudState, initialLateralBudState, initialAxis, !placeBudOnLeft);
                Metamer tail = head;
                for (int i = 1; i < shootLength; i++)
                {
                    bool onLastMetamer = i == shootLength - 1;
                    BudType subsequentActiveBud = onLastMetamer ? BudType.TERMINAL : BudType.LATERAL;
                    BudState subsequentTerminalBudState = onLastMetamer ? BudState.DORMANT : BudState.NODE;
                    BudState subsequentLateralBudState = BudState.DORMANT;

                    // Add metamer onto most recently created metamer
                    tail.mainMetamer = new Metamer(
                        tree,
                        tail,
                        subsequentActiveBud,
                        subsequentTerminalBudState,
                        subsequentLateralBudState,
                        initialAxis,
                        !tail.placeBudOnLeft);

                    // Store tail metamer
                    tail = tail.mainMetamer;
                }
                tail.isTail = true;

                // Create distance constraint between this metamer and the tail
                constraints.Add(new DistanceMetamerConstraint(this, tail, (position - tail.position).Length(), 1f));

                return head;
            }

            return null;
        }

        // calculateResources
        private float widthWeight(float x) { return 1 - (float)Math.Pow(x, 1d / 4d); }
        private float textureWeight(float x) { return 1 - (float)Math.Sqrt(1 - x); }
        public int calculateResources(int previousCount)
        {
            if (isBroken)
                return previousCount;

            int maxCount = previousCount;
            if (mainMetamer != null)
                maxCount = mainMetamer.calculateResources(previousCount + 1);
            if (lateralMetamer != null)
                maxCount = lateralMetamer.calculateResources(previousCount + 1);

            // Store largest count
            tree.longestPath = Math.Max(tree.longestPath, maxCount);

            float apexRatio = (float)previousCount / (float)maxCount;
            width = Math.Max(tree.maxBaseHalfWidth * 2 * widthWeight(apexRatio), tree.minBaseHalfWidth);
            textureWidth = (width / tree.maxBaseHalfWidth);

            // Calculate new mass
            mass = (float)maxCount / (float)previousCount;
            inverseMass = isRoot ? 0 : 1 / mass;
            inverseMassSq = inverseMass * inverseMass;

            // Find texture shadow value
            //float shadowValue = Math.Max(Math.Min(budQuality, 1f), 0.5f);
            //textureColor = new Color(new Vector3(shadowValue, shadowValue, shadowValue));
            float shadowValue = 1f;
            shadowValue = Math.Max(budQuality, 0.5f);
            textureColor = new Color(shadowValue, shadowValue, shadowValue);

            // Modify z index based on shadow value
            _z = tree.layerDepth + StasisMathHelper.floatBetween(-0.02f, 0.01f, tree.random) + shadowValue / 100f;

            // Assign texture
            if (previousCount > 3)
            {
                int variation = tree.random.Next(TreeSystem.NUM_LEAF_VARIATIONS);
                int group = tree.random.Next((int)((float)TreeSystem.NUM_LEAF_GROUPS * textureWeight(apexRatio)));
                leafTexture = tree.leafTextures[variation][group];
                _renderObject = (tree.treeSystem.systemManager.getSystem(SystemType.Render) as RenderSystem).createSpritePrimitiveObject(
                    leafTexture,
                    position,
                    textureColor,
                    new Vector2(leafTexture.Width, leafTexture.Height) / 2f,
                    0f,
                    1f,
                    _z);
            }
            else
            {
                leafTexture = null;
                _renderObject = null;
            }

            return maxCount;
        }

        // accumulateForces
        public void accumulateForces()
        {
            if (mainMetamer != null)
                mainMetamer.accumulateForces();
            if (lateralMetamer != null)
                lateralMetamer.accumulateForces();

            // Wind force
            //externalForce += GameEnvironment.wind * 0.0035f;

            // If there is a body, allow it to influence the metamer
            if (body != null && mouseJoint != null)
            {
                // Set joint target
                //mouseJoint.SetTarget(position);
                mouseJoint.WorldAnchorB = position;

                // Distance between body and metamer position
                Vector2 delta = (body.Position - position);

                // Calculate forces for different types of limbs
                if (isTrunk)
                    delta *= 2;

                externalForce += delta;
            }

            if (isBroken)
                force = tree.brokenGravity + externalForce;
            else
                force = tree.gravity + externalForce;
            externalForce = Vector2.Zero;
        }

        // integrate
        public void integrate()
        {
            if (mainMetamer != null)
                mainMetamer.integrate();
            if (lateralMetamer != null)
                lateralMetamer.integrate();

            Vector2 temporary = position;
            if (isBroken)
                position += 0.98f * (position - oldPosition) + force;
            else
                position += (position - oldPosition) + force;
            oldPosition = temporary;
        }

        // satisfyConstraints
        public void satisfyConstraints()
        {
            if (mainMetamer != null)
                mainMetamer.satisfyConstraints();
            if (lateralMetamer != null)
                lateralMetamer.satisfyConstraints();

            for (int i = 0; i < constraints.Count; i++)
                constraints[i].solve();

            if (internodeConstraint != null)
                internodeConstraint.solve();
        }

        // createConstraints
        public void createConstraints()
        {
            if (isBroken)
                return;

            if (mainMetamer != null)
                mainMetamer.createConstraints();
            if (lateralMetamer != null)
                lateralMetamer.createConstraints();

            if (!isRoot && !constraintsCreated)
            {
                // 1. Find grid cells within a cone pointing in the direction of the previous branching point
                // 2. Create distance constraints with branching points and tails
                // 2a. Possible optimization: don't create constraints with points that are nearly perpendicular

                Metamer previousBranchingPoint = previousMetamer;
                while (!(previousBranchingPoint.isBranchingPoint() || previousBranchingPoint.isRoot))
                    previousBranchingPoint = previousBranchingPoint.previousMetamer;

                Vector2 branchRelative = (previousBranchingPoint.position - position);
                //debugBranchNormal = branchRelative;
                //debugBranchNormal.Normalize();
                Vector2 perpendicularNormal = new Vector2(branchRelative.Y, -branchRelative.X);
                perpendicularNormal.Normalize();
                float coneRadius = tree.internodeLength * 8.1f;
                float coneHalfAngle = 0.5f;
                float minDifference = 0.3f;
                float coneAngle = (float)Math.Atan2(branchRelative.Y, branchRelative.X);

                int gridLowerBoundX = tree.treeSystem.getPlantGridX(position.X - coneRadius);
                int gridLowerBoundY = tree.treeSystem.getPlantGridY(position.Y - coneRadius);
                int gridUpperBoundX = tree.treeSystem.getPlantGridX(position.X + coneRadius);
                int gridUpperBoundY = tree.treeSystem.getPlantGridY(position.Y + coneRadius);
                Dictionary<int, List<Metamer>> gridX;
                List<Metamer> gridY;

                for (int i = gridLowerBoundX; i < gridUpperBoundX; i++)
                {
                    for (int j = gridLowerBoundY; j < gridUpperBoundY; j++)
                    {
                        if (tree.treeSystem.metamerGrid.TryGetValue(i, out gridX) && gridX.TryGetValue(j, out gridY))
                        {
                            for (int n = 0; n < gridY.Count; n++)
                            {
                                Metamer metamer = gridY[n];
                                if (metamer.tree != tree || metamer == this || metamer.isBroken || !(metamer.isTail || isBranchingPoint()))
                                    continue;

                                Vector2 relative = metamer.position - position;
                                float distance = relative.Length();

                                // Cone radius check
                                if (distance <= coneRadius)
                                {
                                    float metamerAngle = (float)Math.Atan2(relative.Y, relative.X);
                                    float difference = Math.Abs(MathHelper.WrapAngle(metamerAngle - coneAngle));

                                    // Cone angle check
                                    if (difference <= coneHalfAngle)
                                    {
                                        if (difference > minDifference)
                                        {
                                            // Lateral
                                            constraints.Add(new DistanceMetamerConstraint(this, metamer, distance, 0.01f));
                                        }
                                        else
                                        {
                                            // Perpendicular
                                            constraints.Add(new DistanceMetamerConstraint(this, metamer, distance, 0.1f));
                                        }
                                    }
                                }
                            }
                        }
                    }
                }

                constraintsCreated = true;
            }
        }

        // findConstraintPoints
        private void findConstraintPoints(Metamer exclude, Metamer start)
        {
            float minLateralDot = 0.4f;
            int maxLateralConstraints = 8;
            int maxPreviousConstraints = 4;

            // Search previous path
            if (start.previousMetamer != null && start.previousMetamer != exclude)
            {
                Metamer current = start;
                bool done = false;
                while (!done)
                {
                    // Stop searching and spawn a new constraint point search if current.previous is a branching point or tail
                    if (current.previousMetamer != null && (current.previousMetamer.isBranchingPoint()))
                    {
                        done = true;
                        previousConstraintPoints.Add(current.previousMetamer);
                        if (previousConstraintPoints.Count < maxPreviousConstraints)
                            findConstraintPoints(current, current.previousMetamer);
                    }

                    if (!done)
                    {
                        // Continue searching if the previous metamer exists
                        if (current.previousMetamer != null)
                            current = current.previousMetamer;
                        else
                            done = true;
                    }
                }
            }

            // Search main path
            if (start.mainMetamer != null && start.mainMetamer != exclude)
            {
                Metamer current = start;
                bool done = false;
                while (!done)
                {
                    // Stop searching and spawn a new constraint point search if current.main is a branching point or tail
                    if (current.mainMetamer != null && (current.mainMetamer.isBranchingPoint() || current.mainMetamer.isTail))
                    {
                        done = true;
                        Vector2 relative = position - current.mainMetamer.position;
                        relative.Normalize();
                        Vector2 normal = start.position - current.mainMetamer.position;
                        normal.Normalize();
                        if (Math.Abs(Vector2.Dot(relative, normal)) >= minLateralDot)
                            constraintPoints.Add(current.mainMetamer);
                        if (constraintPoints.Count < maxLateralConstraints)
                            findConstraintPoints(current, current.mainMetamer);
                    }

                    if (!done)
                    {
                        // Continue searching if the previous metamer exists
                        if (current.mainMetamer != null)
                            current = current.mainMetamer;
                        else
                            done = true;
                    }
                }
            }

            // Search lateral path
            if (start.lateralMetamer != null && start.lateralMetamer != exclude)
            {
                Metamer current = start;
                bool done = false;
                while (!done)
                {
                    // Stop searching and spawn a new constraint point search if current.main is a branching point or tail
                    if (current.lateralMetamer != null && (current.lateralMetamer.isBranchingPoint() || current.lateralMetamer.isTail))
                    {
                        done = true;
                        Vector2 relative = position - current.lateralMetamer.position;
                        relative.Normalize();
                        Vector2 normal = start.position - current.lateralMetamer.position;
                        normal.Normalize();
                        if (Math.Abs(Vector2.Dot(relative, normal)) >= minLateralDot)
                            constraintPoints.Add(current.lateralMetamer);
                        if (constraintPoints.Count < maxLateralConstraints)
                            findConstraintPoints(current, current.lateralMetamer);
                    }

                    if (!done)
                    {
                        // Continue searching if the previous metamer exists
                        if (current.lateralMetamer != null)
                            current = current.lateralMetamer;
                        else
                            done = true;
                    }
                }
            }
        }

        // resolveCollisions
        public void resolveCollisions()
        {
            if (mainMetamer != null)
                mainMetamer.resolveCollisions();
            if (lateralMetamer != null)
                lateralMetamer.resolveCollisions();

            if (!isRoot)
            {
                for (int i = 0; i < numFixturesToTest; i++)
                {
                    Fixture fixture = fixturesToTest[i];
                    if (fixture.Shape == null)     // fixtures can be destroyed before they're tested
                        continue;

                    Body fixtureBody = fixture.Body;
                    int entityId = (int)fixtureBody.UserData;
                    IComponent ignoreTreeCollisionComponent = tree.treeSystem.entityManager.getComponent(tree.levelUid, entityId, ComponentType.IgnoreTreeCollision);

                    if (ignoreTreeCollisionComponent == null)
                    {
                        if (fixture.TestPoint(ref position, 0.1f))
                        {
                            // Continue with normal collisions
                            Vector2 closestPoint = Vector2.Zero;
                            Vector2 normal = Vector2.Zero;
                            if (fixture.ShapeType == ShapeType.Polygon)
                            {
                                // Polygons
                                PolygonShape shape = fixture.Shape as PolygonShape;
                                fixtureBody.GetTransform(out collisionXF);

                                for (int v = 0; v < shape.Vertices.Count; v++)
                                {
                                    collisionVertices[v] = MathUtils.Multiply(ref collisionXF, shape.Vertices[v]);
                                    collisionNormals[v] = MathUtils.Multiply(ref collisionXF.R, shape.Vertices[v]);
                                }

                                // Find closest edge
                                float shortestDistance = 9999999f;
                                for (int v = 0; v < shape.Vertices.Count; v++)
                                {
                                    float distance = Vector2.Dot(collisionNormals[v], collisionVertices[v] - position);
                                    if (distance < shortestDistance)
                                    {
                                        shortestDistance = distance;
                                        closestPoint = collisionNormals[v] * (distance) + position;
                                        normal = collisionNormals[v];
                                    }
                                }

                                // Move metamer
                                if (fixtureBody.BodyType == BodyType.Static)
                                    position = closestPoint + 0.15f * normal;
                                else
                                {
                                    externalForce += normal * 0.05f;
                                    fixtureBody.ApplyForce(externalForce, position);
                                }
                            }
                            else if (fixture.ShapeType == ShapeType.Circle)
                            {
                                // Circles
                                CircleShape circleShape = fixture.Shape as CircleShape;
                                Vector2 center = circleShape.Position + fixtureBody.Position;
                                Vector2 difference = position - center;
                                normal = difference;
                                normal.Normalize();
                                closestPoint = center + difference * (circleShape.Radius / difference.Length());

                                // Move metamer
                                if (fixtureBody.BodyType == BodyType.Static)
                                    position = closestPoint + 0.15f * normal;
                                else
                                {
                                    externalForce += normal * 0.05f;
                                    fixtureBody.ApplyForce(externalForce, position);
                                }
                            }

                            // Handle fast moving bodies
                            if (fixtureBody.LinearVelocity.LengthSquared() > 100f)
                            {
                                Vector2 bodyVelocity = fixtureBody.GetLinearVelocityFromWorldPoint(oldPosition);
                                fixtureBody.LinearVelocity = fixtureBody.LinearVelocity * 0.999f;
                                fixtureBody.AngularVelocity = fixtureBody.AngularVelocity * 0.98f;
                                externalForce += bodyVelocity * 0.005f;
                            }
                        }
                    }
                }
            }

            // Reset number of fixtures to test
            numFixturesToTest = 0;
        }

        // breakConstraints
        private void breakConstraints(List<Metamer> metamersOnBranch)
        {
            // Destroy constraints for this metamer
            List<MetamerConstraint> constraintsToRemove = new List<MetamerConstraint>();
            for (int i = 0; i < constraints.Count; i++)
            {
                // If constraint isn't related to a metamer on this branch, get rid of it
                if (!constraints[i].isRelatedTo(metamersOnBranch))
                    constraintsToRemove.Add(constraints[i]);
            }
            for (int i = 0; i < constraintsToRemove.Count; i++)
                constraints.Remove(constraintsToRemove[i]);

            // Remove related constraints from other metamers
            List<MetamerConstraint> relatedConstraintsToRemove = new List<MetamerConstraint>();
            for (int i = 0; i < relatedConstraints.Count; i++)
            {
                switch (relatedConstraints[i].type)
                {
                    // If the related constraint's metamerA isn't on this branch, get rid of it
                    case ConstraintType.DISTANCE:
                        DistanceMetamerConstraint constraint = relatedConstraints[i] as DistanceMetamerConstraint;
                        if (!metamersOnBranch.Contains(constraint.metamerA))
                        {
                            constraint.metamerA.constraints.Remove(constraint);
                            relatedConstraintsToRemove.Add(relatedConstraints[i]);
                        }
                        break;
                }
            }
            for (int i = 0; i < relatedConstraintsToRemove.Count; i++)
                relatedConstraints.Remove(relatedConstraintsToRemove[i]);

            // Destroy trunk bodies
            if (isTrunk && body != null)
            {
                tree.treeSystem.physicsSystem.getWorld(tree.levelUid).RemoveBody(body);
                body = null;
                mouseJoint = null;
            }

            // Detach rope
            //for (int i = 0; i < attachedRopes.Count; i++)
            //    attachedRopes[i].detach();
        }

        // breakUpwards
        private void breakUpwards(List<Metamer> metamersOnBranch)
        {
            breakConstraints(metamersOnBranch);

            if (mainMetamer != null)
                mainMetamer.breakUpwards(metamersOnBranch);
            if (lateralMetamer != null)
                lateralMetamer.breakUpwards(metamersOnBranch);
        }

        // findMetamersOnBranch
        private void findMetamersOnBranch(List<Metamer> metamersOnBranch)
        {
            metamersOnBranch.Add(this);

            if (mainMetamer != null)
                mainMetamer.findMetamersOnBranch(metamersOnBranch);
            if (lateralMetamer != null)
                lateralMetamer.findMetamersOnBranch(metamersOnBranch);
        }

        // breakLimb
        public void breakLimb()
        {
            // Should never break a broken limb
            Debug.Assert(doBreak && !isBroken);

            doBreak = false;
            isBroken = true;

            // Destroy internode constraint
            internodeConstraint = null;

            // Build a list of metamers on this branch
            List<Metamer> metamersOnBranch = new List<Metamer>();
            findMetamersOnBranch(metamersOnBranch);

            // Break acropetal constraints
            breakUpwards(metamersOnBranch);
        }

        // constructRenderVertices
        private void constructRenderVertices()
        {
            Vector2 normal;
            float quarterTurn = StasisMathHelper.pi / 2;
            if (isRoot)
            {
                float perpAxis = axis + quarterTurn;
                normal = new Vector2((float)Math.Cos(axis), (float)Math.Sin(axis));
            }
            if (isBranchingPoint())
            {
                float mainPerpAngle = mainMetamer.isBroken ? currentAngle + quarterTurn : mainMetamer.currentAngle + quarterTurn;
                Vector2 mainNormal = new Vector2((float)Math.Cos(mainPerpAngle), (float)Math.Sin(mainPerpAngle));
                float lateralPerpAngle = lateralMetamer.isBroken ? currentAngle + quarterTurn : lateralMetamer.currentAngle + quarterTurn;
                Vector2 lateralNormal = new Vector2((float)Math.Cos(lateralPerpAngle), (float)Math.Sin(lateralPerpAngle));
                normal = (mainNormal + lateralNormal) / 2;
            }
            else
            {
                float perpAngle = currentAngle + quarterTurn;
                normal = new Vector2((float)Math.Cos(perpAngle), (float)Math.Sin(perpAngle));
            }

            _vertices[0].Position = new Vector3(position + -normal * width * 0.5f, 0);
            _vertices[1].Position = new Vector3(position + normal * width * 0.5f, 0);

            if (!(isRoot || isBroken))
            {
                // Construct triangles
                // 1----2   4       1 = vertices[0]
                // |   /   /|       2 = vertices[1]
                // |  /   / |       3 = previousMetamer.vertices[0]
                // | /   /  |       4 = vertices[1]
                // |/   /   |       5 = previousMetamer.vertices[1]
                // 3   6----5       6 = previousMetamer.vertices[0]
                int count = tree.numVertices;
                // 1
                tree.vertices[count].Position = _vertices[0].Position;
                tree.vertices[count].TextureCoordinate.X = 0f;
                tree.vertices[count].TextureCoordinate.Y = 0f;
                tree.vertices[count].Color = textureColor;
                count++;
                // 2
                tree.vertices[count].Position = _vertices[1].Position;
                tree.vertices[count].TextureCoordinate.X = textureWidth;
                tree.vertices[count].TextureCoordinate.Y = 0f;
                tree.vertices[count].Color = textureColor;
                count++;
                // 3
                tree.vertices[count].Position = previousMetamer.vertices[0].Position;
                tree.vertices[count].TextureCoordinate.X = 0f;
                tree.vertices[count].TextureCoordinate.Y = 1f;
                tree.vertices[count].Color = previousMetamer.textureColor;
                count++;
                // 4
                tree.vertices[count].Position = vertices[1].Position;
                tree.vertices[count].TextureCoordinate.X = textureWidth;
                tree.vertices[count].TextureCoordinate.Y = 0;
                tree.vertices[count].Color = textureColor;
                count++;
                // 5
                tree.vertices[count].Position = previousMetamer.vertices[1].Position;
                tree.vertices[count].TextureCoordinate.X = previousMetamer.textureWidth;
                tree.vertices[count].TextureCoordinate.Y = 1f;
                tree.vertices[count].Color = previousMetamer.textureColor;
                count++;
                // 6
                tree.vertices[count].Position = previousMetamer.vertices[0].Position;
                tree.vertices[count].TextureCoordinate.X = 0f;
                tree.vertices[count].TextureCoordinate.Y = 1f;
                tree.vertices[count].Color = previousMetamer.textureColor;
                tree.numVertices += 6;
                tree.primitiveCount += 2;
            }
        }

        // kill
        private void kill()
        {
            if (mainMetamer != null)
                mainMetamer.kill();
            if (lateralMetamer != null)
                lateralMetamer.kill();

            // Remove from metamer grid
            tree.treeSystem.metamerGrid[ci][cj].Remove(this);
            if (tree.treeSystem.metamerGrid[ci][cj].Count == 0)
                tree.treeSystem.metamerGrid[ci].Remove(cj);
            if (tree.treeSystem.metamerGrid[ci].Count == 0)
                tree.treeSystem.metamerGrid.Remove(ci);

            // Should have no bodies or mouse joints
            Debug.Assert(body == null);
            Debug.Assert(mouseJoint == null);

            // Clear connections
            mainMetamer = null;
            lateralMetamer = null;
            if (previousMetamer != null)
            {
                if (previousMetamer.mainMetamer == this)
                    previousMetamer.mainMetamer = null;
                else if (previousMetamer.lateralMetamer == this)
                    previousMetamer.lateralMetamer = null;
            }
            previousMetamer = null;

            // Clear lists
            constraints.Clear();
            relatedConstraints.Clear();
            constraintPoints.Clear();
            previousConstraintPoints.Clear();
            collisionNormals = null;
            collisionVertices = null;
        }

        // update
        public void update()
        {
            // Construct render vertices
            constructRenderVertices();

            // Logic before this point acts acropetally (towards apex)
            if (mainMetamer != null)
                mainMetamer.update();
            if (lateralMetamer != null)
                lateralMetamer.update();
            // Logic after this point acts basipetally (towards base)

            if (!isRoot)
            {
                // Update angle
                Vector2 relative = position - previousMetamer.position;
                currentAngle = (float)Math.Atan2(relative.Y, relative.X);

                // Update texture angle
                currentTextureAngle += (MathHelper.WrapAngle(currentAngle - currentTextureAngle)) / 4f;

                if (!isBroken)
                {
                    // Check limb distance
                    if (relative.LengthSquared() > 2f * tree.internodeLengthSq)
                        doBreak = true;
                }
            }

            // Break limbs
            if (doBreak)
                breakLimb();

            // Update cell
            int i = tree.treeSystem.getPlantGridX(position.X);
            int j = tree.treeSystem.getPlantGridY(position.Y);

            if (ci == i && cj == j)
                return;
            else
            {
                tree.treeSystem.metamerGrid[ci][cj].Remove(this);

                if (tree.treeSystem.metamerGrid[ci][cj].Count == 0)
                {
                    tree.treeSystem.metamerGrid[ci].Remove(cj);

                    if (tree.treeSystem.metamerGrid[ci].Count == 0)
                    {
                        tree.treeSystem.metamerGrid.Remove(ci);
                    }
                }

                if (!tree.treeSystem.metamerGrid.ContainsKey(i))
                    tree.treeSystem.metamerGrid[i] = new Dictionary<int, List<Metamer>>();
                if (!tree.treeSystem.metamerGrid[i].ContainsKey(j))
                    tree.treeSystem.metamerGrid[i][j] = new List<Metamer>(10);

                tree.treeSystem.metamerGrid[i][j].Add(this);
                ci = i;
                cj = j;
            }

            // Update tree aabb
            tree.expandAABB(position);

            // Update time to live
            if (isBroken)
                timeToLive--;
            if (timeToLive <= 0)
                kill();
        }

        // updateAABB -- called after tree.iterate()
        public void updateAABB()
        {
            if (mainMetamer != null)
                mainMetamer.updateAABB();
            if (lateralMetamer != null)
                lateralMetamer.updateAABB();

            tree.expandAABB(position);
        }
        
        // draw
        public void draw(RenderSystem renderSystem)
        {
            if (_renderObject != null)
            {
                _renderObject.worldMatrix = _renderObject.originMatrix * Matrix.CreateRotationZ(currentTextureAngle) * Matrix.CreateTranslation(new Vector3(position, 0));
                _renderObject.updateVertices();
                renderSystem.addRenderablePrimitive(_renderObject);
            }

            if (mainMetamer != null)
                mainMetamer.draw(renderSystem);
            if (lateralMetamer != null)
                lateralMetamer.draw(renderSystem);
        }
    }
}
