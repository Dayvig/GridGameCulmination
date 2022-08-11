using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
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
        public SpriteRenderer tint;
        public Color movementTintColor;
        public Model_Game gameModel;
        public bool isMovementSelectable;
        public GameManager gameManager;

        
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
            gameModel = GameObject.Find("GameModel").GetComponent<Model_Game>();
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            manager.selectedCell = null;
            hover.SetActive(false);
            selector.SetActive(false);
            tint.gameObject.SetActive(false);
            isMovementSelectable = false;
            movementTintColor = gameModel.movementTint;
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
            switch (gameManager.currentState)
            {
                case GameManager.GameState.Neutral:
                    selectCell();
                    break;
                
                case GameManager.GameState.CharacterMovement:
                    if (isMovementSelectable)
                    {
                        moveCharacterToCell(this, manager.selectedCharacterBehavior);
                    }
                    break;
                
                default:
                    Debug.Log("Something went wrong");
                    break;
            }
        }

        public void selectCell()
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
                    manager.selectedCharacterBehavior = manager.selectedCharacter.GetComponent<BaseBehavior>();
                    manager.selectedCharacterBehavior.onSelect();
                }
                else
                {
                    Deselect();
                }
            }
            //If selecting the same cell
            else if (manager.selectedCell.Equals(this))
            {
                //sets the selected cell and character to null
                Deselect();
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
                    manager.selectedCharacterBehavior = manager.selectedCharacter.GetComponent<BaseBehavior>();
                    manager.selectedCharacterBehavior.onSelect();
                }
                else
                {
                    Deselect();
                }
            }

        }

        public void moveCharacterToCell(GridCell moveTo, BaseBehavior character)
        {
            character.currentCell.occupant = null;
            character.currentCell = moveTo;
            moveTo.occupant = character.gameObject;
            character.transform.position = moveTo.transform.position;
            gameManager.currentState = GameManager.GameState.Neutral;
            manager.DeselectAll();
        }

        public void Deselect()
        {
            if (manager.selectedCell != null)
            {
                manager.selectedCell.selector.SetActive(false);
            }
            tint.gameObject.SetActive(false);
            cannotMoveTo();

            manager.selectedCharacter = null;
            manager.selectedCell = null;
            manager.selectedCharacterBehavior = null;
        }

        public void canMoveTo()
        {
            isMovementSelectable = true;
            tint.color = gameModel.movementTint;
            tint.gameObject.SetActive(true);
        }

        public void cannotMoveTo()
        {
            isMovementSelectable = false;
            tint.gameObject.SetActive(false);
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