using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SetText : MonoBehaviour
{

    public GameManager gameManager;
    public GameManager.Player prevTurn;
    public TextMeshProUGUI text;
    
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        prevTurn = gameManager.currentTurn;
        text.text = gameManager.currentTurn.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameManager.currentTurn != prevTurn)
        {
            text.text = gameManager.currentTurn.ToString();
            prevTurn = gameManager.currentTurn;
        }

        if (gameManager.currentState == GameManager.GameState.GameOver)
        {
            text.text = gameManager.winner.ToString()+" wins!";
        }
    }
}
