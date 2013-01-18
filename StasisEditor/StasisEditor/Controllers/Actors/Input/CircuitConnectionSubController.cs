﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisCore.Models;
using StasisEditor.Models;

namespace StasisEditor.Controllers.Actors
{
    public class CircuitConnectionSubController : PointSubController
    {
        private ActorController _connectedActorController;
        private CircuitConnectionProperties _properties;
        private Gate _gate;

        public bool connected { get { return _connectedActorController != null; } }
        public Gate gate { get { return _gate; } }
        public List<ActorProperties> properties { get { return new List<ActorProperties>(new[] { _properties }); } }
        public XElement data
        {
            get
            {
                XElement d = new XElement("CircuitConnection",
                    new XAttribute("gate_id", _gate.id),
                    new XAttribute("message", _properties.message));
                if (_connectedActorController != null)
                    d.SetAttributeValue("actor_id", _connectedActorController.id);
                return d;
            }
        }

        // Create new
        public CircuitConnectionSubController(Vector2 position, IPointSubControllable circuitActorController, Gate gate)
            : base(position, circuitActorController)
        {
            _gate = gate;
            _properties = new CircuitConnectionProperties("");
        }

        // Load from xml
        public CircuitConnectionSubController(ActorController connectedActorController, IPointSubControllable circuitActorController, Gate gate, XElement data)
            : base(connectedActorController.connectionPosition, circuitActorController)
        {
            _gate = gate;
            _properties = new CircuitConnectionProperties(data);
            connectToActorController(connectedActorController);
        }

        // Handle mouse down
        public override void handleMouseDown()
        {
            // Deselect like normal
            base.handleMouseDown();

            // Hit test actor controllers (excluding circuit actor controllers)
            foreach (ActorController actorController in _actorController.levelController.actorControllers)
            {
                if (!(actorController is CircuitActorController))
                {
                    if (actorController.hitTest(_actorController.levelController.getWorldMouse()).Count > 0)
                    {
                        connectToActorController(actorController);
                    }
                }
            }
        }

        // Connect to actor controller
        public void connectToActorController(ActorController actorController)
        {
            Debug.Assert(_connectedActorController == null);
            _connectedActorController = actorController;
            actorController.connectCircuit(this);
        }

        // Disconnect from actor controller
        public void disconnect()
        {
            _connectedActorController.disconnectCircuit(this);
            _connectedActorController = null;
        }

        // Update position
        public void updatePosition(Vector2 position)
        {
            _position = position;
        }
    }
}
