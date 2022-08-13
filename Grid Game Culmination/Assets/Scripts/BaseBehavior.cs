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
    public String name;
    public GridCell currentCell;

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
        Debug.Assert(currentCell != null, "Character is not on a cell");
    }

    void Update()
    {
        HP = values.hp;
        move = values.move;
        name = values.name;
    }

    public void onSelect()
    {
        manager.currentState = GameManager.GameState.CharacterMovement;
        gridManager.showMovementSquares(this.currentCell, move);
    }
}