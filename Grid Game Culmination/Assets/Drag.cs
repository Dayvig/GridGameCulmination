using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drag : MonoBehaviour
{
    private UIManager UIManager;
    public int index;
    public bool held = false;
    public bool locked = false;
    public int dist;
    public int hitBox;
    public int character;
    void Start()
    {
        UIManager = GameObject.Find("GameManager").GetComponent<UIManager>();
    }
    
    private void OnMouseDown()
    {
        if (!locked)
        {
            if (UIManager.selectedCharacter != null && !held)
            {
                GameObject heldChar = Instantiate(this.gameObject, transform.parent);
                heldChar.GetComponent<Drag>().held = true;
                heldChar.GetComponent<Drag>().dist = 80;
                UIManager.selectedCharacter = heldChar.transform.GetChild(0).gameObject;
            }
        }
    }

    void Update()
    {
        if (!locked)
        {
            if (held)
            {
                if (Input.GetMouseButtonUp(1))
                {
                    UIManager.selectedCharacter = null;
                    Destroy(this.gameObject);
                }
                Vector3 toPos = new Vector3(Input.mousePosition.x, Input.mousePosition.y,
                    dist);
                transform.position = Camera.main.ScreenToWorldPoint(toPos);
                gameObject.layer = 2;
                UIManager.selectedCharacter = transform.GetChild(0).gameObject;
            }
        }
    }

    void OnMouseOver()
    {
        if (!locked) 
            UIManager.selectedCharacter = transform.GetChild(0).gameObject;
    }
    void OnMouseExit()
    {
        if (!locked)
            UIManager.selectedCharacter = null;
    }
}
