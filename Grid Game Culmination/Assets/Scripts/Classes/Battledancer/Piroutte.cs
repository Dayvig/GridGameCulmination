using System.Collections.Generic;
using DefaultNamespace;
using UnityEditor;

namespace Classes.Battledancer
{
    public class Piroutte : AbstractAttack, GroundTarget
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
            for (int i = 0; i < 8; i++)
            {
                if (origin.neighbors[i] != null && origin.neighbors[i].terrainType != 0)
                    origin.neighbors[i].showAttackHovered(isBuff);
            }
        }

        public void groundUse(BaseBehavior initiator, GridCell target)
        {
            GameManager.Sounds.PlayOneShot(attackSound, GameManager.MasterVolume);

            //Decrease the current amount of attacks
            initiator.currentAttacks--;
            
            //moves to cell
            initiator.currentCell.occupant = null;
            initiator.currentCell = target;
            initiator.currentCell.occupant = initiator.gameObject;
            initiator.onDisplace(target);

            //hits all enemies around
            for (int i = 0; i < 8; i++)
            {
                if (target.neighbors[i] != null && target.neighbors[i].occupant != null)
                {
                    BaseBehavior targetB = target.neighbors[i].occupant.GetComponent<BaseBehavior>();
                    if (targetB.owner != initiator.owner)
                        use(initiator, targetB, false);
                }
            }
            
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