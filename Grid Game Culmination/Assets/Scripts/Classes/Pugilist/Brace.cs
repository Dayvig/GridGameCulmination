using System;
using System.Drawing;
using UnityEngine;

namespace DefaultNamespace
{
    public class Brace : AbstractAttack
    {
        public override void use(BaseBehavior initiator, BaseBehavior target, bool isOptimal)
        {
            //Decrease the current amount of attacks
            initiator.currentAttacks--;

            //puts the move on cooldown
            onCooldown = true;
            currentCooldown = cooldown;
            initiator.currentSelectedAttack = initiator.Attacks[0];

            DamageReduction newMod = new DamageReduction(buffAmount, buffTurns);
            newMod.setStrings();
            //buff target
            target.Modifiers.Add(newMod);
        }
        
        public override void showSelectedSquares(GridCell origin, bool isBuff)
        {
            origin.showAttackHovered(isBuff);
        }

        public override void showAttackingSquares(GridCell startingCell, int range, AttackType targetingType)
        {
            startingCell.isAttackable(true);
        }
        
    }
}