using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Classes.Airbender;
using Classes.Battledancer;
using DefaultNamespace;
using UnityEngine;

public class AirbenderBehavior : BaseBehavior
{
    public override void Initialize()
    {
        base.Initialize();
        values = gameModel.GetComponent<Airbender>();
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

        Attacks[0] = GetComponent<BasicBlast>();
        Attacks[1] = GetComponent<SpinBlast>();
        Attacks[2] = GetComponent<Flight>();
        Attacks[3] = GetComponent<WindManip>();
        
        currentSelectedAttack = Attacks[0];
    }

    public override void showMovementSquares(GridCell startingCell, int movementValue, int dashValue)
    {

        gridManager.MasterGrid.wipeTimesSeen();
        //Creates a list for all tiles that can be moved to, and adds the starting cell to it.
        List<GridCell> inRangeCells = new List<GridCell>();
        List<GridCell> surroundingCells = new List<GridCell>();
        List<GridCell> prevCells = new List<GridCell>();
        startingCell.movementCount = movementValue + dashValue;
        inRangeCells.Add(startingCell);
        prevCells.Add(startingCell);
        bool stop = false;

        do
        {
            gridManager.count++;
            stop = true;
            foreach (GridCell g in prevCells)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (g.neighbors[i] != null)
                    {
                        int movePenaltyToGive = 1;

                        if (g.neighbors[i].movementCount < g.movementCount)
                        {
                            if (g.neighbors[i].isNumModified)
                            {
                                if (g.neighbors[i].movementCount < g.movementCount - movePenaltyToGive)
                                {
                                    g.neighbors[i].movementCount = g.movementCount - movePenaltyToGive;
                                    surroundingCells.Add(g.neighbors[i]);
                                }
                            }
                            else
                            {
                                g.neighbors[i].movementCount = g.movementCount - movePenaltyToGive;
                                g.neighbors[i].isNumModified = true;
                                surroundingCells.Add(g.neighbors[i]);
                            }
                        }

                        if (g.neighbors[i].movementCount >= 0 && g.neighbors[i].occupant == null)
                        {
                            inRangeCells.Add(g.neighbors[i]);
                            stop = false;
                        }
                    }
                }
            }

            prevCells.Clear();
            prevCells = surroundingCells.Distinct().ToList();

            if (gridManager.count > (movementValue + dashValue) * 2)
            {
                stop = true;
            }
        } while (!stop);

        foreach (GridCell g in inRangeCells)
        {
            g.canMoveTo(g.movementCount > dashValue - 1);
        }
    }
}