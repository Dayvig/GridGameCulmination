using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public abstract class AbstractAttack : MonoBehaviour

    {
        public int AttackRange;
        public int AttackDamage; 
        public int OptimalDamage;
        public string AttackName;
        public string AttackDesc;
        public int ID;
        public AttackType targeting = AttackType.ORTHOGONAL;
        public bool onCooldown = false;
        public int currentCooldown;
        public int cooldown;
        
        public enum AttackType
        {
            ORTHOGONAL,
            COLUMNONLY,
            ROWONLY
        }

        public abstract void use(BaseBehavior initiator, BaseBehavior target, bool isOptimal);

        public void showAttackingSquares(GridCell startingCell, int range)
        {
            showAttackingSquares(startingCell, range, AttackType.ORTHOGONAL);
        }

        public void reduceCooldown()
        {
            currentCooldown--;
            if (currentCooldown <= 0)
            {
                onCooldown = false;
                currentCooldown = cooldown;
            }
        }

        public void setCooldown(int i)
        {
            currentCooldown = i;
            if (currentCooldown < 0)
            {
                onCooldown = false;
                currentCooldown = cooldown;
            }
        }
        
        public virtual void showAttackingSquares(GridCell startingCell, int range, AttackType targetingType)
        {
            
            Debug.Log(targetingType);
            
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
                    
                    //Adds to surrounding differently depending on attack type
                    switch (targetingType)
                    {
                        case AttackType.ORTHOGONAL:
                            for (int i = 0; i < 4; i++)
                            {
                                GridCell n = nextCell.neighbors[i];
                                if (n != null)
                                    surroundingCells.Add(n);
                            }
                            break;
                        case AttackType.COLUMNONLY:
                            if (nextCell.getNorth() != null)
                                surroundingCells.Add(nextCell.getNorth());
                            if (nextCell.getSouth() != null)
                                surroundingCells.Add(nextCell.getSouth());
                            break;
                        default:
                            foreach (GridCell n in nextCell.neighbors)
                            {
                                if (n != null)
                                    surroundingCells.Add(n);
                            }
                            break;
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
}