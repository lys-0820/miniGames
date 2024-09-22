using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerButton : MonoBehaviour
{
    [SerializeField] ButtonManager _buttonManager;  
    [SerializeField] private Animator _buttonAnimator;

    private bool _hasPressed;
    public void PressButton()
    {
        if (_hasPressed) return;
        _hasPressed = true;
        _buttonAnimator.SetBool("press", true);
        _buttonManager.StartTimer();
        
    }

    public void ReleaseButton()
    {
        _buttonAnimator.SetBool("press", false);
        _buttonManager.StopTimer(); 
        _buttonManager.CheckWinState();
    }
}
