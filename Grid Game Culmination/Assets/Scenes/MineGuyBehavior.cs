﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class MineGuyBehavior : BaseBehavior
    {

        public void Start()
        {
        }

        public override void Initialize()
        {
            base.Initialize();
            values = gameModel.GetComponent<MineGuy>();
            HP = values.hp;
            baseMove = move = values.baseMove;
            baseDash = dash = values.baseDash;
            name = values.name;
            movesPerTurn = values.movesPerTurn;
            attacksPerTurn = values.attacksPerTurn;
            currentAttacks = attack = values.attacksPerTurn;
            currentMoves = movesPerTurn = values.movesPerTurn;

            
            Attacks[0] = gameModel.GetComponent<ShovelAttack>();
            Attacks[1] = gameModel.GetComponent<FlySting>();
            Attacks[2] = gameModel.GetComponent<KnockbackPunch>();
            Attacks[3] = gameModel.GetComponent<Brace>();
        
            currentSelectedAttack = Attacks[0];
            Debug.Assert(currentCell != null, "Character is not on a cell");
        }
    }
}