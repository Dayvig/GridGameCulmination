namespace DefaultNamespace
{
    public class BasicGuyAttack : AbstractAttack
    {
        public BasicGuyAttack()
        {
            AttackRange = 1;
            AttackDamage = 2;
            AttackName = "Basic Attack";
            AttackDesc = "Hit the enemy with your fists.";
        }

        public override void use(BaseBehavior initiator, BaseBehavior target)
        {
            //Decrease the current amount of attacks
            initiator.currentAttacks--;
            
            //attack target
            int damage = initiator.calculateDamage(AttackDamage, target);
            target.HP -= damage;
            target.updateBars();
        }
    }
}