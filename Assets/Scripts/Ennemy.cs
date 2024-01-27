using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;

public class Ennemy : MonoBehaviour
{
    public static List<Ennemy> ennemiesList = new List<Ennemy>();
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private int patience;
    [SerializeField] private GameObject patiencePoint;
    [SerializeField] private Transform patienceLayout;
    [SerializeField] private EmotionDisplay prefabEmotionDisplay;
    [SerializeField] private Transform emotionLayout;
    private List<GameObject> patiencePoints = new List<GameObject>();
    private Dictionary<string, int> dictEmotions = new Dictionary<string, int>();

    private Dictionary<string, EmotionDisplay> dictKeyToEmotionDisplay = new Dictionary<string, EmotionDisplay>();
    private int damage;

    private void Awake()
    {
        ennemiesList.Add(this);
        dictEmotions["Sadness"] = 1;
        damage = patience / 3 + 1;
    }

    public void setup(float position)
    {
        rectTransform.anchoredPosition = 800f * Vector2.right;
        rectTransform.DOAnchorPosX(position, 2f).SetEase(Ease.OutQuad);
    }

    private void Start()
    {
        for (int i = 0; i < patience; i++)
        {
            GameObject go = Instantiate(patiencePoint, patienceLayout);
            patiencePoints.Add(go);
        }

        foreach (var value in dictEmotions)
        {
            EmotionDisplay emoDisplay = Instantiate(prefabEmotionDisplay, emotionLayout);
            emoDisplay.setup(value.Key, value.Value);
            dictKeyToEmotionDisplay[value.Key] = emoDisplay;
        }
    }

    private void OnDestroy()
    {
        ennemiesList.Remove(this);
    }

    public void ApplyEffect(Recipe recipe)
    {
        ApplyEffect(recipe.getOutput());
    }

    public void ApplyEffect(Dictionary<string, int> input)
    {
        foreach (var effet in input)
        {
            if (!dictEmotions.ContainsKey(effet.Key)) continue;
            dictEmotions[effet.Key] -= effet.Value;
            dictKeyToEmotionDisplay[effet.Key].setValue(dictEmotions[effet.Key]);
            if (dictEmotions[effet.Key] <= 0) dictEmotions.Remove(effet.Key);
        }

        if (dictEmotions.Count == 0)
        {
            Cure();
            return;
        }

        patience--;
        GameObject go = patiencePoints.Last();
        patiencePoints.Remove(go);
        go.SetActive(false);
        if (patience == 0)
        {
            Fail();
        }
    }

    private void Cure()
    {
        Debug.Log("Patient is cured !");
        Player.IncreaseGold(patience);
        updateEnnemyList(this);
        Leave();
    }

    private void Fail()
    {
        Debug.Log("Ennemy has left");
        Player.TakeDamage(damage);
        Leave();
    }

    private void Leave()
    {
        updateEnnemyList(this);
        rectTransform.DOScaleX(-1f, 0.3f).SetEase(Ease.InCubic);
        rectTransform.DOAnchorPosX(800f, 2f).SetDelay(0.3f).OnComplete(
            delegate { Destroy(gameObject); });
    }
    

    private static void updateEnnemyList(Ennemy ennemy)
    {
        ennemiesList.Remove(ennemy);
        if (ennemiesList.isEmpty()) Player.WaveOver();
    }
}
