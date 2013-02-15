using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Box2D.XNA;
using StasisGame.Systems;
using StasisCore;

namespace StasisGame
{
    using Settings = StasisCore.Settings;

    public class Tree
    {
        public const int NUM_CONSTRAINT_ITERATIONS = 2;
        public const int SHADOW_DEPTH = 4;
        public const int MAX_VERTICES = 8000;
        public TreeSystem treeSystem;
        public Vector2 position;
        public float angle;
        public int seed;
        public float age;
        public float internodeLength;
        public float internodeLengthSq;
        public int maxShootLength;
        public float perceptionAngle;
        public float perceptionRadius;
        public float occupancyRadius;
        public float lateralAngle;
        public float fullExposure;
        public float penumbraA;
        public float penumbraB;
        public float optimalGrowthWeight;
        public float tropismWeight;
        public Vector2 tropism;
        public float maxBaseWidth;
        public float minBaseWidth = 0.04f;
        public Vector2 gravity = new Vector2(0, 0.005f);
        public Vector2 brokenGravity = new Vector2(0, 0.02f);
        //public Vector2 gravity = new Vector2(0, 0);
        public Vector2 anchorNormal;
        public Vector2 rootPosition;
        public int numVertices;
        public Random random;
        public Metamer rootMetamer;
        public int iterations;
        public AABB aabb;
        private Vector2 aabbMargin = new Vector2(64f, 64f) / Settings.BASE_SCALE;
        public bool active;
        public int longestPath;
        private Texture2D barkTexture;
        public Vector3 barkColor;

        // Debug
        public List<Vector2> pointsInTerminalBudPerceptionCone;
        public List<Vector2> pointsInLateralBudPerceptionCone;
        public static bool drawGrid = false;
        public static bool drawOccupancyZones = false;
        public static bool drawPerceptionPoints = false;
        public static bool drawMarkerPoints = true;
        public static bool drawInternodes = true;
        public static bool drawOptimalGrowthDirection = false;
        public static bool drawDistanceConstraints = false;
        public static bool drawAngularConstraints = false;
        public static bool drawTails = true;
        public static bool drawBranchingPoints = true;
        public static bool drawMetamers = true;
        public static bool drawTrunkBodies = true;

        // Constructor
        public Tree(
            TreeSystem treeSystem,
            Vector2 position,
            float angle,
            int seed,
            float age,
            float internodeLength,
            int maxShootLength,
            float maxBaseWidth,
            float perceptionAngle,
            float perceptionRadius,
            float occupancyRadius,
            float lateralAngle,
            float fullExposure,
            float penumbraA,
            float penumbraB,
            float optimalGrowthWeight,
            float tropismWeight,
            Vector2 tropism)
        {
            this.treeSystem = treeSystem;
            this.angle = angle;
            this.seed = seed;
            this.age = age;
            this.internodeLength = internodeLength;
            this.maxShootLength = maxShootLength;
            this.maxBaseWidth = maxBaseWidth;
            this.perceptionAngle = perceptionAngle;
            this.perceptionRadius = perceptionRadius;
            this.occupancyRadius = occupancyRadius;
            this.lateralAngle = lateralAngle;
            this.fullExposure = fullExposure;
            this.penumbraA = penumbraA;
            this.penumbraB = penumbraB;
            this.optimalGrowthWeight = optimalGrowthWeight;
            this.tropismWeight = tropismWeight;
            this.tropism = tropism;

            random = new Random(seed);
            internodeLengthSq = internodeLength * internodeLength;
            aabb = new AABB();
            aabb.lowerBound = position;
            aabb.upperBound = position;

            // Calculate root position
            float rootAngle = angle + (StasisMathHelper.pi);
            rootPosition = position + new Vector2((float)Math.Cos(rootAngle), (float)Math.Sin(rootAngle)) * 5f;

            // Calculate anchor normals
            float anchorAngle = angle - (StasisMathHelper.pi * 0.5f);
            anchorNormal = new Vector2((float)Math.Cos(anchorAngle), (float)Math.Sin(anchorAngle));

            // Initialize vertices
            //vertices = new CustomVertexFormat[MAX_VERTICES];

            // Fix tropism vector
            this.tropism -= position;

            // Debug -- perception points
            pointsInTerminalBudPerceptionCone = new List<Vector2>();
            pointsInLateralBudPerceptionCone = new List<Vector2>();

            // Create first metamer
            rootMetamer = new Metamer(this, null, BudType.TERMINAL, BudState.DORMANT, BudState.DEAD, angle);
            rootMetamer.isTail = true;
        }

