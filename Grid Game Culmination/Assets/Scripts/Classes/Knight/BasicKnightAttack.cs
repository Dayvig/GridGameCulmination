using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public class BasicKnightAttack : AbstractAttack
    {

        public override void use(BaseBehavior initiator, BaseBehavior target, bool optimalAttack)
        {
            //Decrease the current amount of attacks
            initiator.currentAttacks--;

            //attack target
            int damage;
            if (optimalAttack)
            {
                damage = initiator.calculateDamage(OptimalDamage, target, initiator);
            }
            else
            {
                damage = initiator.calculateDamage(AttackDamage, target, initiator);
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
            GridCell n = startingCell.getEast();
            if (n != null)
            {
                inRangeCells.Add(n);
                n.isOptimal = true;
            }

            n = startingCell.getWest();
            if (n != null)
            {
                inRangeCells.Add(n);
                n.isOptimal = true;
            }

            n = startingCell.getSouth();
                if (n != null) 
                    inRangeCells.Add(n);
                
            n = startingCell.getNorth();
                if (n != null) 
                    inRangeCells.Add(n);

            foreach (GridCell g in inRangeCells)
            {
                g.isAttackable();
            }
        }
    }
}