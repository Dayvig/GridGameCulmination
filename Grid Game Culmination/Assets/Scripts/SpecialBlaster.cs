namespace DefaultNamespace
{
    public class SpecialBlaster : AbstractAttack
    {
        public SpecialBlaster()
        {
            AttackRange = 3;
            AttackDamage = 4;
            AttackName = "Special Blaster";
            AttackDesc = "A ray that hits an enemy in your column only.";
            ColumnOnly = true;
        }

        public override void use(BaseBehavior initiator, BaseBehavior target)
        {
            initiator.currentAttacks--;
            //attack target

            int damage = initiator.calculateDamage(AttackDamage, target);
            target.HP -= damage;
            target.updateBars();
        }
    }
}