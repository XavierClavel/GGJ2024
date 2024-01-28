using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour
{
    [SerializeField] private RectTransform curtainLeft;
    [SerializeField] private RectTransform curtainRight;
    private float posVisible = 450f;
    private float posHidden = 1500f;
    private static TransitionManager instance;

    private void Awake()
    {
        instance = this;
        curtainLeft.gameObject.SetActive(true);
        curtainRight.gameObject.SetActive(true);
        curtainLeft.DOAnchorPosX(-posHidden, 1.5f)
            .SetEase(Ease.InOutQuad)
            .SetDelay(0.5f);
        curtainRight.DOAnchorPosX(posHidden, 1.5f)
            .SetEase(Ease.InOutQuad)
            .SetDelay(0.5f);
    }

    public static void TransitionToScene(string scene)
    {
        instance.StartCoroutine(nameof(transitionToScene), scene);
    }

    private IEnumerator transitionToScene(string scene)
    {
        yield return Helpers.getWait(0.5f);
        curtainLeft.DOAnchorPosX(-posVisible, 1.5f)
            .SetEase(Ease.InOutQuad);
        curtainRight.DOAnchorPosX(posVisible, 1.5f)
            .SetEase(Ease.InOutQuad);
        yield return Helpers.getWait(2f);
        SceneManager.LoadScene(scene);
    }

    public static void Quit()
    {
        instance.StartCoroutine(nameof(quit));
    }

    private IEnumerator quit()
    {
        yield return Helpers.getWait(0.5f);
        curtainLeft.DOAnchorPosX(-posVisible, 1.5f)
            .SetEase(Ease.InOutQuad);
        curtainRight.DOAnchorPosX(posVisible, 1.5f)
            .SetEase(Ease.InOutQuad);
        yield return Helpers.getWait(2f);
        Application.Quit();
    }
}
