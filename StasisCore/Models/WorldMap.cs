using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StasisCore.Models
{
    public class WorldMap
    {
        protected string _uid;
        protected string _textureUID;
        protected Texture2D _texture;
        protected List<LevelIcon> _levelIcons;
        protected List<WorldPath> _worldPaths;
        protected Vector2 _halfTextureSize;

        virtual public string uid { get { return _uid; } set { _uid = value; } }
        virtual public string textureUID { get { return _textureUID; } set { _textureUID = value; } }
        virtual public Texture2D texture { get { return _texture; } }
        virtual public List<LevelIcon> levelIcons { get { return _levelIcons; } }
        virtual public List<WorldPath> worldPaths { get { return _worldPaths; } }
        virtual public Vector2 halfTextureSize { get { return _halfTextureSize; } }

        public WorldMap()
        {
            _levelIcons = new List<LevelIcon>();
            _worldPaths = new List<WorldPath>();
        }

        public WorldMap(XElement data)
        {
            _uid = data.Attribute("uid").Value;
            _levelIcons = new List<LevelIcon>();
            _worldPaths = new List<WorldPath>();
            _texture = ResourceManager.getTexture(Loader.loadString(data.Attribute("texture_uid"), "world_map"));
            _halfTextureSize = new Vector2(_texture.Width, _texture.Height) / 2f;
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

        public LevelIcon getLevelIcon(int id)
        {
            foreach (LevelIcon levelIcon in _levelIcons)
            {
                if (levelIcon.id == id)
                    return levelIcon;
            }
            return null;
        }

        public WorldPath getWorldPath(int id)
        {
            foreach (WorldPath worldPath in _worldPaths)
            {
                if (worldPath.id == id)
                    return worldPath;
            }
            return null;
        }
    }
}
