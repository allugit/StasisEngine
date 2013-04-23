using System;
using System.Collections.Generic;
using System.Xml.Linq;
using StasisGame.Systems;

namespace StasisGame.Data
{
    public class PlayerData
    {
        private string _playerUID;
        private PlayerSystem _playerSystem;
        private CurrentLocation _currentLocation;
        private List<WorldMapData> _worldMapData;

        public PlayerData(XElement data)
        {
            throw new NotImplementedException();
        }
    }
}
