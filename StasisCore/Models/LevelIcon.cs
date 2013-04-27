using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StasisCore.Models
{
    public enum LevelIconState
    {
        Undiscovered,
        Unfinished,
        Finished
    }

    public class LevelIcon
    {
        protected Vector2 _position;
        protected string _title;
        protected string _description;
        protected Texture2D _unfinishedIcon;
        protected Texture2D _finishedIcon;
        protected string _levelUID;
        protected int _id;
        protected LevelIconState _state;

        public Vector2 position { get { return _position; } set { _position = value; } }
        virtual public string title { get { return _title; } set { _title = value; } }
        virtual public string description { get { return _description; } set { _description = value; } }
        virtual public Texture2D unfinishedIcon { get { return _unfinishedIcon; } set { _unfinishedIcon = value; } }
        virtual public Texture2D finishedIcon { get { return _finishedIcon; } set { _finishedIcon = value; } }
        public string levelUID { get { return _levelUID; } set { _levelUID = value; } }
        public int id { get { return _id; } }
        virtual public LevelIconState state { get { return _state; } set { _state = value; } }

        public LevelIcon(Vector2 position, string title, string description, Texture2D unfinishedIcon, Texture2D finishedIcon, string levelUID, int id)
        {
            _title = title;
            _description = description;
            _position = position;
            _unfinishedIcon = unfinishedIcon;
            _finishedIcon = finishedIcon;
            _levelUID = levelUID;
            _id = id;
        }

        public LevelIcon(XElement data)
        {
            string unfinishedIconUID = Loader.loadString(data.Attribute("unfinished_icon_uid"), "unfinished_level_icon");
            string finishedIconUID = Loader.loadString(data.Attribute("finished_icon_uid"), "finished_level_icon");
            
            _position = Loader.loadVector2(data.Attribute("position"), Vector2.Zero);
            _title = Loader.loadString(data.Attribute("title"), "");
            _description = Loader.loadString(data.Attribute("description"), "");
            _unfinishedIcon = ResourceManager.getTexture(unfinishedIconUID);
            _finishedIcon = ResourceManager.getTexture(finishedIconUID);
            _levelUID = data.Attribute("level_uid").Value;
            _id = int.Parse(data.Attribute("id").Value);
        }
    }
}
