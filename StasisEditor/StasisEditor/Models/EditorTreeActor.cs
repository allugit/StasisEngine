using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using StasisCore;

namespace StasisEditor.Models
{
    public class EditorTreeActor : EditorActor, IActorComponent
    {
        private float _angle;
        private int _seed;
        private float _age;
        private float _internodeHalfLength;
        private int _maxShootLength;
        private float _maxBaseHalfWidth;
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
        private float _minLeafRatioCutoff;
        private float _leafRatioOffset;
        private Vector2 _position;
        private string _leafMaterialUID;
        private string _barkMaterialUID;

        public float angle { get { return _angle; } set { _angle = value; } }
        public int seed { get { return _seed; } set { _seed = value; } }
        public float age { get { return _age; } set { _age = value; } }
        public float internodeHalfLength { get { return _internodeHalfLength; } set { _internodeHalfLength = value; } }
        public int maxShootLength { get { return _maxShootLength; } set { _maxShootLength = value; } }
        public float maxBaseHalfWidth { get { return _maxBaseHalfWidth; } set { _maxBaseHalfWidth = value; } }
        public float perceptionAngle { get { return _perceptionAngle; } set { _perceptionAngle = value; } }
        public float perceptionRadius { get { return _perceptionRadius; } set { _perceptionRadius = value; } }
        public float occupancyRadius { get { return _occupancyRadius; } set { _occupancyRadius = value; } }
        public float lateralAngle { get { return _lateralAngle; } set { _lateralAngle = value; } }
        public float fullExposure { get { return _fullExposure; } set { _fullExposure = value; } }
        public float penumbraA { get { return _penumbraA; } set { _penumbraA = value; } }
        public float penumbraB { get { return _penumbraB; } set { _penumbraB = value; } }
        public float optimalGrowthWeight { get { return _optimalGrowthWeight; } set { _optimalGrowthWeight = value; } }
        public float tropismWeight { get { return _tropismWeight; } set { _tropismWeight = value; } }
        public Vector2 tropism { get { return _tropism; } set { _tropism = value; } }
        public float minLeafRatioCutoff { get { return _minLeafRatioCutoff; } set { _minLeafRatioCutoff = Math.Min(Math.Max(value, 0f), 1f); } }
        public float leafRatioOffset { get { return _leafRatioOffset; } set { _leafRatioOffset = Math.Min(Math.Max(value, 0f), 1f); } }
        public string leafMaterialUID { get { return _leafMaterialUID; } set { _leafMaterialUID = value; } }
        public string barkMaterialUID { get { return _barkMaterialUID; } set { _barkMaterialUID = value; } }
        [Browsable(false)]
        public override Vector2 circuitConnectionPosition { get { return _position; } }
        [Browsable(false)]
        public override XElement data
        {
            get
            {
                XElement d = base.data;
                d.SetAttributeValue("angle", _angle);
                d.SetAttributeValue("seed", _seed);
                d.SetAttributeValue("age", _age);
                d.SetAttributeValue("internode_half_length", _internodeHalfLength);
                d.SetAttributeValue("max_shoot_length", _maxShootLength);
                d.SetAttributeValue("max_base_half_width", _maxBaseHalfWidth);
                d.SetAttributeValue("perception_angle", _perceptionAngle);
                d.SetAttributeValue("perception_radius", _perceptionRadius);
                d.SetAttributeValue("occupancy_radius", _occupancyRadius);
                d.SetAttributeValue("lateral_angle", _lateralAngle);
                d.SetAttributeValue("full_exposure", _fullExposure);
                d.SetAttributeValue("penumbra_a", _penumbraA);
                d.SetAttributeValue("penumbra_b", _penumbraB);
                d.SetAttributeValue("optimal_growth_weight", _optimalGrowthWeight);
                d.SetAttributeValue("tropism_weight", _tropismWeight);
                d.SetAttributeValue("tropism", _tropism);
                d.SetAttributeValue("min_leaf_ratio_cutoff", _minLeafRatioCutoff);
                d.SetAttributeValue("leaf_ratio_offset", _leafRatioOffset);
                d.SetAttributeValue("position", _position);
                d.SetAttributeValue("leaf_material_uid", _leafMaterialUID);
                d.SetAttributeValue("bark_material_uid", _barkMaterialUID);
                return d;
            }
        }

        public EditorTreeActor(EditorLevel level)
            : base(level, ActorType.Tree, level.controller.getUnusedActorID())
        {
            _angle = 0f;
            _seed = 12345;
            _age = 0f;
            _internodeHalfLength = 0.5f;
            _maxShootLength = 4;
            _maxBaseHalfWidth = 0.25f;
            _perceptionAngle = 0.6f;
            _perceptionRadius = 4f;
            _occupancyRadius = 1f;
            _lateralAngle = 0.6f;
            _fullExposure = 1f;
            _penumbraA = 1f;
            _penumbraB = 2f;
            _optimalGrowthWeight = 1f;
            _tropismWeight = 1f;
            _tropism = Vector2.Zero;
            _minLeafRatioCutoff = 0f;
            _leafRatioOffset = 0f;
            _position = level.controller.worldMouse;
            _layerDepth = 0.1f;
            _leafMaterialUID = "default";
            _barkMaterialUID = "default";
        }

