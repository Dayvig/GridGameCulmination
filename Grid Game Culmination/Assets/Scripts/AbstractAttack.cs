using UnityEngine;

namespace DefaultNamespace
{
    public class AbstractAttack : MonoBehaviour

    {
        public int AttackRange;
        public int AttackDamage; 
        public string AttackName;
        public string AttackDesc;
        public bool ColumnOnly = false;
    }
}