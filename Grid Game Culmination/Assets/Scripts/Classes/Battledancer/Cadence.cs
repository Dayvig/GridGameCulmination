using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;

namespace DefaultNamespace
{
    public class Cadence : AbstractAttack, GroundTarget
    {
        public override void use(BaseBehavior initiator, BaseBehavior target, bool isOptimal)
        {
            CadenceModifier newMod = new CadenceModifier(buffAmount, buffTurns);
            newMod.setStrings();
            //buff target
            target.Modifiers.Add(newMod);
        }
        
        public override void showSelectedSquares(GridCell origin, bool isBuff)
        {
            origin.showAttackHovered(true, true);
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
                g.isAttackable(true, false);
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

            //buffs self
            use(initiator, initiator, false);
        }
    }
}