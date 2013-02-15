using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace StasisGame
{
    public class MarkerCompetition
    {
        public float distanceToBud;
        public BudType budType;
        public Metamer metamer;
        public MarkerCompetition(BudType budType, float distanceToBud, Metamer metamer)
        {
            this.budType = budType;
            this.distanceToBud = distanceToBud;
            this.metamer = metamer;
        }
    }

    public class MetamerMarker
    {
        public MarkerCell cell;
        public Vector2 point;
        public MarkerCompetition competition;

        public MetamerMarker(MarkerCell cell, Vector2 point)
        {
            this.cell = cell;
            this.point = point;
        }

        // addCompetition
        public void addCompetition(BudType budType, float distanceToBud, Metamer metamer)
        {
            if (competition == null)
                competition = new MarkerCompetition(budType, distanceToBud, metamer);
            else if (competition.budType == budType && competition.distanceToBud > distanceToBud)
            {
                competition.distanceToBud = distanceToBud;
                competition.metamer = metamer;
            }
        }
    }
}
