using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PointsData
{
    public int _maxPoint;
    public int _lastRoundPoints;
    public bool _isContinuation;

    public PointsData(GameManager _GameManager)
    {
        _maxPoint = _GameManager.maxPoints;
        _lastRoundPoints = _GameManager.lastRoundPoints;
        _isContinuation = _GameManager.isContinuation;
    }
}