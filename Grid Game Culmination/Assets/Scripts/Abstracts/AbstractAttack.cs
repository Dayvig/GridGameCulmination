﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public abstract class AbstractAttack : MonoBehaviour

    {
        public int AttackRange;
        public int AttackDamage; 
        public int OptimalDamage;
        public int Healing;
        public int OptimalHealing;
        public string AttackName;
        public string AttackDesc;
        public int ID;
        public AttackType targeting = AttackType.ENEMY;
        public bool onCooldown = false;
        public int currentCooldown;
        public int cooldown;
        public int buffAmount;
        public int buffTurns;
        public AudioClip attackSound;
        public int charges;
        public int maxCharges;
        
        public enum AttackType
        {
            ENEMY,
            SELF,
            GROUND,
            ALLY
        }
        public abstract void use(BaseBehavior initiator, BaseBehavior target, bool isOptimal);
        public void showAttackingSquares(GridCell startingCell, int range)
        {
            showAttackingSquares(startingCell, range, AttackType.ENEMY);
        }

        public abstract void showSelectedSquares(GridCell origin, bool isBuff);

        public void reduceCooldown()
        {
            if (this is ChargeAbility)
            {
                if (charges < maxCharges)
                {
                    currentCooldown--;
                    if (currentCooldown <= 0)
                    {
                        charges++;
                        if (charges > 0)
                        {
                            onCooldown = false;
                        }
                        if (charges != maxCharges)
                        {
                            currentCooldown = cooldown;
                        }
                    }
                }
            }
            else
            {
                currentCooldown--;
                if (currentCooldown <= 0)
                {
                    onCooldown = false;
                    currentCooldown = cooldown;
                }
            }
        }

        public void setCooldown(int i)
        {
            currentCooldown = i;
            if (currentCooldown < 0)
            {
                onCooldown = false;
                currentCooldown = cooldown;
            }
        }

        public virtual void showAttackingSquares(GridCell startingCell, int range, AttackType targetingType) { }
    }
}