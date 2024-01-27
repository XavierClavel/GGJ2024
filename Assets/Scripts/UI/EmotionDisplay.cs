using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class EmotionDisplay : MonoBehaviour
{
    [SerializeField] private Image image;
    [SerializeField] private TextMeshProUGUI valueDisplay;
    private string emotionKey;

    public EmotionDisplay setType(string key)
    {
        emotionKey = key;
        if(DataManager.dictKeyToEmotion[key].getIcon() != null) image.sprite = DataManager.dictKeyToEmotion[key].getIcon();
        return this;
    }

    public EmotionDisplay setValue(int value)
    {
        if (value <= 0)
        {
            gameObject.SetActive(false);
            return null;
        }
        valueDisplay.SetText(value.ToString());
        Debug.Log(valueDisplay.text);
        return this;
    }

    public EmotionDisplay setup(string key, int value)
    {
        Debug.Log(value);
        setType(key);
        setValue(value);
        return this;
    }
}
