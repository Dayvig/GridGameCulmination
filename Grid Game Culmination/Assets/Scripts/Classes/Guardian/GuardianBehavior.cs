using System.Collections;
using System.Collections.Generic;
using Classes.Battledancer;
using Classes.Guardian;
using DefaultNamespace;
using UnityEngine;

public class GuardianBehavior : BaseBehavior
{
    public override void Initialize()
    {
        base.Initialize();
        values = gameModel.GetComponent<Guardian>();
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

        Attacks[0] = GetComponent<BasicBash>();
        BasicBash tmp = (BasicBash) Attacks[0];
        tmp.facingUp = (owner == GameManager.Player.Player1);
        Attacks[1] = GetComponent<ShieldBash>();
        Attacks[2] = GetComponent<ShieldBlock>();
        Attacks[3] = GetComponent<Rally>();
        
        currentSelectedAttack = Attacks[0];

    }

    public override void onSpawn()
    {
        InitAttack += ApplyGuard;
    }
    void ApplyGuard(BaseBehavior initiator)
    {
        //if attack is initiated by opposing team
        if (initiator.owner != this.owner)
        {
            foreach (BaseBehavior b in gridManager.MasterGrid.getAllCharacters())
            {
                if (b.owner != initiator.owner)
                {
                    int pos = checkDir(initiator);
                    if (isGuardianTarget(pos, b))
                    {
                        b.redirectTo = this;
                        b.updateBars();
                        break;
                    }
                }
            }
        }
    }

    bool isGuardianTarget(int dir, BaseBehavior target)
    {
        //cannot guard self
        if (target == this)
        {
            return false;
        }
        //too far
        if (Mathf.Abs(target.currentCell.row - currentCell.row) > 2 ||
            Mathf.Abs(target.currentCell.column - currentCell.column) > 2)
        {
            return false;
        }
        //must be behind
        Debug.Log(dir);
        switch (dir)
        {
            case 0:
                return target.currentCell.row < currentCell.row;
            case 1:
                return target.currentCell.row > currentCell.row;
            case 2:
                return target.currentCell.column < currentCell.column;
            case 3:
                return target.currentCell.column > currentCell.column;
            case 4:
                return target.currentCell.row < currentCell.row && target.currentCell.column < currentCell.column;
            case 5:
                return target.currentCell.row > currentCell.row && target.currentCell.column < currentCell.column;
            case 6:
                return target.currentCell.row < currentCell.row && target.currentCell.column > currentCell.column;
            case 7:
                return target.currentCell.row > currentCell.row && target.currentCell.column > currentCell.column;
            default:
                return false;
        }
    }

    int checkDir(BaseBehavior toCheck)
    {
        int xDiff = toCheck.currentCell.column - currentCell.column;
        int yDiff = toCheck.currentCell.row - currentCell.row;
        Debug.Log(xDiff+" "+yDiff);
        if (Mathf.Abs(xDiff) == Mathf.Abs(yDiff))
        {
            //Both positive: North East
            if (xDiff > 0 && yDiff > 0)
            {
                return 4;
            }
            
            //Row negative column positive: South East
            if (xDiff > 0 && yDiff < 0)
            {
                return 5;
            }
            
            //Row positive column negative: North West
            if (xDiff < 0 && yDiff > 0)
            {
                return 6;
            }
            
            //both negative: South West
            if (xDiff < 0 && yDiff < 0)
            {
                return 7;
            }
        }
        else
        {
            //row positive: North
            if (yDiff > 0)
            {
                return 0;
            }

            //row negative: south
            if (yDiff < 0)
            {
                return 1;
            }
            
            //col positive: East
            if (xDiff > 0)
            {
                return 2;
            }

            //row negative: West
            if (xDiff < 0)
            {
                return 3;
            }
        }
        
        return -1;
    }
}