using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisCore;

namespace StasisEditor.Models
{
    public class EditorCircuitActor : EditorActor
    {
        private EditorCircuit _circuit;

        private Vector2 _position;

        public override XElement data
        {
            get
            {
                XElement d = base.data;
                d.SetAttributeValue("position", _position);
                d.SetAttributeValue("circuit_uid", _circuit.uid);
                return d;
            }
        }

        public EditorCircuitActor(EditorLevel level, string circuitUID)
            : base(level, ActorType.Circuit, level.controller.getUnusedActorID())
        {
            _circuit = level.controller.editorController.circuitController.getCircuit(circuitUID);
            _position = _level.controller.worldMouse;
        }

        public EditorCircuitActor(EditorLevel level, XElement data) 
            : base(level, data)
        {
            _circuit = level.controller.editorController.circuitController.getCircuit(data.Attribute("circuit_uid").Value);
            _position = Loader.loadVector2(data.Attribute("position"), Vector2.Zero);
        }

        public override bool hitTest()
        {
            Vector2 worldMouse = _level.controller.worldMouse;
            return _level.controller.hitTestPoint(worldMouse, _position, 12f);
        }

        public override void update()
        {
            Vector2 worldDelta = _level.controller.worldDeltaMouse;

            if (selected)
            {
                if (!_level.controller.ctrl)
                {
                    _position += worldDelta;
                }
            }
        }

        public override void draw()
        {
            _level.controller.view.drawIcon(ActorType.Circuit, _position, _layerDepth);
        }
    }
}
