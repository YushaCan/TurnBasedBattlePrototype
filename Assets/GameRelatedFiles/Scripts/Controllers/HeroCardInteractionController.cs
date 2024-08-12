using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class HeroCardInteractionController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    #region Variables

    [SerializeField] private HeroCard heroCard;

    private bool isPointerDown = false;
    private float pointerDownTimer = 0f;

    private bool onLongPress;

    #endregion

    #region Action Variables



    #endregion

    #region Pointer Interface Implementations

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
        pointerDownTimer = 0;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (onLongPress)
        {
            heroCard.CloseInfoPopup();
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

    public void OnPointerExit(PointerEventData eventData)
    {
        ResetPointer();
    }

    #endregion

    #region Unity Methods

    private void Update()
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

    #endregion

    #region Custom Methods

    private void ResetPointer()
    {
        pointerDownTimer = 0;
        isPointerDown = false;
    }

    private void OnShortPress()
    {
        bool isPlayerHave = heroCard.GetHeroOwnership();
        if (isPlayerHave && !HasSelectedForBattleBefore())
        {
            heroCard.SelectHeroCard();
        }
        else if (isPlayerHave && HasSelectedForBattleBefore())
        {
            heroCard.DeselectHeroCard();
        }
        else
        {
            Debug.LogWarning("This hero is not owned!");
        }
        
    }

    private void OnLongPress()
    {
        onLongPress = true;

        Debug.Log("Long Press Detected - Showing Card Info");
        // Add your logic for showing card info here

        heroCard.OpenInfoPopup();
    }

    private bool HasSelectedForBattleBefore()
    {
        bool hasSelectedBefore = GameManager.Instance.SelectedHerosForBattle.Contains(heroCard.GetHeroSpecs());


        //print("HAS SELECTED BEFORE: " + hasSelectedBefore);

        return hasSelectedBefore;
    }

    #endregion



}
