using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPhase : BaseState
{

    #region Variables



    #endregion

    #region Action Variables



    #endregion

    #region Constructor

    public PlayerPhase(BattleStateMachine stateMachine) : base("PlayerPhase", stateMachine)
    {
    }

    #endregion

    #region Overrided Methods

    public override void Enter()
    {
        base.Enter();

        GameManager.Instance.OnEnemyTargeted += AttackToEnemy;

        GameManager.Instance.IsPlayerTurn = true;


        if (BattleSceneCanvas.Instance.IsFirstOpening)
        {
            BattleSceneCanvas.Instance.IsFirstOpening = false;
        }
        else
        {
            BattleSceneCanvas.Instance.TurnPanel(true);
        }
    }

    public override void Update()
    {
        base.Update();


        //stateMachine.ChangeState(((BattleStateMachine)stateMachine).enemyPhase);
    }

    public override void Exit()
    {
        base.Exit();

        GameManager.Instance.OnEnemyTargeted -= AttackToEnemy;

        GameManager.Instance.IsPlayerTurn = false;
    }

    #endregion

    #region Custom Methods

    private void AttackToEnemy()
    {
        Hero attackingHero = GameManager.Instance.GetSelectedHero();

        GameManager.Instance.SetAllHeroesDeselected();


        // ATTACK EVENT HERE

        AttackEnemyEvent(attackingHero);

        /////////////////////////
       
    }

    private void AttackEnemyEvent(Hero attackingHero)
    {
        float animDuration = 0.75f;
        float spinAnimDuration = 0.5f;

        Vector3 targetPoint = BattleAnimationManager.Instance.enemyAttackPoint.position;
        Vector3 defaultPoint = attackingHero.gameObject.transform.position;

        attackingHero.CloseHealthbarForAttack();

        attackingHero.gameObject.transform.DOMove(targetPoint, animDuration).OnComplete(delegate
        {
            // This is for trigger the damage given to the enemy
            Enemy.OnDamageTaken?.Invoke(attackingHero.HeroSpecs.AttackDammage);

            //Debug.Log("<color=cyan> Damage Dealt: " + attackingHero.HeroSpecs.AttackDammage + "</color>");

            BattleAnimationManager.Instance.CameraShake();
            BattleAnimationManager.Instance.SpawnDamageCanvas(attackingHero.HeroSpecs.AttackDammage, true, attackingHero);

            attackingHero.gameObject.transform.DORotate(new Vector3(0, 0, 360), spinAnimDuration, RotateMode.FastBeyond360).OnComplete(delegate
            {
                DOVirtual.DelayedCall(0.25f, delegate
                {
                    attackingHero.gameObject.transform.DOMove(defaultPoint, animDuration).OnComplete(delegate
                    {
                        attackingHero.OpenHealthbarAfterAttack();

                        ControlBattleStatus();
                    });
                });
                
            });
        });
    }

    private void ControlBattleStatus()
    {
        bool isBattleFinished = GameManager.Instance.ControlBattleFinished();

        if (isBattleFinished)
        {
            stateMachine.ChangeState(((BattleStateMachine)stateMachine).endPhase);
        }
        else
        {
            stateMachine.ChangeState(((BattleStateMachine)stateMachine).enemyPhase);
        }
    }

    #endregion
}
