using System;

namespace DefaultNamespace
{
    public class ShockModifier : AbstractModifier
    {
        public static string modID = "Shock and Awe";
        public ShockModifier(int stackAmount, int turnsLeft)
        {
            ID = modID;
            stackable = true;
            turnBased = true;
            type = Type.DEBUFF;
            aType = applicationType.DEFENSIVE;
            amount = stackAmount;
            turns = turnsLeft;
        }
        
        public override int applyModifier(int input, BaseBehavior target, BaseBehavior initiator)
        {
            return input;
        }

        public override int getKey()
        {
            return 10;
        }
        
        public override String setDesc()
        {
            return modifierDescriptions[0] + turns + modifierDescriptions[2];
        }

        public override void setStrings()
        {
            modifierDescriptions[0] = "Cannot use special attacks for ";
            modifierDescriptions[1] = "";
            modifierDescriptions[2] = " turns.";
        }
    }
}