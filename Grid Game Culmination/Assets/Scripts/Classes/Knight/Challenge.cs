using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

public class Challenge : AbstractAttack
{
    public override void use(BaseBehavior initiator, BaseBehavior target, bool isOptimal)
    {
        //Decrease the current amount of attacks
        initiator.currentAttacks--;
        
        //adds challenge debuff
        ChallengeModifier newMod = new ChallengeModifier(buffAmount, isOptimal ? buffTurns+2 : buffTurns+1, initiator);
        newMod.setStrings();
        target.Modifiers.Add(newMod);

        
        //puts the move on cooldown
        onCooldown = true;
        currentCooldown = cooldown;
        initiator.currentSelectedAttack = initiator.Attacks[0];

    }
    
    public override void showSelectedSquares(GridCell origin, bool isBuff)
    {
        origin.showAttackHovered(isBuff);
    }
    
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
                            for (int i = 0; i < 8; i++)
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
            
            setOptimalCells(0, range, startingCell);

            foreach (GridCell g in inRangeCells)
            {
                if (g.Equals(startingCell))
                {
                    g.isOptimal = false;
                }
                g.isAttackable();
            }

        }
    
    private void setOptimalCells(int i, int range, GridCell startingCell)
    {
        int currentMove = 0;
        GridCell cursor = startingCell;

        while (currentMove <= range)
        {
            cursor.isOptimal = true;
            if (cursor.getDirCellFromInt(i, cursor) != null)
            {
                cursor = cursor.getDirCellFromInt(i, cursor);
                currentMove++;
            }
            else
            {
                break;
            }
        }
    }
}
