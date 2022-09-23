using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private int[,] matrix =
    {
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 0, 1, 1, 0, 0, 1, 1, 0, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 1, 1, 0, 0, 0, 0, 1, 1, 1},
        {1, 0, 1, 0, 0, 0, 0, 1, 0, 1},
        {1, 0, 1, 0, 0, 0, 0, 1, 0, 1},
        {1, 1, 1, 0, 0, 0, 0, 1, 1, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1},
        {1, 0, 1, 1, 0, 0, 1, 1, 0, 1},
        {1, 1, 1, 1, 1, 1, 1, 1, 1, 1}
    };
        
        public TacticsGrid MasterGrid;
    public Model_Game gameModel;
    public GameManager gameManager;
    public GridCell selectedCell;
    public GameObject selectedCharacter;
    public BaseBehavior selectedCharacterBehavior;
    public int currentSelectedAttack = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        gameModel = GameObject.Find("GameModel").GetComponent<Model_Game>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        Debug.Log("Length:" + matrix.GetLength(0));
        Debug.Log("Rank:" + matrix.GetLength(1));
        MasterGrid.createGrid(matrix);
        MasterGrid.assignNeighbors();
        MasterGrid.transform.position = new Vector3(
            -(gameModel.cellOffset * matrix.GetLength(0) / 2),
            -(gameModel.cellOffset * matrix.GetLength(1) / 2),
            (20));
        selectedCell = null;
        
        //make the first guy
        GameObject firstGuy = Instantiate(gameModel.SwordGuy);
        MasterGrid.contents[0].contents[4].occupant = firstGuy;
        BaseBehavior behavior = firstGuy.GetComponent<BaseBehavior>();
        behavior.currentCell = MasterGrid.contents[0].contents[4];
        behavior.owner = GameManager.Player.Player1;
        
        //make the second guy
        GameObject secondGuy = Instantiate(gameModel.Guy2);
        MasterGrid.contents[9].contents[4].occupant = secondGuy;
        BaseBehavior behavior2 = secondGuy.GetComponent<BaseBehavior>();
        behavior2.currentCell = MasterGrid.contents[9].contents[4];
        behavior2.owner = GameManager.Player.Player2;

    }

    public int numCalls = 0;
    public void showMovementSquares(GridCell startingCell, int movementValue)
    {
        //Creates a list for all tiles that can be moved to, and adds the starting cell to it.
        List<GridCell> inRangeCells = new List<GridCell>();
        inRangeCells.Add(startingCell);
        //sets the move counter to 0
        int currentMove = 0;

        //tracks the currently selected tiles
        List<GridCell> previousCells = new List<GridCell>();
        previousCells.Add(startingCell);
        
        List<GridCell> surroundingCells = new List<GridCell>();
        
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
    }

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
}


