using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private RectTransform tutorial;
    private static float tutoPosVisible = 75f;
    private static float tutoPosHidden = 1100;
    public RectTransform buttons;
    public RectTransform title;

    private void Awake()
    {
        if (title == null) return;
        buttons.DOAnchorPosY(90, 1f).SetEase(Ease.InOutQuad);
        title.DOAnchorPosX(-400f,1.2f).SetEase(Ease.InOutQuad);
    }

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
        StartCoroutine(nameof(WaitBeforePlay));
        
        //TransitionManager.TransitionToScene("SampleScene");
    }

    public void ToMainMenu()
    {
        if (AudioManager.playingBossMusic) AudioManager.playMainMusic();
        TransitionManager.TransitionToScene("MainMenu");
    }

    public void Quit()
    {
        if (title == null) TransitionManager.Quit();
        else Application.Quit();
    }

    public void EraseData()
    {
        SaveManager.Erase();
        if (AudioManager.playingBossMusic) AudioManager.playMainMusic();
        TransitionManager.TransitionToScene("SampleScene");
    }

    IEnumerator WaitBeforePlay()
    {
        AudioManager.PlaySfx("New");
        buttons.DOAnchorPosY(1100f, 1f).SetEase(Ease.InOutQuad);
        title.DOAnchorPosX(-1600f,1f).SetEase(Ease.InOutQuad);
        yield return Helpers.getWait(1.5f);
        SceneManager.LoadScene("SampleScene");
    }
}
