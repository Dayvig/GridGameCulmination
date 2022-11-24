using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;

public class CharacterSelectDisplay : MonoBehaviour
{

    public List<TextMeshProUGUI> descriptions = new List<TextMeshProUGUI>();
    public List<String> BasicDesc = new List<string>();
    public BaseBehavior character;
    private UIManager uiManager;
    private GameManager gameManager;
    public Canvas thisCanvas;
    void Start()
    {
        uiManager = GameObject.Find("GameManager").GetComponent<UIManager>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
    }
    void Update()
    {
        if (gameManager.currentState == GameManager.GameState.CharacterSelection)
        {
            if (uiManager.selectedCharacter != null)
            {
                thisCanvas.enabled = true;
                descriptionsUpdate();
            }
            else
            {               
                thisCanvas.enabled = false;
            }
        }
    }
    
    void descriptionsUpdate()
    {
        BaseBehavior behavior = uiManager.selectedCharacter.GetComponent<BaseBehavior>();
        Debug.Log(uiManager.selectedCharacter.name);
        descriptions[0].text = getBasicDesc(uiManager.selectedCharacter.name);
        descriptions[1].text = behavior.passive;
        for (int i = 0; i < 4; i++)
        {
            descriptions[2 + i].text = behavior.Attacks[i].AttackDesc;
            if (behavior.Attacks[i].AttackDamage != 0)
            {
                descriptions[2 + i].text += "\nDamage: "+behavior.Attacks[i].AttackDamage;
            }
            if (behavior.Attacks[i].OptimalDamage != 0)
            {
                descriptions[2 + i].text += "\nOptimal Damage: "+behavior.Attacks[i].OptimalDamage;
            }
            if (behavior.Attacks[i].Healing != 0)
            {
                descriptions[2 + i].text += "\nHealing: "+behavior.Attacks[i].Healing;
            }
            if (behavior.Attacks[i].OptimalHealing != 0)
            {
                descriptions[2 + i].text += "\nOptimal Healing: "+behavior.Attacks[i].OptimalHealing;
            }
            if (behavior.Attacks[i].buffAmount != 0)
            {
                descriptions[2 + i].text += "\nBuff Amount: "+behavior.Attacks[i].buffAmount;
            }
            if (behavior.Attacks[i].buffTurns != 0)
            {
                descriptions[2 + i].text += "\nBuff Turns: "+behavior.Attacks[i].buffTurns;
            }
            if (behavior.Attacks[i].cooldown != 0)
            {
                descriptions[2 + i].text += "\nCooldown: "+behavior.Attacks[i].buffTurns;
            }
        }

        descriptions[6].text = "Movement: " + behavior.baseMove + "\n" +
                               "Dash: " + behavior.baseDash + "\n" +
                               "Attacks per turn: " + behavior.attacksPerTurn + "\n" +
                               "Hitpoints: " + behavior.HP + "/" + behavior.HP;
    }

    private String getBasicDesc(String name)
    {
        switch (name)
        {
            case "Airbender(Clone)":
                return BasicDesc[0];
            case "BattleDancer(Clone)":
                return BasicDesc[1];
            case "Guardian(Clone)":
                return BasicDesc[2];
            case "Knight(Clone)":
                return BasicDesc[3];
            case "MineMan(Clone)":
                return BasicDesc[4];
            case "Sniper(Clone)":
                return BasicDesc[5];
            case "Striker(Clone)":
                return BasicDesc[6];
            case "SwordMan(Clone)":
                return BasicDesc[7];
        }

        return "Iunno";
    }
}
