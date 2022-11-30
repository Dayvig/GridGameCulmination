using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public class BasicBash : AbstractAttack
    {
        public bool facingUp = true;
        public override void use(BaseBehavior initiator, BaseBehavior target, bool optimalAttack)
        {
            GameManager.Sounds.PlayOneShot(attackSound, GameManager.MasterVolume);
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
            initiator.damageTarget(damage, target);
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
                if (startingCell.neighbors[i] != null)
                {
                    inRangeCells.Add(startingCell.neighbors[i]);
                    if (facingUp)
                    {
                        if (i == 4 || i == 5 || i == 0)
                        {
                            startingCell.neighbors[i].isOptimal = true;
                        }
                    }
                    else
                    {
                        if (i == 2 || i == 6 || i == 7)
                        {
                            startingCell.neighbors[i].isOptimal = true;
                        }
                    }
                }
            }
            foreach (GridCell g in inRangeCells)
            {
                g.isAttackable();
            }
        }
    }
}