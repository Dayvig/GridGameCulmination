using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class GridManager : MonoBehaviour
{
    private int[,] matrix = {{1, 2, 3,}, {1, 2, 2}, {0, 2, 2}};

    public TacticsGrid MasterGrid;
    
    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Length:" + matrix.Length);
        Debug.Log("Rank:" + matrix.Rank);
        MasterGrid.createGrid(matrix);
        MasterGrid.assignNeighbors();
    }

    // Update is called once per frame
    void Update()
    {
    }
}


