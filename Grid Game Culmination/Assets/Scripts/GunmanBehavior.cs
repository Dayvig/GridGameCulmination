﻿using UnityEngine;

namespace DefaultNamespace
{
    public class GunmanBehavior : BaseBehavior
    {

        public override void Initialize()
        {
            base.Initialize();
            values = gameModel.GetComponent<GunMan>();
            HP = values.hp;
            baseMove = move = values.baseMove;
            name = values.name;
            movesPerTurn = values.movesPerTurn;
            attacksPerTurn = values.attacksPerTurn;
            currentAttacks = attack = values.attacksPerTurn;
            currentMoves = movesPerTurn = values.movesPerTurn;

            
            Attacks[0] = gameModel.GetComponent<BasicGunAttack>();
            Attacks[1] = gameModel.GetComponent<SniperShot>();
            Attacks[2] = gameModel.GetComponent<Escape>();
        
            currentSelectedAttack = Attacks[0];
            Debug.Assert(currentCell != null, "Character is not on a cell");
        }
        
        
    }
}