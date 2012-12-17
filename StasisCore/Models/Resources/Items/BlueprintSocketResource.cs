using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisCore.Models
{
    public class BlueprintSocketResource
    {
        private BlueprintScrapItemResource _scrapA;
        private BlueprintScrapItemResource _scrapB;
        private Vector2 _relativePoint;
        private BlueprintSocketResource _opposingSocket;

        public BlueprintScrapItemResource scrapA { get { return _scrapA; } }
        public BlueprintScrapItemResource scrapB { get { return _scrapB; } }
        public Vector2 relativePoint { get { return _relativePoint; } }
        public BlueprintSocketResource opposingSocket { get { return _opposingSocket; } set { _opposingSocket = value; } }

        public BlueprintSocketResource(BlueprintScrapItemResource scrapA, BlueprintScrapItemResource scrapB)
        {
            _scrapA = scrapA;
            _scrapB = scrapB;
            _relativePoint = scrapB.craftingPosition - scrapA.craftingPosition;
        }
    }
}
