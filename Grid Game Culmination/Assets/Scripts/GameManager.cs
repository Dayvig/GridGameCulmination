using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Model_Game gameModel;

    public enum GameState
    {
        Neutral,
        CharacterMovement,
    }

    public GameState currentState;
    
    // Start is called before the first frame update
    void Start()
    {
        gameModel = GameObject.Find("GameModel").GetComponent<Model_Game>();
        currentState = GameState.Neutral;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}