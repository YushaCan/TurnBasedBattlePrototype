using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsManager : MonoBehaviour
{
    #region Variables

    public static PlayerPrefsManager Instance;

    [SerializeField] private HeroSpecs[] heroSpecs;

    private static bool isFirstOpen = true;

    #region Player Prefs Key Strings

    private const string heroPlayerPrefsExisted = "HeroPlayerPrefsExisted";
    private const string totalBattlesPlayedKey = "TotalBattlesPlayed";
    
    private const string healthKey = "Health";
    private const string adKey = "AttackDamage";
    private const string xpKey = "Experience";
    private const string levelKey = "Level";
    private const string ownershipKey = "PlayerHave";
    
    private const string enemyHealthKey = "EnemyHealth";
    private const string enemyAdKey = "EnemyAD";
    private const string enemyPrefExisted = "EnemyPrefExisted";

    private const string tutorialDoneKey = "IsTutorialDone";

    #endregion

    #endregion

    #region Action Variables



    #endregion

    #region Unity Methods

    private void Start()
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

        if (isFirstOpen)
        {
            GetAllPlayerPrefs();

            isFirstOpen = false;
        }
    }

    #endregion 

    #region Custom Methods

    private void GetAllPlayerPrefs()
    {

        // FOR TOTAL BATTLES PLAYED
        if (PlayerPrefs.HasKey(totalBattlesPlayedKey))
        {
            EconomyManager.Instance.SetTotalBattlesPlayed(PlayerPrefs.GetInt(totalBattlesPlayedKey));

            //print("<color=#C5A1AF>Total Battles Played: " + PlayerPrefs.GetInt(totalBattlesPlayedKey) + "</color>");
        }
        else
        {
            // This means game is opened for the first time
            PlayerPrefs.SetInt(totalBattlesPlayedKey, 0);
            EconomyManager.Instance.SetTotalBattlesPlayed(0);

            //print("<color=#C5A1AF>Total Battles Played: Created, 0</color>");
        }


        // FOR HEROES
        if (PlayerPrefs.HasKey(heroPlayerPrefsExisted))
        {
            //print("<color=#C5A1AF>Hero Prefs Found, They're updating...</color>");

            UpdateAllHeroSpecsWithPlayerPrefs();
        }
        else
        {
            //print("<color=#C5A1AF>Hero Prefs NOT found, They're creating...</color>");

            UpdateAllHeroPlayerPrefs();

            PlayerPrefs.SetString(heroPlayerPrefsExisted, "true");
        }


        // FOR ENEMY
        if (!PlayerPrefs.HasKey(enemyPrefExisted))
        {
            UpdateEnemyPref(GameManager.Instance.EnemyStartingHealthValue, GameManager.Instance.EnemyStartingAdValue);
            PlayerPrefs.SetString(enemyPrefExisted, "true");
        }
    }

    public void UpdateTotalBattlePlayPref()
    {
        PlayerPrefs.SetInt(totalBattlesPlayedKey, EconomyManager.Instance.GetTotalBattlesPlayed());
    }

    public void SetHeroOwnershipToTrue(HeroSpecs heroToOwn)
    {
        PlayerPrefs.SetInt(heroToOwn.Name + ownershipKey, 1);
    }

    public void UpdateAllHeroPlayerPrefs()
    {
        for (int i = 0; i < heroSpecs.Length; i++)
        {
            PlayerPrefs.SetInt(heroSpecs[i].Name + healthKey, heroSpecs[i].Health);
            PlayerPrefs.SetInt(heroSpecs[i].Name + adKey, heroSpecs[i].AttackDammage);
            PlayerPrefs.SetInt(heroSpecs[i].Name + xpKey, heroSpecs[i].Experience);
            PlayerPrefs.SetInt(heroSpecs[i].Name + levelKey, heroSpecs[i].Level);
            // If player has this hero, than it'll be 1, otherwise it'll be 0. Like a bool value
            PlayerPrefs.SetInt(heroSpecs[i].Name + ownershipKey, heroSpecs[i].PlayerHave ? 1 : 0);


            //print("<color=#C5A1AF>" + heroSpecs[i].Name + healthKey + "=> " + heroSpecs[i].Health.ToString() + "</color>");
            //print("<color=#C5A1AF>" + heroSpecs[i].Name + adKey + "=> " + heroSpecs[i].AttackDammage.ToString() + "</color>");
            //print("<color=#C5A1AF>" + heroSpecs[i].Name + xpKey + "=> " + heroSpecs[i].Experience.ToString() + "</color>");
            //print("<color=#C5A1AF>" + heroSpecs[i].Name + levelKey + "=> " + heroSpecs[i].Level.ToString() + "</color>");
            //print("<color=#C5A1AF>" + heroSpecs[i].Name + ownershipKey + "=> " + PlayerPrefs.GetInt(heroSpecs[i].Name + ownershipKey) + "</color>");
        }
    }

    public void UpdateAllHeroSpecsWithPlayerPrefs()
    {
        for (int i = 0; i < heroSpecs.Length; i++)
        {
            heroSpecs[i].UpdateHeroData(
                 PlayerPrefs.GetInt(heroSpecs[i].Name + healthKey),
                 PlayerPrefs.GetInt(heroSpecs[i].Name + adKey),
                 PlayerPrefs.GetInt(heroSpecs[i].Name + xpKey),
                 PlayerPrefs.GetInt(heroSpecs[i].Name + levelKey),
                 PlayerPrefs.GetInt(heroSpecs[i].Name + ownershipKey)
                );

            //print("<color=#C5A1AF>UPDATED: " + heroSpecs[i].Name + healthKey + "=> " + heroSpecs[i].Health.ToString() + "</color>");
            //print("<color=#C5A1AF>UPDATED: " + heroSpecs[i].Name + adKey + "=> " + heroSpecs[i].AttackDammage.ToString() + "</color>");
            //print("<color=#C5A1AF>UPDATED: " + heroSpecs[i].Name + xpKey + "=> " + heroSpecs[i].Experience.ToString() + "</color>");
            //print("<color=#C5A1AF>UPDATED: " + heroSpecs[i].Name + levelKey + "=> " + heroSpecs[i].Level.ToString() + "</color>");
            //print("<color=#C5A1AF>UPDATED: " + heroSpecs[i].Name + ownershipKey + "=> " + PlayerPrefs.GetInt(heroSpecs[i].Name + ownershipKey) + "</color>");
        }


        //print("<color=#C5A1AF>Hero Data's UPDATED.</color>");
    }

    /// <summary>
    /// Use this function to get enemy's Health at battle start.
    /// </summary>
    public int GetEnemyHealthPref()
    {
        return PlayerPrefs.GetInt(enemyHealthKey);
    }

    /// <summary>
    /// Use this function to get enemy's Attack Damage at battle start.
    /// </summary>
    public int GetEnemyAttackDamagePref()
    {
        return PlayerPrefs.GetInt(enemyAdKey);
    }

    /// <summary>
    /// Use this function only when the enemy has been powered (Which means enemy has lost a battle)
    /// </summary>
    /// <param name="newHealth">New Health value</param>
    /// <param name="newAd">New Attack Damage Value</param>
    public void UpdateEnemyPref(int newHealth, int newAd)
    {
        PlayerPrefs.SetInt(enemyHealthKey, newHealth);
        PlayerPrefs.SetInt(enemyAdKey, newAd);
    }

    public bool IsTutorialDone()
    {
        if (PlayerPrefs.HasKey(tutorialDoneKey))
        {
            return true;
        }
        else
        {
            PlayerPrefs.SetString(tutorialDoneKey, "false");
            return false;
        }
    }

    #endregion 
}
