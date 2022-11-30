using System;
using System.Drawing;
using UnityEngine;

namespace DefaultNamespace
{
    public class Sprint : AbstractAttack
    {
        public override void use(BaseBehavior initiator, BaseBehavior target, bool isOptimal)
        {
            GameManager.Sounds.PlayOneShot(attackSound, GameManager.MasterVolume);

            //Decrease the current amount of attacks
            initiator.currentAttacks--;

            //puts the move on cooldown
            onCooldown = true;
            currentCooldown = cooldown;
            initiator.currentSelectedAttack = initiator.Attacks[0];

            SprintModifier newMod = new SprintModifier(buffAmount, buffTurns);
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