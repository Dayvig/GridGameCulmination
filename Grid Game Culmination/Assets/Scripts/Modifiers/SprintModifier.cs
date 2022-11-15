using System;

namespace DefaultNamespace
{
    public class SprintModifier : AbstractModifier
    {
        public static string modID = "Sprint";
        public SprintModifier(int stackAmount, int turnsLeft)
        {
            ID = modID;
            stackable = false;
            turnBased = true;
            type = Type.BUFF;
            aType = applicationType.DEFENSIVE;
            amount = stackAmount;
            turns = turnsLeft;
        }
        public override void endTurnTrigger(BaseBehavior target)
        {
            target.move *= 2;
            target.dash *= 2;
        }

        public override int applyModifier(int input, BaseBehavior target, BaseBehavior initiator)
        {
            return input;
        }

        public override int getKey()
        {
            return 9;
        }
        
        public override String setDesc()
        {
            return modifierDescriptions[0];
        }

        public override void setStrings()
        {
            modifierDescriptions[0] = " Doubles all movement next turn.";
            modifierDescriptions[1] = "";
            modifierDescriptions[2] = "";
        }
    }
}