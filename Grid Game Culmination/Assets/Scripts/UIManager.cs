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
        DescBox.gameObject.SetActive(false);
        RangeBox.gameObject.SetActive(false);
        DamageBox.gameObject.SetActive(false);
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
        
        //turns off all selectors other than the selected button
        for (int i = 0; i < Selectors.Length; i++)
        {
            if (i == index)
            {
                Selectors[i].SetActive(true);
            }
            else
            {
                Selectors[i].SetActive(false);
            }
        }
    }
    
    

    // Update is called once per frame
    void Update()
    {
        if (manager.selectedCharacter != null)
        {
            SidePanel.SetActive(true);
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

            AbstractAttack current = manager.selectedCharacterBehavior.Attacks[manager.currentSelectedAttack];
            DescBox.text = current.AttackDesc;
            RangeBox.text = "Range: " + current.AttackRange;
            DamageBox.text = "Damage: " + current.AttackDamage;
        }
        else
        {
            SidePanel.SetActive(false);
        }
    }
}