        public EditorTreeActor(EditorLevel level, XElement data)
            : base(level, data)
        {
            _angle = Loader.loadFloat(data.Attribute("angle"), 0f);
            _seed = Loader.loadInt(data.Attribute("seed"), 12345);
            _age = Loader.loadFloat(data.Attribute("age"), 0f);
            _internodeHalfLength = Loader.loadFloat(data.Attribute("internode_half_length"), 0.5f);
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
            _minLeafRatioCutoff = Loader.loadFloat(data.Attribute("min_leaf_ratio_cutoff"), 0f);
            _leafRatioOffset = Loader.loadFloat(data.Attribute("leaf_ratio_offset"), 0f);
            _position = Loader.loadVector2(data.Attribute("position"), Vector2.Zero);
            _leafMaterialUID = Loader.loadString(data.Attribute("leaf_material_uid"), "default");
            _barkMaterialUID = Loader.loadString(data.Attribute("bark_material_uid"), "default");
        }

        public void rotate(Vector2 anchorPoint, float increment)
        {
            _angle += increment;
        }

        public override void handleSelectedClick(System.Windows.Forms.MouseButtons button)
        {
            deselect();
        }

        public override bool handleUnselectedClick(System.Windows.Forms.MouseButtons button)
        {
            if (button == System.Windows.Forms.MouseButtons.Left)
            {
                return hitTest(_level.controller.worldMouse, (results) =>
                    {
                        if (results.Count == 1 && results[0] == this)
                        {
                            if (_level.controller.shift)
                            {
                                EditorTreeActor copy = (EditorTreeActor)clone();
                                copy.select();
                            }
                            else
                            {
                                select();
                            }
                            return true;
                        }
                        return false;
                    });
            }
            else if (button == System.Windows.Forms.MouseButtons.Right)
            {
                return hitTest(_level.controller.worldMouse, (results) =>
                {
                    if (results.Count == 1)
                    {
                        _level.controller.openActorProperties(results[0]);
                        return true;
                    }
                    return false;
                });
            }
            return false;
        }

        public override bool hitTest(Vector2 testPoint, HitTestCallback callback)
        {
            List<IActorComponent> results = new List<IActorComponent>();

            // Hit test box
            Vector2 relative = new Vector2(_internodeHalfLength / 2f, 0);
            Matrix offset = Matrix.CreateTranslation(new Vector3(relative, 0)) * Matrix.CreateRotationZ(_angle);
            if (_level.controller.hitTestBox(testPoint, _position + Vector2.Transform(relative, offset), _internodeHalfLength, _maxBaseHalfWidth, _angle))
            {
                results.Add(this);
                return callback(results);
            }

            return false;
        }

        public override EditorActor clone()
        {
            XElement data = this.data;
            data.SetAttributeValue("id", _level.controller.getUnusedActorID());
            EditorTreeActor copy = new EditorTreeActor(_level, data);
            _level.addActor(copy);
            return copy;
        }

        public override void update()
        {
            Vector2 worldDelta = _level.controller.worldMouse - _level.controller.oldWorldMouse;
            float angleIncrement = _level.controller.shift ? 0.00005f : 0.0005f;
            float sizeIncrement = _level.controller.shift ? 0.0001f : 0.001f;

            if (selected)
            {
                if (!_level.controller.ctrl)
                {
                    _position += worldDelta;
                }

                if (_level.controller.isKeyHeld(Keys.Q))
                    rotate(_level.controller.worldMouse, -angleIncrement);
                if (_level.controller.isKeyHeld(Keys.E))
                    rotate(_level.controller.worldMouse, angleIncrement);

                if (_level.controller.isKeyHeld(Keys.A))
                    _maxBaseHalfWidth = Math.Max(0.25f, _maxBaseHalfWidth + sizeIncrement);
                if (_level.controller.isKeyHeld(Keys.D))
                    _maxBaseHalfWidth = Math.Max(0.25f, _maxBaseHalfWidth - sizeIncrement);

                if (_level.controller.isKeyHeld(Keys.W))
                    _internodeHalfLength = Math.Max(0.25f, _internodeHalfLength + sizeIncrement);
                if (_level.controller.isKeyHeld(Keys.S))
                    _internodeHalfLength = Math.Max(0.25f, _internodeHalfLength - sizeIncrement);

                if (_level.controller.isKeyPressed(Keys.Escape))
                    deselect();
                else if (_level.controller.isKeyPressed(Keys.Delete))
                    delete();
            }
        }

        public override void draw()
        {
            // Base
            Vector2 relative = new Vector2(_internodeHalfLength / 2f, 0);
            Matrix offset = Matrix.CreateTranslation(new Vector3(relative, 0)) * Matrix.CreateRotationZ(_angle);
            _level.controller.view.drawBox(_position + Vector2.Transform(relative, offset), _internodeHalfLength, _maxBaseHalfWidth, _angle, Color.Teal, _layerDepth);

            // Tropism
            _level.controller.view.drawLine(_position, _position + _tropism, Color.DarkGray, _layerDepth - 0.0001f);
            _level.controller.view.drawPoint(_position + _tropism, Color.Gray, _layerDepth - 0.0001f);
        }
    }
}
