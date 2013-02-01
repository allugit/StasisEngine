using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisCore;

namespace StasisEditor.Models
{
    public class EditorPrismaticActor : EditorActor
    {
        private EditorActor _actor;
        private Vector2 _actorControl;
        private bool _selectedActorControl;
        private bool _moveActor;

        private Vector2 _position;

        public override XElement data
        {
            get
            {
                XElement d = base.data;
                d.SetAttributeValue("position", _position);
                d.SetAttributeValue("actor_id", _actor == null ? -1 : _actor.id);
                return d;
            }
        }

        public EditorPrismaticActor(EditorLevel level)
            : base(level, ActorType.Prismatic, level.controller.getUnusedActorID())
        {
            _position = level.controller.worldMouse;
            initializeControls();
            _selectedActorControl = true;
            _moveActor = true;
        }

        public EditorPrismaticActor(EditorLevel level, XElement data)
            : base(level, data)
        {
            _position = Loader.loadVector2(data.Attribute("position"), Vector2.Zero);
            _actor = data.Attribute("actor_id").Value == "-1" ? null : level.getActor(int.Parse(data.Attribute("actor_id").Value));
            initializeControls();
        }

        private void initializeControls()
        {
            _actorControl = _position + new Vector2(-24f, 0f) / _level.controller.scale;
        }

        public override void deselect()
        {
            _selectedActorControl = false;
            _moveActor = true;
            base.deselect();
        }

        public override void handleMouseDown()
        {
            if (selected)
            {
                if (_selectedActorControl && !_moveActor)
                {
                    foreach (EditorActor actor in _level.actors)
                    {
                        if (actor.type == ActorType.Box || actor.type == ActorType.Circle || actor.type == ActorType.Terrain)
                        {
                            if (actor.hitTest(_actorControl))
                            {
                                _actor = actor;
                            }
                        }
                    }
                }
                deselect();
            }
            else
            {
                select();
            }
        }

        public override bool hitTest(Vector2 testPoint)
        {
            // Hit test icon
            if (_level.controller.hitTestPoint(testPoint, _position))
            {
                _selectedActorControl = true;
                _moveActor = true;
                return true;
            }

            // Hit test control
            if (_level.controller.hitTestPoint(testPoint, _actorControl))
            {
                _selectedActorControl = true;
                _moveActor = false;
                return true;
            }

            return false;
        }

        public override void update()
        {
            Vector2 worldDelta = _level.controller.worldDeltaMouse;

            if (selected)
            {
                if (!_level.controller.ctrl)
                {
                    if (_moveActor)
                        _position += worldDelta;
                    if (_selectedActorControl)
                        _actorControl += worldDelta;
                }
            }
        }

        public override void draw()
        {
            // Icon
            _level.controller.view.drawIcon(_type, _position, _layerDepth);

            // Connections and controls
            if (_actor == null)
            {
                _level.controller.view.drawLine(_position, _actorControl, Color.Purple, _layerDepth);
                _level.controller.view.drawPoint(_actorControl, Color.Purple, _layerDepth);
            }
            else
            {
                _level.controller.view.drawLine(_position, _actor.prismaticConnectionPosition, Color.Purple * 0.5f, _layerDepth);
            }
        }
    }
}
