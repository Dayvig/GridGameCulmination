namespace DefaultNamespace
{
    public class DefenseBonusModifier : AbstractModifier
    {
        public static string modID = "Defense";
        
        public DefenseBonusModifier() : base()
        {
            ID = modID;
            stackable = true;
            turnBased = true;
            type = Type.BUFF;
            amount = 25;
        }
        public DefenseBonusModifier(int stackAmount)
        {
            ID = modID;
            stackable = true;
            turnBased = true;
            type = Type.BUFF;
            amount = stackAmount;
        }
        public DefenseBonusModifier(int stackAmount, int turnsLeft)
        {
            ID = modID;
            stackable = true;
            turnBased = true;
            type = Type.BUFF;
            amount = stackAmount;
            turns = turnsLeft;
        }
        
        public DefenseBonusModifier(bool canStack, bool turnBasedModifier, Type modifierType, applicationType applyType, int stackAmount, int turnsLeft) : base(canStack, turnBasedModifier, modifierType, applyType, stackAmount, turnsLeft)
        {
            ID = modID;
        }
        
        public override int applyModifier(int damage)
        {
            return damage - amount;
        }

        public override int getKey()
        {
            return 1;
        }

        public override void setStrings()
        {
            modifierDescriptions[0] = "Decreases damage taken by ";
            modifierDescriptions[1] = " for ";
            modifierDescriptions[2] = " turns.";
        }
    }
}