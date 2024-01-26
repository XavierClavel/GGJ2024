using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHandler : ScriptableObject
{
    [SerializeField] protected string key;
    [SerializeField] protected Sprite icon;

    public string getKey() { return key.Trim(); }
    public Sprite getIcon() { return icon; }
}

[CreateAssetMenu(fileName = "CardHandler", menuName = Vault.other.scriptableObjectMenu + "CardHandler", order = 0)]
public class CardHandler : ObjectHandler
{
}
