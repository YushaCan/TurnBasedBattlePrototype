using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

[CreateAssetMenu(fileName = "HeroSpecs", menuName = "Hero Specialties")]
public class HeroSpecs : ScriptableObject
{
    #region Variables

    // Hero Specialties Variables

    [SerializeField] private string name;
    [SerializeField] private int health;
    [SerializeField] private int attackDamage;
    [SerializeField] private int experience;
    [SerializeField] private int level;
    
    [SerializeField] private Color32 heroColor;
    
    [SerializeField] private bool playerHave;
    
    private bool isLevelUp;
    private bool isGainExperience;

    #endregion

    #region Properties

    // Hero Specialties Properties 
    public string Name => name;
    public int Health => health;
    public int AttackDammage => attackDamage;
    public int Experience => experience;
    public int Level => level;
    public Color32 HeroColor => heroColor;
    public bool PlayerHave => playerHave;

    #endregion

    #region Custom Methods

    private void LevelUp()
    {
        isGainExperience = false;
        isLevelUp = true;

        Debug.Log("<color=yellow>Hero Level Up!: " + Name + "</color>");

        level += 1;
        experience = 0;

        // Update health for +%10 at level up
        float updatedHealthAmount = ((float)health / 100) * 10;
        health += (int)updatedHealthAmount;

        // Update attack damage for +%10 at level up
        float updatedAdAmount = ((float)attackDamage / 100) * 10;
        attackDamage += (int)updatedAdAmount;


        //Debug.Log("<color=yellow> NEW Hero Default Health: " + Health + "</color>");
        //Debug.Log("<color=yellow> NEW Hero Attack Damage: " + AttackDammage + "</color>");
        //Debug.Log("<color=yellow> NEW Level: " + Level + "</color>");
    }

    public void GainExperience()
    {
        isGainExperience = true;
        isLevelUp = false;

        Debug.Log("<color=cyan>Hero Gained Experience!: " + Name + "</color>");

        experience += 1;

        // If the experience points greater than the level threshold (=5), hero will level up
        if (experience >= 5)
        {
            LevelUp();
        }
    }

    public HeroNotificationControl CheckExperienceAndLevel()
    {
        if (!isGainExperience && isLevelUp)
        {
            // For reset the values
            isGainExperience = false;
            isLevelUp = false;

            return HeroNotificationControl.LevelUp;
        }
        else if (isGainExperience && !isLevelUp)
        {
            // For reset the values
            isGainExperience = false;
            isLevelUp = false;

            return HeroNotificationControl.GainExperience;
        }
        else
        {
            // For reset the values
            isGainExperience = false;
            isLevelUp = false;

            return HeroNotificationControl.Nothing;
        }
    }

    public void SetThisHeroAsOwned()
    {
        playerHave = true;
    }

    public void UpdateHeroData(int updatedHealth, int updatedAd, int updatedXp, int updatedLevel, int updatedPlayerHave)
    {
        health = updatedHealth;
        attackDamage = updatedAd;
        experience = updatedXp;
        level = updatedLevel;

        if (updatedPlayerHave == 1)
        {
            playerHave = true;
        }
        else
        {
            playerHave = false;
        }
        
    }

    #endregion
}

public enum HeroNotificationControl
{
    LevelUp = 0,
    GainExperience = 1,
    Nothing = 2,
}
