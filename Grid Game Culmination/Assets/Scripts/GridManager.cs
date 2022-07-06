using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private int[,] matrix = {{1, 2, 3, 4, 5}, {1, 1, 1, 1, 1}, {2, 2, 2, 2, 2}, {3, 3, 3, 3, 3}, {5, 4, 3, 2, 1}};

    public TacticsGrid MasterGrid;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Length:" + matrix.GetLength(0));
        Debug.Log("Rank:" + matrix.GetLength(1));
        MasterGrid.createGrid(matrix);
        MasterGrid.assignNeighbors();
    }

    // Update is called once per frame
    void Update()
    {
    }
}


