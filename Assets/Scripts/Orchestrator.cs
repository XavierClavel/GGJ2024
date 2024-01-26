using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Orchestrator : MonoBehaviour
{
    [SerializeField] private DataManager dataManager;

    private void Awake()
    {
        dataManager.LoadData();
    }
    
}
