using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using DefaultNamespace;
using UnityEngine;

public class SweepFire : AbstractAttack, GroundTarget
{
    public List<GridCell> NorthCone = new List<GridCell>();
    public List<GridCell> EastCone = new List<GridCell>();
    public List<GridCell> SouthCone = new List<GridCell>();
    public List<GridCell> WestCone = new List<GridCell>();
    public List<GridCell> HoveredCone;
    public List<GridCell> Storage = new List<GridCell>();

    public override void use(BaseBehavior initiator, BaseBehavior target, bool isOptimal)
    {
        //Attack target
        int damage = initiator.calculateDamage(isOptimal ? OptimalDamage : AttackDamage, target, initiator);
        initiator.damageTarget(damage, target);
    }
    
    public void groundUse(BaseBehavior initiator, GridCell target)
    {
        //Decrease the current amount of attacks
        initiator.currentAttacks--;
            
        //puts the move on cooldown
        onCooldown = true;
        currentCooldown = cooldown;
        initiator.currentSelectedAttack = initiator.Attacks[0];
        
        //searches the cone and hits all targets in the cone
        if (HoveredCone != null)
        {
            foreach (GridCell g in HoveredCone)
            {
                if (g.occupant != null)
                {
                    BaseBehavior enemy = g.occupant.GetComponent<BaseBehavior>();
                    if (enemy.owner != initiator.owner)
                    {
                        use(initiator, enemy, g.isOptimal);
                    }
                }
            }
        }
    }
    
    public override void showSelectedSquares(GridCell origin, bool isBuff)
    {
        HoveredCone = null;
        if (NorthCone.Contains(origin))
        {
            foreach (GridCell g in NorthCone)
            {
                g.showAttackHovered(isBuff);
            }

            HoveredCone = NorthCone;
        }
        else if (EastCone.Contains(origin))
        {
            foreach (GridCell g in EastCone)
            {
                g.showAttackHovered(isBuff);
            }

            HoveredCone = EastCone;
        }
        else if (SouthCone.Contains(origin))
        {
            foreach (GridCell g in SouthCone)
            {
                g.showAttackHovered(isBuff);
            }

            HoveredCone = SouthCone;
        }
        else if (WestCone.Contains(origin))
        {
            foreach (GridCell g in WestCone)
            {
                g.showAttackHovered(isBuff);
            }
            HoveredCone = WestCone;
        }
    }
    
    public override void showAttackingSquares(GridCell startingCell, int range, AttackType targetingType)
        {
            SouthCone.Clear();
            NorthCone.Clear();
            EastCone.Clear();
            WestCone.Clear();

            List<GridCell> NextNorth = new List<GridCell>();
            List<GridCell> NextEast = new List<GridCell>();
            List<GridCell> NextSouth = new List<GridCell>();
            List<GridCell> NextWest = new List<GridCell>();

            //Creates a list for all tiles that can be moved to, and adds the starting cell to it.
            List<GridCell> inRangeCells = new List<GridCell>();
            inRangeCells.Add(startingCell);
            //sets the move counter to 0
            int currentMove = 0;
            
            NorthCone.Add(startingCell);
            EastCone.Add(startingCell);
            WestCone.Add(startingCell);
            SouthCone.Add(startingCell);
            NextNorth.Add(startingCell);
            NextEast.Add(startingCell);
            NextWest.Add(startingCell);
            NextSouth.Add(startingCell);

            while (currentMove < range)
            {
                //looks at each in the northern cone and adds to it. Repeats for east and west and south.
                addNewCells(NextNorth, NorthCone, currentMove, range, 0);
                addNewCells(NextEast, EastCone, currentMove, range, 1);
                addNewCells(NextSouth, SouthCone, currentMove, range, 2);
                addNewCells(NextWest, WestCone, currentMove, range, 3);

                //adds the accessible neighbors to the cells in range
                inRangeCells.AddRange(NorthCone);
                inRangeCells.AddRange(EastCone);
                inRangeCells.AddRange(WestCone);
                inRangeCells.AddRange(SouthCone);

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

    //0 N, 1 E, 2 S, 3 W, 4 NE 5 NW 6 SW 7 SE
    private void addNewCells(List<GridCell> dir, List<GridCell> cone, int currentMove, int range, int direction)
    {
        foreach (GridCell nextCell in dir)
        {
            foreach (GridCell g in getNewCellsFromDirection(direction, nextCell)){
                if (g != null)
                {
                    Storage.Add(g);
                }
            }
        }
        if (currentMove == range - 2)
        {
            foreach (GridCell g in Storage)
            {
                if (!dir.Contains(g))
                    g.isOptimal = true;
            }
        }
        dir.Clear();
        dir.AddRange(Storage);
        foreach (GridCell g in dir)
        {
            if (!cone.Contains(g))
                cone.Add(g);
        }
        Storage.Clear();
    }

    private GridCell[] getNewCellsFromDirection(int direction, GridCell origin)
    {
        switch (direction)
        {
            case 0:
                return origin.allNorthFacing(origin);
            case 1:
                return origin.allEastFacing(origin);
            case 2:
                return origin.allSouthFacing(origin);
            case 3:
                return origin.allWestFacing(origin);
            default:
                Debug.Log("What is you doing");
                return null;
        }
    }

}
