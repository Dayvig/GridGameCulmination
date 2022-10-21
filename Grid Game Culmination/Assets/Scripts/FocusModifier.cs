using System;

namespace DefaultNamespace
{
    public class FocusModifier : AbstractModifier
    {
        public static string modID = "Focus";

        public FocusModifier(int stackAmount, int turnsLeft)
        {
            ID = modID;
            stackable = true;
            turnBased = true;
            type = Type.BUFF;
            amount = stackAmount;
            turns = turnsLeft;
        }

        public FocusModifier(bool canStack, bool turnBasedModifier, Type modifierType, int stackAmount, int turnsLeft) : base(canStack, turnBasedModifier, modifierType, stackAmount, turnsLeft)
        {
            ID = modID;
        }
        public override int applyModifier(int damage)
        {
            return damage * (2 * amount);
        }

        public override int getKey()
        {
            return 2;
        }
        
        public override void setStrings()
        {
            modifierDescriptions[0] = "Doubles the damage you deal next turn.";
            modifierDescriptions[1] = "";
            modifierDescriptions[2] = "";
        }
        
        public override String setDesc()
        {
            return modifierDescriptions[0];
        }
    }
}