using System;
using System.Collections.Generic;
using StasisGame.Systems;

namespace StasisGame
{
    public class MarkerCell
    {
        public int i;
        public int j;
        public List<MetamerMarker> markers;
        public float shadowValue;

        public MarkerCell(int i, int j)
        {
            this.i = i;
            this.j = j;
            markers = new List<MetamerMarker>(TreeSystem.MARKERS_PER_CELL);
        }

        public void addMarkerCompetition(int markerIndex, BudType budType, float distanceToBud, Metamer metamer)
        {
            markers[markerIndex].addCompetition(budType, distanceToBud, metamer);
        }

        public void clearMarkerCompetition()
        {
            for (int n = 0; n < markers.Count; n++)
                markers[n].competition = null;
        }

        public List<MetamerMarker> getAssociatedMarkers(BudType budType, Metamer metamer)
        {
            List<MetamerMarker> associatedMarkers = new List<MetamerMarker>();
            for (int n = 0; n < markers.Count; n++)
            {
                if (markers[n].competition != null && markers[n].competition.budType == budType && markers[n].competition.metamer == metamer)
                    associatedMarkers.Add(markers[n]);
            }
            return associatedMarkers;
        }
    }
}
