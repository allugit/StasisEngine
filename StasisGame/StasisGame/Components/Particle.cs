using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Box2D.XNA;
using StasisGame.Systems;

namespace StasisGame.Components
{
    public class Particle
    {
        public const int MAX_INFLUENCES = 100;
        public const int MAX_FIXTURES_TO_TEST = 20;
        //private FluidSimulation simulation;
        public Vector2 position;
        public Vector2 velocity;
        public Vector2 oldPosition;
        public Vector2 oldVelocity;
        public Vector2 delta;
        public AABB aabb;
        public int index;
        public int ci;
        public int cj;
        public int[] neighbors;
        public float[] distances;
        public int neighborCount;
        public bool onScreen;
        public bool alive;
        public Dictionary<int, List<int>> gridX;
        public List<int> gridY;
        public Transform collisionXF;
        public bool skipMovementUpdate;
        public Vector2[] collisionVertices;
        public Vector2[] collisionNormals;
        //private bool neighborSearchDone;
        public Vector2[] relativePosition;
        public float[] oneminusq;
        public float p;
        public float pnear;
        //public Actor[] actorsToInfluence;
        public int[] entitiesToInfluence;
        public int actorInfluenceCount;
        public Fixture[] fixturesToTest;
        public int numFixturesToTest;

        // isActive
        public bool isActive()
        {
            return onScreen && alive;
        }

        // Constructor
        public Particle(FluidSystem fluidSystem, int index, Vector2 position)
        {
            //this.simulation = simulation;
            this.index = index;
            this.position = position;
            onScreen = false;
            alive = false;

            aabb = new AABB();
            neighbors = new int[FluidSystem.MAX_NEIGHBORS];
            distances = new float[FluidSystem.MAX_NEIGHBORS];
            relativePosition = new Vector2[FluidSystem.MAX_NEIGHBORS];
            oneminusq = new float[FluidSystem.MAX_NEIGHBORS];
            entitiesToInfluence = new int[MAX_INFLUENCES];
            fixturesToTest = new Fixture[MAX_FIXTURES_TO_TEST];

            collisionVertices = new Vector2[Settings.b2_maxPolygonVertices];
            collisionNormals = new Vector2[Settings.b2_maxPolygonVertices];

            int i = fluidSystem.getFluidGridX(position.X);
            int j = fluidSystem.getFluidGridY(position.Y);

            if (!fluidSystem.fluidGrid.ContainsKey(i))
                fluidSystem.fluidGrid[i] = new Dictionary<int, List<int>>();
            if (!fluidSystem.fluidGrid[i].ContainsKey(j))
                fluidSystem.fluidGrid[i][j] = new List<int>();

            fluidSystem.fluidGrid[i][j].Add(index);
            ci = i;
            cj = j;
        }
    }
}
