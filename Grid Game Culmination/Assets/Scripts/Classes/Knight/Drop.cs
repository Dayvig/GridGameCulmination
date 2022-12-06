using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Classes.Knight;
using DefaultNamespace;
using UnityEngine;

public class Drop : AbstractAttack, GroundTarget
{
    public void groundUse(BaseBehavior initiator, GridCell target)
    {
        GameManager.Sounds.PlayOneShot(attackSound, GameManager.MasterVolume);

        KnightBehavior initiatorKnight = (KnightBehavior) initiator;
        
        //Decrease the current amount of attacks
        initiator.currentAttacks--;
        
        //picks up rescued unit
        if (initiatorKnight.hasRescue)
        {   
            initiatorKnight.RescueTarget.currentCell = target;
            initiatorKnight.RescueTarget.currentCell.occupant = initiatorKnight.RescueTarget.gameObject;
            initiatorKnight.RescueTarget.gameObject.SetActive(true);
            initiatorKnight.RescueTarget.pickedUp = false;
            initiatorKnight.hasRescue = false;
            initiatorKnight.RescueTarget = null;
        }

        //puts the move on cooldown
        onCooldown = true;
        currentCooldown = cooldown;
        initiator.currentSelectedAttack = initiator.Attacks[0];
    }

    public override void use(BaseBehavior initiator, BaseBehavior target, bool isOptimal)
    {
        
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
            
        //adds the cells manually
        GridCell n = startingCell.getEast();
        if (n != null)
        {
            inRangeCells.Add(n);
        }

        n = startingCell.getWest();
        if (n != null)
        {
            inRangeCells.Add(n);
        }

        n = startingCell.getSouth();
        if (n != null) 
            inRangeCells.Add(n);
                
        n = startingCell.getNorth();
        if (n != null) 
            inRangeCells.Add(n);

        foreach (GridCell g in inRangeCells)
        {
            g.isAttackable(true);
        }
    } 
}