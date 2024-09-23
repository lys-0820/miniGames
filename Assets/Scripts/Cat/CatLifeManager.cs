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
        ShowLives();
    }

    private void ShowLives()
    {
        foreach (var life in _lives)
        {
            life.ShowLife();
            //yield return new WaitForSeconds(0.3f);
        }
    }

    public void LoseLives()
    {
        foreach (var life in _lives)
        {
            life.LoseLife();
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
