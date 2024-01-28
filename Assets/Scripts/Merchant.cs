using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Merchant : MonoBehaviour
{
    [SerializeField] private Sprite healSprite;
    [SerializeField] private Sprite harpSprite;
    [SerializeField] private Sprite tambourinSprite;
    [SerializeField] private Sprite mandolinSprite;
    [SerializeField] private Consumable prefabConsumable;
    [SerializeField] private RectTransform emptyGameObject;
    [SerializeField] private RectTransform consumablesLayout;
    public static Merchant instance;

    private void Awake()
    {
        instance = this;
    }

    public void SpawnShop()
    {
        SetupHealConsumable();
        SetupHarpConsumable();
        SetupMandolinConsumable();
        SetupTambourinConsumable();
    }

    public void DespawnShop()
    {
        Debug.Log("Despawn shop");
        consumablesLayout.KillAllChildren();
    }

    Consumable setupConsumable()
    {
        RectTransform go = Instantiate(emptyGameObject, consumablesLayout);
        Consumable consumable = Instantiate(prefabConsumable, go);
        consumable.setSlot(go);
        return consumable;
    }

    void SetupHealConsumable()
    {
        setupConsumable().setup(healSprite, Player.Heal);
    }

    void SetupHarpConsumable()
    {
        setupConsumable().setup(harpSprite, Ennemy.IncreasePatience);
    }

    void SetupMandolinConsumable()
    {
        setupConsumable().setup(mandolinSprite, Ennemy.ReduceEmotions);
    }

    void SetupTambourinConsumable()
    {
        setupConsumable().setup(tambourinSprite, delegate {  });
    }
    
}
