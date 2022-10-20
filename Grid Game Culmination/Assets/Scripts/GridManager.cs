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
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 0, 1, 2, 0, 0, 1, 1, 0, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1},
        {1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1},
        {1, 0, 1, 1, 1, 1, 1, 1, 0, 1, 1, 1, 1},
        {1, 1, 1, 0, 0, 0, 0, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 0, 1, 1, 0, 0, 1, 1, 0, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
    };
        
    public TacticsGrid MasterGrid;
    public Model_Game gameModel;
    public GameManager gameManager;
    public GridCell selectedCell;
    public GameObject selectedCharacter;
    public BaseBehavior selectedCharacterBehavior;
    public int currentSelectedAttack = 0;
    public int displayIndex = 0;
    public int count = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        gameModel = GameObject.Find("GameModel").GetComponent<Model_Game>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        MasterGrid.createGrid(matrix);
        MasterGrid.assignNeighbors();
        MasterGrid.transform.position = new Vector3(
            -(gameModel.cellOffset * matrix.GetLength(0) / 2) - 2,
            -(gameModel.cellOffset * matrix.GetLength(1) / 2) - 2,
            (32));
        selectedCell = null;
        
        //make the first guy
        addNewCharacter(Instantiate(gameModel.SwordGuy), 0, 4, GameManager.Player.Player1);
        
        //make the second guy
        addNewCharacter(Instantiate(gameModel.Guy2), 13, 4, GameManager.Player.Player2);

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
        switch (index)
        {
            case 0:
                CharacterDisplay thisDisplay = gameModel.Display1.GetComponent<CharacterDisplay>();
                thisDisplay.character = characterToAdd;
                thisDisplay.initialize();
                break;
            case 1:
                CharacterDisplay thisDisplay2 = gameModel.Display2.GetComponent<CharacterDisplay>();
                thisDisplay2.character = characterToAdd;
                thisDisplay2.initialize();
                break;
        }
    }

    public void addNewCharacter(GameObject newChar, int gridXPos, int gridYPos, GameManager.Player charOwner)
    {
        MasterGrid.contents[gridXPos].contents[gridYPos].occupant = newChar;
        BaseBehavior behavior = newChar.GetComponent<BaseBehavior>();
        behavior.currentCell = MasterGrid.contents[gridXPos].contents[gridYPos];
        behavior.owner = charOwner;
        behavior.Initialize();
        SetupDisplay(displayIndex, behavior);
        displayIndex++;
    }
    
    
}


