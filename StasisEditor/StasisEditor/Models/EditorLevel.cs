using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisEditor.Controllers;
using StasisCore.Models;
using StasisCore;

namespace StasisEditor.Models
{
    public class EditorLevel : Level
    {
        private LevelController _controller;
        private SortedDictionary<float, List<EditorActor>> _sortedActors;
        private List<EditorActor> _actorsToAdd;
        private List<EditorActor> _actorsToRemove;
        private bool _firstDraw = true;

        [Browsable(false)]
        public SortedDictionary<float, List<EditorActor>> sortedActors { get { return _sortedActors; } }
        [Browsable(false)]
        public XElement data
        {
            get
            {
                List<XElement> actorControllerData = new List<XElement>();
                foreach (List<EditorActor> actors in _sortedActors.Values)
                {
                    foreach (EditorActor actor in actors)
                    {
                        actorControllerData.Add(actor.data);
                    }
                }

                XElement d = new XElement("Level",
                    new XAttribute("name", _name),
                    new XAttribute("gravity", _gravity),
                    new XAttribute("wind", _wind),
                    new XAttribute("background_uid", _backgroundUID),
                    actorControllerData);

                return d;
            }
        }
        [Browsable(false)]
        public LevelController controller { get { return _controller; } }

        // Create new
        public EditorLevel(LevelController levelController, string name) : base(name)
        {
            _controller = levelController;
            _sortedActors = new SortedDictionary<float, List<EditorActor>>();
            _actorsToAdd = new List<EditorActor>();
            _actorsToRemove = new List<EditorActor>();
        }

        // Load from xml
        public EditorLevel(LevelController levelController, XElement data) : base(data)
        {
            _controller = levelController;
            _sortedActors = new SortedDictionary<float, List<EditorActor>>();
            _actorsToAdd = new List<EditorActor>();
            _actorsToRemove = new List<EditorActor>();
            List<XElement> secondPassData = new List<XElement>();

            // First pass -- load independent actors
            foreach (XElement actorData in data.Elements("Actor"))
            {
                switch (actorData.Attribute("type").Value)
                {
                    case "Box":
                        addActor(new EditorBoxActor(this, actorData), true);
                        break;

                    case "Circle":
                        addActor(new EditorCircleActor(this, actorData), true);
                        break;

                    case "Circuit":
                        secondPassData.Add(actorData);
                        break;

                    case "Fluid":
                        addActor(new EditorFluidActor(this, actorData), true);
                        break;

                    case "Item":
                        addActor(new EditorItemActor(this, actorData), true);
                        break;

                    case "PlayerSpawn":
                        addActor(new EditorPlayerSpawnActor(this, actorData), true);
                        break;

                    case "Rope":
                        addActor(new EditorRopeActor(this, actorData), true);
                        break;

                    case "Terrain":
                        addActor(new EditorTerrainActor(this, actorData), true);
                        break;

                    case "Tree":
                        addActor(new EditorTreeActor(this, actorData), true);
                        break;

                    case "Revolute":
                        secondPassData.Add(actorData);
                        break;

                    case "Prismatic":
                        secondPassData.Add(actorData);
                        break;

                    case "CollisionFilter":
                        secondPassData.Add(actorData);
                        break;

                    case "Goal":
                        addActor(new EditorGoalActor(this, actorData), true);
                        break;

                    case "Decal":
                        addActor(new EditorDecalActor(this, actorData));
                        break;
                }
            }

            // Second pass -- load dependent actors
            foreach (XElement actorData in secondPassData)
            {
                switch (actorData.Attribute("type").Value)
                {
                    case "Circuit":
                        addActor(new EditorCircuitActor(this, actorData), true);
                        break;

                    case "Revolute":
                        addActor(new EditorRevoluteActor(this, actorData), true);
                        break;

                    case "Prismatic":
                        addActor(new EditorPrismaticActor(this, actorData), true);
                        break;

                    case "CollisionFilter":
                        addActor(new EditorCollisionFilterActor(this, actorData), true);
                        break;
                }
            }
        }

        // Get actor by id
        public EditorActor getActor(int id)
        {
            foreach (List<EditorActor> actors in _sortedActors.Values)
            {
                foreach (EditorActor actor in actors)
                {
                    if (actor.id == id)
                        return actor;
                }
            }

            return null;
        }

        // Get actors by type
        public List<T> getActors<T>(ActorType actorType) where T : EditorActor
        {
            List<T> results = new List<T>();

            foreach (List<EditorActor> actors in _sortedActors.Values)
            {
                foreach (EditorActor actor in actors)
                {
                    if (actor.type == actorType)
                        results.Add(actor as T);
                }
            }

            return results;
        }

        // Contains actor
        public bool containsActor(EditorActor actor)
        {
            foreach (List<EditorActor> actors in _sortedActors.Values)
            {
                if (actors.Contains(actor))
                    return true;
            }
            return false;
        }

