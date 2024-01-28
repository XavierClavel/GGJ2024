using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectHandler : ScriptableObject
{
    [SerializeField] protected string key;
    [SerializeField] protected Sprite icon;
    [SerializeField] protected Color accentColor;

    public string getKey() => key.Trim();
    public Sprite getIcon() => icon;
    public Color getAccentColor() => accentColor;
    public virtual Color getBackgroundColor() => new Color32(244, 235, 212, 255);
    public virtual bool isIntonation() => false;
}

[CreateAssetMenu(fileName = "CardHandler", menuName = Vault.other.scriptableObjectMenu + "CardHandler", order = 0)]
public class CardHandler : ObjectHandler
{
}
