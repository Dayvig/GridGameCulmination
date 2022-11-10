﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using TMPro;
using UnityEditor.Experimental;
using UnityEngine;

public class BaseBehavior : MonoBehaviour
{
    public Model_Game gameModel;
    public GameManager manager;
    public GridManager gridManager;
    public AbstractCharacter values;
    public int HP;
    public int move;
    public int dash;
    public int baseDash;
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
    public Sprite portrait;
    public SpriteRenderer mapRen;
    public String passive;

    public GameManager.Player owner;
    public List<AbstractModifier> Modifiers = new List<AbstractModifier>();
    public List<AbstractModifier> toRemove = new List<AbstractModifier>();
    public bool specialMovement;
    public bool isOverHealed;
    public bool isGhost;
    
    void Start()
    {
    }

    void Update()
    {
        
    }

    public virtual void onSpecialMovement() { }

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
            gridManager.count = 0;
            gridManager.MasterGrid.wipeTimesSeen();
            showMovementSquares(this.currentCell, move, dash);
        }
    }

    public virtual void onMove(GridCell moveTo)
    {
        onDisplace(moveTo);
        currentMoves--;
        if (moveTo.movementCount < dash)
        {
            currentAttacks--;
        }
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

    public virtual void onDisplace(GridCell moveTo)
    {
        List<AbstractModifier> toRemove = new List<AbstractModifier>();
        foreach (AbstractModifier a in Modifiers)
        {
            if (a.isTerrainModifier)
            {
                toRemove.Add(a);
            }
        }
        foreach (AbstractModifier r in toRemove)
        {
            Modifiers.Remove(r);
        }

        AbstractModifier newTerrainMod = moveTo.getTerrainMod();
        if (newTerrainMod != null)
        {
            newTerrainMod.setStrings();
            Modifiers.Add(newTerrainMod);
        }
    }
    
    public virtual void showMovementSquares(GridCell startingCell, int movementValue, int dashValue)
    {
        gridManager.MasterGrid.wipeTimesSeen();
        //Creates a list for all tiles that can be moved to, and adds the starting cell to it.
        List<GridCell> inRangeCells = new List<GridCell>();
        List<GridCell> surroundingCells = new List<GridCell>();
        List<GridCell> prevCells = new List<GridCell>();
        startingCell.movementCount = movementValue + dashValue;
        inRangeCells.Add(startingCell);
        prevCells.Add(startingCell);
        bool stop = false;

        do
        {
            gridManager.count++;
            stop = true;
            foreach (GridCell g in prevCells)
            {
                for (int i = 0; i < 4; i++)
                {
                    if (g.neighbors[i] != null && g.neighbors[i].terrainType != 0)
                    {
                        int movePenaltyToGive = g.movementPenalty();

                        if (g.neighbors[i].movementCount < g.movementCount)
                        {
                            if (g.neighbors[i].isNumModified)
                            {
                                if (g.neighbors[i].movementCount < g.movementCount - movePenaltyToGive)
                                {
                                    g.neighbors[i].movementCount = g.movementCount - movePenaltyToGive;
                                    surroundingCells.Add(g.neighbors[i]);
                                }
                            }
                            else
                            {
                                g.neighbors[i].movementCount = g.movementCount - movePenaltyToGive;
                                g.neighbors[i].isNumModified = true;
                                surroundingCells.Add(g.neighbors[i]);
                            } 
                        }
                        if (g.neighbors[i].movementCount >= 0 && g.neighbors[i].occupant == null)
                        {
                            inRangeCells.Add(g.neighbors[i]);
                            stop = false;
                        }
                    }
                }
            }
            prevCells.Clear();
            prevCells = surroundingCells.Distinct().ToList();
            
            if (gridManager.count > (movementValue+dashValue) * 2)
            {
                stop = true; 
            }
        } while (!stop);

        foreach (GridCell g in inRangeCells)
        {
            g.canMoveTo(g.movementCount > dashValue-1);
        }
    }

    public virtual void onAttack(BaseBehavior target, bool isOptimal)
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

    public void onAttackGround(GridCell target)
    {
        GroundTarget nextAttack = (GroundTarget)currentSelectedAttack;
        //launches the attack
        nextAttack.groundUse(this, target);
        
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
        int damage = baseDamage;
        foreach (AbstractModifier a in initiator.Modifiers)
        {
            if (a.aType == AbstractModifier.applicationType.OFFENSIVE) 
                damage = a.applyModifier(damage, target, initiator);
        }
        foreach (AbstractModifier a in target.Modifiers)
        {
            if (a.aType == AbstractModifier.applicationType.DEFENSIVE) 
            damage = a.applyModifier(damage, target, initiator);
        }
        return damage;
    }
    
    public int calculateDamage(int baseDamage, BaseBehavior target)
    {
        int damage = baseDamage;
        foreach (AbstractModifier a in target.Modifiers)
        {
            if (a.aType == AbstractModifier.applicationType.DEFENSIVE) 
                damage = a.applyModifier(damage, target, this);
        }
        return damage;
    }

    public void endTurnModifierCheck()
    {
        for (int i = 0; i < Modifiers.Count; i++)
        {
            //Decrements turn based modifiers
            if (Modifiers[i] != null && Modifiers[i].turnBased)
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
        if (Modifiers.Count > 0)
        {
            Debug.Log("Turns" + Modifiers[0].turns);
        }
        
        //ticks down health if overhealed;
        if (isOverHealed)
        {
            HP--;
            if (HP <= values.hp)
            {
                isOverHealed = false;
            }
        }
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


    public virtual void onReset()
    {
        GlowRen.color = Color.blue;
        if (isGhost)
        {
            if (owner == GameManager.Player.Player1)
            {
                Color thisColor = mapRen.color;
                mapRen.color = new Color(thisColor.r, thisColor.g, thisColor.b, 0.5f);
            }
        }
    }

    public virtual void Initialize()
    {
        gameModel = GameObject.Find("GameModel").GetComponent<Model_Game>();
        gridManager = GameObject.Find("GameManager").GetComponent<GridManager>();
        manager = GameObject.Find("GameManager").GetComponent<GameManager>();
        mapRen = GetComponent<SpriteRenderer>();
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