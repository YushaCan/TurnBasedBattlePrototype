using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPhase : BaseState
{

    #region Variables



    #endregion

    #region Action Variables



    #endregion

    #region Constructor

    public EnemyPhase(BattleStateMachine stateMachine) : base("EnemyPhase", stateMachine)
    {
    }

    #endregion

    #region Overrided Methods

    public override void Enter()
    {
        base.Enter();

        BattleSceneCanvas.Instance.TurnPanel(false);

        DOVirtual.DelayedCall(1.75f, delegate
        {
            AttackToRandomHero();
        });
    }

    public override void Update()
    {
        base.Update();

        //stateMachine.ChangeState(((BattleStateMachine)stateMachine).playerPhase);
    }

    public override void Exit()
    {
        base.Exit();
    }

    #endregion

    #region Custom Methods

    private void AttackToRandomHero()
    {
        float animDuration = 0.75f;
        float spinAnimDuration = 0.5f;

        List<Hero> heroes = GameManager.Instance.GetAllHeroes();

        int randomHeroIndex = Random.Range(0, heroes.Count);

        //Vector3 targetPoint = BattleAnimationManager.Instance.heroesAttackPoints[randomHeroIndex].position;
        Vector3 targetPoint = heroes[randomHeroIndex].GetEnemyAttackPosition().position;
        Vector3 defaultPoint = BattleSpawnManager.Instance.GetEnemySpawnPoint().transform.position;

        Enemy.OnCloseHealthbar?.Invoke();

        ////////////////////////////////////////////////
        Enemy.EnemyGameObject.transform.DOMove(targetPoint, animDuration).OnComplete(delegate
        {
            // This is for trigger the damage given to the hero
            heroes[randomHeroIndex].OnDamageTaken?.Invoke(heroes[randomHeroIndex], Enemy.AttackDamage);

            //Debug.Log("<color=red> Damage Taken: " + Enemy.AttackDamage + "</color>");

            BattleAnimationManager.Instance.CameraShake();
            BattleAnimationManager.Instance.SpawnDamageCanvas(Enemy.AttackDamage, false, heroes[randomHeroIndex]);

            Enemy.EnemyGameObject.transform.DORotate(new Vector3(0, 0, 360), spinAnimDuration, RotateMode.FastBeyond360).OnComplete(delegate
            {
                DOVirtual.DelayedCall(0.25f, delegate
                {
                    Enemy.EnemyGameObject.transform.DOMove(defaultPoint, animDuration).OnComplete(delegate
                    {
                        Enemy.OnOpenHealthbar?.Invoke();

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
            stateMachine.ChangeState(((BattleStateMachine)stateMachine).playerPhase);
        }
    }

    #endregion
}
