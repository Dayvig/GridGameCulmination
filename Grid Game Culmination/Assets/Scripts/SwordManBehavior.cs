using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class SwordManBehavior : BaseBehavior
    {
        
        public override void Initialize()
        {
            Attacks[0] = gameModel.GetComponent<BasicGuyAttack>();
            Attacks[1] = gameModel.GetComponent<DaggerThrow>();
        
            currentSelectedAttack = Attacks[0];
            Debug.Assert(currentCell != null, "Character is not on a cell");
        }

        
    }
}