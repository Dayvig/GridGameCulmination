using UnityEngine;

namespace DefaultNamespace
{
    public abstract class AbstractAttack : MonoBehaviour

    {
        public int AttackRange;
        public int AttackDamage; 
        public string AttackName;
        public string AttackDesc;
        public bool ColumnOnly = false;
        public int ID;

        public abstract void use(BaseBehavior initiator, BaseBehavior target);
    }
}