using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEditor;
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
    public TextMeshProUGUI PassiveBox;
    public GameObject SidePanel;
    public Button[] AttackButtonsReal = new Button[5];
    public GameObject[] Clocks = new GameObject[5];
    public TextMeshProUGUI[] cooldownDisplay = new TextMeshProUGUI[5];
    public Button ReplayButton;
    public GameObject tooltip;
    public TextMeshPro tooltiptext;
    public int tooltipOffsetX;
    public int tooltipOffsetY;
    public int tooltipDist;
    public float hoverCtr;
    public float hoverTime;
    public bool toolTipTrigger = false;
    
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
        //executes only if the attack is not on cooldown or unit is not out of attacks
        if (!manager.selectedCharacterBehavior.Attacks[index].onCooldown && manager.selectedCharacterBehavior.currentAttacks > 0)
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


    public void tooltipUpdate()
    {
        if (toolTipTrigger) {hoverCtr += Time.deltaTime;}
        if (toolTipTrigger && hoverCtr > hoverTime)
        {
            tooltip.SetActive(true);
            Vector3 toPos = new Vector3(Input.mousePosition.x - tooltipOffsetX, Input.mousePosition.y - tooltipOffsetY,
                tooltipDist);
            tooltip.transform.position = Camera.main.ScreenToWorldPoint(toPos);
        }
        else
        {
            tooltip.SetActive(false);
        }
    }
    
    
    // Update is called once per frame
    void Update()
    {
        tooltipUpdate();
        
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
                PassiveBox.text = "Passive: "+manager.selectedCharacterBehavior.passive;
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
                        //sets it to a normal state
                        var colors = AttackButtonsReal[i].colors;
                        colors.normalColor = Color.white;
                        colors.selectedColor = Color.white;
                        colors.highlightedColor = Color.red;
                        AttackButtonsReal[i].colors = colors;
                        AttackButtons[i].SetActive(true);
                        ButtonTexts[i].text = manager.selectedCharacterBehavior.Attacks[i].AttackName;
                        
                        if (manager.selectedCharacterBehavior.currentAttacks <= 0)
                        {
                            colors.normalColor = Color.grey;
                            colors.selectedColor = Color.grey;
                            colors.highlightedColor = new Color(0.4f, 0.4f, 0.2f, 1f);
                            AttackButtonsReal[i].colors = colors;
                        }
                        if (manager.selectedCharacterBehavior.Attacks[i].onCooldown)
                        {
                            Clocks[i].SetActive(true);
                            cooldownDisplay[i].gameObject.SetActive(true);
                            cooldownDisplay[i].text = "" + manager.selectedCharacterBehavior.Attacks[i].currentCooldown;
                            colors = AttackButtonsReal[i].colors;
                            colors.normalColor = Color.grey;
                            colors.selectedColor = Color.grey;
                            colors.highlightedColor = new Color(0.4f, 0.4f, 0.2f, 1f);
                            AttackButtonsReal[i].colors = colors;
                        }
                        else
                        {
                            Clocks[i].SetActive(false);
                            cooldownDisplay[i].gameObject.SetActive(false);
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
    
    //From FlashG
    /*https://gist.github.com/FlaShG/ac3afac0ef65d98411401f2b4d8a43a5
        public static Vector3 WorldToCanvasPosition(this Canvas canvas, Vector3 worldPosition, Camera camera = null)
        {
            if (camera == null)
            {
                camera = Camera.main;
            }
            var viewportPosition = camera.WorldToViewportPoint(worldPosition);
            return canvas.ViewportToCanvasPosition(viewportPosition);
        }

        public static Vector3 ScreenToCanvasPosition(this Canvas canvas, Vector3 screenPosition)
        {
            var viewportPosition = new Vector3(screenPosition.x / Screen.width,
                screenPosition.y / Screen.height,
                0);
            return canvas.ViewportToCanvasPosition(viewportPosition);
        }

        public static Vector3 ViewportToCanvasPosition(this Canvas canvas, Vector3 viewportPosition)
        {
            var centerBasedViewPortPosition = viewportPosition - new Vector3(0.5f, 0.5f, 0);
            var canvasRect = canvas.GetComponent<RectTransform>();
            var scale = canvasRect.sizeDelta;
            return Vector3.Scale(centerBasedViewPortPosition, scale);
        }*/
        
}


