using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PointsData
{
    public int _maxScans;
    public int _lastRoundScans;
    public bool _isContinuation;

    public PointsData(GameManager _GameManager)
    {
        _maxScans = _GameManager.maxScans;
        _lastRoundScans = _GameManager.lastRoundScans;
        _isContinuation = _GameManager.isContinuation;
    }
}