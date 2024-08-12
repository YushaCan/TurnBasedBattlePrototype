using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleStateMachine : StateMachine
{

    #region Variables

    [HideInInspector]
    public StartingPhase startingPhase;

    [HideInInspector]
    public PlayerPhase playerPhase;

    [HideInInspector]
    public EnemyPhase enemyPhase;

    [HideInInspector]
    public EndPhase endPhase;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        startingPhase = new StartingPhase(this);
        playerPhase = new PlayerPhase(this);
        enemyPhase = new EnemyPhase(this);
        endPhase = new EndPhase(this);
    }

    #endregion

    #region Override Methods

    protected override BaseState GetInitialState()
    {
        return startingPhase;
    }

    #endregion
}
