using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace DefaultNamespace
{
    public class GridCell : MonoBehaviour
    {

        public int terrainType;
        public AbstractCharacter occupant;
        public List<int> modifiers = new List<int>();

        public List<GridCell> neighbors = new List<GridCell>(4);
        //0 - N
        //1 - E
        //2 - S
        //3 - W;

        public GridCell()
        {
            terrainType = 0;
            occupant = null;
        }
        
        public GridCell getNorth()
        {
            return neighbors[0];
        }
        public GridCell getEast()
        {
            return neighbors[1];
        }

        public GridCell getWest()
        {
            return neighbors[3];
        }

        public GridCell getSouth()
        {
            return neighbors[2];
        }
        
    }
}