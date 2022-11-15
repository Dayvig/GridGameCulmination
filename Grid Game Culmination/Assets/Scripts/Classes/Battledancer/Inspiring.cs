﻿using System.Collections.Generic;
using DefaultNamespace;
using UnityEditor;

namespace Classes.Battledancer
{
    public class Inspiring : AbstractAttack, GroundTarget
    {

        
        public override void use(BaseBehavior initiator, BaseBehavior target, bool isOptimal)
        {
            //Heal Target
            target.HP += Healing;
            if (target.HP > target.values.hp)
            {
                target.HP = target.values.hp;
            }
            target.updateBars();
            
            //reduce target's cooldowns
            foreach (AbstractAttack a in target.Attacks)
            {
                if (a != null && a.onCooldown)
                {
                    a.reduceCooldown();
                }
            }
        }

        public override void showSelectedSquares(GridCell origin, bool isBuff)
        {
            origin.showAttackHovered(isBuff, true);
            for (int i = 0; i < 8; i++)
            {
                if (origin.neighbors[i] != null && origin.neighbors[i].terrainType != 0)
                    origin.neighbors[i].showAttackHovered(true);
            }
        }

        public void groundUse(BaseBehavior initiator, GridCell target)
        {
            //Decrease the current amount of attacks
            initiator.currentAttacks--;
            
            //puts the move on cooldown
            onCooldown = true;
            currentCooldown = cooldown;
            initiator.currentSelectedAttack = initiator.Attacks[0];
            
            //moves to cell
            initiator.currentCell.occupant = null;
            initiator.currentCell = target;
            initiator.currentCell.occupant = initiator.gameObject;
            initiator.onDisplace(target);

            //hits all allies around
            for (int i = 0; i < 8; i++)
            {
                if (target.neighbors[i].occupant != null)
                {
                    BaseBehavior targetB = target.neighbors[i].occupant.GetComponent<BaseBehavior>();
                    if (targetB.owner == initiator.owner)
                        use(initiator, targetB, false);
                }
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
                if (startingCell.neighbors[i] !=null && startingCell.neighbors[i].terrainType != 0 && 
                    startingCell.neighbors[i].occupant == null)
                {
                    inRangeCells.Add(startingCell.neighbors[i]);
                }
            }

            foreach (GridCell g in inRangeCells)
            {
                g.isAttackable(false, true);
            }
        }
        
    }
}