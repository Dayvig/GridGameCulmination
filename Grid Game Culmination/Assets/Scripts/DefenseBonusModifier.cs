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
        
        public DefenseBonusModifier(bool canStack, bool turnBasedModifier, Type modifierType, int stackAmount, int turnsLeft) : base(canStack, turnBasedModifier, modifierType, stackAmount, turnsLeft)
        {
            ID = modID;
        }
    }
}