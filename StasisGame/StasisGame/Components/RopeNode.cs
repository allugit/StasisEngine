using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using FarseerPhysics.Dynamics;
using FarseerPhysics.Dynamics.Joints;
using StasisCore.Models;

namespace StasisGame.Components
{
    public struct RopeNodeTexture
    {
        public Texture2D texture;
        public Vector2 center;
        public float angleOffset;
        public RopeNodeTexture(Texture2D texture, Vector2 center, float angleOffset)
        {
            this.texture = texture;
            this.center = center;
            this.angleOffset = angleOffset;
        }
    };

    public class RopeNode
    {
        private RopeComponent _ropeComponent;
        private RopeNode _next;
        private RopeNode _previous;
        private Body _body;
        private RevoluteJoint _joint;
        private RevoluteJoint _anchorJoint;
        private float _halfLength;
        private RopeNodeTexture[] _ropeNodeTextures;

        public RopeComponent ropeComponent { get { return _ropeComponent; } set { _ropeComponent = value; } }
        public RopeNode next { get { return _next; } set { _next = value; } }
        public RopeNode previous { get { return _previous; } set { _previous = value; } }
        public RevoluteJoint joint { get { return _joint; } set { _joint = value; } }
        public RevoluteJoint anchorJoint { get { return _anchorJoint; } set { _anchorJoint = value; } }
        public Body body { get { return _body; } }
        public float halfLength { get { return _halfLength; } }
        public RopeNodeTexture[] ropeNodeTextures { get { return _ropeNodeTextures; } }
        public RopeNode head
        {
            get
            {
                RopeNode current = this;
                while (current.previous != null)
                    current = current.previous;
                return current;
            }
        }
        public RopeNode tail
        {
            get
            {
                RopeNode current = this;
                while (current.next != null)
                    current = current.next;
                return current;
            }
        }
        public int count
        {
            get
            {
                RopeNode current = head;
                int i = 1;
                while (current.next != null)
                {
                    i++;
                    current = current.next;
                }
                return i;
            }
        }

        public RopeNode(RopeNodeTexture[] ropeNodeTextures, Body body, RevoluteJoint joint, float halfLength)
        {
            _ropeNodeTextures = ropeNodeTextures;
            _body = body;
            _joint = joint;
            _halfLength = halfLength;
        }

        public RopeNode getByIndex(int index)
        {
            int i = 0;
            RopeNode current = head;

            while (current != null)
            {
                if (i == index)
                    return current;

                i++;
                current = current.next;
            }

            return null;
        }

        public void insert(RopeNode node)
        {
            if (_next != null)
            {
                _next.previous = node;
            }
            node.previous = this;
            node.next = _next;
            _next = node;
        }

        public void remove()
        {
            if (_previous != null)
            {
                _previous.next = _next;
            }
            if (_next != null)
            {
                _next.previous = _previous;
            }
            _previous = null;
            _next = null;
        }
    }
}
