using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class TacticsGrid : MonoBehaviour
    {
        public List<GridRow> contents;
        public List<GameObject> rows;
        
        public Model_Game gameModel;

        public void Start()
        {
            gameModel = GameObject.Find("GameModel").GetComponent<Model_Game>();
        }
        
        
        public GridCell getCell(int row, int column)
        {
            return contents[row].contents[column];
        }

        private GameObject nextRow;
        public void createGrid(int[,] gridMatrix)
        {
            
            for (int i = 0; i < gridMatrix.Rank; i++)
            {
                    nextRow = Instantiate(gameModel.BaseRow, transform);
                    rows.Add(nextRow);
                    contents.Add(nextRow.GetComponent<GridRow>());
                    contents[i].Setup();
                    contents[i].createCells(gridMatrix.GetLength(i));
            }
        }
    }
}