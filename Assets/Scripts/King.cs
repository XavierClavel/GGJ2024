using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class King : Ennemy
{
    protected override void Fail()
    {
        Player.TakeDamage(damage);
        patience = 3;
    }
}
