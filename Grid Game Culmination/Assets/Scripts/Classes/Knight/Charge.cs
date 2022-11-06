using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

namespace Classes.Knight
{
    public class Charge : AbstractAttack
    {
        public Model_Game gameModel;
        public GridManager gridManager;
        public void Start()
        {
            gameModel = GameObject.Find("GameModel").GetComponent<Model_Game>();
            gridManager = GameObject.Find("GameManager").GetComponent<GridManager>();
        }
        public override void use(BaseBehavior initiator, BaseBehavior target, bool isOptimal)
        {
            initiator.currentAttacks--;
            
            //puts the move on cooldown
            onCooldown = true;
            currentCooldown = cooldown;
            initiator.currentSelectedAttack = initiator.Attacks[0];

            //Attack target
            int damage = initiator.calculateDamage(isOptimal ? OptimalDamage : AttackDamage, target, initiator);
            damage += (AttackRange-1) - target.currentCell.movementCount;
            target.HP -= damage;
            target.updateBars();
            
            //move into adjacent zone
            for (int i = 0; i < 4; i++)
            {
                if (target.currentCell.neighbors[i].isMovementSelectable)
                {
                    initiator.currentCell.occupant = null;
                    initiator.currentCell = target.currentCell.neighbors[i];
                    initiator.currentCell.occupant = initiator.gameObject;
                }
            }
        }
        
        public override void showSelectedSquares(GridCell origin, bool isBuff)
        {
            origin.showAttackHovered(isBuff);
        }
        
        public void movementUse(BaseBehavior initiator)
        {
            initiator.currentAttacks--;
            
            //puts the move on cooldown
            onCooldown = true;
            currentCooldown = cooldown;
            initiator.currentSelectedAttack = initiator.Attacks[0];
            
        }
        
        public override void showAttackingSquares(GridCell startingCell, int range, AttackType targetingType)
        {
            
            BaseBehavior initiator = gridManager.selectedCharacterBehavior;
            initiator.specialMovement = true;
            findTargetinDirection(0, range, initiator.currentCell, initiator);
            findTargetinDirection(1, range, initiator.currentCell, initiator);
            findTargetinDirection(2, range, initiator.currentCell, initiator);
            findTargetinDirection(3, range, initiator.currentCell, initiator);
        }
        
        private void findTargetinDirection(int i, int range, GridCell startingCell, BaseBehavior initiator)
        {
            int currentMove = 0;
            GridCell cursor = startingCell;

            while (currentMove < range)
            {
                cursor.movementCount = range;
                int movePenaltyToGive = 1;
                if (cursor.terrainType == 2 || cursor.modifiers.Contains(0))
                {
                    movePenaltyToGive = 2;
                }
                range -= movePenaltyToGive;
                GridCell next = cursor.getDirCellFromInt(i, cursor);
                if (next != null && next.terrainType != 0)
                {
                    if (next.occupant != null && next.occupant.GetComponent<BaseBehavior>().owner != initiator.owner)
                    {
                        next.movementCount = range;
                        next.isAttackable();
                        return;
                    }
                    next.canMoveTo(false);
                    cursor = cursor.getDirCellFromInt(i, cursor);
                }
                else
                {
                    break;
                }
            }
        }


        
        
    }
}