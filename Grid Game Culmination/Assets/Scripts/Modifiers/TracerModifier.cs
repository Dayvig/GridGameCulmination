namespace DefaultNamespace
{
    public class TracerModifier : AbstractModifier
    {
        public static string modID = "Tracer";
        public TracerModifier(int stackAmount, int turnsLeft)
        {
            ID = modID;
            stackable = true;
            turnBased = true;
            type = Type.DEBUFF;
            aType = applicationType.DEFENSIVE;
            amount = stackAmount;
            turns = turnsLeft;
        }
        
        public override int applyModifier(int input)
        {
            return input + 1;
        }

        public override int getKey()
        {
            return 4;
        }

        public override void setStrings()
        {
            modifierDescriptions[0] = "Takes ";
            modifierDescriptions[1] = " more damage from all sources for ";
            modifierDescriptions[2] = " turns.";
        }
    }
}