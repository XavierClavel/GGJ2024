using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
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
