using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class SwordManBehavior : BaseBehavior
    {

        public void Start()
        {
        }

        public void Update()
        {
            if (!hasModifier("Attack") && HP <= 5)
            {
                AbstractModifier newMod = new AttackBonusModifier(false, false, AbstractModifier.Type.BUFF,
                    AbstractModifier.applicationType.OFFENSIVE, 1, 1);
                newMod.setStrings();
                Modifiers.Add(newMod);
            }

            if (HP > 5 && hasModifier("Attack"))
            {
                Modifiers.Remove(getModifier("Attack"));
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            values = gameModel.GetComponent<SwordMan>();
            HP = values.hp;
            baseMove = move = values.baseMove;
            baseDash = dash = values.baseDash;
            name = values.name;
            movesPerTurn = values.movesPerTurn;
            attacksPerTurn = values.attacksPerTurn;
            currentAttacks = attack = values.attacksPerTurn;
            currentMoves = movesPerTurn = values.movesPerTurn;
            passive = values.passiveText;

            
            Attacks[0] = gameModel.GetComponent<BasicGuyAttack>();
            Attacks[1] = gameModel.GetComponent<FlySting>();
            Attacks[2] = gameModel.GetComponent<KnockbackPunch>();
            Attacks[3] = gameModel.GetComponent<Brace>();
        
            currentSelectedAttack = Attacks[0];
            Debug.Assert(currentCell != null, "Character is not on a cell");
        }
    }
}