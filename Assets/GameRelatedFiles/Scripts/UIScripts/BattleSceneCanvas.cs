using TMPro;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BattleSceneCanvas : MonoBehaviour
{
    #region Variables

    public static BattleSceneCanvas Instance;

    public bool IsFirstOpening { get; set; } = true;

    [SerializeField] private Image gameStartPanelBg;
    [SerializeField] private Image battleImage;
    [SerializeField] private TextMeshProUGUI battleText;
    
    [SerializeField] private TextMeshProUGUI tutorialText;

    [SerializeField] private CanvasGroup turnPanel;
    [SerializeField] private TextMeshProUGUI turnText;
    private const string playersTurnText = "YOUR TURN!";
    private const string enemyTurnText = "ENEMY'S TURN!";
    private RectTransform turnPanelRectTransform;
    [SerializeField] private Vector2 turnPanelStartingPos;
    [SerializeField] private Vector2 turnPanelMidPos;
    [SerializeField] private Vector2 turnPanelTargetPos;

    [SerializeField] private GameObject gameOverPanelGO;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private TextMeshProUGUI gameOverText;
    [SerializeField] private Button returnButton;
    [SerializeField] private Color32 battleWinColor;
    [SerializeField] private Color32 battleLoseColor;
    private const string battleWinText = "BATTLE WIN!";
    private const string battleLoseText = "BATTLE LOSE!";

    #endregion

    #region Action Variables



    #endregion

    #region Unity Methods

    private IEnumerator Start()
    {
        yield return new WaitUntil(() => SceneManagement.Instance.HasSceneLoaded);

        OpenStartingUI();

        turnPanelRectTransform = turnPanel.GetComponent<RectTransform>();
    }

    private void Awake()
    {
        Instance = this;
    }

    #endregion

    #region Custom Methods

    private void OpenStartingUI()
    {
        float animDuration = 0.5f;
        float fullScale = 1f;

        battleImage.transform.DOScale(fullScale, animDuration).OnComplete(delegate
        {
            battleText.transform.DOScale(fullScale, animDuration).OnComplete(delegate
            {
                DOVirtual.DelayedCall(1f, delegate
                {
                    CloseStartingUI();
                });
            });
        });
    }

    private void CloseStartingUI()
    {
        float animDuration = 0.25f;
        float targetScale = 0f;

        battleText.transform.DOScale(targetScale, animDuration).OnComplete(delegate
        {
            battleImage.transform.DOScale(targetScale, animDuration).OnComplete(delegate
            {
                gameStartPanelBg.gameObject.SetActive(false);

                // OPEN YOUR TURN UI HERE
                TurnPanel(GameManager.Instance.IsPlayerTurn);
            });
        });
    }

    public void TurnPanel(bool isPlayerTurn)
    {
        if (isPlayerTurn)
        {
            turnText.text = playersTurnText;
        }
        else
        {
            turnText.text = enemyTurnText;
        }

        float animDuration = 0.5f;
       
        turnPanel.alpha = 0;
        turnPanelRectTransform.anchoredPosition = turnPanelStartingPos;

        turnPanelRectTransform.DOAnchorPos(turnPanelMidPos, animDuration);

        turnPanel.DOFade(1, animDuration).OnComplete(delegate
        {
            DOVirtual.DelayedCall(animDuration, delegate
            {
                turnPanelRectTransform.DOAnchorPos(turnPanelTargetPos, animDuration);

                turnPanel.DOFade(0, animDuration).OnComplete(delegate
                {
                    if (!GameManager.Instance.HasBattleStarted)
                    {
                        GameManager.Instance.SetBattleStarted(true);
                    }
                });
            });
        });
    }

    public void OpenGameOverUI(bool isGameWin)
    {
        float animDuration = 0.5f;
        float targetScale = 1f;

        if (isGameWin)
        {
            gameOverText.text = battleWinText;
            gameOverText.color = battleWinColor;
        }
        else
        {
            gameOverText.text = battleLoseText;
            gameOverText.color = battleLoseColor;
        }

        gameOverPanel.transform.localScale = Vector2.zero;

        gameOverPanelGO.gameObject.SetActive(true);

        gameOverPanel.transform.DOScale(targetScale, animDuration).OnComplete(delegate
        {
            returnButton.onClick.AddListener(GameOverButtonFunction);
        });

    }

    private void GameOverButtonFunction()
    {
        float targetScale = 0f;
        float animDuration = 0.25f;

        returnButton.onClick.RemoveAllListeners();

        gameOverPanel.transform.DOScale(targetScale, animDuration).OnComplete(delegate
        {
            GameManager.Instance.OnBattleFinished?.Invoke();
            gameOverPanelGO.gameObject.SetActive(false);

            DOVirtual.DelayedCall(0.5f, delegate
            {
                SceneManagement.Instance.ChangeSceneByIndex(SceneManagement.Instance.HeroSelectionScene);
            });
        });
    }

    public void OpenTutorialText()
    {
        float animDuration = 0.2f;
        float fullAlpha = 1f;

        tutorialText.gameObject.SetActive(true);

        tutorialText.DOFade(fullAlpha, animDuration);
    }

    public void CheckTutorialText()
    {
        if (tutorialText.gameObject.activeInHierarchy)
        {
            float animDuration = 0.2f;
            float targetAlpha = 0f;

            tutorialText.DOFade(targetAlpha, animDuration);

            tutorialText.gameObject.SetActive(false);
        }
    }

    #endregion
}
