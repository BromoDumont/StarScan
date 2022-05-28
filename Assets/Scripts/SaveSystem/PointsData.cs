using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PointsData
{
    public int _maxPoint;

    public PointsData(GameManager _GameManager)
    {
        _maxPoint = _GameManager.maxPoints;
    }
}