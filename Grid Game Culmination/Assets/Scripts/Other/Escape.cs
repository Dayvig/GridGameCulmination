using UnityEngine;

namespace DefaultNamespace
{
    public class Escape : AbstractAttack
    {
        
        public override void use(BaseBehavior initiator, BaseBehavior target, bool isOptimal)
        {
            //Decrease the current amount of attacks
            initiator.currentAttacks--;

            //puts the move on cooldown
            onCooldown = true;
            currentCooldown = cooldown;
            initiator.currentSelectedAttack = initiator.Attacks[0];

            //buff target
            target.Modifiers.Add(new DefenseBonusModifier(buffAmount, buffTurns));
            
            //activate buff Display
            
            
            
            //grant extra movement
            target.GlowRen.color = Color.blue;
            if (target.currentMoves <= 0)
            {
                target.currentMoves++;
                target.move = 3;
            }
            else
            {
                target.currentMoves++;
            }
        }

        public override void showAttackingSquares(GridCell startingCell, int range, AttackType targetingType)
        {
            startingCell.isAttackable(true);
        }
        
    }
}