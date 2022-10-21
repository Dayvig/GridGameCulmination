﻿using System;
using System.Drawing;
using UnityEngine;

namespace DefaultNamespace
{
    public class Focus : AbstractAttack
    {
        public override void use(BaseBehavior initiator, BaseBehavior target, bool isOptimal)
        {
            //Decrease the current amount of attacks
            initiator.currentAttacks--;

            //puts the move on cooldown
            onCooldown = true;
            currentCooldown = cooldown;
            initiator.currentSelectedAttack = initiator.Attacks[0];

            FocusModifier newMod = new FocusModifier(buffAmount, buffTurns);
            newMod.setStrings();
            //buff target
            target.Modifiers.Add(newMod);
        }

        public override void showAttackingSquares(GridCell startingCell, int range, AttackType targetingType)
        {
            startingCell.isAttackable(true);
        }
        
    }
}