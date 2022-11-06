using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class FlySting : AbstractAttack
    {
        public override void use(BaseBehavior initiator, BaseBehavior target, bool isOptimal)
        {
            //Decrease the current amount of attacks
            initiator.currentAttacks--;
            
            
            //puts the move on cooldown
            onCooldown = true;
            currentCooldown = cooldown;
            initiator.currentSelectedAttack = initiator.Attacks[0];
            
            //Attack target
            int damage = initiator.calculateDamage(isOptimal ? OptimalDamage : AttackDamage, target, initiator);
            target.HP -= damage;
            target.updateBars();
            
            //Re-set movement
            initiator.GlowRen.color = Color.blue;
            if (initiator.currentMoves <= 0)
            {
                initiator.currentMoves++;
                initiator.move = initiator.baseMove;
            }
            else
            {
                initiator.currentMoves++;
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
                if (i == 1 || i == 3)
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