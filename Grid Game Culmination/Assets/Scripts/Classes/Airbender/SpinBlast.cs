using System.Collections.Generic;
using DefaultNamespace;
using UnityEditor;
using UnityEngine;

namespace Classes.Airbender
{
    public class SpinBlast : AbstractAttack, GroundTarget
    {
        private int knockback = 1;
        public override void use(BaseBehavior initiator, BaseBehavior target, bool isOptimal)
        {
            
        }

        public override void showSelectedSquares(GridCell origin, bool isBuff)
        {
            origin.showAttackHovered(isBuff, true);
            for (int i = 0; i < 8; i++)
            {
                if (origin.neighbors[i] != null)
                    origin.neighbors[i].showAttackHovered(isBuff);
            }
        }

        public void groundUse(BaseBehavior initiator, GridCell target)
        {
            GameManager.Sounds.PlayOneShot(attackSound, GameManager.MasterVolume);

            //Decrease the current amount of attacks
            initiator.currentAttacks--;
            
            //hits all enemies around and knocks back
            for (int i = 0; i < 8; i++)
            {
                if (target.neighbors[i] != null && target.neighbors[i].occupant != null)
                {
                    BaseBehavior targetB = target.neighbors[i].occupant.GetComponent<BaseBehavior>();
                    if (targetB.owner != initiator.owner)
                        KnockBack(initiator, targetB, knockback, i);
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
        
        public override void showAttackingSquares(GridCell startingCell, int range, AttackType targetingType)
        {

            //Creates a list for all tiles that can be moved to, and adds the starting cell to it.
            List<GridCell> inRangeCells = new List<GridCell>();
            inRangeCells.Add(startingCell);

            foreach (GridCell g in inRangeCells)
            {
                g.isAttackable(false);
            }
        }
        
    }
}