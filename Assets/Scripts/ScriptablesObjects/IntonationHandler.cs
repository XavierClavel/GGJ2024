using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IntonationHandler", menuName = Vault.other.scriptableObjectMenu + "IntonationHandler", order = 0)]
public class IntonationHandler : CardHandler
{
    [SerializeField] private Color backgroundColor;
    public override bool isIntonation() => true;
    public override Color getBackgroundColor()
    {
        return backgroundColor;
    }
}