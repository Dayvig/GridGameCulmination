using System.Collections.Generic;

namespace DefaultNamespace
{
    public class ShovelAttack : AbstractAttack, GroundTarget
    {
        public override void use(BaseBehavior initiator, BaseBehavior target, bool isOptimal)
        {
            //Attack target
            int damage = initiator.calculateDamage(isOptimal ? OptimalDamage : AttackDamage, target, initiator);
            target.HP -= damage;
            target.updateBars();
        }

        public void groundUse(BaseBehavior initiator, GridCell target)
        {
            //Decrease the current amount of attacks
            initiator.currentAttacks--;
            if (target.occupant != null && target.occupant.GetComponent<BaseBehavior>().owner != initiator.owner)
            {
                BaseBehavior t = target.occupant.GetComponent<BaseBehavior>();
                use(initiator, t, target.isOptimal);
            }
            if (target.terrainType == 0)
            {
                target.cell.enabled = true;
                target.terrainType = 1;
            }
        }

        public override void showAttackingSquares(GridCell startingCell, int range, AttackType targetingType)
        {

            //Creates a list for all tiles that can be moved to, and adds the starting cell to it.
            List<GridCell> inRangeCells = new List<GridCell>();
            inRangeCells.Add(startingCell);

            //adds the cells manually
            for (int i = 0; i < 4; i++)
            {
                inRangeCells.Add(startingCell.neighbors[i]);
                if (i == 0)
                {
                    startingCell.neighbors[i].isOptimal = true;
                }
            }

            foreach (GridCell g in inRangeCells)
            {
                g.isAttackable();
            }
        }
    }
}