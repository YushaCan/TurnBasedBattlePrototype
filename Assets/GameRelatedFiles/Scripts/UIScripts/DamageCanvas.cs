using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DamageCanvas : MonoBehaviour
{
    #region Variables

    [SerializeField] private TextMeshProUGUI damageText;
    
    [SerializeField] private float midPosition;
    [SerializeField] private float targetPosition;

    #endregion

    #region Unity Methods



    #endregion

    #region Custom Methods

    public void StartAnimation(int damageGiven)
    {
        float animDuration = 0.5f;
        float fullAlpha = 1f;
        float minAlpha = 0f;

        float midPos = damageText.rectTransform.anchoredPosition.y + midPosition;
        float targetPos = damageText.rectTransform.anchoredPosition.y + targetPosition;

        damageText.text = "-" + damageGiven.ToString();

        damageText.DOFade(fullAlpha, animDuration);
        damageText.rectTransform.DOAnchorPosY(midPos, animDuration).OnComplete(delegate
        {
            DOVirtual.DelayedCall(animDuration, delegate
            {
                damageText.DOFade(minAlpha, animDuration);
                damageText.rectTransform.DOAnchorPosY(targetPos, animDuration).OnComplete(delegate
                {
                    Destroy(gameObject);
                });
            });
        });
       
    }

    #endregion
}
