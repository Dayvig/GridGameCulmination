using System;
using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class GridRow : MonoBehaviour
    {
        public List<GridCell> contents = new List<GridCell>();
        public List<GameObject> cells = new List<GameObject>();

        public Model_Game gameModel;

        public void Setup()
        {
            gameModel = GameObject.Find("GameModel").GetComponent<Model_Game>();
        }

        public String DisplayRowContents()
        {
            String temp = "";
            foreach (GridCell gridCell in contents)
            {
                temp += gridCell.terrainType;
            }

            return temp;
        }

        public void DisplayValues(int[] v)
        {
            String s = "";
            for (int i = 0; i < v.Length; i++)
            {
                s += v[i];
            }
            Debug.Log(s);
        }

        private GameObject tempCell;
        public void createCells(int i)
        {
            for (int k = 0; k <= i; k++)
            {
                tempCell = Instantiate(gameModel.BaseCell, transform);
                cells.Add(tempCell);
                contents[k] = cells[k].GetComponent<GridCell>();
            }
        }
        public void createCells(int[] values)
        {
            cells.Clear();
            contents.Clear();
            for (int k = 0; k < values.Length; k++)
            {
                tempCell = Instantiate(gameModel.BaseCell, transform);
                cells.Add(tempCell);
                tempCell.transform.position = new Vector3(k *gameModel.cellOffset, transform.position.y, 20);

                contents.Add(cells[k].GetComponent<GridCell>());
                contents[k].terrainType = values[k];
            }
        }
    }
}