        /*
        // load
        public static Tree load(BaseEnvironment environment, XmlNode node)
        {
            Tree actor = new Tree(
                environment,
                new Vector2(
                    Convert.ToSingle(node.Attributes["x"].Value),
                    Convert.ToSingle(node.Attributes["y"].Value)),
                Convert.ToSingle(node.Attributes["angle"].Value),
                (int)Convert.ToSingle(node.Attributes["seed"].Value),
                Convert.ToSingle(node.Attributes["age"].Value),
                Convert.ToSingle(node.Attributes["internode_length"].Value),
                (int)Convert.ToSingle(node.Attributes["max_shoot_length"].Value),
                Convert.ToSingle(node.Attributes["max_base_width"].Value),
                Convert.ToSingle(node.Attributes["perception_angle"].Value),
                Convert.ToSingle(node.Attributes["perception_radius"].Value),
                Convert.ToSingle(node.Attributes["occupancy_radius"].Value),
                Convert.ToSingle(node.Attributes["lateral_angle"].Value),
                Convert.ToSingle(node.Attributes["full_exposure"].Value),
                Convert.ToSingle(node.Attributes["penumbra_a"].Value),
                Convert.ToSingle(node.Attributes["penumbra_b"].Value),
                Convert.ToSingle(node.Attributes["optimal_growth_weight"].Value),
                Convert.ToSingle(node.Attributes["tropism_weight"].Value),
                new Vector2(
                    Convert.ToSingle(node.Attributes["tropism_x"].Value),
                    Convert.ToSingle(node.Attributes["tropism_y"].Value)));

            // Set ID
            actor.id = System.Convert.ToInt32(node.Attributes["id"].Value);

            // Set material
            actor.setMaterial(TreeMaterial.load(actor, node));

            return actor;
        }*/
        /*
        // setMaterial
        public override void setMaterial(Material material)
        {
            this.material = material;

            // Set bark texture and color
            barkTexture = (material as TreeMaterial).barkTexture;
            barkColor = new Vector3(
                (float)material.colors[0].R / 255f,
                (float)material.colors[0].G / 255f,
                (float)material.colors[0].B / 255f);
        }*/
        /*
        // addToRenderLayer
        public override void addToRenderLayer()
        {
            (environment as GameEnvironment).renderLayers[(int)RenderLayer.ACTORS].Add(this);
        }

        // removeFromRenderLayer
        public override void removeFromRenderLayer()
        {
            (environment as GameEnvironment).renderLayers[(int)RenderLayer.ACTORS].Remove(this);
        }
        */

        // expandAABB
        public void expandAABB(Vector2 point)
        {
            aabb.lowerBound = Vector2.Min(point - aabbMargin, aabb.lowerBound);
            aabb.upperBound = Vector2.Max(point + aabbMargin, aabb.upperBound);
        }

        // iterate
        public void iterate(int count = 1)
        {
            // Debug -- clear list of perception points to draw
            if (drawPerceptionPoints)
            {
                pointsInTerminalBudPerceptionCone.Clear();
                pointsInLateralBudPerceptionCone.Clear();
            }

            // Clear marker competition and shadow values
            foreach (Dictionary<int, MarkerCell> gridRow in treeSystem.markerGrid.Values)
            {
                foreach (MarkerCell gridCell in gridRow.Values)
                {
                    gridCell.clearMarkerCompetition();
                    gridCell.shadowValue = 0;
                }
            }

            for (int i = 0; i < count; i++)
            {
                // Calculate local environment of buds
                rootMetamer.calculateLocalEnvironment();

                // Determine bud fate
                rootMetamer.determineBudFate();

                // Append new shoots
                rootMetamer.appendNewShoots();

                // Update branch width
                rootMetamer.calculateResources(1);

                // Create constraints
                rootMetamer.createConstraints();

                iterations++;
            }

            // Force aabb update
            rootMetamer.updateAABB();
        }

        // step
        public void step()
        {
            rootMetamer.accumulateForces();
            rootMetamer.integrate();
            for (int n = 0; n < NUM_CONSTRAINT_ITERATIONS; n++)
            {
                // Satisfy constraints
                rootMetamer.satisfyConstraints();

                if (!rootMetamer.isBroken)
                {
                    // Pin root metamer
                    rootMetamer.position = position;
                    rootMetamer.oldPosition = position;
                }
            }
        }

