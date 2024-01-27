using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveData
{
    public Vector2Int ennemies;
    public Vector2Int emotionsAmount;
    public Vector2Int emotionsPoints;
}

public class WaveDataBuilder : DataBuilder<WaveData>
{
    protected override WaveData BuildData(List<string> s)
    {
        WaveData waveData = new WaveData();
        SetValue(ref waveData.ennemies, "Ennemies");
        SetValue(ref waveData.emotionsAmount, "EmotionsAmount");
        SetValue(ref waveData.emotionsPoints, "EmotionsPoints");
        return waveData;
    }
}
