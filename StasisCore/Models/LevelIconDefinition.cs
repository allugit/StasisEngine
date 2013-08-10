using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StasisCore.Models
{
    public class LevelIconDefinition
    {
        WorldMapDefinition _definition;
        private string _uid;
        private string _levelUid;
        private string _finishedTextureUid;
        private string _unfinishedTextureUid;
        private string _title;
        private string _description;
        private Vector2 _position;
        private Texture2D _finishedTexture;
        private Texture2D _unfinishedTexture;

        public WorldMapDefinition definition { get { return _definition; } }
        public string uid { get { return _uid; } set { _uid = value; } }
        public string levelUid { get { return _levelUid; } set { _levelUid = value; } }
        public string finishedTextureUid { get { return _finishedTextureUid; } set { _finishedTextureUid = value; } }
        public string unfinishedTextureUid { get { return _unfinishedTextureUid; } set { _unfinishedTextureUid = value; } }
        public string title { get { return _title; } set { _title = value; } }
        public string description { get { return _description; } set { _description = value; } }
        public Vector2 position { get { return _position; } set { _position = value; } }
        public Texture2D finishedTexture { get { return _finishedTexture; } set { _finishedTexture = value; } }
        public Texture2D unfinishedTexture { get { return _unfinishedTexture; } set { _unfinishedTexture = value; } }

        public LevelIconDefinition(WorldMapDefinition definition, string uid, string levelUid, string finishedTextureUid, string unfinishedTextureUid, string title, string description, Vector2 position)
        {
            _definition = definition;
            _uid = uid;
            _levelUid = levelUid;
            _finishedTextureUid = finishedTextureUid;
            _unfinishedTextureUid = unfinishedTextureUid;
            _title = title;
            _description = description;
            _position = position;
        }
    }
}
