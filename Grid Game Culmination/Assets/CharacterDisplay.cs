using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterDisplay : MonoBehaviour
{

    protected Model_Game gameModel;
    public GameManager gameManager;
    public GridManager gridManager;
    public BaseBehavior character;
    public int currentHP;
    public int maxHP;
    public bool hasDefenseBuff;
    public bool hasAttackBuff;
    public TextMeshProUGUI defenseText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI hpText;
    public List<GameObject> Mods = new List<GameObject>();
    public RectTransform HealthBar;
    public List<TextMeshProUGUI> ModTexts = new List<TextMeshProUGUI>();
    public List<Image> ModIcons = new List<Image>();
    public Model_Modifiers modsModel;

    // Start is called before the first frame update
    void Start()
    {
        gridManager = GameObject.Find("GameManager").GetComponent<GridManager>();
        gameModel = GameObject.Find("GameModel").GetComponent<Model_Game>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        modsModel = GameObject.Find("GameModel").GetComponent<Model_Modifiers>();
    }

    public void initialize()
    {
        maxHP = character.HP;
        currentHP = maxHP;
        hpText.text = "HP: " + currentHP + "/" + maxHP;
        hasAttackBuff = false;
        hasDefenseBuff = false;
        foreach (GameObject g in Mods)
        {
            g.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        setBuffs();
        setHealth();
    }

    void setHealth()
    {
        currentHP = character.HP;
        float hpScale = (float) currentHP / maxHP;
        HealthBar.localScale = new Vector3(1, hpScale, 1);
        hpText.text = "HP: " + currentHP + "/" + maxHP;
    }

    void setBuffs()
    {
        foreach (GameObject g in Mods)
        {
            g.SetActive(false);
        }
        
        if (character.Modifiers.Count != 0)
        {

            for (int k = 0; k < character.Modifiers.Count; k++)
            {
                if (character.Modifiers[k] != null)
                {
                    Mods[k].SetActive(true);
                    ModTexts[k].text = character.Modifiers[k].setDesc();
                    ModIcons[k].sprite = modsModel.modIcons[character.Modifiers[k].getKey()];
                }
            }
        }
    }
}
