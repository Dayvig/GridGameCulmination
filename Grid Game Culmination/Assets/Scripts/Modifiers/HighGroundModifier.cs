using System;

namespace DefaultNamespace
{
    public class HighGroundModifier : AbstractModifier
    {
        public static string modID = "High Ground";

        public HighGroundModifier() : base()
        {
            ID = modID;
            stackable = false;
            turnBased = false;
            type = Type.BUFF;
            aType = applicationType.OFFENSIVE;
            amount = 1;
            isTerrainModifier = true;
        }

        public override int applyModifier(int input, BaseBehavior target, BaseBehavior initiator)
        {
            return input + amount;
        }

        public override int getKey()
        {
            return 6;
        }
        
        public override String setDesc()
        {
            return modifierDescriptions[0] + amount + modifierDescriptions[1];
        }

        public override void setStrings()
        {
            modifierDescriptions[0] = "Deal +";
            modifierDescriptions[1] = " damage.";
            modifierDescriptions[2] = "";
        }
    }
}