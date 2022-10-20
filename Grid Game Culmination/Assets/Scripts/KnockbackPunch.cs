using System.Collections.Generic;

namespace DefaultNamespace
{
    public class KnockbackPunch : AbstractAttack
    {
        public override void use(BaseBehavior initiator, BaseBehavior target, bool isOptimal)
        {
            //Decrease the current amount of attacks
            initiator.currentAttacks--;

            int damage = initiator.calculateDamage(AttackDamage, target, initiator);
            
            //puts the move on cooldown
            onCooldown = true;
            currentCooldown = cooldown;
            initiator.currentSelectedAttack = initiator.Attacks[0];

            //blast target back
            for (int i = 0; i < 7; i++)
            {
                if (initiator.currentCell.neighbors[i].occupant != null && initiator.currentCell.neighbors[i].occupant.GetComponent<BaseBehavior>().Equals(target))
                {
                    //check available cells to push to, dealing extra damage if blocked
                    if (target.currentCell.neighbors[i] == null || target.currentCell.neighbors[i].terrainType == 0 ||
                        target.currentCell.neighbors[i].occupant != null)
                    {
                        damage = initiator.calculateDamage(OptimalDamage, target, initiator);
                        break;
                    }
                    else if (target.currentCell.neighbors[i].neighbors[i] == null || target.currentCell.neighbors[i].neighbors[i].terrainType == 0 ||
                             target.currentCell.neighbors[i].neighbors[i].occupant != null)
                    {
                        damage = initiator.calculateDamage(OptimalDamage, target, initiator);
                        target.currentCell.occupant = null;
                        target.currentCell = target.currentCell.neighbors[i];
                        target.currentCell.neighbors[i].occupant = target.gameObject;
                        break;
                    }
                    else
                    {
                        damage = initiator.calculateDamage(AttackDamage, target, initiator);
                        target.currentCell.occupant = null;
                        target.currentCell = target.currentCell.neighbors[i].neighbors[i];
                        target.currentCell.occupant = target.gameObject;
                        break;
                    }
                }
            }

            target.HP -= damage;
            target.updateBars();
        }

        public override void showAttackingSquares(GridCell startingCell, int range, AttackType targetingType)
        {

            //Creates a list for all tiles that can be moved to, and adds the starting cell to it.
            List<GridCell> inRangeCells = new List<GridCell>();
            inRangeCells.Add(startingCell);

            //adds the cells manually
            GridCell n = startingCell.getNorth();
            if (n != null)
                inRangeCells.Add(n);

            n = startingCell.getEast();
            if (n != null)
                inRangeCells.Add(n);

            n = startingCell.getWest();
            if (n != null)
                inRangeCells.Add(n);

            n = startingCell.getSouth();
            if (n != null)
                inRangeCells.Add(n);

            foreach (GridCell g in inRangeCells)
            {
                g.isAttackable();
            }
        }
    }
}