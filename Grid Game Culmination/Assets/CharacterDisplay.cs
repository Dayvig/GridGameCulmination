using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEngine;

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
    public int DefenseBuffAmount;
    public int AttackBuffAmount;
    public int DefenseBuffTurns;
    public int AttackBuffTurns;
    public TextMeshProUGUI defenseText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI hpText;
    public GameObject DefenseBuff;
    public GameObject AttackBuff;
    public RectTransform HealthBar;


    // Start is called before the first frame update
    void Start()
    {
        gridManager = GameObject.Find("GameManager").GetComponent<GridManager>();
        gameModel = GameObject.Find("GameModel").GetComponent<Model_Game>();
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        
    }

    public void initialize()
    {
        maxHP = character.HP;
        currentHP = maxHP;
        hpText.text = "HP: " + currentHP + "/" + maxHP;
        hasAttackBuff = false;
        hasDefenseBuff = false;
    }

    // Update is called once per frame
    void Update()
    {
        checkBuffs();
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

    void checkBuffs()
    {
        hasAttackBuff = false;
        hasDefenseBuff = false;
        foreach (AbstractModifier m in character.Modifiers)
        {
            if (m.ID.Equals(DefenseBonusModifier.modID))
            {
                activateDefenseBuff();
                DefenseBuffAmount = m.amount;
                DefenseBuffTurns = m.turns;
            }
            if (m.ID.Equals(AttackBonusModifier.modID))
            {
                activateAttaclBuff();
                AttackBuffAmount = m.amount;
                AttackBuffTurns = m.turns;
            }
        }
    }
    
    void setBuffs()
    {
        if (!hasAttackBuff)
        {
            AttackBuff.SetActive(false);
        }
        else
        {
            attackText.text = "Increases damage dealt by " + AttackBuffAmount + " for " + AttackBuffTurns;
            if (AttackBuffTurns == 1)
            {
                attackText.text += " turn.";
            }
            else
            {
                attackText.text += " turns.";
            }
        }

        if (!hasDefenseBuff)
        {
            DefenseBuff.SetActive(false);
        }
        else
        {
            defenseText.text = "Decreases damage taken by " + DefenseBuffAmount + " for " + DefenseBuffTurns;
            if (DefenseBuffTurns == 1)
            {
                defenseText.text += " turn.";
            }
            else
            {
                defenseText.text += " turns.";
            }
        }
    }

    void activateDefenseBuff()
    {
        DefenseBuff.SetActive(true);
        hasDefenseBuff = true;
    }
    
    void activateAttaclBuff()
    {
        AttackBuff.SetActive(true);
        hasAttackBuff = true;
    }
}
