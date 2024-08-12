using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Hero : MonoBehaviour
{
    #region Variables

    public bool IsHeroSelected { get; private set; } = false;

    [HideInInspector]
    public HeroSpecs HeroSpecs;

    [SerializeField] private GameObject selectedFrame;
    private Tween selectedFrameRotationTween;

    [SerializeField] private HealthBar healthBar;
    [SerializeField] private TextMeshProUGUI heroNameText;
    [SerializeField] private TextMeshProUGUI heroCurrentHealthText;

    [SerializeField] private GameObject infoAnchorGO;
    [SerializeField] private TextMeshProUGUI attackDamageText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private TextMeshProUGUI experienceText;
    private Tween popupTween;

    public int CurrentHealth { get; private set; }
    
    private Transform EnemyAttackPosition;

    #endregion

    #region Action Variables

    public Action<Hero, int> OnDamageTaken;

    #endregion

    #region Unity Methods

    private void OnEnable()
    {
        OnDamageTaken += OnDamageTakenMethod;
    }

    private void OnDisable()
    {
        OnDamageTaken -= OnDamageTakenMethod;
    }

    #endregion

    #region Custom Methods

    public void SetEnemyAttackPositionForHero(Transform targetPosition)
    {
        EnemyAttackPosition = targetPosition;
    }

    public Transform GetEnemyAttackPosition()
    {
        return EnemyAttackPosition;
    }

    public void SetHeroSpecs(HeroSpecs heroSpecsToAssign)
    {
        HeroSpecs = heroSpecsToAssign;

        CurrentHealth = HeroSpecs.Health;   

        ArrangeHeroCanvas();
    }

    public void ArrangeHeroCanvas()
    {
        healthBar.ArrangeHealthBarAtStart();

        heroNameText.text = HeroSpecs.Name;
        heroCurrentHealthText.text = CurrentHealth.ToString();
    }

    private void OnDamageTakenMethod(Hero targetHero, int damageTaken)
    {
        CurrentHealth -= damageTaken;

        if (CurrentHealth <= 0)
        {
            heroCurrentHealthText.text = "0";

            // HERO KILLED!
            HeroKilled();
        }
        else
        {
            heroCurrentHealthText.text = CurrentHealth.ToString();
        }
        
    }

    private void HeroKilled()
    {
        HeroTouchController heroTouchController = gameObject.GetComponent<HeroTouchController>();
        Destroy(heroTouchController);

        float cleanDuration = 1.25f;
        DOVirtual.DelayedCall(cleanDuration, delegate
        {
            GameManager.Instance.RemoveHeroFromList(this);
            Destroy(gameObject);
        });
    }

    public void OpenHeroInfoPopup()
    {
        popupTween.Kill();

        float animDuration = 0.5f;
        float fullScale = 1f;

        attackDamageText.text = "Attack Damage: " + HeroSpecs.AttackDammage.ToString();
        levelText.text = "Level: " + HeroSpecs.Level.ToString();
        experienceText.text = "Experience: " + HeroSpecs.Experience.ToString();

        popupTween = infoAnchorGO.transform.DOScaleX(fullScale, animDuration);
    }

    public void CloseHeroInfoPopup()
    {
        popupTween.Kill();

        float animDuration = 0.25f;
        float targetScale = 0f;

        popupTween = infoAnchorGO.transform.DOScaleX(targetScale, animDuration);
    }

    public void SelectHero()
    {
        GameManager.Instance.SetAllHeroesDeselected();

        IsHeroSelected = true;

        //print("<color=white>SELECTED HERO: " + HeroSpecs.name + " </color>");

        selectedFrame.gameObject.SetActive(true);

        float oneTourDuration = 8f;

        selectedFrame.transform.DORotate(new Vector3(0, 360, 0), oneTourDuration, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental);


        // FOR TUTORIAL
        bool isTutorialDone = PlayerPrefsManager.Instance.IsTutorialDone();
        if (!isTutorialDone)
        {
            BattleSceneCanvas.Instance.OpenTutorialText();
        }
    }

    public void DeselectHero()
    {
        IsHeroSelected = false;

        selectedFrameRotationTween.Kill();

        selectedFrame.gameObject.SetActive(false);

        //print("<color=red>HEROES DESELECTED </color>");
    }

    public void CloseHealthbarForAttack()
    {
        healthBar.CloseHealthbar();
    }

    public void OpenHealthbarAfterAttack()
    {
        healthBar.OpenHealthbar();
    }

    #endregion
}
