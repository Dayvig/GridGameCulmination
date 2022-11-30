using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class BasicBlast : AbstractAttack
    {
        private int knockback = 2;
        public override void use(BaseBehavior initiator, BaseBehavior target, bool isOptimal)
        {
            GameManager.Sounds.PlayOneShot(attackSound, GameManager.MasterVolume);

            //Decrease the current amount of attacks
            initiator.currentAttacks--;
            
            //puts the move on cooldown
            onCooldown = true;
            currentCooldown = cooldown;
            initiator.currentSelectedAttack = initiator.Attacks[0];

            //blast target back
            for (int i = 0; i < 7; i++)
            {
                if (initiator.currentCell.neighbors[i].occupant != null && initiator.currentCell.neighbors[i].occupant.GetComponent<BaseBehavior>().Equals(target))
                {
                    KnockBack(initiator, target, knockback, i);
                    break;
                }
            }
        }

        void KnockBack(BaseBehavior initiator, BaseBehavior target, int knockback, int dir)
        {
            int damage;
            GridCell destinationCell = target.currentCell;
            for (int i = 0; i < knockback; i++)
            {
                if (!canKnockBackToCell(destinationCell.neighbors[dir]))
                {
                    if (destinationCell != target.currentCell)
                    {
                        damage = initiator.calculateDamage(OptimalDamage, target, initiator);
                        target.currentCell.occupant = null;
                        target.currentCell = destinationCell;
                        target.currentCell.occupant = target.gameObject;
                        target.onDisplace(target.currentCell);
                    }
                    else
                    {
                        damage = initiator.calculateDamage(OptimalDamage, target, initiator);
                    }

                    if (i != knockback - 1)
                    {
                        bonkDamage(destinationCell.neighbors[dir], initiator);
                    }
                    initiator.damageTarget(damage, target);
                    return;
                }
                destinationCell = destinationCell.neighbors[dir];
            }
            //knock to final cell
            damage = initiator.calculateDamage(AttackDamage, target, initiator);
            target.currentCell.occupant = null;
            target.currentCell = destinationCell;
            target.currentCell.occupant = target.gameObject;
            target.onDisplace(target.currentCell);
            initiator.damageTarget(damage, target);
        }

        private void bonkDamage(GridCell g, BaseBehavior initiator)
        {
            if (g.occupant != null)
            {
                BaseBehavior bonkTarget = g.occupant.GetComponent<BaseBehavior>();
                if (bonkTarget.owner != initiator.owner)
                {
                    int bonkDamage = bonkTarget.calculateDamage(2, bonkTarget);
                    bonkTarget.HP -= bonkDamage;
                    bonkTarget.updateBars();
                }
            }
        }

        private bool canKnockBackToCell(GridCell g)
        {
            return (g != null && g.terrainType != 0 && g.occupant == null);
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

            for (int i = 0; i < 8; i++)
            {
                if (startingCell.neighbors[i] != null && startingCell.neighbors[i].terrainType != 0)
                {
                    inRangeCells.Add(startingCell.neighbors[i]);
                }
            }
            foreach (GridCell g in inRangeCells)
            {
                g.isAttackable();
            }
        }
    }
}