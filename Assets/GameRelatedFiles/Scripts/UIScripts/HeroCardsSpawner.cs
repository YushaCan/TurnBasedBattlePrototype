using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroCardsSpawner : MonoBehaviour
{
    #region Variables

    [SerializeField] private HeroSpecs[] HeroSpecs;
    [SerializeField] private GameObject heroCardPrefab;
    [SerializeField] private Transform heroCardParent;

    private const int totalHeroCount = 10;

    private HeroCard currentHeroCard;

    private int heroIndexToUnlock;

    private static bool isFirstOpen = true;

    #endregion

    #region Unity Methods

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => SceneManagement.Instance.HasSceneLoaded);

        bool newHeroToUnlock = EconomyManager.Instance.ControlHeroUnlock();

        if (newHeroToUnlock)
        {
            heroIndexToUnlock = EconomyManager.Instance.GetHeroAmountPlayerHas();
        }

        if (!isFirstOpen)
        {
            // Get All Player Prefs
            PlayerPrefsManager.Instance.UpdateAllHeroPlayerPrefs();
        }
        
        for (int i = 0; i < totalHeroCount; i++)
        {
            SpawnHeroCard(i);
        }

        isFirstOpen = false;
    }

    #endregion

    #region Custom Methods

    /// <summary>
    /// This function is used for spawn a hero card for Hero Selection Page. 
    /// </summary>
    /// <param name="index">The current index to getting the right HeroSpecs scriptable object by the value</param>
    private void SpawnHeroCard(int index)
    {
        GameObject newlyCreatedGO = Instantiate(heroCardPrefab, heroCardParent);
        currentHeroCard = newlyCreatedGO.GetComponent<HeroCard>();
        currentHeroCard.InitiateHeroCard(HeroSpecs[index]);

        if (index == heroIndexToUnlock && !HeroSpecs[index].PlayerHave && !isFirstOpen)
        {
            currentHeroCard.TriggerUnlock();
        }
    }

    #endregion
}
