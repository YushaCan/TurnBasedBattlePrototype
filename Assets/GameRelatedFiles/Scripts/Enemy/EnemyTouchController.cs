using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTouchController : MonoBehaviour
{
    #region Variables

    private Enemy enemy;

    #endregion

    #region Action Variables

    

    #endregion

    #region Mouse Mechanic Implementations

    private void OnMouseDown()
    { 
        bool isHeroSelected = GameManager.Instance.IsAnyHeroSelected();

        if (GameManager.Instance.HasBattleStarted && GameManager.Instance.IsPlayerTurn && isHeroSelected)
        {
            OnEnemyTarget();
        }
        else if (GameManager.Instance.HasBattleStarted && GameManager.Instance.IsPlayerTurn && !isHeroSelected)
        {
            // POPUP THAT SAYS: SELECT A HERO FIRST, THEN ATTACK
            //Debug.LogWarning("SELECT A HERO FIRST, THEN ATTACK");
        }
    }

    private void OnMouseUp()
    {

    }

    #endregion

    #region Unity Methods

    private void Start()
    {
        enemy = gameObject.GetComponent<Enemy>();
    }

    private void Update()
    {
       
    }

    #endregion

    #region Custom Methods

    private void OnEnemyTarget()
    {
        // This is for blocking player to make any movements.
        GameManager.Instance.IsPlayerTurn = false;
 
        // HERO ATTACK TO ENEMY
        GameManager.Instance.OnEnemyTargeted?.Invoke();


        // FOR close the tutorial text if its open
        BattleSceneCanvas.Instance.CheckTutorialText();
    }

    #endregion
}
