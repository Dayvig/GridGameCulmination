using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSlot : MonoBehaviour
{
    private UIManager UIManager;
    public int index;
    public GameManager.Player team;
    public GameObject objInSlot;
    void Start()
    {
        UIManager = GameObject.Find("GameManager").GetComponent<UIManager>();
    }
    void OnMouseUp()
    {
        if (objInSlot != null)
        {
            Destroy(objInSlot);
        }

        if (UIManager.selectedCharacter != null)
        {
            if (team == GameManager.Player.Player1)
            {
                UIManager.team1[index] = UIManager.selectedCharacter;
            }
            else
            {
                UIManager.team2[index] = UIManager.selectedCharacter;
            }

            var parent = UIManager.selectedCharacter.transform.parent;
            parent.GetComponent<Drag>().held = false;
            parent.transform.position =
                new Vector3(transform.position.x, transform.position.y, transform.position.z - 2);
            parent.GetComponent<Drag>().locked = true;
            objInSlot = parent.gameObject;
            UIManager.selectedCharacter = null;
        }
    }
}
