using DefaultNamespace;
using UnityEngine;

namespace Modifiers
{
    public class ShieldBlockModifier : AbstractModifier
    {
        public int direction;
        public static string modID = "Shield Block";
        private GridManager gridManager;
        
        public void Start()
        {
            gridManager = GameObject.Find("GameManager").GetComponent<GridManager>();
        }
        public ShieldBlockModifier(int stackAmount, int turnsLeft)
        {
            ID = modID;
            stackable = true;
            turnBased = true;
            type = Type.BUFF;
            aType = applicationType.DEFENSIVE;
            amount = stackAmount;
            turns = turnsLeft;
        }
        
        public override int applyModifier(int input, BaseBehavior target, BaseBehavior initiator)
        {
            int final = input;
            if (isShieldAffected(direction, target, initiator.currentCell))
            {
                final -= amount;
                return final < 0 ? 0 : final;
            }
            return final < 0 ? 0 : final;
        }
        
        bool isShieldAffected(int dir, BaseBehavior target, GridCell initiatorCell)
        {
            BaseBehavior guardian = target;
            //must be behind
            Debug.Log("Blocking "+directionFromInt(direction));
            switch (dir)
            {
                case 0:
                    return initiatorCell.row < guardian.currentCell.row;
                case 1:
                    return initiatorCell.row > guardian.currentCell.row;
                case 2:
                    return initiatorCell.column < guardian.currentCell.column;
                case 3:
                    return initiatorCell.column > guardian.currentCell.column;
                case 4:
                    return initiatorCell.row < guardian.currentCell.row && initiatorCell.column < guardian.currentCell.column;
                case 5:
                    return initiatorCell.row > guardian.currentCell.row && initiatorCell.column > guardian.currentCell.column;
                case 6:
                    return initiatorCell.row < guardian.currentCell.row && initiatorCell.column < guardian.currentCell.column;
                case 7:
                    return initiatorCell.row > guardian.currentCell.row && initiatorCell.column > guardian.currentCell.column;
                default:
                    return false;
            }
        }


        public override int getKey()
        {
            return 12;
        }

        public string directionFromInt(int dir)
        {
            switch (dir)
            {
                case 0:
                    return "South";
                case 1:
                    return "North";
                case 2:
                    return "West";
                case 3:
                    return "East";
                case 4:
                    return "Southwest";
                case 5:
                    return "Northwest";
                case 6:
                    return "Southeast";
                case 7:
                    return "Northeast";
                default:
                    return "Aaaaa";
            }
        }

        public override void setStrings()
        {
            modifierDescriptions[0] = "Takes ";
            modifierDescriptions[1] = " less damage from the "+directionFromInt(direction)+" for ";
            modifierDescriptions[2] = " turns.";
        }
    }
}