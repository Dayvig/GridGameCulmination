using System.Collections.Generic;
using UnityEngine;

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

                    //checks if can be knocked 1 cell away
                    if (canKnockBackToCell(target.currentCell.neighbors[i]))
                    {
                        Debug.Log("Can be knocked 1 cell away");
                        //checks if it can be knocked 2 cells away
                        if (canKnockBackToCell(target.currentCell.neighbors[i].neighbors[i]))
                        {
                            Debug.Log("Can be knocked 2 cells away");
                            //if it can, moves it to that zone and deals normal damage.
                            damage = initiator.calculateDamage(AttackDamage, target, initiator);
                            target.currentCell.occupant = null;
                            target.currentCell = target.currentCell.neighbors[i].neighbors[i];
                            target.currentCell.occupant = target.gameObject;
                            target.onDisplace(target.currentCell);
                            break;
                        }
                        //if not, deals extra damage to target, possible bonk damage, and moves one zone.
                        else
                        {
                            Debug.Log("Can be knocked 1 cell away, 2nd cell blocked");
                            damage = initiator.calculateDamage(OptimalDamage, target, initiator);
                            bonkDamage(target.currentCell.neighbors[i].neighbors[i], initiator);
                            target.currentCell.occupant = null;
                            target.currentCell = target.currentCell.neighbors[i];
                            target.currentCell.occupant = target.gameObject;
                            target.onDisplace(target.currentCell);
                            break;
                        }
                    }
                    //if not, deals extra damage to target, possible bonk damage, and doesn't move.
                    else
                    {
                        Debug.Log("1st Cell blocked");
                        damage = initiator.calculateDamage(OptimalDamage, target, initiator);
                        bonkDamage(target.currentCell.neighbors[i], initiator);
                        break;
                    }
                }
            }
            target.HP -= damage;
            target.updateBars();
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
            Debug.Log(g.name);
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