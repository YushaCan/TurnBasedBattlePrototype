using TMPro;
using DG.Tweening;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleAnimationManager : MonoBehaviour
{
    #region Variables

    public static BattleAnimationManager Instance;

    [SerializeField] private GameObject damageCanvas;

    public Transform enemyAttackPoint;
    public Transform[] heroesAttackPoints;

    // CAMERA SHAKE VARIABLES
    [SerializeField] private float shake;
    [SerializeField] private int vibrato;
    [SerializeField] private float randomness;

    #endregion

    #region Action Variables



    #endregion

    #region Unity Methods

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    #region Custom Methods

    public void CameraShake()
    {
        // FILL THIS UP
        float animDuration = 0.25f;

        Camera.main.DOShakeRotation(animDuration, shake, vibrato, randomness);
        
        //print("<color=orange>Camera Shake</color>");
    }

    /// <summary>
    /// This function is called when damage has been dealt
    /// </summary>
    /// <param name="damageGiven">The value damage has given</param>
    /// <param name="toEnemy">Is damage given to enemy or heroes?</param>
    /// <param name="heroIndex">If to heroes, hero in which index? (IF DAMAGE HAS BEEN DEALT TO ENEMY, PLEASE ENTER 0 TO THIS PARAMETER)</param>
    public void SpawnDamageCanvas(int damageGiven, bool toEnemy, Hero heroToDamage)
    {
        Vector3 spawnPos = new Vector3();
        if (toEnemy)
        {
            spawnPos = new Vector3(enemyAttackPoint.position.x + 0.2f, enemyAttackPoint.position.y + .25f, enemyAttackPoint.position.z);
        }
        else
        {
            spawnPos = new Vector3(heroToDamage.GetEnemyAttackPosition().position.x - 0.5f,
                heroToDamage.GetEnemyAttackPosition().position.y + .25f,
                heroToDamage.GetEnemyAttackPosition().position.z);
        }

        GameObject newlySpawnedDamageCanvas = Instantiate(damageCanvas, spawnPos, damageCanvas.transform.rotation);
        newlySpawnedDamageCanvas.GetComponent<DamageCanvas>().StartAnimation(damageGiven);
    }

    #endregion
}
