using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HeroSelectionCanvas : MonoBehaviour
{
    #region Variables

    [SerializeField] private TextMeshProUGUI selectText;
    [SerializeField] private Button battleButton;
    [SerializeField] private CanvasGroup heroCardsCanvasGroup;

    // New Hero Panel
    [SerializeField] private GameObject newHeroPanelGO;
    [SerializeField] private Button newHeroPanelButton;
    [SerializeField] private Image newHeroPanel_Mini;
    [SerializeField] private TextMeshProUGUI heroNameText;
    [SerializeField] private Image heroColorImage;
    [SerializeField] private TextMeshProUGUI tapToContinueText;
    
    
    [SerializeField] private TextMeshProUGUI tooManyHeroesSelectedText;

    #endregion

    #region Action Variables



    #endregion

    #region Unity Methods

    private IEnumerator Start()
    {
        yield return new WaitUntil( () => SceneManagement.Instance.HasSceneLoaded);

        AnimateSelectionText(true);

        DOVirtual.DelayedCall(0.7f, delegate
        {
            AnimateCanvasGroup(true);
            //AnimateButton(true);
        });
    }

    private void OnEnable()
    {
        GameManager.Instance.OnBattleButtonActivated += AnimateButton;
        
        GameManager.Instance.OnTooManyHeroesSelected += IsTooManyHeroesSelected;

        HeroCard.OnHeroActivated += ActivateHeroOpenedPanel;

        battleButton.onClick.AddListener(BattleButtonFunction);
    }

    private void OnDisable()
    {
        GameManager.Instance.OnBattleButtonActivated -= AnimateButton;

        GameManager.Instance.OnTooManyHeroesSelected -= IsTooManyHeroesSelected;

        HeroCard.OnHeroActivated -= ActivateHeroOpenedPanel;

        battleButton.onClick.RemoveAllListeners();
    }

    #endregion

    #region Custom Methods

    private void IsTooManyHeroesSelected(bool isTrue)
    {
        tooManyHeroesSelectedText.gameObject.SetActive(isTrue);
    }

    private void ActivateHeroOpenedPanel(HeroSpecs heroToUnlock)
    {
        float animDuration = 0.5f;

        // MAKE ALL UI NECESSARITIES HERE
        newHeroPanel_Mini.transform.localScale = Vector2.zero;
        heroNameText.text = heroToUnlock.Name;
        heroColorImage.color = heroToUnlock.HeroColor;

        newHeroPanelGO.gameObject.SetActive(true);

        newHeroPanel_Mini.transform.DOScale(Vector3.one, animDuration).OnComplete(delegate
        {
            // ALSO, SET THIS HERO AS PLAYER HAVE AT THE END OF EVERYTHING
            heroToUnlock.SetThisHeroAsOwned();
            PlayerPrefsManager.Instance.SetHeroOwnershipToTrue(heroToUnlock);

            newHeroPanelButton.onClick.AddListener(NewHeroPanelClose);
           
            tapToContinueText.transform.DOScale(Vector3.one, animDuration / 2);
        });
    }

    private void DeactivateHeroOpenedPanel()
    {
        float animDuration = 0.25f;

        newHeroPanel_Mini.transform.DOScale(Vector3.zero, animDuration).OnComplete(delegate
        {
            newHeroPanelGO.gameObject.SetActive(false);
        });

        tapToContinueText.transform.DOScale(Vector3.zero, animDuration / 2);
    }

    private void NewHeroPanelClose()
    {
        newHeroPanelButton.onClick.RemoveAllListeners();

        DeactivateHeroOpenedPanel();
    }

    private void AnimateSelectionText(bool isOnStart)
    {
        float animDuration = 0.5f;

        if (isOnStart)
        {
            float targetScale = 1;
            selectText.transform.DOScale(targetScale, animDuration);
        }
        else
        {
            float targetScale = 0;
            selectText.transform.DOScale(targetScale, animDuration);
        }
        
    }

    private void AnimateButton(bool isOnStart)
    {
        float animDuration = 0.5f;

        if (isOnStart)
        {
            battleButton.enabled = true;

            float targetScale = 1;
            battleButton.transform.DOScale(targetScale, animDuration);
        }
        else
        {
            battleButton.enabled = false;

            float targetScale = 0;
            battleButton.transform.DOScale(targetScale, animDuration);
        }

    }

    private void AnimateCanvasGroup(bool isOnStart)
    {
        float animDuration = 0.5f;

        if (isOnStart)
        {
            float targetFade = 1;
            heroCardsCanvasGroup.DOFade(targetFade, animDuration);
        }
        else
        {
            float targetFade = 0;
            heroCardsCanvasGroup.DOFade(targetFade, animDuration);
        }

    }

    private void BattleButtonFunction()
    {
        SceneManagement.Instance.ChangeSceneByIndex(SceneManagement.Instance.BattleScene);
    }

    #endregion
}
