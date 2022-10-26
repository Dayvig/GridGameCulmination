using System;

namespace DefaultNamespace
{
    public class ChallengeModifier : AbstractModifier
    {
        public static string modID = "Challenge";
        public BaseBehavior challengeTarget;
        public ChallengeModifier(int stackAmount, int turnsLeft, BaseBehavior challenger)
        {
            ID = modID;
            stackable = true;
            turnBased = true;
            type = Type.DEBUFF;
            aType = applicationType.OFFENSIVE;
            amount = stackAmount;
            turns = turnsLeft;
            challengeTarget = challenger;
        }
        
        public override int applyModifier(int input, BaseBehavior target, BaseBehavior initiator)
        {
            if (!target.Equals(challengeTarget))
            {
                return (int)Math.Floor(input * 0.5f);
            }
            return input;
        }

        public override int getKey()
        {
            return 6;
        }
        
        public override String setDesc()
        {
            return modifierDescriptions[0] + turns + modifierDescriptions[2];
        }

        public override void setStrings()
        {
            modifierDescriptions[0] = "Deals 50% less damage to all targets except "+challengeTarget.name+" for ";
            modifierDescriptions[1] = "";
            modifierDescriptions[2] = " turns.";
        }
    }
}