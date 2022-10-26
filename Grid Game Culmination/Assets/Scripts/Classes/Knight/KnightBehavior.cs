using DefaultNamespace;
using UnityEngine;

namespace Classes.Knight
{
    public class KnightBehavior : BaseBehavior
    {
        public bool isRescuing;

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
            if (currentMoves <= 0)
            {
                currentMoves++;
            }
            Debug.Log(move);
        }
        
        public override void onMove(GridCell moveTo)
        {
            currentMoves--;
            move = moveTo.movementCount - dash;
            if (move < 0) move = 0;
            if (moveTo.movementCount < dash)
            {
                currentAttacks--;
            }
            if (currentMoves <= 0 && currentAttacks <= 0)
            {
                GlowRen.color = Color.gray;
            }
            else if (currentMoves <= 0)
            {
                GlowRen.color = Color.red;
            }
            manager.checkForNextTurn(owner);
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

            
                Attacks[0] = gameModel.GetComponent<BasicKnightAttack>();
                Attacks[1] = gameModel.GetComponent<Charge>();
                Attacks[2] = gameModel.GetComponent<Challenge>();
                Attacks[3] = gameModel.GetComponent<Excavate>();
        
                currentSelectedAttack = Attacks[0];
                Debug.Assert(currentCell != null, "Character is not on a cell");
            }
        }
    }
