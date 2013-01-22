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
        private bool _satisfied;

        public BlueprintScrap scrapA { get { return _scrapA; } }
        public BlueprintScrap scrapB { get { return _scrapB; } }
        public BlueprintSocket opposingSocket { get { return _opposingSocket; } set { _opposingSocket = value; } }
        public Vector2 relativePoint { get { return _relativePoint; } set { _relativePoint = value; } }
        public bool satisfied { get { return _satisfied; } set { _satisfied = value; } }

        public XElement data
        {
            get
            {
                return new XElement("Socket",
                    new XAttribute("scrap_a_uid", _scrapA.uid),
                    new XAttribute("scrap_b_uid", _scrapB.uid),
                    new XAttribute("relative_point", _relativePoint));
            }
        }

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
            _relativePoint = Loader.loadVector2(data.Attribute("relative_point"), Vector2.Zero);
            _scrapA = scrapA;
            _scrapB = scrapB;
        }

        // test
        public void test(float distanceTolerance = 10f)
        {
            if (_satisfied && _opposingSocket.satisfied)
                return;

            // Check socket
            Vector2 transformedRelativePointA = _scrapA.currentCraftPosition + Vector2.Transform(_relativePoint, _scrapA.rotationMatrix);
            Vector2 difference = _scrapB.currentCraftPosition - transformedRelativePointA;
            float distance = difference.Length();
            _satisfied = distance <= distanceTolerance;

            if (_satisfied && _opposingSocket.satisfied)
            {
                // Snap to ideal position before connecting
                _scrapB.move(-difference);
                _scrapB.rotate(_scrapA.currentCraftAngle - _scrapB.currentCraftAngle);

                // Form a connection
                _scrapA.connectScrap(_scrapB);
                _scrapB.connectScrap(_scrapA);
            }
        }
    }
}
