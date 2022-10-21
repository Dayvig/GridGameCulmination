namespace DefaultNamespace
{
    public class AttackBonusModifier : AbstractModifier
    {
        public static string modID = "Attack";
        
        public AttackBonusModifier() : base()
        {
            ID = modID;
            stackable = true;
            turnBased = true;
            type = Type.BUFF;
            amount = 2;
        }
        public AttackBonusModifier(int stackAmount)
        {
            ID = modID;
            stackable = true;
            turnBased = true;
            type = Type.BUFF;
            amount = stackAmount;
        }
        public AttackBonusModifier(int stackAmount, int turnsLeft)
        {
            ID = modID;
            stackable = true;
            turnBased = true;
            type = Type.BUFF;
            amount = stackAmount;
            turns = turnsLeft;
        }
        
        public AttackBonusModifier(bool canStack, bool turnBasedModifier, Type modifierType, int stackAmount, int turnsLeft) : base(canStack, turnBasedModifier, modifierType, stackAmount, turnsLeft)
        {
            ID = modID;
        }
        
        public override int applyModifier(int damage)
        {
            return damage + amount;
        }

        public override int getKey()
        {
            return 0;
        }

        public override void setStrings()
        {
            modifierDescriptions[0] = "Increases damage dealt by ";
            modifierDescriptions[1] = " for ";
            modifierDescriptions[2] = " turns.";
        }
    }
}