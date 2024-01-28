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
        TransitionManager.TransitionToScene("MainMenu");
    }

    public void Quit()
    {
        TransitionManager.Quit();
    }

    IEnumerator WaitBeforePlay()
    {
        buttons.DOAnchorPosY(1100f, 1f).SetEase(Ease.InOutQuad);
        title.DOAnchorPosX(-1600f,1f).SetEase(Ease.InOutQuad);
        yield return Helpers.getWait(1.5f);
        SceneManager.LoadScene("SampleScene");
    }
}
