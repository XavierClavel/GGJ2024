using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using TMPro;
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
    [SerializeField] private TextMeshProUGUI damageDisplay;
    private List<GameObject> patiencePoints = new List<GameObject>();
    private Dictionary<string, int> dictEmotions = new Dictionary<string, int>();
    private bool isKing = false;

    private Dictionary<string, EmotionDisplay> dictKeyToEmotionDisplay = new Dictionary<string, EmotionDisplay>();
    protected int damage;
    private int ennemyIndex = 0;

    public void setIndex(int value)
    {
        ennemyIndex = value;
    }
    
    public static void ApplyTambourin()
    {
        AudioManager.PlaySfx("Tambour");
        Ennemy[] ennemies = new Ennemy[ennemiesList.Count];
        ennemiesList.CopyTo(ennemies);
        foreach (var ennemy in ennemies)
        {
            ennemy.Tambourin();
        }
    }

    public void Tambourin()
    {
        if (ennemyIndex == 1) return;
        List<string> keys = dictEmotions.Keys.ToList();
        foreach (var key in keys)
        {
            if (ennemyIndex == 0) dictEmotions[key] += 2;
            if (ennemyIndex == 2) dictEmotions[key] -= 2;
            dictKeyToEmotionDisplay[key].setValue(dictEmotions[key]);
            if (dictEmotions[key] <= 0) dictEmotions.Remove(key);
        }
        
    }

    public static void IncreasePatience()
    {
        foreach (var ennemy in ennemiesList)
        {
            AudioManager.PlaySfx("Harp");
            ennemy.patience++;
            GameObject go = Instantiate(ennemy.patiencePoint, ennemy.patienceLayout);
            ennemy.patiencePoints.Add(go);
        }
    }

    public static void ReduceEmotions()
    {
        AudioManager.PlaySfx("Luth");
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
            ennemy.ApplyEffect(dictEffects, false);
        }
    }

    public Ennemy setup(float position, Dictionary<string, int> dictEmotions, bool king = false)
    {
        isKing = king;
        ennemiesList.Add(this);
        rectTransform.anchoredPosition = 800f * Vector2.right;
        rectTransform.DOAnchorPosX(position, 2f).SetEase(Ease.OutQuad);
        if (position < -50f) ennemyIndex = 0;
        else if (position < 50f) ennemyIndex = 1;
        else ennemyIndex = 2;
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
        if (isKing)
        {
            patience = 3; 
            return;
        }
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
        damageDisplay.SetText(damage.ToString());
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

    public void ApplyEffect(Dictionary<string, int> input, bool endTurn = true)
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
            StartCoroutine(nameof(Cure));
            return;
        }

        if (!endTurn) return;

        patience--;
        GameObject go = patiencePoints.Last();
        patiencePoints.Remove(go);
        go.SetActive(false);
        if (patience == 0)
        {
            StartCoroutine(nameof(Fail));
        }
    }

    public static bool isWaveOver() => ennemiesList.isEmpty();

    private IEnumerator Cure()
    {
        updateEnnemyList(this);
        yield return Helpers.getWait(ennemiesList.Count * 0.4f);
        Debug.Log("Patient is cured !");
        AudioManager.PlaySfx("Cure");
        Player.IncreaseGold(patience);
        Leave();
    }

    private IEnumerator Fail()
    {
        updateEnnemyList(this);
        yield return Helpers.getWait(ennemiesList.Count * 0.4f);
        Debug.Log("Ennemy has left");
        AudioManager.PlaySfx("Fail");
        Player.TakeDamage(damage);
        if (!isKing)
        {
            Leave();
            yield break;
        }
        patience = 3;
        for (int i = 0; i < patience; i++)
        {
            GameObject go = Instantiate(patiencePoint, patienceLayout);
            patiencePoints.Add(go);
        }
    }

    private void Leave()
    {
        rectTransform.DOScaleX(-1f, 0.3f).SetEase(Ease.InCubic);
        rectTransform.DOAnchorPosX(800f, 2f).SetDelay(0.3f).OnComplete(
            delegate { Destroy(gameObject); });
    }
    

    private static void updateEnnemyList(Ennemy ennemy)
    {
        ennemiesList.Remove(ennemy);
        if (!ennemiesList.isEmpty()) return;
        Player.WaveOver();
    }
}
