using System;
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
            currentAttacks = attack = values.attacksPerTurn + 2;
            currentMoves = movesPerTurn = values.movesPerTurn;
            passive = values.passiveText;
            portrait = values.image;

            
            Attacks[0] = GetComponent<ShovelAttack>();
            Attacks[1] = GetComponent<MineThrow>();
            Attacks[2] = GetComponent<Grenade>();
            Attacks[3] = GetComponent<Excavate>();
            Attacks[1].charges = Attacks[1].maxCharges;
            Attacks[3].charges = Attacks[3].maxCharges;
            
            currentSelectedAttack = Attacks[0];
        }
    }
}