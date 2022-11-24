using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Classes.Knight;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.AI;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public Model_Game gameModel;
    public GridManager gridManager;
    private bool SpawnGhostP1;
    private bool SpawnGhostP2;
    public GridCell lastMovementCell;
    public GameObject containedMine;

    public enum GameState
    {
        CharacterSelection,
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
        currentState = GameState.CharacterSelection;
        currentTurn = Player.Player1;
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
            if (SpawnGhostP2)
            {
                SpawnGhost(Player.Player2);
                SpawnGhostP2 = false;
            }
            currentTurn = Player.Player2;
        }
        else if (currentTurn == Player.Player2)
        {
            ResetCharacterValues(Player.Player1);
            if (SpawnGhostP1)
            {
                SpawnGhost(Player.Player1);
                SpawnGhostP1 = false;
            }
            currentTurn = Player.Player1;
        }
        gridManager.MasterGrid.tickHealthPacks();
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
        int Player1Count = 0;
        int Player2Count = 0;
        int P1NonGhostCount = 0;
        int P2NonGhostCount = 0;
        foreach (var characterBehavior in gridManager.getCharList())
        {
            if (characterBehavior.owner == Player.Player1)
            {
                Player1Count++;
                if (!characterBehavior.isGhost)
                    P1NonGhostCount++;
            }
            else
            {
                Player2Count++;
                if (!characterBehavior.isGhost)
                    P2NonGhostCount++;
            }
        }

            if (P1NonGhostCount != 0 && P2NonGhostCount == 0)
            {
                winner = Player.Player1;
                currentState = GameState.GameOver;
                return;
            }
            if (P1NonGhostCount == 0 && P2NonGhostCount != 0)
            {
                winner = Player.Player2;
                currentState = GameState.GameOver;
                return;
            }
            if (P1NonGhostCount == 0 && P2NonGhostCount == 0)
            {
                //tie
                currentState = GameState.GameOver;
                return;
            }

            if (Player1Count == 1)
            {
                SpawnGhostP1 = true;
            }

            if (Player2Count == 1)
            {
                SpawnGhostP2 = true;
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

    public void SpawnGhost(Player player)
    {
        List<GameObject> displayList = new List<GameObject>();
        foreach (GameObject g in gameModel.Displays)
        {
            CharacterDisplay thisDisplay = g.GetComponent<CharacterDisplay>();
            if (thisDisplay.character.owner == player && thisDisplay.currentHP <= 0)
            {
                displayList.Add(g);
            }
        }

        int rand = Random.Range(0, displayList.Count-1);
        CharacterDisplay randDisplay = displayList[rand].GetComponent<CharacterDisplay>();
        int[] spawnCoords = gridManager.MasterGrid.returnSpawnLocation(player);
        gridManager.addNewCharacter(Instantiate(returnCharacterPrefab(randDisplay.character)), spawnCoords[0], spawnCoords[1], player, randDisplay.index, true);
    }

    public GameObject returnCharacterPrefab(BaseBehavior b)
    {
        switch (b.name)
        {
            case "Knight":
                return gameModel.Knight;
            case "Mine Layer":
                return gameModel.MineGuy;
            case "Gun Man":
                return gameModel.Guy2;
            case "Sword Man":
                return gameModel.SwordGuy;
            default:
                return gameModel.Guy2;
        }
    }


    public void Replay()
    {
        gridManager.MasterGrid.destroyAllCharacters();
        gridManager.MasterGrid.DeselectAll();
        gridManager.MasterGrid.ResetPacks();
        //make the first guy
        gridManager.addNewCharacter(Instantiate(gameModel.SwordGuy), 1, 4, GameManager.Player.Player1, 0, false);
        
        //make the second guy
        gridManager.addNewCharacter(Instantiate(gameModel.Guy2), 1, 6, GameManager.Player.Player1, 3, false);

        //make the third guy
        gridManager.addNewCharacter(Instantiate(gameModel.MineGuy), 14, 7, GameManager.Player.Player2, 1, false);
        
        //make the third guy
        gridManager.addNewCharacter(Instantiate(gameModel.Knight), 14, 5, GameManager.Player.Player2, 2, false);
        ResetCharacterValues(Player.Player1);
        ResetCharacterValues(Player.Player2);
        currentTurn = Player.Player1;
        currentState = GameState.Neutral;
        
    }

    public void UndoMovement(BaseBehavior ch)
    {
        Debug.Log("test");
        if (lastMovementCell != null)
        {
            GridCell tmp = ch.currentCell;
            ch.currentCell.occupant = null;
            ch.currentCell = lastMovementCell;
            ch.currentCell.occupant = ch.gameObject;
            ch.currentMoves++;
            gridManager.selectedCharacterBehavior = ch;
            currentState = GameState.CharacterMovement;
            gridManager.MasterGrid.WipeMovement();
            ch.GlowRen.color = Color.blue;
            ch.onSelect();
            
            
            if (containedMine != null)
            {
                tmp.modifiers.Add(0);
                GameObject newMine = Instantiate(containedMine, tmp.gameObject.transform.position, Quaternion.identity);
                mineBehavior newMineB = newMine.GetComponent<mineBehavior>();
                newMineB.setDamage(containedMine.GetComponent<mineBehavior>().damage);
                newMineB.cell = tmp;
                newMineB.owner = (ch.owner == Player.Player1) ? Player.Player2 : Player.Player1;
            }

            lastMovementCell = null;
        }
    }
}