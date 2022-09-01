using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class AbstractCharacter : MonoBehaviour
    {
        public int hp;
        public int move;
        public int attacksPerTurn;
        public int movesPerTurn;
        public int attackRange;
        
        public String name;
        public Sprite image;

        public AbstractAttack[] Attacks = new AbstractAttack[5];
    }
}