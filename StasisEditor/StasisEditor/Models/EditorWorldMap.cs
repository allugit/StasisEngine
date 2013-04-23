using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Graphics;
using StasisCore;
using StasisCore.Models;

namespace StasisEditor.Models
{
    public class EditorWorldMap : WorldMap
    {
        private string _uid;
        private string _textureUID;

        public string uid { get { return _uid; } set { _uid = value; } }
        public string textureUID { get { return _textureUID; } set { _textureUID = value; } }

        [Browsable(false)]
        public override Texture2D texture { get { return base.texture; } }
        [Browsable(false)]
        public override List<LevelIcon> levelIcons { get { return base.levelIcons; } }
        [Browsable(false)]
        public override List<WorldPath> worldPaths { get { return base.worldPaths; } }

        [Browsable(false)]
        public XElement data
        {
            get
            {
                List<XElement> levelIconsData = new List<XElement>();
                List<XElement> worldPathsData = new List<XElement>();
                foreach (EditorLevelIcon icon in _levelIcons)
                    levelIconsData.Add(icon.data);
                foreach (EditorWorldPath path in _worldPaths)
                    worldPathsData.Add(path.data);

                XElement d = new XElement("WorldMap",
                    new XAttribute("uid", _uid),
                    new XAttribute("texture_uid", _textureUID),
                    levelIconsData,
                    worldPathsData);
                return d;
            }
        }

        public EditorWorldMap(string uid) : base()
        {
            _uid = uid;
            _textureUID = "world_map";
            _texture = ResourceManager.getTexture(Loader.loadString(data.Attribute("texture_uid"), "world_map"));
        }

        public EditorWorldMap(XElement data) : base(data)
        {
            _uid = Loader.loadString(data.Attribute("uid"), "");
            _textureUID = Loader.loadString(data.Attribute("texture_uid"), "world_map");
        }

        override public void createLevelIcons(XElement data)
        {
            foreach (XElement levelIconData in data.Elements("LevelIcon"))
                _levelIcons.Add(new EditorLevelIcon(this, levelIconData));
        }

        override public void createWorldPaths(XElement data)
        {
            foreach (XElement worldPathData in data.Elements("WorldPath"))
                _worldPaths.Add(new EditorWorldPath(this, worldPathData));
        }

        public override string ToString()
        {
            return _uid;
        }
    }
}
