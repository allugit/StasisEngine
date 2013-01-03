﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisCore.Controllers;
using StasisCore.Resources;
using StasisCore.Models;
using StasisEditor.Views;

namespace StasisEditor.Controllers
{
    public class CircuitController : Controller
    {
        private EditorController _editorController;
        private CircuitsView _view;
        private BindingList<Circuit> _circuits;
        private System.Drawing.Point _mouse;
        private System.Drawing.Point _oldMouse;
        private Vector2 _screenCenter;
        private float _scale = 35;
        private bool _ctrl;
        private bool _shift;

        public System.Drawing.Point mouse
        {
            get { return _mouse; }
            set { _oldMouse = _mouse; _mouse = value; }
        }
        public float getScale() { return _scale; }
        public Vector2 getWorldOffset() { return _screenCenter + (new Vector2(_view.Width, _view.Height) / 2) / _scale; }
        public Vector2 getWorldMouse() { return new Vector2(_mouse.X, _mouse.Y) / _scale - getWorldOffset(); }
        public Vector2 getOldWorldMouse() { return new Vector2(_oldMouse.X, _oldMouse.Y) / _scale - getWorldOffset(); }
        public bool shift { get { return _shift; } set { _shift = value; } }
        public bool ctrl { get { return _ctrl; } set { _ctrl = value; } }
        public Vector2 screenCenter { get { return _screenCenter; } set { _screenCenter = value; } }
        public BindingList<Circuit> circuits { get { return _circuits; } }

        public CircuitController(EditorController editorController, CircuitsView circuitsView) : base()
        {
            _editorController = editorController;
            _view = circuitsView;
            _circuits = new BindingList<Circuit>();
            _view.setController(this);

            // Initialize resources
            List<ResourceObject> resources = ResourceController.loadCircuits();
            foreach (ResourceObject resource in resources)
                _circuits.Add(new Circuit(resource.data));
        }

        // Get unused gate id
        public int getUnusedGateID(Circuit circuit)
        {
            int id = 0;
            while (true)
            {
                foreach (Gate gate in circuit.gates)
                {
                    if (gate.id == id)
                    {
                        // Increment id and try again
                        id++;
                        break;
                    }
                }

                // Existing id was not found, so use this one
                return id;
            }
        }

        // Save circuits
        public void saveCircuits()
        {
            ResourceController.saveCircuitResources(new List<Circuit>(_circuits));
        }

        // Delete circuit gate
        public void deleteCircuitGate(Circuit circuit, Gate gate)
        {
            // Disconnect gate connections
            foreach (Gate input in gate.inputs)
                input.outputs.Remove(gate);
            foreach (Gate output in gate.outputs)
                output.inputs.Remove(gate);

            // Remove gate from circuit
            circuit.gates.Remove(gate);

            // Deselect gate
            _view.deselectGate();
        }

        // Disconect gates
        public void disconnectGates(Gate gateA, Gate gateB)
        {
            gateA.outputs.Remove(gateB);
            gateB.inputs.Remove(gateA);
        }
    }
}