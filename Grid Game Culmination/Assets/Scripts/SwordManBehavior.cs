using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class SwordManBehavior : BaseBehavior
    {
        
        public override void Initialize()
        {
            values = gameModel.GetComponent<SwordMan>();
            HP = values.hp;
            move = values.move;
            name = values.name;
            movesPerTurn = values.movesPerTurn;
            attacksPerTurn = values.attacksPerTurn;
            currentAttacks = attack = values.attacksPerTurn;
            currentMoves = movesPerTurn = values.movesPerTurn;

            
            Attacks[0] = gameModel.GetComponent<BasicGuyAttack>();
            Attacks[1] = gameModel.GetComponent<DaggerThrow>();
        
            currentSelectedAttack = Attacks[0];
            Debug.Assert(currentCell != null, "Character is not on a cell");
        }

        
    }
}