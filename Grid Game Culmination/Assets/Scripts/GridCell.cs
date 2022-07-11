using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace DefaultNamespace
{
    public class GridCell : MonoBehaviour
    {

        public int terrainType;
        public GameObject occupant;
        public List<int> modifiers = new List<int>();
        public GameObject hover;
        public GameObject selector;
        public GridManager manager;
        
        public List<GridCell> neighbors = new List<GridCell>(4);
        //0 - N
        //1 - E
        //2 - S
        //3 - W;

        //Constructor and Access methods
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

        public void setNorth(GridCell g)
        {
            neighbors[0] = g;
        }
        public void setSouth(GridCell g)
        {
            neighbors[2] = g;
        }
        public void setEast(GridCell g)
        {
            neighbors[1] = g;
        }
        public void setWest(GridCell g)
        {
            neighbors[3] = g;
        }
        
        
        // Regular Methods
        
        Ray ray;
        RaycastHit hit;

        void Start()
        {
            manager = GameObject.Find("GameManager").GetComponent<GridManager>();
            manager.selectedCell = null;
            hover.SetActive(false);
            selector.SetActive(false);
        }
        void OnMouseEnter()
        {
            hover.SetActive(true);
            //cell is hovered
        }
        
        void OnMouseExit()
        {
            hover.SetActive(false);
            //cell is no longer hovered
        }

        public void OnMouseUp()
        {
            //If there is no cell selected
            if (manager.selectedCell == null)
            {
                //Selects the cell if occupied
                if (occupant != null)
               {
                        manager.selectedCell = this;
                        manager.selectedCell.selector.SetActive(true);
                        manager.selectedCharacter = manager.selectedCell.occupant;
                }
                else
                {
                    manager.selectedCharacter = null;
                }
            }
            //If selecting the same cell
            else if (manager.selectedCell.Equals(this))
            {
                //sets the selected cell and character to null
                manager.selectedCell.selector.SetActive(false);
                manager.selectedCell = null;
                manager.selectedCharacter = null;
            }
            // if selecting a different cell
            else
            {
                //sets previous cell's selector off
                manager.selectedCell.selector.SetActive(false);
                //selects the new cell if occupied
                if (occupant != null){
                    manager.selectedCell = this;
                    manager.selectedCell.selector.SetActive(true);
                    manager.selectedCharacter = manager.selectedCell.occupant;
                }
                else
                {
                    manager.selectedCharacter = null;
                    manager.selectedCell = null;
                }
            }
        }

        public void Deselect()
        {
            if (manager.selectedCell != null)
            {
                manager.selectedCell.selector.SetActive(false);
            }
            
            manager.selectedCharacter = null;
            manager.selectedCell = null;
        }

        public void Update()
        {
            if (occupant != null)
            {
                occupant.transform.position = new Vector3(this.gameObject.transform.position.x,
                    gameObject.transform.position.y, gameObject.transform.position.z - 0.1f);
            }
        }
    }
}