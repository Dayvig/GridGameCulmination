using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.UI;

public class mineBehavior : MonoBehaviour
{

    public GridCell cell;
    private int damage = 0;
    public GameManager.Player owner;
    public GameManager gameManager;

    // Start is called before the first frame update
    public void setDamage(int d)
    {
        damage = d;
    }

    public void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    public void Update()
    {
        if (cell != null)
        {
            var position = cell.transform.position;
            transform.position = new Vector3(position.x,
                position.y, position.z - 0.1f);
            if (!cell.modifiers.Contains(0))
            {
                Destroy(this.gameObject);
            }
        }

        if (cell.occupant != null)
        {
            BaseBehavior newEntrant = cell.occupant.GetComponent<BaseBehavior>();
            if (newEntrant.owner == owner)
                damage = newEntrant.calculateDamage(1, newEntrant);
            else
            {
                damage = newEntrant.calculateDamage(damage, newEntrant);
            }

            newEntrant.HP -= damage;
            newEntrant.updateBars();
            int toRemove = -1;
            for (int i = 0; i < cell.modifiers.Count; i++){
                if (cell.modifiers[i] == 0)
                {
                    toRemove = i;
                    break;
                }
            }

            if (toRemove != -1)
            {
                cell.modifiers.RemoveAt(toRemove);
            }
            
            gameManager.checkForNextTurn(owner);
            Destroy(this.gameObject);
        }
    }
}
