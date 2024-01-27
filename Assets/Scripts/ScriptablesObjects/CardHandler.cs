using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHandler : ScriptableObject
{
    [SerializeField] protected string key;
    [SerializeField] protected Sprite icon;

    public string getKey() => key.Trim();
    public Sprite getIcon() => icon;
    public virtual bool isIntonation() => false;
}

[CreateAssetMenu(fileName = "CardHandler", menuName = Vault.other.scriptableObjectMenu + "CardHandler", order = 0)]
public class CardHandler : ObjectHandler
{
}
