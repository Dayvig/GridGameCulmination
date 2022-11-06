using System.Collections.Generic;

namespace DefaultNamespace
{
    public class Excavate : AbstractAttack, GroundTarget
    {
        public override void use(BaseBehavior initiator, BaseBehavior target, bool isOptimal)
        {
            
        }

        public void groundUse(BaseBehavior initiator, GridCell target)
        {
            //Decrease the current amount of attacks
            initiator.currentAttacks--;

            //puts the move on cooldown
            onCooldown = true;
            currentCooldown = cooldown;
            initiator.currentSelectedAttack = initiator.Attacks[0];
            
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

            //adds the cells manually
            for (int i = 0; i < 8; i++)
            {
                inRangeCells.Add(startingCell.neighbors[i]);
            }

            foreach (GridCell g in inRangeCells)
            {
                g.isAttackable();
            }
        }
    }
}