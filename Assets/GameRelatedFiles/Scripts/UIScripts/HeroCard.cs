using TMPro;
using System;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

public class HeroCard : MonoBehaviour
{
    #region Variables

    private HeroSpecs HeroSpecs;

    [SerializeField] private Image lockedImage;
    [SerializeField] private TextMeshProUGUI heroNameText;
    [SerializeField] private Image heroColorImage;
    [SerializeField] private Image heroCardSelectionFrame;
    
    [HideInInspector] private Color32 deselectedColor;
    [SerializeField] private Color32 selectedColor_Min;
    [SerializeField] private Color32 selectedColor_Max;
    
    private bool playerHave;
    private bool isSelected;

    private Tween selectedAnimTween;


    // These are for Pop-ups
    [SerializeField] private GameObject infoAnchorGO;
    [SerializeField] private Image infoPopup;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private TextMeshProUGUI attackDamageText;
    [SerializeField] private TextMeshProUGUI experienceText;
    [SerializeField] private TextMeshProUGUI levelText;

    // Notification System Variables
    [SerializeField] private GameObject notificationPanel;
    [SerializeField] private TextMeshProUGUI gainExperienceText;
    [SerializeField] private TextMeshProUGUI levelUpText;
    [SerializeField] private TextMeshProUGUI newAdText;
    [SerializeField] private TextMeshProUGUI newHealthText;
    private const string gainExperienceString = "+1 Experience";
    private const string levelUpString = "LEVEL UP!";
    private const string newAdString = "New AD: ";
    private const string newHealthString = "New Health: ";

    #endregion

    #region Action Variables

    public static Action<HeroSpecs> OnHeroActivated;

    #endregion

    #region Unity Methods

    private void Start()
    {
        deselectedColor = heroCardSelectionFrame.color;
        isSelected = false;
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }

    #endregion

    #region Custom Methods

    public void InitiateHeroCard(HeroSpecs heroCardSpecs)
    {
        HeroSpecs = heroCardSpecs;

        ArrangeHeroCard();
    }

    private void ArrangeHeroCard()
    {
        ControlOwnership();

        heroNameText.text = HeroSpecs.Name;
        heroColorImage.color = HeroSpecs.HeroColor;

        StartCoroutine(ControlNotification());
    }

    private void ControlOwnership()
    {
        if (HeroSpecs.PlayerHave)
        {
            LockCard(false);
        }
        else
        {
            LockCard(true);
        }
    }

    private void LockCard(bool hasLocked)
    {
        lockedImage.gameObject.SetActive(hasLocked);
    }

    public void SelectHeroCard()
    {
        GameManager.Instance.AddHeroToSelectedList(HeroSpecs);
       
        isSelected = true;

        PlaySelectedAnimation();
    }

    public void DeselectHeroCard()
    {
        GameManager.Instance.RemoveHeroFromSelectedList(HeroSpecs);

        heroCardSelectionFrame.color = deselectedColor;
        isSelected = false;
        selectedAnimTween.Kill();
    }

    private void ClearHeroCard()
    {
        isSelected = false;
    }

    public bool GetHeroOwnership()
    {
        return HeroSpecs.PlayerHave;
    }

    public HeroSpecs GetHeroSpecs()
    {
        return HeroSpecs;
    }

    private void PlaySelectedAnimation()
    {
        if (selectedAnimTween.IsActive())
        {
            selectedAnimTween.Kill();
        }

        float animDuration = 0.5f;

        selectedAnimTween = heroCardSelectionFrame.DOColor(selectedColor_Max, animDuration).OnComplete(delegate 
        {
            selectedAnimTween = heroCardSelectionFrame.DOColor(selectedColor_Min, animDuration).OnComplete(delegate
            {
                if (isSelected)
                {
                    PlaySelectedAnimation();
                }
            });
        });
    }

    public void TriggerUnlock()
    {
        DOVirtual.DelayedCall(1.5f, OnHeroOpened);
    }

    private void OnHeroOpened()
    {
        float animDuration = 0.5f;
        float fadeOut = 0;

        lockedImage.DOFade(fadeOut, animDuration).OnComplete(delegate
        {
            // OPEN, HERO ACTIVATED PANEL HERE.
            OnHeroActivated?.Invoke(HeroSpecs);
        });
    }

