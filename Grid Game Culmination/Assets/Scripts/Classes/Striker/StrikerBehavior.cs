using System.Collections;
using System.Collections.Generic;
using Classes.Striker;
using DefaultNamespace;
using UnityEngine;

public class StrikerBehavior : BaseBehavior
{
    public override void Initialize()
    {
        base.Initialize();
        values = gameModel.GetComponent<Striker>();
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

        Attacks[0] = gameModel.GetComponent<BasicStrikerAttack>();
        Attacks[1] = gameModel.GetComponent<SweepFire>();
        Attacks[2] = gameModel.GetComponent<Sprint>();
        Attacks[3] = gameModel.GetComponent<ShockAndAwe>();

        FirstStrikeModifier firstStrike = new FirstStrikeModifier(1);
        firstStrike.setStrings();
        
        Modifiers.Add(firstStrike);

        currentSelectedAttack = Attacks[0];
        Debug.Assert(currentCell != null, "Character is not on a cell");
    }
}
