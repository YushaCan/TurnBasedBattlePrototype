using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    #region Variables

    public static GameManager Instance;

    [HideInInspector]
    public List<HeroSpecs> SelectedHerosForBattle;
    [HideInInspector]
    public List<Hero> Heroes;

    // This is for the enemy for the first open of this game ever.
    public int EnemyStartingHealthValue = 300;
    public int EnemyStartingAdValue = 45;
    //////////////////////////////////////////////////////////////
    public bool HasBattleStarted {  get; private set; }
    public bool IsPlayerTurn {  get; set; }
    public bool IsEnemyDead { get; set; }
    public bool AreHeroesDead { get; set; } 

    // This variable indicates the duration for a long press
    public float RequiredHoldTimeToExamine {  get; private set; }

    private int requiredAmountOfHero = 3;

    #endregion

    #region Action Variables

    public Action<bool> OnBattleButtonActivated;
    public Action<bool> OnTooManyHeroesSelected;
    public Action OnBattleStart;
    public Action OnEnemyTargeted;
    public Action OnBattleFinished;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;

            // 3 Seconds was tooo long, so I decided to make it 2 seconds. If you want this to be in 3, you may change the value.
            RequiredHoldTimeToExamine = 2f;

            // This line disables the multitouch feature. For this prototype, I thought it would be good to disable it. 
            Input.multiTouchEnabled = false;

            // This line is for this script to be useful between scenes
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private IEnumerator Start()
    {
        HasBattleStarted = false;
        IsPlayerTurn = true;
        IsEnemyDead = false;
        AreHeroesDead = false;

        yield return new WaitUntil(() => SceneManagement.Instance.HasSceneLoaded);

    }

    private void OnEnable()
    {
        OnBattleFinished += ResetGameManager;
    }

    private void OnDisable()
    {
        OnBattleFinished -= ResetGameManager;
    }

    #endregion

    #region Custom Methods

    public void ResetGameManager()
    {
        if (IsGameWin())
        {
            for (int i = 0; i < Heroes.Count; i++)
            {
                EconomyManager.Instance.IncreaseExperienceOfHero(Heroes[i].HeroSpecs);
            }
        }


        HasBattleStarted = false;
        IsPlayerTurn = true;
        IsEnemyDead = false;
        AreHeroesDead = false;

        DOVirtual.DelayedCall(0.2f, delegate
        {
            Heroes.Clear();
            CleanSelectedHeroList();
        });
    }

    /// <summary>
    /// This function is for the heros that selected for the battle.
    /// </summary>
    public void AddHeroToSelectedList(HeroSpecs HeroToAdd)
    {
        SelectedHerosForBattle.Add(HeroToAdd);

        //print("<color=orange> " + HeroToAdd.Name + " is selected for the battle</color>");

        CheckBattleButtonActivation();
    }

    /// <summary>
    /// This function is for the heros that removed from the selected list
    /// </summary>
    public void RemoveHeroFromSelectedList(HeroSpecs HeroToRemove)
    {
        SelectedHerosForBattle.Remove(HeroToRemove);

        //print("<color=red> " + HeroToRemove.Name + " is removed from the list</color>");

        CheckBattleButtonActivation();
    }

    public void CleanSelectedHeroList()
    {
        SelectedHerosForBattle.Clear();
    }

    private void CheckBattleButtonActivation()
    {
        if (SelectedHerosForBattle.Count == requiredAmountOfHero)
        {
            // Activate Button
            OnBattleButtonActivated?.Invoke(true);
            OnTooManyHeroesSelected?.Invoke(false);
        }
        else if (SelectedHerosForBattle.Count > requiredAmountOfHero)
        {
            OnBattleButtonActivated?.Invoke(false);

            // Open Too Many heroes selected UI here
            OnTooManyHeroesSelected?.Invoke(true);
        }
        else if (SelectedHerosForBattle.Count < requiredAmountOfHero)
        {
            OnBattleButtonActivated?.Invoke(false);
            OnTooManyHeroesSelected?.Invoke(false);
        }
    }

    public void SetBattleStarted(bool hasStarted)
    {
        HasBattleStarted = hasStarted;

        if (hasStarted)
        {
            // This is first start so it must change the state
            OnBattleStart?.Invoke();
        }
    }

    public List<Hero> GetAllHeroes()
    {
        return Heroes;
    }

    public Hero GetSelectedHero()
    {
        Hero selectedHero = new Hero();

        for (int i = 0; i < Heroes.Count; i++)
        {
            if (Heroes[i].IsHeroSelected)
            {
                selectedHero = Heroes[i];

                //print("<color=orange>HERO SELECTED: " + selectedHero.HeroSpecs.Name + "</color>");
            }
        }

        if (selectedHero.HeroSpecs.Name == null)
        {
            print("<color=red>ERROR GETTING A HERO!</color>");
        }

        // For return to a value.
        return selectedHero;
    }

    public bool IsAnyHeroSelected()
    {
        for (int i = 0; i < Heroes.Count; i++)
        {
            if (Heroes[i].IsHeroSelected)
            {
                //print("<color=yellow>Selected hero: " + Heroes[i].name + "</color>");
                return true;
            }
        }

        print("<color=red>NO SELECTED HEROES </color>");

        return false; 
    }

    public void SetAllHeroesDeselected()
    {
        for (int i = 0; i < Heroes.Count; i++)
        {
            Heroes[i].DeselectHero();
        }
    }

    public void AddHeroToList(Hero hero)
    {
        Heroes.Add(hero);
    }

    public void RemoveHeroFromList(Hero heroToRemove)
    {
        Heroes.Remove(heroToRemove);

        ControlHeroes();
    }

    private void ControlHeroes()
    {
        if (Heroes.Count <= 0)
        {
            AreHeroesDead = true;
        }
    }

    public bool ControlBattleFinished()
    {
        if (IsEnemyDead || AreHeroesDead)
        {
            //print("<color=cyan>BATTLE FINISHED!</color>");
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsGameWin()
    {
        if (IsEnemyDead && !AreHeroesDead)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    #endregion
}
