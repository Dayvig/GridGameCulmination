using System;
using UnityEngine;

namespace DefaultNamespace
{
    public abstract class AbstractModifier
    {
        public enum Type
        {
            BUFF,
            DEBUFF
        }

        public enum applicationType
        {
            OFFENSIVE,
            DEFENSIVE,
            OTHER
        }
        
        public bool stackable;
        public Type type;
        public applicationType aType;
        public int amount;
        public string ID;
        public bool turnBased;
        public int turns;
        public int key;
        public String[] modifierDescriptions = new String[3];
        public bool isTerrainModifier = false;

        public AbstractModifier()
        {
            stackable = false;
            turnBased = true;
            type = Type.BUFF;
            amount = 1;
            ID = "Unknown";
        }

        public AbstractModifier(bool canStack, bool turnBasedModifier, Type modifierType, applicationType applyType, int stackAmount, int turnsLeft)
        {
            stackable = canStack;
            turnBased = turnBasedModifier;
            type = modifierType;
            aType = applyType;
            amount = stackAmount;
            ID = "Unknown";
            turns = turnsLeft;
        }

        public virtual void endTurnTrigger(BaseBehavior target) {}
        public abstract int applyModifier(int input, BaseBehavior target, BaseBehavior initiator);
        public abstract int getKey();
        public abstract void setStrings();
        
        public virtual String setDesc()
        {
            return modifierDescriptions[0] + amount + modifierDescriptions[1] + turns + modifierDescriptions[2];
        }
    }
}