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
    }
}