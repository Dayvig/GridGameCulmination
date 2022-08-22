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

    public GameManager.Player owner;
    
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
        Debug.Assert(currentCell != null, "Character is not on a cell");
    }

    void Update()
    {
        
    }

    public void onSelect()
    {
        manager.currentState = GameManager.GameState.CharacterMovement;
        gridManager.showMovementSquares(this.currentCell, move);
    }

    public void onMove()
    {
        currentMoves--;
        if (currentMoves <= 0)
        {
            GlowRen.color = Color.gray;
        }
        manager.checkForNextTurn(owner);
    }

    public void onReset()
    {
        GlowRen.color = Color.blue;
    }
}