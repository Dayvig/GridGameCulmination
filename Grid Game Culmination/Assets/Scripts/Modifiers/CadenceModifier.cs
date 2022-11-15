using System;

namespace DefaultNamespace
{
    public class CadenceModifier : AbstractModifier
    {
        public static string modID = "Cadence";
        public CadenceModifier(int stackAmount, int turnsLeft)
        {
            ID = modID;
            stackable = true;
            turnBased = true;
            type = Type.BUFF;
            aType = applicationType.OTHER;
            amount = stackAmount;
            turns = turnsLeft;
        }
        public override void endTurnTrigger(BaseBehavior target)
        {
            target.currentAttacks++;
        }

        public override int applyModifier(int input, BaseBehavior target, BaseBehavior initiator)
        {
            return input;
        }

        public override int getKey()
        {
            return 12;
        }

        public override void setStrings()
        {
            modifierDescriptions[0] = "Grants an extra ";
            modifierDescriptions[1] = " attack for ";
            modifierDescriptions[2] = " turns.";
        }
    }
}