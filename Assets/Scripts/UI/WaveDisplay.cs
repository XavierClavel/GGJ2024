using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class WaveDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private RectTransform waveIndicator;

    public void OnPointerEnter(PointerEventData eventData)
    {
        waveIndicator.DOAnchorPosY(-30f, 0.5f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        DOTween.KillAll();
        waveIndicator.DOAnchorPosY(130f, 0.5f);
    }
}
