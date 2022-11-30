using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

public class ShockAndAwe : AbstractAttack
{
    public override void use(BaseBehavior initiator, BaseBehavior target, bool isOptimal)
    {
        GameManager.Sounds.PlayOneShot(attackSound, GameManager.MasterVolume);
        
        //Decrease the current amount of attacks
        initiator.currentAttacks--;

        //attack target
        int damage;
        if (target.HP >= target.values.hp)
        {
            //debuff target
            ShockModifier newMod = new ShockModifier(buffAmount, buffTurns+1);
            newMod.setStrings();
            target.Modifiers.Add(newMod);
        }
        else
        {
            //debuff target
            ShockModifier newMod = new ShockModifier(buffAmount, buffTurns);
            newMod.setStrings();
            target.Modifiers.Add(newMod);
        }
        
        damage = initiator.calculateDamage(AttackDamage, target, initiator);

        //puts the move on cooldown
        onCooldown = true;
        currentCooldown = cooldown;
        initiator.currentSelectedAttack = initiator.Attacks[0];

        initiator.damageTarget(damage, target);
    }
    
    public override void showSelectedSquares(GridCell origin, bool isBuff)
    {
        origin.showAttackHovered(isBuff);
    }

    //adjacent flood fill, no optimal squares
    public override void showAttackingSquares(GridCell startingCell, int range, AttackType targetingType)
        {
            //Creates a list for all tiles that can be moved to, and adds the starting cell to it.
            List<GridCell> inRangeCells = new List<GridCell>();
            inRangeCells.Add(startingCell);
            //sets the move counter to 0
            int currentMove = 0;

            //tracks the currently selected tiles
            List<GridCell> previousCells = new List<GridCell>();
            previousCells.Add(startingCell);
        
            List<GridCell> surroundingCells = new List<GridCell>();
        
            while (currentMove < range)
            {
                //Looks at the previous cells accessed, then returns all of its accessible neighbors
                foreach (GridCell nextCell in previousCells)
                {
                            for (int i = 0; i < 4; i++)
                            {
                                GridCell n = nextCell.neighbors[i];
                                if (n != null)
                                    surroundingCells.Add(n);
                            }
                }
            
                //adds the accessible neighbors to the cells in range
                inRangeCells.AddRange(surroundingCells);

                //these new accessible neighbors become the previous cells
                previousCells = surroundingCells.Distinct().ToList();
                
                //reduces movement count
                currentMove++;
            }
            
            foreach (GridCell g in inRangeCells)
            {
                if (g.Equals(startingCell))
                {
                    g.isOptimal = false;
                }
                g.isAttackable();
            }
        }
}