        // update
        public void update(GameTime gameTime)
        {
            // Handle initial iterations
            if ((int)age > iterations)
            {
                // Iterate
                iterate();

                // Relax if on last iteration
                if ((int)age == iterations)
                {
                    for (int r = 0; r < 300; r++)
                        step();
                }

                return;
            }

            // Flag as active
            active = AABB.TestOverlap(ref treeSystem.treeAABB, ref aabb);

            if (active)
            {
                // Verlet integration
                if (iterations > 0)
                    step();

                // Prepare collisions
                //prepareCollisions();

                // Resolve collisions
                rootMetamer.resolveCollisions();

                // Reset vertices
                //numVertices = 0;
                //primitiveCount = 0;

                // Update metamers
                rootMetamer.update(gameTime);
            }
        }

        /*
        // draw
        public override void draw(GameTime gameTime)
        {
            if (active)
            {
                if (GameEnvironment.debug)
                {
                    if (Tree.drawGrid || Tree.drawMarkerPoints)
                    {
                        Vector2 screenWorldOffset = Main.getWorldOffset();
                        // Draw grid
                        foreach (KeyValuePair<int, Dictionary<int, Cell>> gridRow in GameEnvironment.markerGrid)
                        {
                            foreach (KeyValuePair<int, Cell> gridCell in gridRow.Value)
                            {
                                if (Tree.drawGrid)
                                {
                                    int gridX = gridRow.Key;
                                    int gridY = gridCell.Key;
                                    float tintValue = 1f - gridCell.Value.shadowValue;
                                    float colorValue = 0.3f * tintValue;
                                    Color gridColor = new Color(colorValue, colorValue, colorValue);
                                    spriteBatch.Draw(
                                            renderer.boxTexture,
                                            (new Vector2(gridX, gridY) * GameEnvironment.PLANT_CELL_SIZE + screenWorldOffset) * BaseEnvironment.scale,
                                            new Rectangle(0, 0, (int)(GameEnvironment.PLANT_CELL_SIZE * BaseEnvironment.scale), (int)(GameEnvironment.PLANT_CELL_SIZE * BaseEnvironment.scale)),
                                            gridColor);
                                }

                                if (Tree.drawMarkerPoints)
                                {
                                    for (int i = 0; i < gridCell.Value.markers.Count; i++)
                                    {
                                        renderer.drawPoint(gridCell.Value.markers[i].point, Color.Red);
                                    }
                                }
                            }
                        }
                    }

                    if (Tree.drawPerceptionPoints)
                    {
                        for (int i = 0; i < pointsInTerminalBudPerceptionCone.Count; i++)
                            renderer.drawPoint(pointsInTerminalBudPerceptionCone[i], Color.Yellow);
                        for (int i = 0; i < pointsInLateralBudPerceptionCone.Count; i++)
                            renderer.drawPoint(pointsInLateralBudPerceptionCone[i], Color.Orange);
                    }

                    // Draw metamer occupancy zones
                    if (Tree.drawOccupancyZones)
                        rootMetamer.drawOccupancyZone();

                    // Draw trunk bodies
                    if (Tree.drawTrunkBodies)
                        rootMetamer.drawTrunkBodies();

                    // Draw internodes
                    if (Tree.drawInternodes)
                        rootMetamer.drawInternode();

                    // Draw branch constraints
                    rootMetamer.drawConstraints();

                    // Draw optimal growth direction
                    if (Tree.drawOptimalGrowthDirection)
                        rootMetamer.drawOptimalGrowthDirection();

                    // Draw metamers
                    if (Tree.drawMetamers)
                        rootMetamer.drawMetamers();
                }
                else
                {
                    material.draw(gameTime);
                     
                    if (primitiveCount > 0)
                    {
                        // Draw limbs
                        renderer.device.Textures[0] = barkTexture;
                        renderer.effects["primitives"].Parameters["world"].SetValue(Matrix.Identity);
                        renderer.effects["primitives"].Parameters["view"].SetValue(Main.viewMatrix);
                        renderer.effects["primitives"].Parameters["projection"].SetValue(Main.projectionMatrix);
                        renderer.effects["primitives"].CurrentTechnique.Passes["texturedPrimitives"].Apply();
                        renderer.device.DrawUserPrimitives(PrimitiveType.TriangleList, vertices, 0, primitiveCount, CustomVertexFormat.VertexDeclaration);
                    }

                    // Draw leaves
                    rootMetamer.draw(gameTime);
                }
            }
        }*/
    }
}
