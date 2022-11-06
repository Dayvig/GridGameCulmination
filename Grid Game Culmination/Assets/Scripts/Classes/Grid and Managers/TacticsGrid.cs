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
           
            for (int i = 0; i < gridMatrix.GetLength(0); i++)
            {
                    nextRow = Instantiate(gameModel.BaseRow, transform);
                    nextRow.name = ""+i;
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
                    
                    //NorthEast
                    if (rowCursor != contents.Count - 1 && colCursor != contents[rowCursor].contents.Count - 1)
                    {
                        temp = contents[rowCursor + 1].contents[colCursor + 1];
                        targetCell.setNorthEast(temp);
                    }
                    
                    //NorthWest
                    if (rowCursor != 0 && colCursor != 0)
                    {
                        temp = contents[rowCursor - 1].contents[colCursor - 1];
                        targetCell.setNorthWest(temp);
                    }
                    
                    //SouthEast
                    if (rowCursor != 0 && colCursor != contents[rowCursor].contents.Count - 1)
                    {
                        temp = contents[rowCursor - 1].contents[colCursor + 1];
                        targetCell.setSouthEast(temp);
                    }
                    
                    //SouthWest
                    if (rowCursor != contents.Count - 1 && colCursor != 0)
                    {
                        temp = contents[rowCursor + 1].contents[colCursor - 1];
                        targetCell.setSouthWest(temp);
                    }
                }
            }
        }

        public void placeHealthPacks(int[] locations)
        {
            for (int i = 0; i < locations.Length; i += 2)
            {
                GridCell c = contents[locations[i+1]].contents[locations[i]];
                c.isHealthPackZone = true;
                c.modifiers.Add(2);
            }
        }
        
        public void placeBoostPacks(int[] locations)
        {
            for (int i = 0; i < locations.Length; i += 2)
            {
                GridCell c = contents[locations[i+1]].contents[locations[i]];
                c.modifiers.Add(4);
            }
        }

        public void tickHealthPacks()
        {
            GridCell targetCell;
            //checks each cell in the matrix
            for (int rowCursor = 0; rowCursor < contents.Count; rowCursor++)
            {
                for (int colCursor = 0; colCursor < contents[rowCursor].contents.Count; colCursor++)
                {
                    targetCell = contents[rowCursor].contents[colCursor];
                    if (targetCell.modifiers.Contains(3))
                    {
                        targetCell.healthPackCtr++;
                        if (targetCell.healthPackCtr >= 6)
                        {
                            targetCell.healthPackCtr = 0;
                            targetCell.modifiers.Remove(3);
                            targetCell.modifiers.Add(2);
                            targetCell.TerrainSprite.sprite = gameModel.Terrainsprites[4];
                        }
                    }
                    if (targetCell.modifiers.Contains(5))
                    {
                        targetCell.boostPackCtr++;
                        if (targetCell.boostPackCtr >= 6)
                        {
                            targetCell.boostPackCtr = 0;
                            targetCell.modifiers.Remove(5);
                            targetCell.modifiers.Add(4);
                            targetCell.TerrainSprite.sprite = gameModel.Terrainsprites[6];
                        }
                    }
                    
                }
            }

        }

        public void wipeTimesSeen()
        {
            GridCell targetCell;
            //checks each cell in the matrix
            for (int rowCursor = 0; rowCursor < contents.Count; rowCursor++)
            {
                for (int colCursor = 0; colCursor < contents[rowCursor].contents.Count; colCursor++)
                {
                    targetCell = contents[rowCursor].contents[colCursor];
                    targetCell.movementCount = -1;
                    targetCell.isNumModified = false;
                }
            }

        }

        public void DeselectAll()
            {
                GridCell targetCell;
                //checks each cell in the matrix
                for (int rowCursor = 0; rowCursor < contents.Count; rowCursor++)
                {
                    for (int colCursor = 0; colCursor < contents[rowCursor].contents.Count; colCursor++)
                    {
                        targetCell = contents[rowCursor].contents[colCursor];
                        targetCell.movementCount = 0;
                        targetCell.Deselect();
                    }
                }
            }

        public void WipeMovement()
        {
            GridCell targetCell;
            //checks each cell in the matrix
            for (int rowCursor = 0; rowCursor < contents.Count; rowCursor++)
            {
                for (int colCursor = 0; colCursor < contents[rowCursor].contents.Count; colCursor++)
                {
                    targetCell = contents[rowCursor].contents[colCursor];
                    targetCell.isMovementSelectable = false;
                    targetCell.movementCount = 0;
                    if (!targetCell.isAttackSelectable){
                        targetCell.tint.gameObject.SetActive(false);
                    }
                }
            }

        }
        public void WipeAttackingSquares()
        {
            GridCell targetCell;
            GridCell temp;
            //checks each cell in the matrix
            for (int rowCursor = 0; rowCursor < contents.Count; rowCursor++)
            {
                for (int colCursor = 0; colCursor < contents[rowCursor].contents.Count; colCursor++)
                {
                    targetCell = contents[rowCursor].contents[colCursor];
                    targetCell.isAttackSelectable = false;
                    targetCell.isOptimal = false;
                    targetCell.tint.gameObject.SetActive(false);
                }
            }

        }

        public List<BaseBehavior> getAllCharacters()
        {
            List<BaseBehavior> thisList = new List<BaseBehavior>();
            
            for (int rowCursor = 0; rowCursor < contents.Count; rowCursor++)
            {
                for (int colCursor = 0; colCursor < contents[rowCursor].contents.Count; colCursor++)
                {
                    if (contents[rowCursor].contents[colCursor].occupant != null)
                    {
                        thisList.Add(contents[rowCursor].contents[colCursor].occupant.GetComponent<BaseBehavior>());
                    }
                }
            }

            return thisList;
        }
        
        public void destroyAllCharacters()
        {
            
            for (int rowCursor = 0; rowCursor < contents.Count; rowCursor++)
            {
                for (int colCursor = 0; colCursor < contents[rowCursor].contents.Count; colCursor++)
                {
                    if (contents[rowCursor].contents[colCursor].occupant != null)
                    {
                        Destroy(contents[rowCursor].contents[colCursor].occupant);
                    }
                }
            }
        }
    }
}