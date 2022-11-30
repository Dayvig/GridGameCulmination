using UnityEngine;

namespace DefaultNamespace
{
    public class Rally : AbstractAttack, GroundTarget
    {
        public override void use(BaseBehavior initiator, BaseBehavior target, bool isOptimal)
        {
            AttackBonusModifier newMod = new AttackBonusModifier(buffAmount, buffTurns);
            newMod.setStrings();
            //buff target
            target.Modifiers.Add(newMod);
        }
        
        public override void showSelectedSquares(GridCell origin, bool isBuff)
        {
            origin.showAttackHovered(true);
            for (int i = 0; i < 8; i++)
            {
                origin.neighbors[i].showAttackHovered(true);
            }
        }

        public override void showAttackingSquares(GridCell startingCell, int range, AttackType targetingType)
        {
            startingCell.isAttackable(true);
        }

        public void groundUse(BaseBehavior initiator, GridCell target)
        {
            //Decrease the current amount of attacks
            initiator.currentAttacks--;

            //puts the move on cooldown
            onCooldown = true;
            currentCooldown = cooldown;
            initiator.currentSelectedAttack = initiator.Attacks[0];

            for (int i = 0; i < 8; i++)
            {
                if (initiator.currentCell.neighbors[i] != null && initiator.currentCell.neighbors[i].occupant != null)
                {
                    BaseBehavior targetB = initiator.currentCell.neighbors[i].occupant.GetComponent<BaseBehavior>();
                    if (targetB.owner == initiator.owner)
                        use(initiator, targetB, false);
                }
            }
            use(initiator, initiator, false);
        }
    }
}