using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StasisCore.Models
{
    public class WorldMapDefinition
    {
        private string _uid;
        private Texture2D _texture;
        private string _textureUid;
        private Vector2 _position;
        private List<LevelIconDefinition> _levelIconDefinitions;
        private List<LevelPathDefinition> _levelPathDefinitions;

        public string uid { get { return _uid; } set { _uid = value; } }
        public Texture2D texture { get { return _texture; } set { _texture = value; } }
        public string textureUid { get { return _textureUid; } set { _textureUid = value; } }
        public Vector2 position { get { return _position; } set { _position = value; } }
        public List<LevelIconDefinition> levelIconDefinitions { get { return _levelIconDefinitions; } }
        public List<LevelPathDefinition> levelPathDefinitions { get { return _levelPathDefinitions; } }

        public WorldMapDefinition(string uid, string textureUid, Vector2 position)
        {
            _uid = uid;
            _textureUid = textureUid;
            _position = position;
            _levelIconDefinitions = new List<LevelIconDefinition>();
            _levelPathDefinitions = new List<LevelPathDefinition>();
        }
    }
}
