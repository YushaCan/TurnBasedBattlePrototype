using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HeroTouchController : MonoBehaviour
{
    #region Variables

    [SerializeField] private Hero hero;

    private bool isPointerDown = false;
    private float pointerDownTimer = 0f;

    private bool onLongPress;

    #endregion

    #region Action Variables



    #endregion

    #region Mouse Mechanic Implementations

    private void OnMouseDown()
    {
        if (GameManager.Instance.HasBattleStarted && GameManager.Instance.IsPlayerTurn)
        {
            isPointerDown = true;
            pointerDownTimer = 0f;
        }
    }

    private void OnMouseUp()
    {

        if (onLongPress)
        {
            hero.CloseHeroInfoPopup();
        }

        if (isPointerDown)
        {
            if (pointerDownTimer < GameManager.Instance.RequiredHoldTimeToExamine)
            {
                OnShortPress();
            }
            ResetPointer();
        }
    }

    #endregion

    #region Unity Methods

    private void Update()
    {
        if (GameManager.Instance.HasBattleStarted && GameManager.Instance.IsPlayerTurn)
        {
            if (isPointerDown)
            {
                pointerDownTimer += Time.deltaTime;

                if (pointerDownTimer >= GameManager.Instance.RequiredHoldTimeToExamine)
                {
                    OnLongPress();
                    ResetPointer();
                }
            }
        }
    }

    #endregion

    #region Custom Methods

    private void ResetPointer()
    {
        pointerDownTimer = 0;
        isPointerDown = false;
    }

    private void OnShortPress()
    {
        if (hero.IsHeroSelected)
        {
            hero.DeselectHero();
        }
        else
        {
            hero.SelectHero();
        }
    }

    private void OnLongPress()
    {
        onLongPress = true;

        hero.OpenHeroInfoPopup();
    }

    #endregion
}
