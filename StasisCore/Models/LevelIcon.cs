using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace StasisCore.Models
{
    public class LevelIcon
    {
        protected Vector2 _position;
        protected Texture2D _unfinishedIcon;
        protected Texture2D _finishedIcon;
        protected string _levelUID;

        public Vector2 position { get { return _position; } set { _position = value; } }
        public Texture2D unfinishedIcon { get { return _unfinishedIcon; } set { _unfinishedIcon = value; } }
        public Texture2D finishedIcon { get { return _finishedIcon; } set { _finishedIcon = value; } }
        public string levelUID { get { return _levelUID; } set { _levelUID = value; } }

        public LevelIcon(Vector2 position, Texture2D unfinishedIcon, Texture2D finishedIcon, string levelUID)
        {
            _position = position;
            _unfinishedIcon = unfinishedIcon;
            _finishedIcon = finishedIcon;
            _levelUID = levelUID;
        }

        public LevelIcon(XElement data)
        {
            string unfinishedIconUID = Loader.loadString(data.Attribute("unfinished_icon_uid"), "unfinished_level_icon");
            string finishedIconUID = Loader.loadString(data.Attribute("finished_icon_uid"), "finished_level_icon");
            _position = Loader.loadVector2(data.Attribute("position"), Vector2.Zero);
            _unfinishedIcon = ResourceManager.getTexture(unfinishedIconUID);
            _finishedIcon = ResourceManager.getTexture(finishedIconUID);
            _levelUID = data.Attribute("level_uid").Value;
        }
    }
}
