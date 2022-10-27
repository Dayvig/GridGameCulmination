using System;
using System.Collections;
using System.Collections.Generic;
using Classes.Knight;
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
        List<BaseBehavior> extraList = new List<BaseBehavior>();
        foreach (var characterBehavior in gridManager.getCharList())
        {
            if (characterBehavior.owner == player)
            {
                ResetChar(characterBehavior);
                //if a unit is picked up by a knight, it adds it to an extra list
                if (characterBehavior is KnightBehavior)
                {
                    KnightBehavior knightB = (KnightBehavior) characterBehavior;
                    if (knightB.hasRescue)
                    {
                        extraList.Add(knightB.RescueTarget);
                    }
                }
            }
        }
        
        //updates all in the extra list
        foreach (var characterBehavior in extraList)
        {
            if (characterBehavior.owner == player)
            {
                ResetChar(characterBehavior);
            }
        }
    }

    public void ResetChar(BaseBehavior ch)
    {
        ch.currentMoves = ch.movesPerTurn;
        ch.currentAttacks = ch.attacksPerTurn;
        ch.move = ch.baseMove;
        foreach (AbstractAttack a in ch.Attacks)
        {
            if (a != null && a.onCooldown)
            {
                a.reduceCooldown();
            }
        }
        ch.endTurnModifierCheck();
        ch.onReset();
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
        gridManager.addNewCharacter(Instantiate(gameModel.SwordGuy), 0, 4, GameManager.Player.Player1, 0);
        gridManager.addNewCharacter(Instantiate(gameModel.Guy2), 12, 4, GameManager.Player.Player2, 1);
        gridManager.addNewCharacter(Instantiate(gameModel.MineGuy), 13, 4, GameManager.Player.Player2, 2);
        ResetCharacterValues(Player.Player1);
        ResetCharacterValues(Player.Player2);
        currentTurn = Player.Player1;
        currentState = GameState.Neutral;
        
    }
}