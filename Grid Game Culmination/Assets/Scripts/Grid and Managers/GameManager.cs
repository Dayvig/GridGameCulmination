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
        GameOver
    }

    public enum Player
    {
        Player1,
        Player2,
        None
    }

    public GameState currentState;
    public Player currentTurn;
    public Player winner;

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
                characterBehavior.move = characterBehavior.baseMove;
                foreach (AbstractAttack a in characterBehavior.Attacks)
                {
                    if (a != null && a.onCooldown)
                    {
                        a.reduceCooldown();
                    }
                }
                characterBehavior.endTurnModifierCheck();
                characterBehavior.onReset();
            }
        }
    }

    public void checkForNextTurn(Player player)
    {
        bool Player1HasCharacters = false;
        bool Player2HasCharacters = false;
        foreach (var characterBehavior in gridManager.getCharList())
            {
                if (characterBehavior.owner == Player.Player1)
                    Player1HasCharacters = true;
                else
                {
                    Player2HasCharacters = true;
                }
            }

            if (!Player1HasCharacters && Player2HasCharacters)
            {
                winner = Player.Player2;
                currentState = GameState.GameOver;
                return;
            }
            if (!Player2HasCharacters && Player1HasCharacters)
            {
                winner = Player.Player2;
                currentState = GameState.GameOver;
                return;
            }
            if (!Player1HasCharacters && !Player2HasCharacters)
            {
                //tie
                currentState = GameState.GameOver;
                return;
            }
        
        bool isNext = true;
        foreach (var characterBehavior in gridManager.getCharList())
        {
            if (characterBehavior.owner == player && (characterBehavior.currentMoves > 0 || characterBehavior.currentAttacks > 0))
            {
                isNext = false;
            }
        }
        if (isNext)
            nextTurn();
    }
    
    
    public void Replay()
    {
        gridManager.MasterGrid.destroyAllCharacters();
        gridManager.MasterGrid.DeselectAll();
        gridManager.displayIndex = 0;
        gridManager.addNewCharacter(Instantiate(gameModel.SwordGuy), 0, 4, GameManager.Player.Player1);
        gridManager.addNewCharacter(Instantiate(gameModel.Guy2), 9, 4, GameManager.Player.Player2);
        ResetCharacterValues(Player.Player1);
        ResetCharacterValues(Player.Player2);
        currentTurn = Player.Player1;
        currentState = GameState.Neutral;
        
    }
}