using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private int[,] matrix = {{1, 2, 3,}, {1, 2, 2}, {0, 2, 2}};
    
    






    // Start is called before the first frame update
    void Start()
    {
        
        Debug.Log(matrix[0, 1]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

public class GridRow
{
    public List<GridCell> contents = new List<GridCell>();
}

public class Grid
{
    public List<GridRow> contents = new List<GridRow>();

    public GridCell getCell(int row, int column)
    {
        return contents[row].contents[column];
    }

    public void createGrid(int[,] gridMatrix)
    {
        
    }
    
    

}
