using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPhase : BaseState
{

    #region Variables



    #endregion

    #region Action Variables



    #endregion

    #region Constructor

    public EndPhase(BattleStateMachine stateMachine) : base("EndPhase", stateMachine)
    {
    }

    #endregion

    #region Overrided Methods

    public override void Enter()
    {
        base.Enter();

        GameManager.Instance.OnBattleFinished += ExitFromStateMachine;

        BattleSceneCanvas.Instance.OpenGameOverUI(GameManager.Instance.IsGameWin());
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();

        GameManager.Instance.OnBattleFinished -= ExitFromStateMachine;

        Debug.Log("<color=white> Exit from the State Machine </color>");
    }

    #endregion

    #region Custom Methods

    private void ExitFromStateMachine()
    {
        stateMachine.ExitStateMachine();


        //int count = GameManager.Instance.GetAllHeroes().Count;
        //Debug.Log("<color=blue> HEROES: " + count + "</color>");

        //int count2 = GameManager.Instance.SelectedHerosForBattle.Count;
        //Debug.Log("<color=blue> SELECTED HEROES: " + count2 + "</color>");
    }

    #endregion



}
