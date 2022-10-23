namespace DefaultNamespace
{
    public class AccuracyModifier : AbstractModifier
    {
        public static string modID = "Accuracy";

        public AccuracyModifier() : base()
        {
            ID = modID;
            stackable = true;
            turnBased = false;
            type = Type.BUFF;
            aType = applicationType.OFFENSIVE;
            amount = 5;
        }
        public AccuracyModifier(int stackAmount)
        {
            ID = modID;
            stackable = true;
            turnBased = false;
            type = Type.BUFF;
            aType = applicationType.OFFENSIVE;
            amount = stackAmount;
        }
        public AccuracyModifier(int stackAmount, int turnsLeft)
        {
            ID = modID;
            stackable = true;
            turnBased = true;
            type = Type.BUFF;
            aType = applicationType.OFFENSIVE;
            amount = stackAmount;
            turns = turnsLeft;
        }
        
        public AccuracyModifier(bool canStack, bool turnBasedModifier, Type modifierType, applicationType applyType, int stackAmount, int turnsLeft) : base(canStack, turnBasedModifier, modifierType, applyType, stackAmount, turnsLeft)
        {
            ID = modID;
        }
        
        public override int applyModifier(int damage)
        {
            return damage * (2 * amount);
        }

        public override int getKey()
        {
            return 3;
        }

        public override void setStrings()
        {
            modifierDescriptions[0] = "";
            modifierDescriptions[1] = "";
            modifierDescriptions[2] = "";
        }
    }
}