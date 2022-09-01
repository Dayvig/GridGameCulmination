using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{

    public Model_Game gameModel;
    public GameManager gameManager;
    public GridManager manager;
    public GameObject[] AttackButtons = new GameObject[5];
    public GameObject[] Selectors = new GameObject[5];
    public Text[] ButtonTexts = new Text[5];
    public TextMeshProUGUI DescBox;
    public TextMeshProUGUI RangeBox;
    public TextMeshProUGUI DamageBox;
    public GameObject SidePanel;
    
    // Start is called before the first frame update
    void Start()
    {
        gameModel = GameObject.Find("GameModel").GetComponent<Model_Game>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        manager = GameObject.Find("GameManager").GetComponent<GridManager>();
        for (int i = 0; i < AttackButtons.Length; i++)
        {
            Button btn = AttackButtons[i].GetComponent<Button>();
            int index = i;
            btn.onClick.AddListener(delegate { ButtonPressed(index); });
        }
        Selectors[0].SetActive(true);
    }

    public void ButtonPressed(int index)
    {
        //Lets the grid manager know what attack is selected
        manager.currentSelectedAttack = index;
        
        //Sets the proper attack in the selected character
        manager.selectedCharacterBehavior.currentSelectedAttack = manager.selectedCharacterBehavior.Attacks[manager.currentSelectedAttack];
        
        //Sets up the proper attack squares
        manager.MasterGrid.WipeAttackingSquares();
        manager.selectedCharacterBehavior.onSelect();
        
        
    }
    
    

    // Update is called once per frame
    void Update()
    {
        if (manager.selectedCharacter != null)
        {
            SidePanel.SetActive(true);
            AbstractAttack current = manager.selectedCharacterBehavior.currentSelectedAttack;
            manager.currentSelectedAttack = manager.selectedCharacterBehavior.currentSelectedAttack.ID;
            DescBox.text = current.AttackDesc;
            RangeBox.text = "Range: " + current.AttackRange;
            DamageBox.text = "Damage: " + current.AttackDamage;
            
            //turns off all selectors other than the selected button
            for (int i = 0; i < Selectors.Length; i++)
            {
                if (i == manager.currentSelectedAttack)
                {
                    Selectors[i].SetActive(true);
                }
                else
                {
                    Selectors[i].SetActive(false);
                }
            }
            
            for (int i = 0; i < manager.selectedCharacterBehavior.Attacks.Length; i++)
            {
                if (manager.selectedCharacterBehavior.Attacks[i] != null)
                {
                    AttackButtons[i].SetActive(true);
                    ButtonTexts[i].text = manager.selectedCharacterBehavior.Attacks[i].AttackName;
                }
                else
                {
                    AttackButtons[i].SetActive(false);
                }
            }
        }
        else
        {
            SidePanel.SetActive(false);
        }
    }
}
