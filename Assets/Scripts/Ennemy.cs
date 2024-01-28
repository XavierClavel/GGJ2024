using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class Ennemy : MonoBehaviour
{
    [SerializeField] private Image image;
    public static List<Ennemy> ennemiesList = new List<Ennemy>();
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] protected int patience;
    [SerializeField] private GameObject patiencePoint;
    [SerializeField] private Transform patienceLayout;
    [SerializeField] private EmotionDisplay prefabEmotionDisplay;
    [SerializeField] private Transform emotionLayout;
    private List<GameObject> patiencePoints = new List<GameObject>();
    private Dictionary<string, int> dictEmotions = new Dictionary<string, int>();

    private Dictionary<string, EmotionDisplay> dictKeyToEmotionDisplay = new Dictionary<string, EmotionDisplay>();
    protected int damage;

    public static void IncreasePatience()
    {
        foreach (var ennemy in ennemiesList)
        {
            ennemy.patience++;
            GameObject go = Instantiate(ennemy.patiencePoint, ennemy.patienceLayout);
            ennemy.patiencePoints.Add(go);
        }
    }

    public static void ReduceEmotions()
    {
        Dictionary<string, int> dictEffects = new Dictionary<string, int>
        {
            { Vault.emotion.Sadness, 1 },
            { Vault.emotion.Fear, 1 },
            { Vault.emotion.Depression, 1 },
            { Vault.emotion.Disdain, 1 },
            { Vault.emotion.Anger, 1},
            { Vault.emotion.Disgust, 1}
        };
        Ennemy[] ennemies = new Ennemy[ennemiesList.Count];
        ennemiesList.CopyTo(ennemies);
        foreach (var ennemy in ennemies)
        {
            ennemy.ApplyEffect(dictEffects);
        }
    }

    public Ennemy setup(float position, Dictionary<string, int> dictEmotions)
    {
        ennemiesList.Add(this);
        rectTransform.anchoredPosition = 800f * Vector2.right;
        rectTransform.DOAnchorPosX(position, 2f).SetEase(Ease.OutQuad);
        this.dictEmotions = dictEmotions;
        setPatience();
        setDamage();
        SetupDisplay();
        return this;
    }

    public Ennemy setSprite(Sprite sprite)
    {
        image.sprite = sprite;
        return this;
    }
    
    

    private void setPatience()
    {
        int total = 0;
        foreach (var e in this.dictEmotions.Values)
        {
            total += e;
        }
        
        patience = Mathf.Max(3,total / 2 + 1);
    }

    private void setDamage()
    {
        damage = patience / 3;
    }
    
    

    private void SetupDisplay()
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

    public static bool isWaveOver() => ennemiesList.isEmpty();

    private void Cure()
    {
        Debug.Log("Patient is cured !");
        Player.IncreaseGold(patience);
        updateEnnemyList(this);
        Leave();
    }

    protected virtual void Fail()
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