    #region Popup Info Methods

    public void OpenInfoPopup()
    {
        float fullScale = 1f;
        float animDuration = 0.5f;

        // This is the close version of this popup
        infoAnchorGO.transform.localScale = new Vector3(1, 0, 1);

        infoAnchorGO.SetActive(true);

        healthText.text = "Health: " + HeroSpecs.Health.ToString(); 
        attackDamageText.text = "Attack Damage: " + HeroSpecs.AttackDammage.ToString(); 
        experienceText.text = "Experience: " + HeroSpecs.Experience.ToString(); 
        levelText.text = "Level: " + HeroSpecs.Level.ToString(); 


        infoAnchorGO.transform.DOScaleY(fullScale, animDuration);
    }

    public void CloseInfoPopup()
    {
        float targetScale = 0f;
        float animDuration = 0.5f;

        infoAnchorGO.transform.DOScaleY(targetScale, animDuration).OnComplete(delegate
        {
            infoAnchorGO.SetActive(false);
        });

    }

    #endregion

    #region Notification Info Popup

    private IEnumerator ControlNotification()
    {
        float waitUntilCardsShowUp = 1.5f;
        yield return new WaitUntil(() => SceneManagement.Instance.HasSceneLoaded);

        DOVirtual.DelayedCall(waitUntilCardsShowUp, delegate
        {
            HeroNotificationControl heroNotification = HeroSpecs.CheckExperienceAndLevel();

            //Debug.Log("<color=orange>HERO NOTIFICATION: " + heroNotification.ToString() + "</color>");

            switch (heroNotification)
            {
                case HeroNotificationControl.LevelUp:

                    notificationPanel.SetActive(true);
                    LevelUpNotification();

                    break;

                case HeroNotificationControl.GainExperience:

                    notificationPanel.SetActive(true);
                    GainExperienceNotification();

                    break;

                case HeroNotificationControl.Nothing:

                    // It means nothing new, battle was lost.
                    notificationPanel.SetActive(false);

                    break;

                default:

                    break;
            }
        });
    }

    private void LevelUpNotification()
    {
        Debug.Log("<color=cyan>NOTIFICATION OPENED!</color>");

        float totalAnimTime;

        totalAnimTime = NotificationTextAnim(levelUpText, levelUpString);

        DOVirtual.DelayedCall(totalAnimTime, delegate
        {
            string newAdStringWithValue = newAdString + HeroSpecs.AttackDammage;
            NotificationTextAnim(newAdText, newAdStringWithValue);

            DOVirtual.DelayedCall(totalAnimTime, delegate
            {
                string newHealthStringWithValue = newHealthString + HeroSpecs.Health;
                NotificationTextAnim(newHealthText, newHealthStringWithValue);
            });
        });
    }

    private void GainExperienceNotification()
    {
        //Debug.Log("<color=cyan>NOTIFICATION OPENED!</color>");

        NotificationTextAnim(gainExperienceText, gainExperienceString);
    }

    private float NotificationTextAnim(TextMeshProUGUI targetText, string targetString)
    {
        targetText.text = targetString;

        float midPos = 75;
        float lastPos = 125;

        float animDuration = 0.5f;
        float waitDuration = 0.75f;
        float fullAlpha = 1;
        float minAlpha = 0;

        targetText.alpha = 0;

        targetText.gameObject.SetActive(true);

        targetText.DOFade(fullAlpha, animDuration).OnComplete(delegate
        {
            DOVirtual.DelayedCall(waitDuration, delegate
            {
                targetText.DOFade(minAlpha, animDuration).OnComplete(delegate
                {
                    targetText.gameObject.SetActive(false);
                });
            });
        });

        targetText.rectTransform.anchoredPosition = Vector2.zero;

        targetText.rectTransform.DOAnchorPosY(midPos, animDuration).OnComplete(delegate
        {
            DOVirtual.DelayedCall(waitDuration, delegate
            {
                targetText.rectTransform.DOAnchorPosY(lastPos, animDuration);
            });
        });

        float totalAnimTime = animDuration * 3;
        return totalAnimTime;
    }

    #endregion

    #endregion
}
