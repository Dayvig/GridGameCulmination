using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Model_Game gameModel;
    public GridManager gridManager;

    public enum GameState
    {
        Neutral,
        CharacterMovement,
        CharacterAttacking,
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
        gridManager = GetComponent<GridManager>();
        currentState = GameState.Neutral;
        currentTurn = Player.Player1;
        ResetCharacterValues(Player.Player1);
        ResetCharacterValues(Player.Player2);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetTurn(Player player)
    {
        ResetCharacterValues(player);
        currentTurn = player;
    }

    public void nextTurn()
    {
        if (currentTurn == Player.Player1)
        {
            ResetCharacterValues(Player.Player2);
            currentTurn = Player.Player2;
        }
        else if (currentTurn == Player.Player2)
        {
            ResetCharacterValues(Player.Player1);
            currentTurn = Player.Player1;
        }
    }

    public void ResetCharacterValues(Player player)
    {
        foreach (var characterBehavior in gridManager.getCharList())
        {
            if (characterBehavior.owner == player)
            {
                characterBehavior.currentMoves = characterBehavior.movesPerTurn;
                characterBehavior.currentAttacks = characterBehavior.attacksPerTurn;
                characterBehavior.onReset();
            }
        }
    }

    public void checkForNextTurn(Player player)
    {
        bool isNext = true;
        foreach (var characterBehavior in gridManager.getCharList())
        {
            if (characterBehavior.owner == player && (characterBehavior.currentMoves > 0 || characterBehavior.currentAttacks > 0))
            {
                Debug.Log("False");
                isNext = false;
            }
        }
        if (isNext)
            nextTurn();
    }
}