        // Add an actor
        public void addActor(EditorActor actor, bool immediate = false)
        {
            if (immediate)
            {
                if (!_sortedActors.ContainsKey(actor.layerDepth))
                    _sortedActors.Add(actor.layerDepth, new List<EditorActor>());
                _sortedActors[actor.layerDepth].Add(actor);
            }
            else
            {
                _actorsToAdd.Add(actor);
            }
        }

        // Remove an actor
        public void removeActor(EditorActor actor, bool immediate = false)
        {
            if (immediate)
            {
                _sortedActors[actor.layerDepth].Remove(actor);
                if (_sortedActors[actor.layerDepth].Count == 0)
                    _sortedActors.Remove(actor.layerDepth);
            }
            else
            {
                _actorsToRemove.Add(actor);
            }
        }

        // Save
        public void save()
        {
            string filePath = ResourceManager.levelPath + "\\" + _name + ".xml";
            if (File.Exists(filePath))
            {
                string backupFilePath = filePath + ".bak";
                if (File.Exists(backupFilePath))
                    File.Delete(backupFilePath);
                File.Move(filePath, backupFilePath);
            }

            XDocument doc = new XDocument();
            doc.Add(data);
            doc.Save(filePath);
        }

        // Get unused actor id
        public int getUnusedActorId()
        {
            // Method to test if an id is being used
            Func<int, bool> isIdUsed = (id) =>
            {
                foreach (List<EditorActor> actors in _sortedActors.Values)
                {
                    foreach (EditorActor actor in actors)
                    {
                        if (actor.id == id)
                        {
                            id++;
                            return true;
                        }
                    }
                }
                return false;
            };

            // Start at zero, and increment until an id is not used
            int current = 0;
            while (isIdUsed(current))
                current++;

            return current;
        }

        // validateActorLayerDepths
        public void validateActorLayerDepths()
        {
            Dictionary<float, List<EditorActor>> invalidActors = new Dictionary<float, List<EditorActor>>();

            // Search through our sorted dictionary of actors and look for actors whose layer depth doesn't match their key
            foreach (KeyValuePair<float, List<EditorActor>> actorLayerPair in _sortedActors)
            {
                foreach (EditorActor actor in actorLayerPair.Value)
                {
                    if (actor.layerDepth != actorLayerPair.Key)
                    {
                        if (!invalidActors.ContainsKey(actorLayerPair.Key))
                            invalidActors.Add(actorLayerPair.Key, new List<EditorActor>());

                        invalidActors[actorLayerPair.Key].Add(actor);
                    }
                }
            }

            // Remove invalid actors from the sorted dictionary
            foreach (KeyValuePair<float, List<EditorActor>> invalidActorLayerPair in invalidActors)
            {
                foreach (EditorActor actor in invalidActorLayerPair.Value)
                {
                    _sortedActors[invalidActorLayerPair.Key].Remove(actor);

                    if (_sortedActors[invalidActorLayerPair.Key].Count == 0)
                        _sortedActors.Remove(invalidActorLayerPair.Key);
                }
            }

            // Re-add actors to the sorted dictionary
            foreach (List<EditorActor> actors in invalidActors.Values)
            {
                foreach (EditorActor actor in actors)
                {
                    addActor(actor, true);
                }
            }
        }

        // Update
        public void update()
        {
            // Check actor layer depths and reorder them if necessary
            validateActorLayerDepths();

            foreach (EditorActor actor in _actorsToAdd)
            {
                if (!_sortedActors.ContainsKey(actor.layerDepth))
                    _sortedActors.Add(actor.layerDepth, new List<EditorActor>());
                _sortedActors[actor.layerDepth].Add(actor);
            }
            _actorsToAdd.Clear();

            foreach (EditorActor actor in _actorsToRemove)
            {
                _sortedActors[actor.layerDepth].Remove(actor);
                if (_sortedActors[actor.layerDepth].Count == 0)
                    _sortedActors.Remove(actor.layerDepth);
            }
            _actorsToRemove.Clear();

            foreach (List<EditorActor> actors in _sortedActors.Values)
            {
                foreach (EditorActor actor in actors)
                {
                    actor.update();
                }
            }
        }

        // Draw
        public void draw()
        {
            // Fix polygon texture (is black for some reason when first drawn, despite all vertices' colors being set correctly)
            if (_firstDraw)
            {
                _firstDraw = false;
                List<EditorPolygonActor> polygonActors = getActors<EditorPolygonActor>(ActorType.Terrain);
                polygonActors.AddRange(getActors<EditorPolygonActor>(ActorType.Fluid));

                foreach (EditorPolygonActor polygonActor in polygonActors)
                {
                    polygonActor.triangulate();
                }
            }

            foreach (List<EditorActor> actors in _sortedActors.Values)
            {
                foreach (EditorActor actor in actors)
                {
                    actor.draw();
                }
            }
        }
    }
}
