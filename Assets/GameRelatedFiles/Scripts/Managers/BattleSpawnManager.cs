using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleSpawnManager : MonoBehaviour
{
    #region Variables

    public static BattleSpawnManager Instance;

    [SerializeField] private GameObject enemyGO;
    [SerializeField] private Transform enemyTransform;
    [SerializeField] private GameObject heroGO;
    [SerializeField] private Transform[] heroTransforms;

    public bool EnemySpawnProcessFinished {  get; set; }

    #endregion

    #region Action Variables



    #endregion

    #region Unity Methods

    private void Awake()
    {
        Instance = this;
        EnemySpawnProcessFinished = false;
    }

    private void Start()
    {
        SpawnHeroes();
        SpawnEnemy();
    }

    #endregion

    #region Custom Methods

    private void SpawnHeroes()
    {
        List<HeroSpecs> selectedHeroes = new List<HeroSpecs>();
        selectedHeroes = GameManager.Instance.SelectedHerosForBattle;

        for (int i = 0; i < selectedHeroes.Count; i++)
        {
            GameObject newlySpawnedHero = Instantiate(heroGO, heroTransforms[i].transform);
            newlySpawnedHero.GetComponent<MeshRenderer>().material.color = selectedHeroes[i].HeroColor;
            Hero hero = newlySpawnedHero.GetComponent<Hero>();
            GameManager.Instance.AddHeroToList(hero);
            hero.SetHeroSpecs(selectedHeroes[i]);
            hero.SetEnemyAttackPositionForHero(BattleAnimationManager.Instance.heroesAttackPoints[i]);
        }
    }

    private void SpawnEnemy()
    {
        GameObject newlySpawnedEnemy = Instantiate(enemyGO, enemyTransform);
        newlySpawnedEnemy.GetComponent<Enemy>().SetEnemySpecs();

        //DOVirtual.DelayedCall(0.1f, delegate
        //{
        //    SpawnProcessFinished = true;
        //});
    }

    public Transform GetEnemySpawnPoint()
    {
        return enemyTransform;
    }

    #endregion
}
