using System;

namespace DefaultNamespace
{
    public abstract class AbstractModifier
    {
        public enum Type
        {
            BUFF,
            DEBUFF
        }
        
        public bool stackable;
        public Type type;
        public int amount;
        public string ID;
        public bool turnBased;
        public int turns;

        public AbstractModifier()
        {
            stackable = false;
            turnBased = true;
            type = Type.BUFF;
            amount = 1;
            ID = "Unknown";
        }

        public AbstractModifier(bool canStack, bool turnBasedModifier, Type modifierType, int stackAmount, int turnsLeft)
        {
            stackable = canStack;
            turnBased = turnBasedModifier;
            type = modifierType;
            amount = stackAmount;
            ID = "Unknown";
            turns = turnsLeft;
        }

        public void endTurnTrigger() {}
    }
}