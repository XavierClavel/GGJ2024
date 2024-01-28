using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private RectTransform tutorial;
    private static float tutoPosVisible = 75f;
    private static float tutoPosHidden = 1100;
    public void Resume()
    {
        Player.instance.PauseUnpause();
    }

    public void ShowTutorial()
    {
        tutorial.DOAnchorPosY(tutoPosVisible, 1f).SetEase(Ease.InOutQuad);
    }

    public void HideTutorial()
    {
        tutorial.DOAnchorPosY(tutoPosHidden, 1f).SetEase(Ease.InOutQuad);
    }

    public void Play()
    {
        TransitionManager.TransitionToScene("SampleScene");
    }

    public void ToMainMenu()
    {
        TransitionManager.TransitionToScene("MainMenu");
    }

    public void Quit()
    {
        TransitionManager.Quit();
    }
}
