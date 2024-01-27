using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class WaveDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private RectTransform waveIndicator;
    [SerializeField] private RectTransform playerIndicator;
    [SerializeField] private HorizontalLayoutGroup layoutGroup;
    [SerializeField] private RectTransform waveItem;
    private Tween tweenWaveDisplay = null;
    public static WaveDisplay instance;

    private void Awake()
    {
        instance = this;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        DisplayWaveIndicator();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
         HideWaveIndicator();
    }

    public void DisplayWaveIndicator(bool unstoppable = false)
    {
        tweenWaveDisplay?.Kill();
        tweenWaveDisplay = waveIndicator.DOAnchorPosY(-30f, 0.5f);
        if (unstoppable) tweenWaveDisplay = null;
    }

    public void HideWaveIndicator()
    {
        tweenWaveDisplay?.Kill();
        tweenWaveDisplay = waveIndicator.DOAnchorPosY(130f, 0.5f);
    }

    public void MovePlayerIndicator()
    {
        RectTransform target = layoutGroup.transform.GetChild(WaveManager.getCurrentWave() - 1).GetComponent<RectTransform>();
        tweenWaveDisplay = playerIndicator.DOAnchorPosX(target.anchoredPosition.x, 0.5f);
    }
}
