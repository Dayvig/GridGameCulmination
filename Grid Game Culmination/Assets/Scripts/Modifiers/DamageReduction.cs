using System;

namespace DefaultNamespace
{
    public class DamageReduction : AbstractModifier
    {
        public static string modId = "Brace";
        public DamageReduction(int stackAmount, int turnsLeft)
        {
            ID = modId;
            stackable = true;
            turnBased = true;
            type = Type.BUFF;
            aType = applicationType.DEFENSIVE;
            amount = stackAmount;
            turns = turnsLeft;
        }
        public override int applyModifier(int input, BaseBehavior target, BaseBehavior initiator)
        {
            return input / (2 * amount);
        }

        public override int getKey()
        {
            return 5;
        }

        public override void setStrings()
        {
            modifierDescriptions[0] = "Halves damage taken for ";
            modifierDescriptions[1] = "";
            modifierDescriptions[2] = " turns.";
        }

        public override String setDesc()
        {
            return modifierDescriptions[0] + turns + modifierDescriptions[2];
        }
        
    }
}