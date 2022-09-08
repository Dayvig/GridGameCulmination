using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class BaseBehavior : MonoBehaviour
{
    private Model_Game gameModel;
    private GameManager manager;
    private GridManager gridManager;
    private Guy values;
    public int HP;
    public int move;
    public int attack;
    public String name;
    public GridCell currentCell;
    public int currentMoves;
    public int movesPerTurn;
    public int attacksPerTurn;
    public int currentAttacks;
    public SpriteRenderer GlowRen;
    public RectTransform HealthBar;
    public AbstractAttack[] Attacks = new AbstractAttack[5];
    public AbstractAttack currentSelectedAttack;

    public GameManager.Player owner;
    public List<AbstractModifier> Modifiers = new List<AbstractModifier>();
    public List<AbstractModifier> toRemove = new List<AbstractModifier>();
    
    void Start()
    {
        gameModel = GameObject.Find("GameModel").GetComponent<Model_Game>();
        gridManager = GameObject.Find("GameManager").GetComponent<GridManager>();
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        values = gameModel.GetComponent<Guy>();
        HP = values.hp;
        move = values.move;
        name = values.name;
        movesPerTurn = values.movesPerTurn;
        attacksPerTurn = values.attacksPerTurn;
        currentAttacks = attack = values.attacksPerTurn;
        currentMoves = movesPerTurn = values.movesPerTurn;
        Attacks[0] = gameModel.GetComponent<BasicGuyAttack>();
        Attacks[1] = gameModel.GetComponent<SpecialBlaster>();
        
        currentSelectedAttack = Attacks[0];
        Debug.Assert(currentCell != null, "Character is not on a cell");
    }

    void Update()
    {
        
    }

    public void onSelect()
    {
        if (currentMoves <= 0)
        {
            manager.currentState = GameManager.GameState.CharacterAttacking;
            gridManager.showAttackingSquares(this.currentCell, currentSelectedAttack.AttackRange);
            gridManager.currentSelectedAttack = currentSelectedAttack.ID;
        }
        else
        {
            manager.currentState = GameManager.GameState.CharacterMovement;
            gridManager.showMovementSquares(this.currentCell, move);
        }
    }

    public void onMove()
    {
        currentMoves--;
        if (currentMoves <= 0 && currentAttacks <= 0)
        {
            GlowRen.color = Color.gray;
        }
        else if (currentMoves <= 0)
        {
            GlowRen.color = Color.red;
        }
        manager.checkForNextTurn(owner);
    }

    public void onAttack(BaseBehavior target)
    {
       
        
        if (currentMoves <= 0 && currentAttacks <= 0)
        {
            GlowRen.color = Color.gray;
        }
        else if (currentMoves <= 0)
        {
            GlowRen.color = Color.red;
        }
        manager.gridManager.DeselectAll();
        manager.checkForNextTurn(owner);
    }

    public int calculateDamage(int baseDamage, BaseBehavior target)
    {
        return baseDamage;
    }

    public void endTurnModifierCheck()
    {
        for (int i = 0; i < Modifiers.Count; i++)
        {
            //Decrements turn based modifiers
            if (Modifiers[i].turnBased)
            {
                Modifiers[i].turns--;
                if (Modifiers[i].turns <= 0)
                {
                    toRemove.Add(Modifiers[i]);
                }
            }
            //Activates any end of turn modifiers
            
            
        }

        //Removes the mods that have expired
        for (int k = 0; k < toRemove.Count; k++)
        {
            Modifiers.Remove(toRemove[k]);
        }
        toRemove.Clear();
    }

    public void endTurn()
    {
        currentAttacks = 0;
        currentMoves = 0;
        GlowRen.color = Color.gray;
        manager.gridManager.DeselectAll();
        manager.checkForNextTurn(owner);
    }
    
    public void updateBars()
    {
            float hpScale = (float) HP / values.hp;
            HealthBar.localScale = new Vector3(1, hpScale, 1);
    }


    public void onReset()
    {
        GlowRen.color = Color.blue;
    }
}