using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using TMPro;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.AI;

public class BaseBehavior : MonoBehaviour
{
    public Model_Game gameModel;
    public GameManager manager;
    public GridManager gridManager;
    public AbstractCharacter values;
    public int HP;
    public int move;
    public int attack;
    public String name;
    public GridCell currentCell;
    public int currentMoves;
    public int movesPerTurn;
    public int attacksPerTurn;
    public int currentAttacks;
    public int baseMove;
    public SpriteRenderer GlowRen;
    public RectTransform HealthBar;
    public AbstractAttack[] Attacks = new AbstractAttack[5];
    public AbstractAttack currentSelectedAttack;
    public List<GameObject> buffIcons = new List<GameObject>();
    public List<TextMeshProUGUI> buffTexts = new List<TextMeshProUGUI>();

    public GameManager.Player owner;
    public List<AbstractModifier> Modifiers = new List<AbstractModifier>();
    public List<AbstractModifier> toRemove = new List<AbstractModifier>();
    
    void Start()
    {
    }

    void Update()
    {
        
    }

    public void onSelect()
    {
        if (currentMoves <= 0)
        {
            manager.currentState = GameManager.GameState.CharacterAttacking;
            gridManager.currentSelectedAttack = currentSelectedAttack.ID;
            currentSelectedAttack.showAttackingSquares(this.currentCell, currentSelectedAttack.AttackRange, currentSelectedAttack.targeting);
        }
        else
        {
            manager.currentState = GameManager.GameState.CharacterMovement;
            gridManager.showMovementSquares(this.currentCell, move);
        }
    }

    public void onMove()
    {
        currentMoves--;
        if (currentMoves <= 0 && currentAttacks <= 0)
        {
            GlowRen.color = Color.gray;
        }
        else if (currentMoves <= 0)
        {
            GlowRen.color = Color.red;
        }
        manager.checkForNextTurn(owner);
    }

    public void onAttack(BaseBehavior target, bool isOptimal)
    {
        //launches the attack
        currentSelectedAttack.use(this, target, isOptimal);
        
        //sets the correct glow
        if (currentMoves <= 0 && currentAttacks <= 0)
        {
            GlowRen.color = Color.gray;
        }
        else if (currentMoves <= 0)
        {
            GlowRen.color = Color.red;
        }
        
        //resets the gamestate and checks for next turn
        manager.gridManager.DeselectAll();
        manager.checkForNextTurn(owner);
    }

    public int calculateDamage(int baseDamage, BaseBehavior target, BaseBehavior initiator)
    {
        if (initiator.hasModifier(AttackBonusModifier.modID))
        {
            Debug.Log(baseDamage);
            baseDamage += initiator.getModifier(AttackBonusModifier.modID).amount;
        }
        if (target.hasModifier(DefenseBonusModifier.modID))
        {
            Debug.Log(baseDamage);
            baseDamage -= target.getModifier(DefenseBonusModifier.modID).amount;
        }
        
        Debug.Log(baseDamage);
        return baseDamage;
    }

    public void endTurnModifierCheck()
    {
        for (int i = 0; i < Modifiers.Count; i++)
        {
            //Decrements turn based modifiers
            if (Modifiers[i].turnBased)
            {
                Modifiers[i].turns--;
                if (Modifiers[i].turns <= 0)
                {
                    toRemove.Add(Modifiers[i]);
                }
            }
            //Activates any end of turn modifiers
            
            
        }

        //Removes the mods that have expired
        for (int k = 0; k < toRemove.Count; k++)
        {
            Modifiers.Remove(toRemove[k]);
        }
        toRemove.Clear();
    }

    public void endTurn()
    {
        currentAttacks = 0;
        currentMoves = 0;
        GlowRen.color = Color.gray;
        manager.gridManager.DeselectAll();
        manager.checkForNextTurn(owner);
    }
    
    public void updateBars()
    {
        float hpScale = (float) HP / values.hp;
        HealthBar.localScale = new Vector3(1, hpScale, 1);
        if (HP <= 0)
        {
            kill();
        }
    }


    public void onReset()
    {
        GlowRen.color = Color.blue;
    }

    public virtual void Initialize()
    {
        gameModel = GameObject.Find("GameModel").GetComponent<Model_Game>();
        gridManager = GameObject.Find("GameManager").GetComponent<GridManager>();
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    public void kill()
    {
        this.currentCell.occupant = null;
        this.currentCell = null;
        this.currentSelectedAttack = null;
        this.gameObject.SetActive(false);
    }

    public bool hasModifier(String modID)
    {
        foreach (AbstractModifier nextMod in Modifiers)
        {
            if (nextMod.ID.Equals(modID))
            {
                return true;
            }
        }

        return false;
    }

    public AbstractModifier getModifier(String m)
    {
        foreach (AbstractModifier nextMod in Modifiers)
        {
            if (nextMod.ID.Equals(m))
            {
                return nextMod;
            }
        }
        return null;
    }
}