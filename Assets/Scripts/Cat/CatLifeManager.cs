using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatLifeManager : MonoBehaviour
{
    [SerializeField] private CatLife[] _lives;
    private bool hasWon;
    private int _currentIndex = 0;
    private void Start()
    {
        StartCoroutine(ShowLives());
    }

    private IEnumerator ShowLives()
    {
        foreach (var life in _lives)
        {
            life.ShowLife();
            yield return new WaitForSeconds(0.3f);
        }
    }

    public void GainLife()
    {
        _lives[_currentIndex].GainLife();
        _currentIndex++;
        if (_currentIndex >= _lives.Length)
            hasWon = true;
    }

    public bool GetGameState()
    {
        return hasWon;
    }
}
