using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using Classes.Knight;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public Model_Game gameModel;
    public UIManager uiManager;
    public GridManager gridManager;
    public bool SpawnGhostP1;
    public bool SpawnGhostP2;
    public GridCell lastMovementCell;
    public GameObject containedMine;
    public bool nextTurnChecked = false;
    public AudioClip BattleTheme;
    public AudioClip SelectTheme;
    public AudioSource MainMusic;
    public float startVolume = 0.25f;
    [Range(0.0f, 2.0f)] public float volAdjust;

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
        uiManager = GetComponent<UIManager>();
        gridManager = GetComponent<GridManager>();
        currentState = GameState.CharacterSelection;
        currentTurn = Player.Player1;
        MainMusic.clip = SelectTheme;
        MainMusic.Play();
    }

    // Update is called once per frame
    void Update()
    {
        MainMusic.volume = startVolume * volAdjust;
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
            Debug.Log("P1");
            if (SpawnGhostP2)
            {
                SpawnGhost(Player.Player2);
            }
            SetTurn(Player.Player2);
        }
        else if (currentTurn == Player.Player2)
        {
            Debug.Log("P2");
            if (SpawnGhostP1)
            {
                SpawnGhost(Player.Player1);
            }
            SetTurn(Player.Player1);
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
        ch.dashed = false;
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
        Debug.Log("Next turn check");
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
        GameObject newChar;
        if (player.Equals(Player.Player1))
        {
            newChar = Instantiate(uiManager.team1[randDisplay.index]);
        }
        else
        {
            newChar = Instantiate(uiManager.team2[randDisplay.index - 3]);
        }
        gridManager.addNewCharacter(newChar, spawnCoords[0], spawnCoords[1], player, randDisplay.index, true);
        SpawnGhostP1 = false;
    }


    public void Replay()
    {
        gridManager.MasterGrid.destroyAllCharacters();
        gridManager.MasterGrid.DeselectAll();
        foreach (GameObject display in gameModel.Displays)
        {
            display.SetActive(false);
        }

        foreach (GridRow row in gridManager.MasterGrid.contents)
        {
            Destroy(row.gameObject);
        }
        foreach (GameObject rowobject in gridManager.MasterGrid.rows)
        {
            Destroy(rowobject.gameObject);
        }
        gridManager.MasterGrid.contents.Clear();
        gridManager.MasterGrid.rows.Clear();
        gridManager.MasterGrid.transform.position = new Vector3(-5, -1, 20);
        ResetCharacterValues(Player.Player1);
        ResetCharacterValues(Player.Player2);
        currentTurn = Player.Player1;
        currentState = GameState.CharacterSelection;
        foreach (GameObject g in uiManager.CharacterSelectObjects)
        {
            g.SetActive(true);
            BaseBehavior[] bb = g.GetComponentsInChildren<BaseBehavior>();
            foreach (BaseBehavior b in bb)
            {
                b.enabled = true;
            }
        }
        uiManager.ReplayButton.gameObject.SetActive(false);
        uiManager.UndoButton.gameObject.SetActive(false);
    }

    public void UndoMovement(BaseBehavior ch)
    {
        if (lastMovementCell != null)
        {
            GridCell tmp = ch.currentCell;
            ch.currentCell.occupant = null;
            ch.currentCell = lastMovementCell;
            ch.currentCell.occupant = ch.gameObject;
            ch.currentMoves++;
            if (ch.dashed)
            {
                ch.currentAttacks++;
                ch.dashed = false;
            }
            currentState = GameState.CharacterMovement;
            gridManager.MasterGrid.WipeMovement();
            ch.GlowRen.color = Color.blue;
            gridManager.selectedCharacter = ch.gameObject;
            gridManager.selectedCharacterBehavior = ch;
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