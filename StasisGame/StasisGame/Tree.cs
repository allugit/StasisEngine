﻿using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Collision;
using StasisGame.Systems;
using StasisCore;

namespace StasisGame
{
    using Settings = StasisCore.Settings;

    public class Tree : IRenderablePrimitive
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
        private VertexPositionColorTexture[] _vertices;
        private int _primitiveCount;
        private Texture2D _barkTexture;
        private List<List<Texture2D>> _leafTextures;
        private float _layerDepth;
        private int _entityId;
        private string _levelUid;

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
        public VertexPositionColorTexture[] vertices { get { return _vertices; } }
        public int numVertices { get { return _numVertices; } set { _numVertices = value; } }
        public int primitiveCount { get { return _primitiveCount; } set { _primitiveCount = value; } }
        public Texture2D barkTexture { get { return _barkTexture; } }
        public List<List<Texture2D>> leafTextures { get { return _leafTextures; } }
        public Metamer rootMetamer { get { return _rootMetamer; } }
        public bool active { get { return _active; } }
        public float layerDepth { get { return _layerDepth; } }
        public Matrix worldMatrix { get { return Matrix.Identity; } }
        public Texture2D texture { get { return _barkTexture; } }
        public int entityId { get { return _entityId; } }
        public string levelUid { get { return _levelUid; } }

        // Constructor
        public Tree(TreeSystem treeSystem, string levelUid, Texture2D barkTexture, List<List<Texture2D>> leafTextures, XElement data)
        {
            _treeSystem = treeSystem;
            _levelUid = levelUid;
            _leafTextures = leafTextures;
            _barkTexture = barkTexture;
            _angle = Loader.loadFloat(data.Attribute("angle"), 0f);
            _seed = Loader.loadInt(data.Attribute("seed"), 12345);
            _age = Loader.loadFloat(data.Attribute("age"), 0f);
            _internodeHalfLength = Loader.loadFloat(data.Attribute("internode_half_length"), 0.5f);
            _internodeLength = _internodeHalfLength * 2f;
            _maxShootLength = Loader.loadInt(data.Attribute("max_shoot_length"), 4);
            _maxBaseHalfWidth = Loader.loadFloat(data.Attribute("max_base_half_width"), 0.25f);
            _perceptionAngle = Loader.loadFloat(data.Attribute("perception_angle"), 0.6f);
            _perceptionRadius = Loader.loadFloat(data.Attribute("perception_radius"), 4f);
            _occupancyRadius = Loader.loadFloat(data.Attribute("occupancy_radius"), 1f);
            _lateralAngle = Loader.loadFloat(data.Attribute("lateral_angle"), 0.6f);
            _fullExposure = Loader.loadFloat(data.Attribute("full_exposure"), 1f);
            _penumbraA = Loader.loadFloat(data.Attribute("penumbra_a"), 1f);
            _penumbraB = Loader.loadFloat(data.Attribute("penumbra_b"), 2f);
            _optimalGrowthWeight = Loader.loadFloat(data.Attribute("optimal_growth_weight"), 1f);
            _tropismWeight = Loader.loadFloat(data.Attribute("tropism_weight"), 1f);
            _tropism = Loader.loadVector2(data.Attribute("tropism"), Vector2.Zero);
            _position = Loader.loadVector2(data.Attribute("position"), Vector2.Zero);
            _layerDepth = Loader.loadFloat(data.Attribute("layer_depth"), 0.1f);
            _entityId = int.Parse(data.Attribute("id").Value);

            _vertices = new VertexPositionColorTexture[MAX_VERTICES];
            for (int i = 0; i < MAX_VERTICES; i++)
            {
                _vertices[i].Color = Color.White;
            }
            _random = new Random(_seed);
            _internodeLengthSq = _internodeLength * _internodeLength;
            _aabb = new AABB();
            _aabb.LowerBound = _position;
            _aabb.UpperBound = _position;

            // Calculate root position
            float rootAngle = _angle + (StasisMathHelper.pi);
            _rootPosition = _position + new Vector2((float)Math.Cos(rootAngle), (float)Math.Sin(rootAngle)) * 5f;

            // Calculate anchor normals
            float anchorAngle = _angle - (StasisMathHelper.pi * 0.5f);
            _anchorNormal = new Vector2((float)Math.Cos(anchorAngle), (float)Math.Sin(anchorAngle));

            // Create first metamer
            _rootMetamer = new Metamer(this, null, BudType.TERMINAL, BudState.DORMANT, BudState.DEAD, _angle, true);
            _rootMetamer.isTail = true;
        }

        // expandAABB
        public void expandAABB(Vector2 point)
        {
            _aabb.LowerBound = Vector2.Min(point - _aabbMargin, _aabb.LowerBound);
            _aabb.UpperBound = Vector2.Max(point + _aabbMargin, _aabb.UpperBound);
        }

        // iterate
        public void iterate(string levelUid, int count)
        {
            // Clear marker competition and shadow values
            foreach (Dictionary<int, MarkerCell> gridRow in _treeSystem.markerGrid[levelUid].Values)
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

                // Resolve collisions
                _rootMetamer.resolveCollisions();

                // Reset vertices
                numVertices = 0;
                primitiveCount = 0;

                // Update metamers
                _rootMetamer.update();
            }
        }
    }
}
