using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class BoostMod : AbstractModifier
    {
        private String modID = "Boost";
        public BoostMod(int stackAmount, int turnsLeft)
        {
            ID = modID;
            stackable = true;
            turnBased = true;
            type = Type.BUFF;
            aType = applicationType.OFFENSIVE;
            amount = stackAmount;
            turns = turnsLeft;
        }
        public override int applyModifier(int input, BaseBehavior target, BaseBehavior initiator)
        {
            float num = input * (1 + ((float) amount / 100));
            return Mathf.CeilToInt(num);
        }

        public override int getKey()
        {
            return 7;
        }

        public override void setStrings()
        {
            modifierDescriptions[0] = "+";
            modifierDescriptions[1] = "% Damage (rounded up.) for";
            modifierDescriptions[2] = " turns";
        }
    }
}