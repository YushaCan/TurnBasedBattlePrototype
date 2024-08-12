using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    #region Variables

    public static SceneManagement Instance { get; private set; }
    [HideInInspector]
    public bool HasSceneLoaded;

    public int HeroSelectionScene { get; private set; } = 0;
    public int BattleScene { get; private set; } = 1;

    // For UI Animation purposes.
    [SerializeField] private Image sceneChangingImage;

    #endregion

    #region Action Variables



    #endregion

    #region Unity Methods

    private void Awake()
    {
        // This line is just for first opening.
        OpenedSceneAnimation();

        if (Instance == null)
        {
            Instance = this;

            // This line is for this script to be useful between scenes
            DontDestroyOnLoad(gameObject);
            
            // This line is for getting the callback when new scene has been loaded.
            SceneManager.sceneLoaded += OnNewSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnNewSceneLoaded;
    }

    // TOTALLY TEMPORARY
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.D))
    //    {
    //        ChangeSceneByIndex(BattleScene);
    //    }
    //    else if (Input.GetKeyDown(KeyCode.A))
    //    {
    //        ChangeSceneByIndex(HeroSelectionScene);
    //    }
    //}

    #endregion

    #region Custom Methods

    /// <summary>
    /// This method is for moving between scenes. To use it properly, please use the Singleton instance for this script and use constant scene indexes as parameter
    /// </summary>
    /// <param name="sceneIndex">Please use it like: \n e.g. SceneManagement.Instance.BattleScene </param>
    public void ChangeSceneByIndex(int sceneIndex)
    {
        float fullScale = 1f;
        float animationDuration = 0.5f;

        sceneChangingImage.transform.DOScale(fullScale, animationDuration).OnComplete(delegate
        {
            HasSceneLoaded = false;

            SceneManager.LoadScene(sceneIndex);
        });
    }

    private void OpenedSceneAnimation()
    {
        float minScale = 0f;
        float animationDuration = 0.35f;

        sceneChangingImage.transform.DOScale(minScale, animationDuration).OnComplete(delegate
        {
            // Game Start

            HasSceneLoaded = true;
        });
    }

    private void OnNewSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        OpenedSceneAnimation();
    }

    #endregion
}
