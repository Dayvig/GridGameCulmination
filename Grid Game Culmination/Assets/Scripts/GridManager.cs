using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private int[,] matrix = {{1, 2, 3, 4, 5}, {1, 1, 1, 1, 1}, {2, 2, 2, 2, 2}, {3, 3, 3, 3, 3}, {5, 4, 3, 2, 1}};

    public TacticsGrid MasterGrid;
    public Model_Game gameModel;
    public GridCell selectedCell;
    public GameObject selectedCharacter;
    public BaseBehavior selectedCharacterBehavior;
    
    // Start is called before the first frame update
    void Start()
    {
        gameModel = GameObject.Find("GameModel").GetComponent<Model_Game>();
        Debug.Log("Length:" + matrix.GetLength(0));
        Debug.Log("Rank:" + matrix.GetLength(1));
        MasterGrid.createGrid(matrix);
        MasterGrid.assignNeighbors();
        MasterGrid.transform.position = new Vector3(
            -(gameModel.cellOffset * matrix.GetLength(0) / 2),
            -(gameModel.cellOffset * matrix.GetLength(1) / 2),
            (20));
        selectedCell = null;
        GameObject firstGuy = Instantiate(gameModel.Guy);
        MasterGrid.contents[0].contents[0].occupant = firstGuy;
        firstGuy.GetComponent<BaseBehavior>().currentCell = MasterGrid.contents[0].contents[0];
        
    }

    public int numCalls = 0;
    public void showMovementSquares(GridCell startingCell, int movementValue)
    {
        foreach (GridCell nextCell in startingCell.neighbors)
        {
            if (nextCell != null)
            {
                nextCell.canMoveTo();
                numCalls++;
                Debug.Log("Moves Remaining: " + movementValue);
                Debug.Log("Number of Calls: " + numCalls);
                if (movementValue > 1)
                {
                    showMovementSquares(nextCell, movementValue - 1);
                }
            }
        }
    }
    

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonUp(1))
        {
            MasterGrid.DeselectAll();
        }
    }
}


