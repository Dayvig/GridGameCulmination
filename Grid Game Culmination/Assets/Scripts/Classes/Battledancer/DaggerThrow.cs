using System.Collections.Generic;
using DefaultNamespace;
using UnityEditor;
using UnityEditor.Animations;

namespace Classes.Battledancer
{
    public class DaggerThrow : AbstractAttack, GroundTarget
    {

        public override void use(BaseBehavior initiator, BaseBehavior target, bool isOptimal)
        {
            //Attack target
            int damage = initiator.calculateDamage(isOptimal ? OptimalDamage : AttackDamage, target, initiator);
            initiator.damageTarget(damage, target);
        }

        public override void showSelectedSquares(GridCell origin, bool isBuff)
        {
            origin.showAttackHovered(isBuff, true);
            showDiag(origin, 4, AttackRange, isBuff);
            showDiag(origin, 5, AttackRange, isBuff);
            showDiag(origin, 6, AttackRange, isBuff);
            showDiag(origin, 7, AttackRange, isBuff);

        }

        private void showDiag(GridCell origin, int dir, int range, bool isBuff)
        {
            GridCell cursor = origin;
            for (int i = 0; i < range; i++)
            {
                if (cursor.neighbors[dir] == null)
                {
                    break;
                }
                cursor = cursor.neighbors[dir];
                if (cursor.terrainType != 0)
                {
                    if (i == range - 1)
                    {
                        cursor.showOptimalAttackHovered();
                    }
                    else
                    {
                        cursor.showAttackHovered(isBuff);
                    }
                }
            }
        }
        
        private void hitDiag(GridCell origin, int dir, int range, bool isBuff, BaseBehavior initiator)
        {
            GridCell cursor = origin;
            for (int i = 0; i < range; i++)
            {
                if (cursor.neighbors[dir] == null)
                {
                    break;
                }
                cursor = cursor.neighbors[dir];
                if (cursor.occupant != null)
                {
                    BaseBehavior targetB = cursor.occupant.GetComponent<BaseBehavior>();
                    if (targetB.owner != initiator.owner)
                    {
                        use(initiator, targetB, i == range-1);
                    }
                }
            }
        }

        public void groundUse(BaseBehavior initiator, GridCell target)
        {
            //Decrease the current amount of attacks
            initiator.currentAttacks--;
            
            //moves to cell
            initiator.currentCell.occupant = null;
            initiator.currentCell = target;
            initiator.currentCell.occupant = initiator.gameObject;
            initiator.onDisplace(target);

            //hits all enemies a certain diagonal distance away
            hitDiag(initiator.currentCell, 4, AttackRange, false, initiator);
            hitDiag(initiator.currentCell, 5, AttackRange, false, initiator);
            hitDiag(initiator.currentCell, 6, AttackRange, false, initiator);
            hitDiag(initiator.currentCell, 7, AttackRange, false, initiator);

        }
        
        public override void showAttackingSquares(GridCell startingCell, int range, AttackType targetingType)
        {

            //Creates a list for all tiles that can be moved to, and adds the starting cell to it.
            List<GridCell> inRangeCells = new List<GridCell>();
            inRangeCells.Add(startingCell);
            
            //adds the cells manually
            for (int i = 0; i < 4; i++)
            {
                if (startingCell.neighbors[i] !=null && startingCell.neighbors[i].terrainType != 0 && 
                    startingCell.neighbors[i].occupant == null)
                {
                    inRangeCells.Add(startingCell.neighbors[i]);
                }
            }

            foreach (GridCell g in inRangeCells)
            {
                g.isAttackable(false, true);
            }
        }
        
    }
}