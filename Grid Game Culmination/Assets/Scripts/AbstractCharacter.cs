using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class AbstractCharacter : MonoBehaviour
    {
        public int hp;
        public int baseMove;
        public int attacksPerTurn;
        public int movesPerTurn;
        public int baseDash;
        
        public String name;
        public Sprite image;

        public AbstractAttack[] Attacks = new AbstractAttack[5];
    }
}