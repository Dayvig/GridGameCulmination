using UnityEngine;

namespace DefaultNamespace
{
    public class GunmanBehavior : BaseBehavior
    {

        public override void Initialize()
        {
            values = gameModel.GetComponent<GunMan>();
            HP = values.hp;
            move = values.move;
            name = values.name;
            movesPerTurn = values.movesPerTurn;
            attacksPerTurn = values.attacksPerTurn;
            currentAttacks = attack = values.attacksPerTurn;
            currentMoves = movesPerTurn = values.movesPerTurn;

            
            Attacks[0] = gameModel.GetComponent<BasicGunAttack>();
            Attacks[1] = gameModel.GetComponent<SniperShot>();
        
            currentSelectedAttack = Attacks[0];
            Debug.Assert(currentCell != null, "Character is not on a cell");
        }
        
        
    }
}