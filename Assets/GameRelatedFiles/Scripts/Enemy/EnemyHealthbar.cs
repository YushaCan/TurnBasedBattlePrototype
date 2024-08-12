using TMPro;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EnemyHealthbar : MonoBehaviour
{
    #region Variables

    private Enemy enemy;

    [SerializeField] private GameObject canvasAnchor;
    [SerializeField] private Slider enemyHealthbarSlider;

    #endregion

    #region Action Variables



    #endregion

    #region Unity Methods

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => BattleSpawnManager.Instance.EnemySpawnProcessFinished);

      
        //ArrangeEnemyHealthBar();

        canvasAnchor.transform.LookAt(Camera.main.transform);
    }

    private void OnEnable()
    {
        DOVirtual.DelayedCall(0.2f, delegate
        {
            Enemy.OnDamageTaken += UpdateHealthBar;
        });
    }

    private void OnDisable()
    {
        Enemy.OnDamageTaken -= UpdateHealthBar;
    }

    #endregion

    #region Custom Methods

    private void GetEnemy()
    {
        enemy = gameObject.GetComponent<Enemy>();
    }

    public void ArrangeEnemyHealthBar()
    {
        GetEnemy();

        enemyHealthbarSlider.maxValue = enemy.Health;
        enemyHealthbarSlider.value = enemy.Health;
        enemyHealthbarSlider.minValue = 0;
    }

    private void UpdateHealthBar(int damageTaken)
    {
        float animDuration = 0.5f;
        float targetHealthbarValue = enemyHealthbarSlider.value - damageTaken;

        enemyHealthbarSlider.DOValue(targetHealthbarValue, animDuration);
    }

    public void CloseHealthbar()
    {
        canvasAnchor.gameObject.SetActive(false);
    }

    public void OpenHealthbar()
    {
        canvasAnchor.gameObject.SetActive(true);
    }

    #endregion
}
