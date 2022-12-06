using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public class Grenade : AbstractAttack, GroundTarget
    {
        public override void use(BaseBehavior initiator, BaseBehavior target, bool isOptimal)
        {
            //Attack target
            int damage = initiator.calculateDamage(isOptimal ? OptimalDamage : AttackDamage, target, initiator);
            initiator.damageTarget(damage, target);
        }

        public void groundUse(BaseBehavior initiator, GridCell target)
        {
            GameManager.Sounds.PlayOneShot(attackSound, GameManager.MasterVolume);

            //Decrease the current amount of attacks
            initiator.currentAttacks = 0;
            
            //puts the move on cooldown
            onCooldown = true;
            currentCooldown = cooldown;
            initiator.currentSelectedAttack = initiator.Attacks[0];

            // hits targeted square
            if (target.occupant != null)
            {
                BaseBehavior t = target.occupant.GetComponent<BaseBehavior>();
                use(initiator, t, target.isOptimal);
            }
            
            //hits adjacent+diagonal squares and blasts targets 1 zone away if able.
            for (int i = 0; i<8; i++)
            {
                if (target.neighbors[i].occupant != null)
                {
                    BaseBehavior targetB = target.neighbors[i].occupant.GetComponent<BaseBehavior>();
                    if (targetB.owner != initiator.owner)
                    {
                        GridCell blastTo = target.neighbors[i].neighbors[i];
                        if (blastTo != null && blastTo.terrainType != 0 && blastTo.occupant == null)
                        {
                            targetB.currentCell.occupant = null;
                            targetB.currentCell = targetB.currentCell.neighbors[i];
                            targetB.currentCell.occupant = targetB.gameObject;
                            targetB.onDisplace(targetB.currentCell);
                        }
                        use(initiator, targetB, target.isOptimal);
                    }
                }
            }
        }
        
        public override void showSelectedSquares(GridCell origin, bool isBuff)
        {
            origin.showAttackHovered(isBuff);
            for (int i = 0; i < 8; i++)
            {
                if (origin.neighbors[i] != null && origin.neighbors[i].terrainType != 0)
                    origin.neighbors[i].showAttackHovered(isBuff);
            }
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
        
            List<GridCell> surroundingCells = new List<GridCell>();
            List<GridCell> allCheckedCells = new List<GridCell>();
            allCheckedCells.Add(startingCell);
        
            while (currentMove < range)
            {
                //Looks at the previous cells accessed, then returns all of its accessible neighbors
                foreach (GridCell nextCell in previousCells)
                {
                            for (int i = 0; i < 8; i++)
                            {
                                GridCell n = nextCell.neighbors[i];
                                if (n != null && !allCheckedCells.Contains(n))
                                {
                                    surroundingCells.Add(n);
                                    allCheckedCells.Add(n);
                                }
                            }
                }
                
                
                if (currentMove != 0)
                {
                    inRangeCells.AddRange(surroundingCells);
                }

                //these new accessible neighbors become the previous cells
                previousCells = surroundingCells.Distinct().ToList();
                
                //clears previous surrounding cells
                surroundingCells.Clear();
                
                //reduces movement count
                currentMove++;
            }
            
            setOptimalCells(0, range, startingCell);
            setOptimalCells(1, range, startingCell);
            setOptimalCells(2, range, startingCell);
            setOptimalCells(3, range, startingCell);

            foreach (GridCell g in inRangeCells)
            {
                if (g.Equals(startingCell))
                {
                    g.isOptimal = false;
                }
                g.isAttackable();
            }

        }
    
    private void setOptimalCells(int i, int range, GridCell startingCell)
    {
        int currentMove = 0;
        GridCell cursor = startingCell;

        while (currentMove <= range)
        {
            if (currentMove == range)
                cursor.isOptimal = true;
            if (cursor.getDirCellFromInt(i, cursor) != null)
            {
                cursor = cursor.getDirCellFromInt(i, cursor);
                currentMove++;
            }
            else
            {
                break;
            }
        }
    }
    }
}