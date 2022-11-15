using System.Collections;
using System.Collections.Generic;
using Classes.Battledancer;
using DefaultNamespace;
using UnityEngine;

public class BattleDancerBehavior : BaseBehavior
{
    public override void Initialize()
    {
        base.Initialize();
        values = gameModel.GetComponent<BattleDancer>();
        HP = values.hp;
        baseMove = move = values.baseMove;
        baseDash = dash = values.baseDash;
        name = values.name;
        movesPerTurn = values.movesPerTurn;
        attacksPerTurn = values.attacksPerTurn;
        currentAttacks = attack = values.attacksPerTurn;
        currentMoves = movesPerTurn = values.movesPerTurn;
        passive = values.passiveText;
        portrait = values.image;

        Attacks[0] = gameModel.GetComponent<Piroutte>();
        Attacks[1] = gameModel.GetComponent<Inspiring>();
        Attacks[2] = gameModel.GetComponent<DaggerThrow>();
        Attacks[3] = gameModel.GetComponent<Cadence>();
        
        currentSelectedAttack = Attacks[0];
        Debug.Assert(currentCell != null, "Character is not on a cell");
    }
}