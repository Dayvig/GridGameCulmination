using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private int[,] matrix = {{1, 2, 3, 4, 5}, {1, 1, 1, 1, 1}, {2, 2, 2, 2, 2}, {3, 3, 3, 3, 3}, {5, 4, 3, 2, 1}};

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
        GameObject firstGuy = Instantiate(gameModel.Guy);
        MasterGrid.contents[0].contents[0].occupant = firstGuy;
        BaseBehavior behavior = firstGuy.GetComponent<BaseBehavior>();
        behavior.currentCell = MasterGrid.contents[0].contents[0];
        behavior.owner = GameManager.Player.Player1;
        behavior.Modifiers.Add(new AccuracyModifier());
        Debug.Log(behavior.Modifiers[0].ID + ", "+behavior.Modifiers[0].amount);
        
        //make the second guy
        GameObject secondGuy = Instantiate(gameModel.Guy2);
        MasterGrid.contents[3].contents[3].occupant = secondGuy;
        BaseBehavior behavior2 = secondGuy.GetComponent<BaseBehavior>();
        behavior2.currentCell = MasterGrid.contents[3].contents[3];
        behavior2.owner = GameManager.Player.Player2;
        
        //make the third guy
        GameObject thirdGuy = Instantiate(gameModel.Guy2);
        MasterGrid.contents[3].contents[2].occupant = thirdGuy;
        BaseBehavior behavior3 = thirdGuy.GetComponent<BaseBehavior>();
        behavior3.currentCell = MasterGrid.contents[3].contents[2];
        behavior3.owner = GameManager.Player.Player2;

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
                foreach (GridCell n in nextCell.neighbors)
                {
                    if (n != null && n.occupant == null)
                        surroundingCells.Add(n);
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
            if (g.occupant == null || g.occupant.Equals(selectedCharacter)) {g.canMoveTo();}
        }
    }

    public void showAttackingSquares(GridCell startingCell, int range)
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
        
        while (currentMove < range)
        {
            //Looks at the previous cells accessed, then returns all of its accessible neighbors
            foreach (GridCell nextCell in previousCells)
            {
                foreach (GridCell n in nextCell.neighbors)
                {
                    if (n != null)
                        surroundingCells.Add(n);
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
            g.isAttackable();
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


