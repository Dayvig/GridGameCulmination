using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Model_Game gameModel;

    public enum GameState
    {
        Neutral,
        CharacterMovement,
    }

    public enum Player
    {
        Player1,
        Player2,
        None
    }

    public GameState currentState;
    public Player currentTurn;
    
    // Start is called before the first frame update
    void Start()
    {
        gameModel = GameObject.Find("GameModel").GetComponent<Model_Game>();
        currentState = GameState.Neutral;
        currentTurn = Player.Player1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTurn(Player player)
    {
        currentTurn = player;
    }

    public void nextTurn()
    {
        if (currentTurn == Player.Player1)
        {
            currentTurn = Player.Player2;
        }
        else if (currentTurn == Player.Player2)
        {
            currentTurn = Player.Player1;
        }
    }
}