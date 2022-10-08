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
    public Button[] AttackButtonsReal = new Button[5];
    public GameObject[] Clocks = new GameObject[5];
    public TextMeshProUGUI[] cooldownDisplay = new TextMeshProUGUI[5];
    public Button ReplayButton;
    
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
        for (int i = 0; i < Clocks.Length; i++)
        {
            cooldownDisplay[i] = Clocks[i].GetComponentInChildren<TextMeshProUGUI>();
        }
        
        ReplayButton.onClick.AddListener(delegate { gameManager.Replay(); });
    }


    public void ButtonPressed(int index)
    {
        //executes only if the attack is not on cooldown
        if (!manager.selectedCharacterBehavior.Attacks[index].onCooldown)
        {
            //Lets the grid manager know what attack is selected
            manager.currentSelectedAttack = index;

            //Sets the proper attack in the selected character
            manager.selectedCharacterBehavior.currentSelectedAttack =
                manager.selectedCharacterBehavior.Attacks[manager.currentSelectedAttack];

            //Sets up the proper attack squares
            manager.MasterGrid.WipeAttackingSquares();
            manager.selectedCharacterBehavior.onSelect();
        }
    }
    
    

    // Update is called once per frame
    void Update()
    {
        if (gameManager.currentState == GameManager.GameState.GameOver)
        {
            SidePanel.SetActive(false);
            ReplayButton.gameObject.SetActive(true);
            return;
        }
        
        ReplayButton.gameObject.SetActive(false);
        
        if (manager.selectedCharacter != null)
        {
                SidePanel.SetActive(true);
                AbstractAttack current = manager.selectedCharacterBehavior.currentSelectedAttack;
                manager.currentSelectedAttack = manager.selectedCharacterBehavior.currentSelectedAttack.ID;
                if (current.targeting.Equals(AbstractAttack.AttackType.SELF))
                {
                    DescBox.text = current.AttackDesc;
                    RangeBox.text = "Amount: " + current.buffAmount;
                    DamageBox.text = "Duration: " + current.buffTurns;
                }
                else
                {
                    DescBox.text = current.AttackDesc;
                    RangeBox.text = "Range: " + current.AttackRange;
                    DamageBox.text = "Damage: " + current.AttackDamage + " / " + current.OptimalDamage;
                }

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
                        if (manager.selectedCharacterBehavior.Attacks[i].onCooldown)
                        {
                            Clocks[i].SetActive(true);
                            cooldownDisplay[i].gameObject.SetActive(true);
                            cooldownDisplay[i].text = "" + manager.selectedCharacterBehavior.Attacks[i].currentCooldown;
                            var colors = AttackButtonsReal[i].colors;
                            colors.normalColor = Color.grey;
                            colors.selectedColor = Color.grey;
                            colors.highlightedColor = new Color(0.4f, 0.4f, 0.2f, 1f);
                            AttackButtonsReal[i].colors = colors;
                        }
                        else
                        {
                            Clocks[i].SetActive(false);
                            cooldownDisplay[i].gameObject.SetActive(false);
                            var colors = AttackButtonsReal[i].colors;
                            colors.normalColor = Color.white;
                            colors.selectedColor = Color.white;
                            colors.highlightedColor = Color.red;
                            AttackButtonsReal[i].colors = colors;
                        }
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
