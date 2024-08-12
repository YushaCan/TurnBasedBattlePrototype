using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    #region Variables

    public static EconomyManager Instance;

    [SerializeField] private HeroSpecs[] AllHeroes;

    private int TotalBattlesPlayed { get; set; }
    private int HeroAmountPlayerHas { get; set; }

    #endregion

    #region Action Variables



    #endregion

    #region Unity Methods

    private void Awake()
    {

        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        GameManager.Instance.OnBattleFinished += IncreaseTotalBattlePlayed;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnBattleFinished -= IncreaseTotalBattlePlayed;
    }

    #endregion

    #region Custom Methods

    public int GetHeroAmountPlayerHas()
    {
        HeroAmountPlayerHas = 0;

        for (int i = 0; i < AllHeroes.Length; i++)
        {
            if (AllHeroes[i].PlayerHave)
            {
                HeroAmountPlayerHas++;
            }
        }

        return HeroAmountPlayerHas;
    }

    public void IncreaseTotalBattlePlayed()
    {
        TotalBattlesPlayed += 1;
        //Debug.Log("<color=#90EE90>Total Games Played: " + TotalBattlesPlayed + "</color>");

        PlayerPrefsManager.Instance.UpdateTotalBattlePlayPref();
    }

    public bool ControlHeroUnlock()
    {
        if (TotalBattlesPlayed % 5 == 0 && TotalBattlesPlayed != 0)
        {
            return true;
        }
        else
        {
            return false;   
        }
    }

    public int GetTotalBattlesPlayed()
    {
        return TotalBattlesPlayed;
    }

    public void SetTotalBattlesPlayed(int value)
    {
        TotalBattlesPlayed = value;
    }

    public void IncreaseExperienceOfHero(HeroSpecs hero)
    {
        hero.GainExperience();
    }

    #endregion
}
