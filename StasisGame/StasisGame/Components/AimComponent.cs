﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StasisGame.Components
{
    public class AimComponent : IComponent
    {
        private float _angle;
        private float _length;

        public ComponentType componentType { get { return ComponentType.Aim; } }
        public float angle { get { return _angle; } set { _angle = value; } }
        public float length { get { return _length; } set { _length = value; } }

        public AimComponent(float angle, float length)
        {
            _angle = angle;
            _length = length;
        }
    }
}