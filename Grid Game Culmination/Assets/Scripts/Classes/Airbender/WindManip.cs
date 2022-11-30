using DefaultNamespace;
using UnityEngine;

namespace Classes.Airbender
{
    public class WindManip : AbstractAttack, GroundTarget
    {
        private GridManager gridManager;
        public Sprite upArrow;
        public Sprite downArrow;
        public int targetColumn;
        private bool knockBackUp;
        private int knockback = 2;
        public void Start()
        {
            gridManager = GameObject.Find("GameManager").GetComponent<GridManager>();
        }
        public override void use(BaseBehavior initiator, BaseBehavior target, bool isOptimal)
        {
            KnockBack(initiator, target, knockback, knockBackUp ? 0 : 2);
        }

        public override void showSelectedSquares(GridCell origin, bool isBuff)
        {
            TacticsGrid grid = gridManager.MasterGrid;
            GridCell targetCell;
            targetColumn = origin.column;
            
            for (int row = 0; row < grid.contents.Count; row++)
            {
              targetCell = grid.contents[row].contents[targetColumn];
              targetCell.showAttackHovered(isBuff);
              if (gridManager.selectedCharacterBehavior.currentCell.row < origin.row) { 
                  targetCell.TerrainSprite.enabled = true;
                  targetCell.TerrainSprite.sprite = upArrow;
                  knockBackUp = true;
              }
              else
              {
                  targetCell.TerrainSprite.enabled = true;
                  targetCell.TerrainSprite.sprite = downArrow;
                  knockBackUp = false;
              } 
            }
        }

        public void groundUse(BaseBehavior initiator, GridCell target)
        {
            GameManager.Sounds.PlayOneShot(attackSound, GameManager.MasterVolume);

            //Decrease the current amount of attacks
            initiator.currentAttacks--;
            
            //puts the move on cooldown
            onCooldown = true;
            currentCooldown = cooldown;
            initiator.currentSelectedAttack = initiator.Attacks[0];
        
            TacticsGrid grid = gridManager.MasterGrid;
            GridCell targetCell;
            for (int row = 0; row < grid.contents.Count; row++)
            {
                targetCell = grid.contents[row].contents[targetColumn];
                if (targetCell != null && targetCell.occupant != null)
                {
                    BaseBehavior targetB = targetCell.occupant.GetComponent<BaseBehavior>();
                    Debug.Log(targetB.name);
                    if (targetB.owner != initiator.owner)
                        use(initiator, targetB, false);
                }
            }
            grid.resetTerrainSprites();
        }
    
        public override void showAttackingSquares(GridCell startingCell, int range, AttackType targetingType)
        {
            TacticsGrid grid = gridManager.MasterGrid;
            GridCell targetCell;
            for (int rowCursor = 0; rowCursor < grid.contents.Count; rowCursor++)
            {
                for (int colCursor = 0; colCursor < grid.contents[rowCursor].contents.Count; colCursor++)
                {
                    targetCell = grid.contents[rowCursor].contents[colCursor];
                    if (targetCell != startingCell)
                        targetCell.isAttackable();
                }
            }        
        }
        
        void KnockBack(BaseBehavior initiator, BaseBehavior target, int knockback, int dir)
        {
            int damage;
            GridCell destinationCell = target.currentCell;
            for (int i = 0; i < knockback; i++)
            {
                if (!canKnockBackToCell(destinationCell.neighbors[dir]))
                {
                    if (destinationCell != target.currentCell)
                    {
                        damage = initiator.calculateDamage(OptimalDamage, target, initiator);
                        target.currentCell.occupant = null;
                        target.currentCell = destinationCell;
                        target.currentCell.occupant = target.gameObject;
                        target.onDisplace(target.currentCell);
                    }
                    else
                    {
                        damage = initiator.calculateDamage(OptimalDamage, target, initiator);
                    }

                    if (i != knockback - 1)
                    {
                        bonkDamage(destinationCell.neighbors[dir], initiator);
                    }
                    initiator.damageTarget(damage, target);
                    return;
                }
                destinationCell = destinationCell.neighbors[dir];
            }
            //knock to final cell
            damage = initiator.calculateDamage(AttackDamage, target, initiator);
            target.currentCell.occupant = null;
            target.currentCell = destinationCell;
            target.currentCell.occupant = target.gameObject;
            target.onDisplace(target.currentCell);
            initiator.damageTarget(damage, target);
        }

        private void bonkDamage(GridCell g, BaseBehavior initiator)
        {
            if (g != null && g.occupant != null)
            {
                BaseBehavior bonkTarget = g.occupant.GetComponent<BaseBehavior>();
                if (bonkTarget.owner != initiator.owner)
                {
                    int bonkDamage = bonkTarget.calculateDamage(2, bonkTarget);
                    bonkTarget.HP -= bonkDamage;
                    bonkTarget.updateBars();
                }
            }
        }

        private bool canKnockBackToCell(GridCell g)
        {
            return (g != null && g.terrainType != 0 && g.occupant == null);
        }
    }
}