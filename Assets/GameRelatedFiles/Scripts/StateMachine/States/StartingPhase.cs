using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingPhase : BaseState
{

    #region Variables



    #endregion

    #region Action Variables



    #endregion

    #region Constructor

    public StartingPhase(BattleStateMachine stateMachine) : base("StartingPhase", stateMachine)
    {
    }

    #endregion

    #region Overrided Methods

    public override void Enter()
    {
        base.Enter();

        GameManager.Instance.OnBattleStart += EnterBattle;

    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();

        GameManager.Instance.OnBattleStart -= EnterBattle;
    }

    #endregion

    #region Custom Methods

    private void EnterBattle()
    {
        stateMachine.ChangeState(((BattleStateMachine)stateMachine).playerPhase);
    }

    #endregion
}
