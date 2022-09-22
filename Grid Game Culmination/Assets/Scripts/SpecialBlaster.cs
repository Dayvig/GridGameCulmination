using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public class SpecialBlaster : AbstractAttack
    {
        public SpecialBlaster()
        {
            AttackRange = 3;
            AttackDamage = 4;
            AttackName = "Special Blaster";
            AttackDesc = "A ray that hits an enemy in your column only.";
        }

        public override void use(BaseBehavior initiator, BaseBehavior target, bool optimalAttack)
        {
            //Decrease the current amount of attacks
            initiator.currentAttacks--;

            //attack target
            int damage;
            if (optimalAttack)
            {
                damage = initiator.calculateDamage(OptimalDamage, target);
            }
            else
            {
                damage = initiator.calculateDamage(AttackDamage, target);
            }
            target.HP -= damage;
            target.updateBars();
        }
    }
}