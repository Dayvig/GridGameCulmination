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
        public SpriteRenderer cell;
        public Color movementTintColor;
        public Model_Game gameModel;
        public bool isMovementSelectable;
        public bool isAttackSelectable;
        public bool isOptimal;
        public GameManager gameManager;

        
        public List<GridCell> neighbors = new List<GridCell>(8);
        //0 - N
        //1 - E
        //2 - S
        //3 - W;
        //4 - NE
        //5 - NW
        //6 - SE
        //7 - SW

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
        
        public GridCell getNorthEast()
        {
            return neighbors[4];
        }
        public GridCell getNorthWest()
        {
            return neighbors[5];
        }

        public GridCell getSouthEast()
        {
            return neighbors[6];
        }

        public GridCell getSouthWest()
        {
            return neighbors[7];
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
        
        public void setNorthEast(GridCell g)
        {
            neighbors[4] = g;
        }
        public void setNorthWest(GridCell g)
        {
            neighbors[5] = g;
        }
        public void setSouthEast(GridCell g)
        {
            neighbors[6] = g;
        }
        public void setSouthWest(GridCell g)
        {
            neighbors[7] = g;
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
            if (terrainType == 0)
            {
                cell.enabled = false;
            }
        }
        void OnMouseEnter()
        {
            if (cell.enabled)
            {
                hover.SetActive(true);
            }
            //cell is hovered
        }
        
        void OnMouseExit()
        {
            if (cell.enabled)
            {
                hover.SetActive(false);
            }
            //cell is no longer hovered
        }

        public void OnMouseUp()
        {
            switch (gameManager.currentState)
            {
                case GameManager.GameState.Neutral:
                    selectCell(gameManager.currentTurn);
                    break;
                
                case GameManager.GameState.CharacterMovement:
                    if (isMovementSelectable)
                    {
                        if (manager.selectedCharacterBehavior.currentCell != this)
                        {
                            moveCharacterToCell(this, manager.selectedCharacterBehavior);
                        }
                        else
                        {
                            gameManager.currentState = GameManager.GameState.CharacterAttacking;
                            manager.selectedCharacterBehavior.currentMoves = 0;
                            manager.selectedCharacterBehavior.onSelect();
                            manager.MasterGrid.WipeMovement();
                        }
                    }
                    break;
                case GameManager.GameState.CharacterAttacking:
                    if (isAttackSelectable)
                    {
                        if (manager.selectedCharacterBehavior.currentCell != this)
                        {
                            if (occupant != null)
                            {
                                BaseBehavior target = occupant.GetComponent<BaseBehavior>();
                                if (isOptimal)
                                {
                                    manager.selectedCharacterBehavior.onAttack(target, true);
                                }
                                else
                                {
                                    manager.selectedCharacterBehavior.onAttack(target, false);
                                }

                            }
                            else
                            {
                                manager.DeselectAll();
                            }
                        }
                        else
                        {
                            if (manager.selectedCharacterBehavior.currentSelectedAttack.targeting == AbstractAttack.AttackType.SELF)
                            {
                                manager.selectedCharacterBehavior.onAttack(manager.selectedCharacterBehavior, false);
                            }
                            else
                            {
                                manager.selectedCharacterBehavior.endTurn();
                                gameManager.currentState = GameManager.GameState.Neutral;
                            }
                        }
                    }
                    break;

                default:
                    Debug.Log("Something went wrong");
                    break;
            }
        }

        public void selectCell(GameManager.Player currentPlayer)
        {
            //If there is no cell selected
            if (manager.selectedCell == null)
            {
                //Selects the cell if occupied and of the correct owner for the turn
                if (occupant != null)
                {
                    BaseBehavior toSelect = occupant.GetComponent<BaseBehavior>();
                    if (toSelect.owner == currentPlayer && (toSelect.currentMoves > 0 || toSelect.currentAttacks > 0))
                    {
                        manager.selectedCell = this;
                        manager.selectedCell.selector.SetActive(true);
                        manager.selectedCharacter = manager.selectedCell.occupant;
                        manager.selectedCharacterBehavior = manager.selectedCharacter.GetComponent<BaseBehavior>();
                        manager.selectedCharacterBehavior.onSelect();
                    }
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
                    BaseBehavior toSelect = occupant.GetComponent<BaseBehavior>();
                    if (toSelect.owner == currentPlayer)
                    {
                        manager.selectedCell = this;
                        manager.selectedCell.selector.SetActive(true);
                        manager.selectedCharacter = manager.selectedCell.occupant;
                        manager.selectedCharacterBehavior = manager.selectedCharacter.GetComponent<BaseBehavior>();
                        manager.selectedCharacterBehavior.onSelect();
                    }
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
            character.onMove();
            manager.DeselectAll();
            gameManager.currentState = GameManager.GameState.Neutral;
        }
        
        public void Deselect()
        {
            if (manager.selectedCell != null)
            {
                manager.selectedCell.selector.SetActive(false);
            }
            tint.gameObject.SetActive(false);
            cannotMoveTo();
            isNotAttackable();

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
        
        public void isAttackable()
        {
            isAttackSelectable = true;
            if (isOptimal)
            {
                tint.color = gameModel.optimalTint;
            }
            else
            {
                tint.color = gameModel.attackTint;
            }

            tint.gameObject.SetActive(true);
        }
        
        public void isAttackable(bool isBuff)
        {
            isAttackSelectable = true;
            if (isBuff)
                tint.color = gameModel.buffTint;

            tint.gameObject.SetActive(true);
        }

        public void isNotAttackable()
        {
            isAttackSelectable = false;
            isOptimal = false;
            tint.gameObject.SetActive(false);
        }

        public void cannotMoveTo()
        {
            isMovementSelectable = false;
            tint.gameObject.SetActive(false);
        }

        public bool checkForOutOfActions()
        {
            Debug.LogFormat(manager.selectedCharacterBehavior.currentMoves + "|" + manager.selectedCharacterBehavior.currentAttacks);
            return (manager.selectedCharacterBehavior.currentMoves <= 0 &&
                   manager.selectedCharacterBehavior.currentAttacks <= 0);
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