﻿using DefaultNamespace;
using UnityEngine;

namespace Classes.Knight
{
    public class KnightBehavior : BaseBehavior
    {
        public bool hasRescue = false;
        public BaseBehavior RescueTarget;

        public override void onAttack(BaseBehavior target, bool isOptimal)
        {
            //launches the attack
            currentSelectedAttack.use(this, target, isOptimal);
        
            //sets the correct glow
            if (currentMoves <= 0 && currentAttacks <= 0)
            {
                GlowRen.color = Color.gray;
            }
            else if (currentMoves <= 0)
            {
                GlowRen.color = Color.red;
            }
        
            //resets the gamestate and activates movement
            manager.gridManager.DeselectAll();
            
            GlowRen.color = Color.blue;
            if (currentMoves <= 0 && move > 0)
            {
                currentMoves++;
            }
            if (checkForEndTurn())
            {
                endTurn();
            }
        }
        
        public override void onMove(GridCell moveTo)
        {
            onDisplace(moveTo);
            currentMoves--;
            if (moveTo.movementCount < dash)
            {
                currentAttacks--;
            }
            move = moveTo.movementCount - dash;
            dash = 0;
            if (move < 0) move = 0;
            if (currentMoves <= 0 && currentAttacks <= 0)
            {
                GlowRen.color = Color.gray;
            }
            else if (currentMoves <= 0)
            {
                GlowRen.color = Color.red;
            }
            if (checkForEndTurn())
            {
                endTurn();
            }
        }

        public override void onSpecialMovement()
        {
            Charge chargeAttack = (Charge) currentSelectedAttack;
            chargeAttack.movementUse(this);
            GlowRen.color = Color.blue;
            if (currentMoves <= 0)
            {
                currentMoves++;
            }
            manager.gridManager.DeselectAll();
            manager.currentState = GameManager.GameState.Neutral;
            base.onSpecialMovement();
        }
        
        public override void Initialize()
            {
                base.Initialize();
                values = gameModel.GetComponent<KnightGuy>();
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

                Attacks[0] = GetComponent<BasicKnightAttack>();
                Attacks[1] = GetComponent<Charge>();
                Attacks[2] = GetComponent<Challenge>();
                Attacks[3] = GetComponent<Rescue>();
        
                currentSelectedAttack = Attacks[0];
            }
        
        public override void onReset()
        {
            GlowRen.color = Color.blue;
            specialMovement = false;
            if (!hasRescue)
            {
                Attacks[4] = null;
            }
            else
            {
                Attacks[4] = GetComponent<Drop>();
            }

            dash = values.baseDash;
        }
    }
}
