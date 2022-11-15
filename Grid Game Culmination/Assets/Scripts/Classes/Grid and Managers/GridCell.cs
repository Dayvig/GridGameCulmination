using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Transactions;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace DefaultNamespace
{
    public class GridCell : MonoBehaviour
    {

        public int terrainType;
        public GameObject occupant;
        public List<int> modifiers = new List<int>();
        //0: Has a mine
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
        public UIManager uiManager;
        public int movementCount = 0;
        public bool isNumModified;
        public SpriteRenderer TerrainSprite;
        public bool isHealthPackZone = false;
        public int healthPackCtr = 0;
        public int boostPackCtr = 0;
        public bool isBoostPackZone = false;

        public List<GridCell> neighbors = new List<GridCell>(8);
        //0 - N
        //1 - E
        //2 - s
        //3 - W
        //4 - NE
        //5 - NW
        //6 - SW
        //7 - SE

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
            return neighbors[7];
        }

        public GridCell getSouthWest()
        {
            return neighbors[6];
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
            neighbors[7] = g;
        }
        public void setSouthWest(GridCell g)
        {
            neighbors[6] = g;
        }

        public bool isNorthFacing(GridCell origin, GridCell g)
        {
            return (g.Equals(origin.getNorth()) || g.Equals(origin.getNorthEast()) || g.Equals(origin.getNorthWest()));
        }
        
        public bool isEastFacing(GridCell origin, GridCell g)
        {
            return (g.Equals(origin.getEast()) || g.Equals(origin.getNorthEast()) || g.Equals(origin.getSouthEast()));
        }
        
        public bool isWestFacing(GridCell origin, GridCell g)
        {
            return (g.Equals(origin.getWest()) || g.Equals(origin.getNorthWest()) || g.Equals(origin.getSouthWest()));
        }
        
        public bool isSouthFacing(GridCell origin, GridCell g)
        {
            return (g.Equals(origin.getSouth()) || g.Equals(origin.getSouthEast()) || g.Equals(origin.getSouthWest()));
        }

        public GridCell[] allNorthFacing(GridCell origin)
        {
            GridCell[] ret = {origin.getNorth(), origin.getNorthEast(), origin.getNorthWest()};
            return ret;
        }
        public GridCell[] allEastFacing(GridCell origin)
        {
            GridCell[] ret = {origin.getEast(), origin.getNorthEast(), origin.getSouthEast()};
            return ret;
        }
        public GridCell[] allSouthFacing(GridCell origin)
        {
            GridCell[] ret = {origin.getSouth(), origin.getSouthEast(), origin.getSouthWest()};
            return ret;
        }
        public GridCell[] allWestFacing(GridCell origin)
        {
            GridCell[] ret = {origin.getWest(), origin.getNorthWest(), origin.getSouthWest()};
            return ret;
        }
        
        
        // Regular Methods
        
        Ray ray;
        RaycastHit hit;

        void Start()
        {
            manager = GameObject.Find("GameManager").GetComponent<GridManager>();
            gameModel = GameObject.Find("GameModel").GetComponent<Model_Game>();
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            uiManager = GameObject.Find("GameManager").GetComponent<UIManager>();
            manager.selectedCell = null;
            hover.SetActive(false);
            selector.SetActive(false);
            tint.gameObject.SetActive(false);
            isMovementSelectable = false;
            movementTintColor = gameModel.movementTint;
            TerrainSprite.enabled = false;
            switch (terrainType)
            {
                //Empty/hole
                case 0:
                    cell.enabled = false;
                    break;
                //case 1:
                //normal
                //Hills
                case 2:
                    TerrainSprite.enabled = true;
                    TerrainSprite.sprite = gameModel.Terrainsprites[2];
                    break;
                //High Ground
                case 3:
                    TerrainSprite.enabled = true;
                    TerrainSprite.sprite = gameModel.Terrainsprites[3];
                    break;
            }

            if (modifiers.Contains(2))
            {
                TerrainSprite.enabled = true;
                TerrainSprite.sprite = gameModel.Terrainsprites[4];
            }
            else if (modifiers.Contains(3))
            {
                TerrainSprite.enabled = true;
                TerrainSprite.sprite = gameModel.Terrainsprites[5];
            }
            else if (modifiers.Contains(4))
            {
                TerrainSprite.enabled = true;
                TerrainSprite.sprite = gameModel.Terrainsprites[6];
            }
            else if (modifiers.Contains(5))
            {
                TerrainSprite.enabled = true;
                TerrainSprite.sprite = gameModel.Terrainsprites[7];
            }
            else if (modifiers.Contains(7))
            {
                TerrainSprite.enabled = true;
                TerrainSprite.sprite = gameModel.Terrainsprites[8];
            }
            else if (modifiers.Contains(8))
            {
                TerrainSprite.enabled = true;
                TerrainSprite.sprite = gameModel.Terrainsprites[9];
            }
        }
        void OnMouseEnter()
        {
            //cell is hovered
            if (cell.enabled)
            {
                hover.SetActive(true);
                if (manager.selectedCharacterBehavior != null && gameManager.currentState == GameManager.GameState.CharacterAttacking && isAttackSelectable)
                {
                    manager.selectedCharacterBehavior.currentSelectedAttack.showSelectedSquares(this,
                        manager.selectedCharacterBehavior.currentSelectedAttack.targeting ==
                        AbstractAttack.AttackType.ALLY ||
                        manager.selectedCharacterBehavior.currentSelectedAttack.targeting ==
                        AbstractAttack.AttackType.SELF);
                }
            }

            uiManager.toolTipTrigger = true;
            uiManager.hoverCtr = 0;
            uiManager.tooltiptext.text = SetToolText(this);

            if (occupant != null && gameManager.currentState == GameManager.GameState.CharacterAttacking && manager.selectedCharacterBehavior != null)
            {
                BaseBehavior hoveredChar = occupant.GetComponent<BaseBehavior>();
                if (hoveredChar.owner != gameManager.currentTurn)
                {
                    uiManager.dmgtoolTipTrigger = true;
                    uiManager.dmghoverCtr = 0;
                    uiManager.dmgtooltext.text = SetDMGToolText(manager.selectedCharacterBehavior, hoveredChar);
                }
            }
        }

        private String SetDMGToolText(BaseBehavior initiator, BaseBehavior target)
        {
            String tmp = "";
            
            tmp += initiator.calculateDamage(isOptimal ? initiator.currentSelectedAttack.OptimalDamage : initiator.currentSelectedAttack.AttackDamage, target, initiator);
            tmp += " damage.";
            return tmp;
        }

        private String SetToolText(GridCell g)
        {
            String tmp = "";
            switch (terrainType)
            {
                case 0:
                    tmp += "Hole Tile. Cannot be traversed.";
                    break;
                case 1:
                    tmp += "Normal Tile.";
                    break;
                case 2:
                    tmp += "Hill Tile. Costs 2 movement points to enter.";
                    break;
                case 3:
                    tmp +=
                        "High Ground. Costs 2 movement points to enter. Units on high ground deal +1 damage to units not on high ground.";
                    break;
                default:
                    tmp += "What is you doing?";
                    break;
            }

            foreach (int m in modifiers)
            {
                if (m == 0)
                {
                    tmp += "\nMine. Deals damage when stepped on, and Costs 3 movement points to enter.";
                }
                if (m == 2)
                {
                    tmp += "\nHealth Pack. Heals 6 HP. Any health above the maximum will become Overheal which lowers by 1 each turn.";
                }
                if (m == 3)
                {
                    tmp += "\nDepleted Health Pack. Will return in "+(6-healthPackCtr)/2+" turns.";
                }
                if (m == 4)
                {
                    tmp += "\nBoost Pack. Increases your damage by 25% for 4 turns.";
                }
                if (m == 5)
                {
                    tmp += "\nDepleted Boost Pack. Will return in "+(6-boostPackCtr)/2+" turns.";
                }
                if (m == 7)
                {
                    tmp += "\nPlayer 2 Spawn. A ghost of a fallen unit will spawn here when Player 2 has only one unit left.";
                }
                if (m == 8)
                {
                    tmp += "\nPlayer 1 Spawn. A ghost of a fallen unit will spawn here when Player 1 has only one unit left.";
                }
            }

            return tmp;
        }
        
        void OnMouseExit()
        {
            //cell is no longer hovered
            if (cell.enabled)
            {
                hover.SetActive(false);
                if (manager.selectedCharacterBehavior != null && gameManager.currentState == GameManager.GameState.CharacterAttacking && isAttackSelectable)
                {
                    BaseBehavior chara = manager.selectedCharacterBehavior;
                    manager.MasterGrid.WipeAttackingSquares();
                    chara.currentSelectedAttack.showAttackingSquares(chara.currentCell, chara.currentSelectedAttack.AttackRange, chara.currentSelectedAttack.targeting);
                }
            }
            uiManager.toolTipTrigger = false;
            uiManager.hoverCtr = 0;
            uiManager.dmgtoolTipTrigger = false;
            uiManager.dmghoverCtr = 0;
        }

        public void OnMouseUp()
        {
            switch (gameManager.currentState)
            {
                case GameManager.GameState.GameOver:
                    break;
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
                            manager.MasterGrid.WipeMovement();
                            manager.selectedCharacterBehavior.onSelect();
                        }
                    }
                    break;
                case GameManager.GameState.CharacterAttacking:
                    if (isMovementSelectable && manager.selectedCharacterBehavior.specialMovement)
                    {
                        if (manager.selectedCharacterBehavior.currentCell != this)
                        {
                            BaseBehavior character = manager.selectedCharacterBehavior;
                            character.onSpecialMovement();
                            character.currentCell.occupant = null;
                            character.currentCell = this;
                            character.currentCell.occupant = character.gameObject;
                        }
                    }
                    if (isAttackSelectable)
                    {
                        //check if it is a ground targeted ability
                        if (manager.selectedCharacterBehavior.currentSelectedAttack.targeting == AbstractAttack.AttackType.GROUND && manager.selectedCharacterBehavior.currentSelectedAttack is GroundTarget)
                        {
                            manager.selectedCharacterBehavior.onAttackGround(this);
                        }
                        //if not targeting the square the unit is on
                        else if (manager.selectedCharacterBehavior.currentCell != this)
                        {
                            //if targeting a unit
                            if (occupant != null)
                            {
                                BaseBehavior target = occupant.GetComponent<BaseBehavior>();
                                
                                //if the unit is an enemy and the ability targets enemies, attack it.
                                if (target.owner != manager.selectedCharacterBehavior.owner)
                                {
                                    if (manager.selectedCharacterBehavior.currentSelectedAttack.targeting != AbstractAttack.AttackType.ALLY)
                                        manager.selectedCharacterBehavior.onAttack(target, isOptimal);
                                }
                                else
                                {
                                    //if the ability targets allies
                                    if (manager.selectedCharacterBehavior.currentSelectedAttack.targeting == AbstractAttack.AttackType.ALLY) {
                                        manager.selectedCharacterBehavior.onAttack(target, isOptimal);
                                    }
                                    else { manager.DeselectAll(); }
                                }
                            }
                            else { manager.DeselectAll(); }
                        }
                        else
                        {
                            //if it is a buffing attack
                            if (manager.selectedCharacterBehavior.currentSelectedAttack.targeting == AbstractAttack.AttackType.SELF)
                            {
                                manager.selectedCharacterBehavior.onAttack(manager.selectedCharacterBehavior, false);
                            }
                            else
                            {
                                //end the character's turn
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

        public AbstractModifier getTerrainMod()
        {
            switch (terrainType)
            {
                case 3:
                    return new HighGroundModifier();
                    break;
            }

            return null;
        }

        public void moveCharacterToCell(GridCell moveTo, BaseBehavior character)
        {
            character.currentCell.occupant = null;
            character.currentCell = moveTo;
            moveTo.occupant = character.gameObject;
            character.onMove(moveTo);
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

        public void canMoveTo(bool dash)
        {
            if (dash)
            {
                tint.color = gameModel.movementTint2;
            }
            else
            {
                tint.color = gameModel.movementTint;

            }
            isMovementSelectable = true;
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
        
        public void isAttackable(bool isBuff, bool isMovement)
        {
            isAttackSelectable = true;
            if (isBuff)
                tint.color = gameModel.buffTint;
            else if (isMovement)
                tint.color = gameModel.movementTint;

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

        public void showAttackHovered(bool isBuff)
        {
            tint.gameObject.SetActive(true);
            if (isBuff)
            {
                tint.color = gameModel.buffSelectTint;
            }
            else
            {
                tint.color = isOptimal ? gameModel.optimalSelectTint : gameModel.attackSelectTint;
            }
        }
        
        public void showOptimalAttackHovered()
        {
            tint.gameObject.SetActive(true);
            tint.color = gameModel.optimalSelectTint;
        }
        
        public void showAttackHovered(bool isBuff, bool isMove)
        {
            tint.gameObject.SetActive(true);
            if (isMove)
            {
                tint.color = gameModel.moveSelectTint;
            }
            else if (isBuff)
            {
                tint.color = gameModel.buffSelectTint;
            }
            else
            {
                tint.color = isOptimal ? gameModel.optimalSelectTint : gameModel.attackSelectTint;
            }
        }

        public int movementPenalty()
        {
            if (occupant != null && occupant.GetComponent<BaseBehavior>().owner != gameManager.currentTurn)
                return 100;
            
            int penalty = 1;
            switch (terrainType)
            {
                case 2:
                    penalty = pickHighestPenalty(penalty, 2);
                    break;
                case 3:
                    penalty = pickHighestPenalty(penalty, 2);
                    break;
            }

            foreach (int i in modifiers)
            {
                if (i == 0)
                {
                    penalty = pickHighestPenalty(penalty, 3);
                }
            }
            return penalty;
        }

        public int pickHighestPenalty(int penalty, int newPenalty)
        {
            return newPenalty > penalty ? newPenalty : penalty;
        }

        public bool checkForOutOfActions()
        {
            return (manager.selectedCharacterBehavior.currentMoves <= 0 &&
                   manager.selectedCharacterBehavior.currentAttacks <= 0);
        }
        
        public GridCell getDirCellFromInt(int i, GridCell c)
        {
            switch (i)
            {
                case 0:
                    return c.getSouth();
                case 1:
                    return c.getEast();
                case 2:
                    return c.getNorth();
                case 3:
                    return c.getWest();
                default:
                    return c.getNorth();
            }
        }

        public void triggerBoost()
        {
            BaseBehavior newEntrant = occupant.GetComponent<BaseBehavior>();

            BoostMod toAdd = new BoostMod(25, 4);
            toAdd.setStrings();
            newEntrant.Modifiers.Add(toAdd);
            
            int toRemove = -1;
            for (int i = 0; i < modifiers.Count; i++){
                if (modifiers[i] == 4)
                {
                    toRemove = i;
                    break;
                }
            }
            if (toRemove != -1)
            {
                modifiers.RemoveAt(toRemove);
            }
                    
            modifiers.Add(5);
            TerrainSprite.sprite = gameModel.Terrainsprites[7];
        }

        public void triggerHealthPack()
        {
            BaseBehavior newEntrant = occupant.GetComponent<BaseBehavior>();
            int healing = 6;

            newEntrant.HP += healing;
            newEntrant.updateBars();
            if (newEntrant.HP > newEntrant.values.hp)
            {
                newEntrant.isOverHealed = true;
            }
                    
            int toRemove = -1;
            for (int i = 0; i < modifiers.Count; i++){
                if (modifiers[i] == 2)
                {
                    toRemove = i;
                    break;
                }
            }
            if (toRemove != -1)
            {
                modifiers.RemoveAt(toRemove);
            }
                    
            modifiers.Add(3);
            TerrainSprite.sprite = gameModel.Terrainsprites[5];

        }

        public void Update()
        {
            if (occupant != null)
            {
                occupant.transform.position = new Vector3(this.gameObject.transform.position.x,
                    gameObject.transform.position.y, gameObject.transform.position.z - 0.1f);
                if (modifiers.Contains(2))
                {
                    triggerHealthPack();
                }
                if (modifiers.Contains(4))
                {
                    triggerBoost();
                }
            }
        }
    }
}