using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using StasisCore;
using StasisCore.Models;

namespace StasisEditor.Models
{
    public class EditorLevelIcon : LevelIcon, IWorldControl
    {
        private string _unfinishedIconUID;
        private string _finishedIconUID;

        public string unfinishedIconUID { get { return _unfinishedIconUID; } set { _unfinishedIconUID = value; } }
        public string finishedIconUID { get { return _finishedIconUID; } set { _finishedIconUID = value; } }

        public XElement data
        {
            get
            {
                return new XElement("LevelIcon",
                    new XAttribute("position", _position),
                    new XAttribute("unfinished_icon_uid", _unfinishedIconUID),
                    new XAttribute("finished_icon_uid", _finishedIconUID),
                    new XAttribute("level_uid", _levelUID));
            }
        }

        public EditorLevelIcon(Vector2 position, string unfinishedIconUID, string finishedIconUID, string levelUID)
            : base(position, ResourceManager.getTexture(unfinishedIconUID), ResourceManager.getTexture(finishedIconUID), levelUID)
        {
            _unfinishedIconUID = unfinishedIconUID;
            _finishedIconUID = finishedIconUID;
        }

        public EditorLevelIcon(XElement data) : base(data)
        {
            _unfinishedIconUID = Loader.loadString(data.Attribute("unfinished_icon_uid"), "unfinished_level_icon");
            _finishedIconUID = Loader.loadString(data.Attribute("finished_icon_uid"), "finished_level_icon");
        }

        public void delete()
        {
            throw new NotImplementedException();
        }
    }
}
