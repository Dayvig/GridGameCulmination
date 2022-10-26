using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Model_Game : MonoBehaviour
{

    public GameObject BaseCell;
    public GameObject BaseRow;
    public GameObject Guy;
    public GameObject Guy2;
    public GameObject SwordGuy;
    public GameObject MineGuy;
    public GameObject Knight;
    public List <GameObject> Displays = new List<GameObject>();
    public GameObject Mine;
    public float cellOffset;
    public List<Sprite> Terrainsprites = new List<Sprite>();

    public Color movementTint;
    //(0, 0.4f, 1f, 0.4f);
    public Color attackTint;
    //(1f, 0.2f, 0.2f, 0.4f);
    public Color optimalTint;
    //(1f, 0.9f, 0.3f, 0.4f);
    public Color buffTint;
    //(0.4f, 1f, 0.3f, 0.4f);
    public Color movementTint2;
    //(0, 0.8f, 1f, 0.4f)

}
