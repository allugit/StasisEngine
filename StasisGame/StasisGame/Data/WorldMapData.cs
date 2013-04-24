using System;
using System.Collections.Generic;
using System.Xml.Linq;
using StasisCore;

namespace StasisGame.Data
{
    public class WorldMapData
    {
        private string _worldMapUID;
        private List<LevelIconData> _levelIconData;
        private List<WorldPathData> _worldPathData;

        public List<LevelIconData> levelIconData { get { return _levelIconData; } }
        public List<WorldPathData> worldPathData { get { return _worldPathData; } }

        public XElement data
        {
            get
            {
                List<XElement> levelIconDatas = new List<XElement>();
                List<XElement> worldPathDatas = new List<XElement>();

                foreach (LevelIconData levelIconData in _levelIconData)
                    levelIconDatas.Add(levelIconData.data);
                foreach (WorldPathData worldPathData in _worldPathData)
                    worldPathDatas.Add(worldPathData.data);

                return new XElement("WorldMapData",
                    new XAttribute("world_map_uid", _worldMapUID),
                    levelIconDatas,
                    worldPathDatas);
            }
        }

        public WorldMapData(string worldMapUID)
        {
            _worldMapUID = worldMapUID;
            _levelIconData = new List<LevelIconData>();
            _worldPathData = new List<WorldPathData>();
        }

        public WorldMapData(XElement data)
        {
            _worldMapUID = data.Attribute("world_map_uid").Value;
            _levelIconData = new List<LevelIconData>();
            _worldPathData = new List<WorldPathData>();

            foreach (XElement childData in data.Elements("LevelIconData"))
                _levelIconData.Add(new LevelIconData(childData));
            foreach (XElement childData in data.Elements("WorldPathData"))
                _worldPathData.Add(new WorldPathData(childData));
        }
    }
}
