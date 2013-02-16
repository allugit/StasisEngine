using System;
using System.Collections.Generic;
using System.Xml.Linq;
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
        private TreeSystem _treeSystem;
        private Vector2 _position;
        private float _angle;
        private int _seed;
        private float _age;
        private float _internodeHalfLength;
        private float _internodeLength;
        private float _internodeLengthSq;
        private int _maxShootLength;
        private float _perceptionAngle;
        private float _perceptionRadius;
        private float _occupancyRadius;
        private float _lateralAngle;
        private float _fullExposure;
        private float _penumbraA;
        private float _penumbraB;
        private float _optimalGrowthWeight;
        private float _tropismWeight;
        private Vector2 _tropism;
        private float _maxBaseHalfWidth;
        private float _minBaseHalfWidth = 0.04f;
        private Vector2 _gravity = new Vector2(0, 0.005f);
        private Vector2 _brokenGravity = new Vector2(0, 0.02f);
        //public Vector2 gravity = new Vector2(0, 0);
        private Vector2 _anchorNormal;
        private Vector2 _rootPosition;
        private int _numVertices;
        private Random _random;
        private Metamer _rootMetamer;
        private int _iterations;
        private AABB _aabb;
        private Vector2 _aabbMargin = new Vector2(64f, 64f) / Settings.BASE_SCALE;
        private bool _active;
        private int _longestPath;
        //private Texture2D barkTexture;
        //private Vector3 barkColor;

        public TreeSystem treeSystem { get { return _treeSystem; } }
        public float age { get { return _age; } }
        public float perceptionRadius { get { return _perceptionRadius; } }
        public float perceptionAngle { get { return _perceptionAngle; } }
        public Vector2 position { get { return _position; } }
        public float internodeLength { get { return _internodeLength; } }
        public int iterations { get { return _iterations; } }
        public Vector2 rootPosition { get { return _rootPosition; } }
        public Vector2 anchorNormal { get { return _anchorNormal; } }
        public float occupancyRadius { get { return _occupancyRadius; } }
        public Random random { get { return _random; } }
        public float lateralAngle { get { return _lateralAngle; } }
        public float penumbraA { get { return _penumbraA; } }
        public float penumbraB { get { return _penumbraB; } }
        public float fullExposure { get { return _fullExposure; } }
        public int maxShootLength { get { return _maxShootLength; } }
        public float optimalGrowthWeight { get { return _optimalGrowthWeight; } }
        public Vector2 tropism { get { return _tropism; } }
        public float tropismWeight { get { return _tropismWeight; } }
        public int longestPath { get { return _longestPath; } set { _longestPath = value; } }
        public float maxBaseHalfWidth { get { return _maxBaseHalfWidth; } }
        public float minBaseHalfWidth { get { return _minBaseHalfWidth; } }
        public Vector2 brokenGravity { get { return _brokenGravity; } }
        public Vector2 gravity { get { return _gravity; } }
        public float internodeLengthSq { get { return _internodeLengthSq; } }

        /*
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
        */

        // Constructor
        public Tree(TreeSystem treeSystem, XElement data)
        {
            _treeSystem = treeSystem;
            _angle = Loader.loadFloat(data.Attribute("angle"), 0f);
            _seed = Loader.loadInt(data.Attribute("seed"), 12345);
            _age = Loader.loadFloat(data.Attribute("age"), 0f);
            _internodeHalfLength = Loader.loadFloat(data.Attribute("internode_length"), 0.5f);
            _internodeLength = _internodeHalfLength * 2f;
            _maxShootLength = Loader.loadInt(data.Attribute("max_shoot_length"), 4);
            _maxBaseHalfWidth = Loader.loadFloat(data.Attribute("max_base_width"), 0.25f);
            _perceptionAngle = Loader.loadFloat(data.Attribute("perception_angle"), 0.6f);
            _perceptionRadius = Loader.loadFloat(data.Attribute("perception_radius"), 4f);
            _lateralAngle = Loader.loadFloat(data.Attribute("lateral_angle"), 0.6f);
            _fullExposure = Loader.loadFloat(data.Attribute("full_exposure"), 1f);
            _penumbraA = Loader.loadFloat(data.Attribute("penumbra_a"), 1f);
            _penumbraB = Loader.loadFloat(data.Attribute("penumbra_b"), 2f);
            _optimalGrowthWeight = Loader.loadFloat(data.Attribute("optimal_growth_weight"), 1f);
            _tropismWeight = Loader.loadFloat(data.Attribute("tropism_weight"), 1f);
            _tropism = Loader.loadVector2(data.Attribute("tropism"), Vector2.Zero);
            _position = Loader.loadVector2(data.Attribute("position"), Vector2.Zero);
            //_leafMaterialUID = Loader.loadString(data.Attribute("leaf_material_uid"), "default");
            //_barkMaterialUID = Loader.loadString(data.Attribute("bark_material_uid"), "default");

            /*
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
            */

            _random = new Random(_seed);
            _internodeLengthSq = _internodeLength * _internodeLength;
            _aabb = new AABB();
            _aabb.lowerBound = _position;
            _aabb.upperBound = _position;

            // Calculate root position
            float rootAngle = _angle + (StasisMathHelper.pi);
            _rootPosition = _position + new Vector2((float)Math.Cos(rootAngle), (float)Math.Sin(rootAngle)) * 5f;

            // Calculate anchor normals
            float anchorAngle = _angle - (StasisMathHelper.pi * 0.5f);
            _anchorNormal = new Vector2((float)Math.Cos(anchorAngle), (float)Math.Sin(anchorAngle));

            // Initialize vertices
            //vertices = new CustomVertexFormat[MAX_VERTICES];

            // Fix tropism vector
            //this.tropism -= position;

            // Debug -- perception points
            //pointsInTerminalBudPerceptionCone = new List<Vector2>();
            //pointsInLateralBudPerceptionCone = new List<Vector2>();

            // Create first metamer
            _rootMetamer = new Metamer(this, null, BudType.TERMINAL, BudState.DORMANT, BudState.DEAD, _angle);
            _rootMetamer.isTail = true;
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
            _aabb.lowerBound = Vector2.Min(point - _aabbMargin, _aabb.lowerBound);
            _aabb.upperBound = Vector2.Max(point + _aabbMargin, _aabb.upperBound);
        }

        // iterate
        public void iterate(int count = 1)
        {
            // Debug -- clear list of perception points to draw
            //if (drawPerceptionPoints)
            //{
            //    pointsInTerminalBudPerceptionCone.Clear();
            //    pointsInLateralBudPerceptionCone.Clear();
            //}

            // Clear marker competition and shadow values
            foreach (Dictionary<int, MarkerCell> gridRow in _treeSystem.markerGrid.Values)
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
                _rootMetamer.calculateLocalEnvironment();

                // Determine bud fate
                _rootMetamer.determineBudFate();

                // Append new shoots
                _rootMetamer.appendNewShoots();

                // Update branch width
                _rootMetamer.calculateResources(1);

                // Create constraints
                _rootMetamer.createConstraints();

                _iterations++;
            }

            // Force aabb update
            _rootMetamer.updateAABB();
        }

        // step
        public void step()
        {
            _rootMetamer.accumulateForces();
            _rootMetamer.integrate();
            for (int n = 0; n < NUM_CONSTRAINT_ITERATIONS; n++)
            {
                // Satisfy constraints
                _rootMetamer.satisfyConstraints();

                if (!_rootMetamer.isBroken)
                {
                    // Pin root metamer
                    _rootMetamer.position = _position;
                    _rootMetamer.oldPosition = _position;
                }
            }
        }

        // update
        public void update()
        {
            // Flag as active
            _active = AABB.TestOverlap(ref _treeSystem.treeAABB, ref _aabb);

            if (_active)
            {
                // Verlet integration
                if (_iterations > 0)
                    step();

                // Prepare collisions
                //prepareCollisions();

                // Resolve collisions
                _rootMetamer.resolveCollisions();

                // Reset vertices
                //numVertices = 0;
                //primitiveCount = 0;

                // Update metamers
                _rootMetamer.update();
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
