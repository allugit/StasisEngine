using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Xna.Framework;
using StasisCore.Resources;

namespace StasisCore.Models
{
    public class BlueprintSocket
    {
        private BlueprintScrap _scrapA;
        private BlueprintScrap _scrapB;
        private BlueprintSocket _opposingSocket;
        private Vector2 _relativePoint;

        public BlueprintScrap scrapA { get { return _scrapA; } }
        public BlueprintScrap scrapB { get { return _scrapB; } }
        public BlueprintSocket opposingSocket { get { return _opposingSocket; } set { _opposingSocket = value; } }
        public Vector2 relativePoint { get { return _relativePoint; } set { _relativePoint = value; } }

        // Create new
        public BlueprintSocket(BlueprintScrap scrapA, BlueprintScrap scrapB, Vector2 relativePoint)
        {
            _relativePoint = relativePoint;
            _scrapA = scrapA;
            _scrapB = scrapB;
        }

        // Create from xml
        public BlueprintSocket(XElement data, BlueprintScrap scrapA, BlueprintScrap scrapB)
        {
            _relativePoint = XmlLoadHelper.getVector2(data.Attribute("relative_point").Value);
            _scrapA = scrapA;
            _scrapB = scrapB;
        }
    }
}
