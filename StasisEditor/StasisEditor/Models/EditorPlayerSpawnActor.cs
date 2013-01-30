﻿using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisCore;

namespace StasisEditor.Models
{
    public class EditorPlayerSpawnActor : EditorActor
    {
        private Vector2 _position;

        public Vector2 position { get { return _position; } set { _position = value; } }
        public override XElement data
        {
            get
            {
                XElement d = base.data;
                d.SetAttributeValue("position", _position);
                return d;
            }
        }

        public EditorPlayerSpawnActor(EditorLevel level)
            : base(level, ActorType.PlayerSpawn, level.controller.getUnusedActorID())
        {
            _position = level.controller.worldMouse;
        }

        public EditorPlayerSpawnActor(EditorLevel level, XElement data)
            : base(level, data)
        {
            _position = Loader.loadVector2(data.Attribute("position"), Vector2.Zero);
        }

        public override bool hitTest()
        {
            Vector2 worldMouse = _level.controller.worldMouse;
            if (_level.controller.hitTestPoint(worldMouse, _position, 12f))
                return true;

            return false;
        }

        public override void update()
        {
            Vector2 worldDelta = _level.controller.worldMouse - _level.controller.oldWorldMouse;

            if (selected)
            {
                _position += worldDelta;
            }
        }

        public override void draw()
        {
            _level.controller.view.drawIcon(ActorType.PlayerSpawn, _position, _layerDepth);
        }
    }
}
