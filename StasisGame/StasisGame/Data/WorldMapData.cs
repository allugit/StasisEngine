using System;
using System.Collections.Generic;
using System.Xml.Linq;

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
            throw new NotImplementedException();
        }
    }
}
