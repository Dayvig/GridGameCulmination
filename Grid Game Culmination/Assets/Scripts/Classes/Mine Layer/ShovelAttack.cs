﻿using System.Collections.Generic;

namespace DefaultNamespace
{
    public class ShovelAttack : AbstractAttack, GroundTarget
    {
        public override void use(BaseBehavior initiator, BaseBehavior target, bool isOptimal)
        {
            //Attack target
            int damage = initiator.calculateDamage(isOptimal ? OptimalDamage : AttackDamage, target, initiator);
            initiator.damageTarget(damage, target);
        }

        public void groundUse(BaseBehavior initiator, GridCell target)
        {
            GameManager.Sounds.PlayOneShot(attackSound, GameManager.MasterVolume);

            //Decrease the current amount of attacks
            initiator.currentAttacks = 0;
            
            //puts the move on cooldown
            onCooldown = true;
            currentCooldown = cooldown;
            initiator.currentSelectedAttack = initiator.Attacks[0];
            
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

            if (target.modifiers.Contains(0))
            {
                int toRemove = -1;
                for (int i = 0; i < target.modifiers.Count; i++){
                    if (target.modifiers[i] == 0)
                    {
                        toRemove = i;
                        break;
                    }
                }

                if (toRemove != -1)
                {
                    target.modifiers.RemoveAt(toRemove);
                }

                if (initiator.Attacks[1].charges < initiator.Attacks[1].maxCharges)
                {
                    initiator.Attacks[1].charges++;
                    if (initiator.Attacks[1].charges > 0)
                    {
                        initiator.Attacks[1].onCooldown = false;
                    }
                }
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