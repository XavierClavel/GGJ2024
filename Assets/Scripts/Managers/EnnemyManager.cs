using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyManager : MonoBehaviour
{
    [SerializeField] private Ennemy ennemyPrefab;
    [SerializeField] private Transform ennemiesLayout;
    public RectTransform canvas;
    public static EnnemyManager instance;

    private void Awake()
    {
        instance = this;
    }

    public static void SpawnEnnemies()
    {
        List<float> spawnPos = new List<float>
        {
            200f,
            0f,
            -200f
        };
        int wave = WaveManager.getCurrentWave();
        for (int i = 0; i < getEnnemiesAmount(wave); i++)
        {
            instance.SpawnEnnemy(spawnPos.popRandom());   
        }
    }

    private static int getEnnemiesAmount(int wave)
    {
        return wave switch
        {
            1 => 1,
            2 => 1,
            3 => 1,
            4 => 1,
            5 => 1,
            6 => 1,
            7 => 2,
            8 => 1,
            9 => 2,
            10 => 1,
            11 => 2,
            12 => 2,
            13 => 2,
            14 => 3,
            15 => 2,
            16 => 2,
            17 => 3,
            18 => 2,
            19 => 3,
            20 => 3,
            _ => 1
        };
    } 

    private void SpawnEnnemy(float position)
    {
        Ennemy ennemy = Instantiate(ennemyPrefab, ennemiesLayout);
        ennemy.setup(position);
    }
}
