using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyManager : MonoBehaviour
{
    [SerializeField] private Ennemy ennemyPrefab;
    [SerializeField] private Transform ennemiesLayout;
    private static EnnemyManager instance;

    private void Awake()
    {
        instance = this;
    }

    public static void SpawnEnnemies()
    {
        instance.SpawnEnnemy();
    }

    private void SpawnEnnemy()
    {
        Ennemy ennemy = Instantiate(ennemyPrefab, ennemiesLayout);
    }
}
