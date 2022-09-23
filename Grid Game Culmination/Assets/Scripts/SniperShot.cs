using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class SniperShot : AbstractAttack
{
     public override void use(BaseBehavior initiator, BaseBehavior target, bool optimalAttack)
            {
                //Decrease the current amount of attacks
                initiator.currentAttacks--;
    
                //attack target
                int damage;
                if (optimalAttack)
                {
                    damage = initiator.calculateDamage(OptimalDamage, target);
                }
                else
                {
                    damage = initiator.calculateDamage(AttackDamage, target);
                }
                target.HP -= damage;
                target.updateBars();
            }
    
            public override void showAttackingSquares(GridCell startingCell, int range, AttackType targetingType)
            {
    
                //Creates a list for all tiles that can be moved to, and adds the starting cell to it.
                List<GridCell> inRangeCells = new List<GridCell>();
                inRangeCells.Add(startingCell);
                //sets the move counter to 0
                int currentMove = 0;

                //tracks the currently selected tiles
                List<GridCell> previousCells = new List<GridCell>();
                previousCells.Add(startingCell);

                GridCell cursor = startingCell;
            
                addCells(0, 5, startingCell, inRangeCells);
                addCells(1, 5, startingCell, inRangeCells);
    
                foreach (GridCell g in inRangeCells)
                {
                    if (g.Equals(startingCell))
                    {
                        g.isOptimal = false;
                    }
                    g.isAttackable();
                }
            }
            
            private void addCells(int i, int range, GridCell startingCell, List<GridCell> inRangeCells)
            {
                int currentMove = 0;
                GridCell cursor = startingCell;

                while (currentMove < range)
                {
                    if (getDirCellFromInt(i, cursor) != null)
                    {
                        cursor = getDirCellFromInt(i, cursor);
                        inRangeCells.Add(cursor);
                        if (currentMove == range - 1)
                        {
                            cursor.isOptimal = true;
                        }
                        currentMove++;
                    }
                    else
                    {
                        break;
                    }
                }

            }

            private GridCell getDirCellFromInt(int i, GridCell c)
            {
                switch (i)
                {
                    case 0:
                        return c.getNorth();
                    case 1:
                        return c.getSouth();
                    default:
                        return c.getNorth();
                }
            }
}