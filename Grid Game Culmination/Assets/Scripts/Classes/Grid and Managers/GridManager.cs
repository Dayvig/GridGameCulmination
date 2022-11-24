using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private static int[,] matrix =
    {
        {3, 3, 1, 1, 1, 3, 3, 3, 3, 1, 1, 3, 3},
        {3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 0, 0, 1, 0, 0, 0, 0, 1, 1, 1},
        {1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1},
        {1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1},
        {2, 2, 2, 0, 1, 1, 1, 1, 1, 1, 2, 2, 2},
        {2, 2, 2, 1, 1, 1, 1, 1, 1, 0, 2, 2, 2},
        {1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1},
        {1, 1, 1, 0, 1, 1, 1, 1, 1, 0, 1, 1, 1},
        {1, 1, 1, 0, 0, 0, 1, 0, 0, 0, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {3, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 3},
        {3, 3, 1, 1, 3, 3, 3, 3, 1, 1, 1, 3, 3}
    };

    private static int[] hpPackLocations = {0, 9, 0, 6, 12, 9, 12, 6};
    private static int[] boostPackLocations = {6, 8, 5, 7, 8, 15, 4, 0};
    private static int[] redSpawnLocations = {5, 1};
    private static int[] blueSpawnLocations = {6, 14};

    public TacticsGrid MasterGrid;
    public Model_Game gameModel;
    public UIManager uiManager;
    public GameManager gameManager;
    public GridCell selectedCell;
    public GameObject selectedCharacter;
    public BaseBehavior selectedCharacterBehavior;
    public BaseBehavior lastSelectedCharacterBehavior;
    public int currentSelectedAttack = 0;
    public int count = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        gameModel = GameObject.Find("GameModel").GetComponent<Model_Game>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        uiManager = GameObject.Find("GameManager").GetComponent<UIManager>();
    }

    public void StartGame()
    {
        for (int i = 0; i < 3; i++)
        {
            if (uiManager.team1[i] == null)
            {
                return;
            }
        }
        for (int i = 0; i < 3; i++)
        {
            if (uiManager.team2[i] == null)
            {
                return;
            }
        }
        MasterGrid.createGrid(matrix);
        MasterGrid.assignNeighbors();
        MasterGrid.placeHealthPacks(hpPackLocations);
        MasterGrid.placeBoostPacks(boostPackLocations);
        MasterGrid.placeSpawns(blueSpawnLocations, redSpawnLocations);
        MasterGrid.transform.position = new Vector3(
            -(gameModel.cellOffset * matrix.GetLength(0) / 2) - 2,
            -(gameModel.cellOffset * matrix.GetLength(1) / 2) - 2,
            (32));
        selectedCell = null;
        
        //make the first guy
        addNewCharacter(Instantiate(uiManager.team1[0]), 1, 4, GameManager.Player.Player1, 0, false);
        
        //make the second guy
        addNewCharacter(Instantiate(uiManager.team1[1]), 1, 6, GameManager.Player.Player1, 1, false);

        //make the third guy
        addNewCharacter(Instantiate(uiManager.team1[2]), 1, 8, GameManager.Player.Player1, 2, false);
        
        //make the third guy
        addNewCharacter(Instantiate(uiManager.team2[0]), 14, 5, GameManager.Player.Player2, 3, false);
       
        //make the third guy again
        addNewCharacter(Instantiate(uiManager.team2[1]), 14, 7, GameManager.Player.Player2, 4, false);

        //make the third guy once more
        addNewCharacter(Instantiate(uiManager.team2[2]), 14, 9, GameManager.Player.Player2, 5, false);
        
        foreach (GameObject g in uiManager.CharacterSelectObjects)
        {
            g.SetActive(false);
        }
        
        gameManager.currentState = GameManager.GameState.Neutral;
    }

    public int numCalls = 0;






    /*
    while (currentMove < movementValue)
    {
        //Looks at the previous cells accessed, then returns all of its accessible neighbors
        foreach (GridCell nextCell in previousCells)
        {
            for (int i = 0; i < 4; i++)
            {
                GridCell n = nextCell.neighbors[i];
                //check if cell is traversable
                if (n != null && n.terrainType != 0)
                {
                    if (n.occupant != null)
                    {
                        //can move through allies
                        if (n.occupant.GetComponent<BaseBehavior>().owner == selectedCharacterBehavior.owner)
                        {
                            surroundingCells.Add(n);
                        }
                    }
                    else
                    {
                        surroundingCells.Add(n);
                    }
                }
            }
        }
        
        //adds the accessible neighbors to the cells in range
        inRangeCells.AddRange(surroundingCells);
        
        //these new accessible neighbors become the previous cells
        previousCells = surroundingCells.Distinct().ToList();
        
        //reduces movement count
        currentMove++;
    }

    foreach (GridCell g in inRangeCells)
    {
        //but cannot move into an occupied zone
        if (g.occupant == null || g.occupant.Equals(selectedCharacter)) {g.canMoveTo();}
    }

    public void xshowMovementSquares(GridCell currentCell, int movementValue)
    {
        //sets current tile as accessible
        if (currentCell.terrainType != 0 &&
            (currentCell.occupant == null || currentCell.occupant.Equals(selectedCharacter)))
        {
            currentCell.movementCount++;
            currentCell.canMoveTo();
            Debug.Log(currentCell.name + " | "+movementValue + "|" + currentCell.movementCount);
        }
        count++;
        //repeats the function, checking the neighbors and continuing if any are inaccessible
        for (int i = 0; i < 4; i++)
        {
            if (currentCell.neighbors[i] != null && movementValue >= 1 && currentCell.neighbors[i].terrainType != 0)
            {
                if (currentCell.occupant != null)
                {
                    if (currentCell.occupant.GetComponent<BaseBehavior>().owner == gameManager.currentTurn)
                    {
                        showMovementSquares(currentCell.neighbors[i], movementValue - 1);
                    }
                }
                else
                {
                    showMovementSquares(currentCell.neighbors[i], movementValue - 1);
                }
            }
        }
    }*/

    public List<BaseBehavior> getCharList()
    {
        return MasterGrid.getAllCharacters();
    }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            DeselectAll();   
        }
    }

    public void DeselectAll()
    {
        MasterGrid.DeselectAll();
        gameManager.currentState = GameManager.GameState.Neutral;
    }
    
    public void SetupDisplay(int index, BaseBehavior characterToAdd){
        CharacterDisplay thisDisplay = gameModel.Displays[index].GetComponent<CharacterDisplay>();
        thisDisplay.character = characterToAdd;
        thisDisplay.initialize();
    }

    public void addNewCharacter(GameObject newChar, int gridXPos, int gridYPos, GameManager.Player charOwner, int disIndex, bool isGhost)
    {
        MasterGrid.contents[gridXPos].contents[gridYPos].occupant = newChar;
        BaseBehavior behavior = newChar.GetComponent<BaseBehavior>();
        behavior.isGhost = isGhost;
        behavior.currentCell = MasterGrid.contents[gridXPos].contents[gridYPos];
        behavior.owner = charOwner;
        behavior.Initialize();
        SpriteRenderer charImage = newChar.GetComponent<SpriteRenderer>();
        switch (charOwner)
        {
            case GameManager.Player.Player1:
                charImage.color = gameModel.blueTeamColor;
                break;
            case GameManager.Player.Player2:
                charImage.color = gameModel.redTeamColor;
                break;
        }
        SetupDisplay(disIndex, behavior);
        if (isGhost)
        {
            behavior.HP = behavior.values.hp / 4;
            behavior.endTurn();
        }
        else {
            behavior.GlowRen.color = (charOwner == GameManager.Player.Player1) ? Color.blue : Color.gray;
        }
    }
}


