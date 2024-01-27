using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "IntonationHandler", menuName = Vault.other.scriptableObjectMenu + "IntonationHandler", order = 0)]
public class IntonationHandler : ObjectHandler
{
    public override bool isIntonation() => true;
}