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

        public int[] getArrayFromMatrix(int [,] matrix, int size, int index)
        {
            int[] newArray = new int[size];
            for (int i = 0; i < size; i++)
            {
                newArray[i] = matrix[index, i];
            }

            return newArray;
        }

        private GameObject nextRow;
        public void createGrid(int[,] gridMatrix)
        {
           
            for (int i = 0; i < gridMatrix.GetLength(1); i++)
            {
                    nextRow = Instantiate(gameModel.BaseRow, transform);
                    rows.Add(nextRow);
                    nextRow.transform.position = new Vector3(0, i * gameModel.cellOffset, 20);
                    contents.Add(null);
                    contents[i] = nextRow.GetComponent<GridRow>();
                    contents[i].Setup();
                    contents[i].createCells(getArrayFromMatrix(gridMatrix, gridMatrix.GetLength(1),i));
            }
        }

        public void assignNeighbors()
        {
            GridCell targetCell;
            GridCell temp;
            //checks each cell in the matrix
            for (int rowCursor = 0; rowCursor < contents.Count; rowCursor++)
            {
                for (int colCursor = 0; colCursor < contents[rowCursor].contents.Count; colCursor++)
                {
                    targetCell = contents[rowCursor].contents[colCursor];

                    //North
                    if (rowCursor != 0)
                    {
                        temp = contents[rowCursor - 1].contents[colCursor];
                        targetCell.setNorth(temp);
                    }

                    //East
                    if (colCursor != contents[rowCursor].contents.Count - 1)
                    {
                        temp = contents[rowCursor].contents[colCursor + 1];
                        targetCell.setEast(temp);
                    }

                    //West
                    if (colCursor != 0)
                    {
                        temp = contents[rowCursor].contents[colCursor - 1];
                        targetCell.setWest(temp);
                    }

                    //South
                    if (rowCursor != contents.Count - 1)
                    {
                        temp = contents[rowCursor + 1].contents[colCursor];
                        targetCell.setSouth(temp);
                    }
                }
            }
        }

        public void DeselectAll()
            {
                GridCell targetCell;
                GridCell temp;
                //checks each cell in the matrix
                for (int rowCursor = 0; rowCursor < contents.Count; rowCursor++)
                {
                    for (int colCursor = 0; colCursor < contents[rowCursor].contents.Count; colCursor++)
                    {
                        targetCell = contents[rowCursor].contents[colCursor];
                        targetCell.Deselect();
                    }
                }
            }
    }
}