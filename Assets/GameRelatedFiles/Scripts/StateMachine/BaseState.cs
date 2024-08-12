using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState
{

    #region Variables

    public string name;
    protected StateMachine stateMachine;

    #endregion

    #region Base State Constructor

    public BaseState(string name, StateMachine stateMachine)
    {
        this.name = name;
        this.stateMachine = stateMachine;
    }

    #endregion

    #region Virtual Methods

    public virtual void Enter() { Debug.Log("<color=white> Entered NEW State: " + name + "</color>"); }
    public virtual void Update() { }
    public virtual void Exit() { }

    #endregion


}
