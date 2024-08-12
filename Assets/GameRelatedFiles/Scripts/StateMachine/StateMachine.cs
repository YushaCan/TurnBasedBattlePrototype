using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{

    #region Variables

    private BaseState currentState;

    #endregion

    #region Unity Methods

    private void Start()
    {
        currentState = GetInitialState();

        if (currentState != null)
        {
            currentState.Enter();
        }
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentState.Update();
        }
    }

    #endregion

    #region Custom Methods

    public void ChangeState(BaseState newState)
    {
        currentState.Exit();

        currentState = newState;

        currentState.Enter();
    }

    public void ExitStateMachine()
    {
        currentState.Exit();

        currentState = null;
    }

    #endregion

    #region Virtual Methods


    protected virtual BaseState GetInitialState()
    {
        return null;
    }

    #endregion
}
