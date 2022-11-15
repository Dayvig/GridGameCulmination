using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class FirstStrikeModifier : AbstractModifier
    {
        public static string modID = "First Strike";
        public FirstStrikeModifier(int stackAmount)
        {
            ID = modID;
            stackable = false;
            turnBased = false;
            type = Type.BUFF;
            aType = applicationType.OFFENSIVE;
            amount = stackAmount;
        }
        
        public override int applyModifier(int input, BaseBehavior target, BaseBehavior initiator)
        {
            return target.HP >= target.values.hp ? Mathf.CeilToInt(input * 1.5f) : input;
        }

        public override int getKey()
        {
            return 11;
        }
        
        public override String setDesc()
        {
            return modifierDescriptions[0];
        }

        public override void setStrings()
        {
            modifierDescriptions[0] = "Deals +50% damage when hitting full HP enemies.";
            modifierDescriptions[1] = "";
            modifierDescriptions[2] = "";
        }
    }
}