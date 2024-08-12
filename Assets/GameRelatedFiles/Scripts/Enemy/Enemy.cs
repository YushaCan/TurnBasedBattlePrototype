using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    #region Variables

    private EnemyHealthbar enemyHealthBar;

    [SerializeField] private TextMeshProUGUI enemyHealthText;

    public static GameObject EnemyGameObject;

    public int Health {  get; private set; }
    public int EnemyDefaultHealth {  get; private set; }
    public static int AttackDamage { get; private set; } 

    #endregion

    #region Action Variables

    public static Action<int> OnDamageTaken;
    public static Action OnCloseHealthbar;
    public static Action OnOpenHealthbar;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        enemyHealthBar = GetComponent<EnemyHealthbar>();

        BattleSpawnManager.Instance.EnemySpawnProcessFinished = true;

        EnemyGameObject = this.gameObject;
    }

    private void Start()
    {
        
    }

    private void OnEnable()
    {
        OnDamageTaken += OnDamageTakenMethod;
        OnCloseHealthbar += CloseEnemyHealthbar;
        OnOpenHealthbar += OpenEnemyHealthbar;
    }

    private void OnDisable()
    {
        OnDamageTaken -= OnDamageTakenMethod;
        OnCloseHealthbar -= CloseEnemyHealthbar;
        OnOpenHealthbar -= OpenEnemyHealthbar;
    }

    #endregion

    #region Custom Methods

    public void SetEnemySpecs()
    {
        Health = PlayerPrefsManager.Instance.GetEnemyHealthPref();
        EnemyDefaultHealth = Health;

        AttackDamage = PlayerPrefsManager.Instance.GetEnemyAttackDamagePref();

        ArrangeEnemyCanvas();
    }

    public void ArrangeEnemyCanvas()
    {
        enemyHealthBar.ArrangeEnemyHealthBar();

        enemyHealthText.text = Health.ToString();
    }

    private void OnDamageTakenMethod(int damageTaken)
    {
        Health -= damageTaken;

        if (Health <= 0)
        {
            enemyHealthText.text = "0";

            // ENEMY KILLED!
            EnemyKilled();
        }
        else
        {
            enemyHealthText.text = Health.ToString();
        }

    }

    private void EnemyKilled()
    {
        //print("<color=red> ENEMY KILLED! </color>");

        // TRIGGER BATTLE WIN
        GameManager.Instance.IsEnemyDead = true;

        EnemyGainPower();
    }

    private void EnemyGainPower()
    {
        int newEnemyHealth = EnemyDefaultHealth + ((EnemyDefaultHealth * 6) / 100);
        int newEnemyAttackDamage = AttackDamage + ((AttackDamage * 6) / 100);

        PlayerPrefsManager.Instance.UpdateEnemyPref(newEnemyHealth, newEnemyAttackDamage);
    }

    public void CloseEnemyHealthbar()
    {
        enemyHealthBar.CloseHealthbar();
    }

    public void OpenEnemyHealthbar()
    {
        enemyHealthBar.OpenHealthbar();
    }

    #endregion
}
