using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
            baseDash = dash = values.baseDash;
            name = values.name;
            movesPerTurn = values.movesPerTurn;
            attacksPerTurn = values.attacksPerTurn;
            currentAttacks = attack = values.attacksPerTurn;
            currentMoves = movesPerTurn = values.movesPerTurn;
            passive = values.passiveText;


            Attacks[0] = GetComponent<BasicGunAttack>();
            Attacks[1] = GetComponent<SniperShot>();
            Attacks[2] = GetComponent<Focus>();
            Attacks[3] = GetComponent<TracerRound>();

            currentSelectedAttack = Attacks[0];
        }
    }
}