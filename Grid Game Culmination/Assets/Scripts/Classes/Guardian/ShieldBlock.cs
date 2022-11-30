
using Modifiers;
using UnityEngine;

namespace DefaultNamespace
{
    public class ShieldBlock : AbstractAttack, GroundTarget
    {
        private GridManager gridManager;
        private int hoveredDirection;

        public void Start()
        {
            gridManager = GameObject.Find("GameManager").GetComponent<GridManager>();
        }
        public override void use(BaseBehavior initiator, BaseBehavior target, bool isOptimal)
        {
            //Decrease the current amount of attacks
            initiator.currentAttacks--;

            //puts the move on cooldown
            onCooldown = true;
            currentCooldown = cooldown;
            initiator.currentSelectedAttack = initiator.Attacks[0];

            ShieldBlockModifier newMod = new ShieldBlockModifier(buffAmount, buffTurns);
            newMod.direction = hoveredDirection;
            newMod.setStrings();
            //buff target
            target.Modifiers.Add(newMod);
        }
        
        public override void showSelectedSquares(GridCell origin, bool isBuff)
        {
            int direction = checkDir(origin);
            GridCell targetCell;
            TacticsGrid grid = gridManager.MasterGrid;
            for (int rowCursor = 0; rowCursor < grid.contents.Count; rowCursor++)
            {
                for (int colCursor = 0; colCursor < grid.contents[rowCursor].contents.Count; colCursor++)
                {
                    targetCell = grid.contents[rowCursor].contents[colCursor];
                    if (isShieldAffected(direction, targetCell))
                    {
                        targetCell.showAttackHovered(true);
                        hoveredDirection = invert(direction);
                    }
                }
            }
        }

        int invert(int input)
        {
            switch (input)
            {
                case 0:
                    return 1;
                case 1:
                    return 0;
                case 2:
                    return 3;
                case 3:
                    return 2;
                case 4:
                    return 7;
                case 5:
                    return 6;
                case 6:
                    return 5;
                case 7:
                    return 4;
                default:
                    return 0;
            }
        }
        
        int checkDir(GridCell toCheck)
            {
                int xDiff = toCheck.column-gridManager.selectedCharacterBehavior.currentCell.column;
                int yDiff = toCheck.row-gridManager.selectedCharacterBehavior.currentCell.row;
                Debug.Log(xDiff+" "+yDiff);
                if (Mathf.Abs(xDiff) == Mathf.Abs(yDiff))
                {
                    //Both positive: North East
                    if (xDiff > 0 && yDiff > 0)
                    {
                        return 4;
                    }
                    
                    //Row negative column positive: South East
                    if (xDiff > 0 && yDiff < 0)
                    {
                        return 5;
                    }
                    
                    //Row positive column negative: North West
                    if (xDiff < 0 && yDiff > 0)
                    {
                        return 6;
                    }
                    
                    //both negative: South West
                    if (xDiff < 0 && yDiff < 0)
                    {
                        return 7;
                    }
                }
                else
                {
                    //row positive: North
                    if (yDiff > 0)
                    {
                        return 0;
                    }
        
                    //row negative: south
                    if (yDiff < 0)
                    {
                        return 1;
                    }
                    
                    //col positive: East
                    if (xDiff > 0)
                    {
                        return 2;
                    }
        
                    //row negative: West
                    if (xDiff < 0)
                    {
                        return 3;
                    }
                }
                
                return -1;
            }
        
        bool isShieldAffected(int dir, GridCell target)
        {
            BaseBehavior guardian = gridManager.selectedCharacterBehavior;
            //must be behind
            switch (dir)
            {
                case 0:
                    return target.row > guardian.currentCell.row;
                case 1:
                    return target.row < guardian.currentCell.row;
                case 2:
                    return target.column > guardian.currentCell.column;
                case 3:
                    return target.column < guardian.currentCell.column;
                case 4:
                    return target.row > guardian.currentCell.row && target.column > guardian.currentCell.column;
                case 5:
                    return target.row < guardian.currentCell.row && target.column > guardian.currentCell.column;
                case 6:
                    return target.row > guardian.currentCell.row && target.column < guardian.currentCell.column;
                case 7:
                    return target.row < guardian.currentCell.row && target.column < guardian.currentCell.column;
                default:
                    return false;
            }
        }



        public override void showAttackingSquares(GridCell startingCell, int range, AttackType targetingType)
        {
            TacticsGrid grid = gridManager.MasterGrid;
            GridCell targetCell;
            for (int rowCursor = 0; rowCursor < grid.contents.Count; rowCursor++)
            {
                for (int colCursor = 0; colCursor < grid.contents[rowCursor].contents.Count; colCursor++)
                {
                    targetCell = grid.contents[rowCursor].contents[colCursor];
                    if (targetCell != startingCell)
                        targetCell.isAttackable(true);
                }
            }        
        }

        public void groundUse(BaseBehavior initiator, GridCell target)
        {
            use(initiator, initiator, initiator.currentCell);
        }
    }
}