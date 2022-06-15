using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ScansData
{
    public int _maxScans;
    public int _maxCombo;
    public int _lastRoundScans;
    public bool _isContinuation;

    public ScansData(GameManager _GameManager)
    {
        _maxScans = _GameManager.maxScans;
        _maxCombo = _GameManager.maxCombo;
        _lastRoundScans = _GameManager.lastRoundScans;
        _isContinuation = _GameManager.isContinuation;
    }
}