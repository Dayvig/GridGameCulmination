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
    }
}