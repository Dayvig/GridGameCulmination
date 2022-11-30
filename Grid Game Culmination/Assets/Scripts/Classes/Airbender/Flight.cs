using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class Flight : AbstractAttack, GroundTarget
{
    private GridManager gridManager;
    public void Start()
    {
        gridManager = GameObject.Find("GameManager").GetComponent<GridManager>();
    }
    public override void use(BaseBehavior initiator, BaseBehavior target, bool isOptimal)
    {
    }

    public override void showSelectedSquares(GridCell origin, bool isBuff)
    {
        origin.showAttackHovered(true);
    }

    public void groundUse(BaseBehavior initiator, GridCell target)
    {
        GameManager.Sounds.PlayOneShot(attackSound, GameManager.MasterVolume);

        //puts the move on cooldown
        onCooldown = true;
        currentCooldown = cooldown;
        initiator.currentSelectedAttack = initiator.Attacks[0];
        
        //moves to cell
        initiator.currentCell.occupant = null;
        initiator.currentCell = target;
        initiator.currentCell.occupant = initiator.gameObject;
        initiator.onDisplace(target);
    }
    
    public override void showAttackingSquares(GridCell startingCell, int range, AttackType targetingType)
    {
        TacticsGrid grid = gridManager.MasterGrid;
        GridCell targetCell;
        for (int rowCursor = 0; rowCursor < grid.contents.Count; rowCursor++)
        {
            for (int colCursor = 0; colCursor < grid.contents[rowCursor].contents.Count; colCursor++)
            {
                targetCell = grid.contents[rowCursor].contents[colCursor];
                if (targetCell != startingCell)
                    targetCell.isAttackable(true);
            }
        }        
    }

    
}
