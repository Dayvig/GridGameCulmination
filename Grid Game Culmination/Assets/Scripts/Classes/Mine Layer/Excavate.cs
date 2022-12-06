using System.Collections.Generic;
using System.Linq;

namespace DefaultNamespace
{
    public class Excavate : AbstractAttack, GroundTarget, ChargeAbility
    {
        public override void use(BaseBehavior initiator, BaseBehavior target, bool isOptimal)
        {
            
        }

        public void groundUse(BaseBehavior initiator, GridCell target)
        {
            GameManager.Sounds.PlayOneShot(attackSound, GameManager.MasterVolume);

            //Decrease the current amount of attacks
            initiator.currentAttacks--;
            
            spendCharge(initiator);
            
            if (target.terrainType != 0)
            {
                target.cell.enabled = false;
                target.hover.SetActive(false);
                target.terrainType = 0;
            }
        }
        
        public override void showSelectedSquares(GridCell origin, bool isBuff)
        {
            origin.showAttackHovered(isBuff);
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
        
            while (currentMove < range)
            {
                //Looks at the previous cells accessed, then returns all of its accessible neighbors
                foreach (GridCell nextCell in previousCells)
                {
                            for (int i = 0; i < 8; i++)
                            {
                                GridCell n = nextCell.neighbors[i];
                                if (n != null && n.occupant == null)
                                {
                                    surroundingCells.Add(n);
                                }
                            }
                }
                
                inRangeCells.AddRange(surroundingCells);

                //these new accessible neighbors become the previous cells
                previousCells = surroundingCells.Distinct().ToList();
                
                //clears previous surrounding cells
                surroundingCells.Clear();
                
                //reduces movement count
                currentMove++;
            }

            foreach (GridCell g in inRangeCells)
            {
                g.isAttackable();
            }
        }

        public void spendCharge(BaseBehavior initiator)
        {
            if (charges == maxCharges )
            {
                currentCooldown = cooldown;
            }
            charges--;
            if (charges <= 0)
            {
                //puts the move on cooldown
                onCooldown = true;
                initiator.currentSelectedAttack = initiator.Attacks[0];
            }
        }

        public void setCharges(BaseBehavior initiator, int newCharges)
        {
            charges = newCharges;
        }

        public int getCharges(BaseBehavior initiator)
        {
            return charges;
        }
    }
}