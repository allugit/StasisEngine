using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework.Graphics;

namespace StasisCore.Models
{
    public class WorldMap
    {
        protected Texture2D _texture;
        protected List<LevelIcon> _levelIcons;
        protected List<WorldPath> _worldPaths;

        virtual public Texture2D texture { get { return _texture; } }
        virtual public List<LevelIcon> levelIcons { get { return _levelIcons; } }
        virtual public List<WorldPath> worldPaths { get { return _worldPaths; } }

        public WorldMap()
        {
            _levelIcons = new List<LevelIcon>();
            _worldPaths = new List<WorldPath>();
        }

        public WorldMap(XElement data)
        {
            _levelIcons = new List<LevelIcon>();
            _worldPaths = new List<WorldPath>();
            _texture = ResourceManager.getTexture(Loader.loadString(data.Attribute("texture_uid"), "world_map"));
            createLevelIcons(data);
            createWorldPaths(data);
        }

        virtual public void createLevelIcons(XElement data)
        {
            foreach (XElement levelIconData in data.Elements("LevelIcon"))
                _levelIcons.Add(new LevelIcon(levelIconData));
        }

        virtual public void createWorldPaths(XElement data)
        {
            foreach (XElement worldPathData in data.Elements("WorldPath"))
                _worldPaths.Add(new WorldPath(worldPathData));
        }
    }
}
