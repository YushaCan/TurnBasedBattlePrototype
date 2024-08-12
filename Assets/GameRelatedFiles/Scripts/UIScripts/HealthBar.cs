using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class HealthBar : MonoBehaviour
{
    #region Variables

    [SerializeField] private Hero hero;
    [SerializeField] private GameObject healthBarCanvasGO;
    [SerializeField] private Slider healthBarSlider;

    #endregion

    #region Action Variables



    #endregion

    #region Unity Methods

    private void Start()
    {
        healthBarCanvasGO.transform.LookAt(Camera.main.transform);
    }

    private void OnEnable()
    {
        hero.OnDamageTaken += UpdateHealthbar;
    }

    private void OnDisable()
    {
        hero.OnDamageTaken -= UpdateHealthbar;
    }

    #endregion

    #region Custom Methods

    public void ArrangeHealthBarAtStart()
    {
        healthBarSlider.maxValue = hero.HeroSpecs.Health;
        healthBarSlider.value = hero.HeroSpecs.Health;
        healthBarSlider.minValue = 0;
    }

    public void UpdateHealthbar(Hero targetHero, int damageTaken)
    {
        if (hero.HeroSpecs.Name == targetHero.HeroSpecs.Name)
        {
            float animDuration = 0.5f;
            float targetHealthbarValue = healthBarSlider.value - damageTaken;

            healthBarSlider.DOValue(targetHealthbarValue, animDuration);
        }
    }

    public void CloseHealthbar()
    {
        healthBarCanvasGO.gameObject.SetActive(false);
    }

    public void OpenHealthbar()
    {
        healthBarCanvasGO.gameObject.SetActive(true);
    }

    #endregion
}
