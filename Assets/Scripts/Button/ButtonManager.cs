using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    [SerializeField] Animator _animator;
    [SerializeField] GameTimer _gameTimer;
    
    [Header("Cursor Settings")]
    [SerializeField] private Texture2D _cursorTexture;
    [SerializeField] Vector2 hotSpot;

    [Header("Game Settings")]
    [SerializeField]  float timeRemaining = 10;
    [SerializeField]  float _extraTime = 0;
    [SerializeField] TMP_Text _timeText;
    [SerializeField] private float _humanErrorMargin = 0.06f;
    bool timerIsRunning;

    private void Start()
    {
        Cursor.SetCursor(_cursorTexture, hotSpot, CursorMode.Auto);
        //Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    public void StartTimer()
    {
        timerIsRunning = true;
    }

    public void StopTimer()
    {
        timerIsRunning = false;
    }
    
    void Update()
    {
        if (!timerIsRunning) return;
        if (timeRemaining >= 0)
        {
            timeRemaining -= Time.deltaTime;
            DisplayTime(timeRemaining);
        }
        else
        {
            _extraTime += Time.deltaTime;
            timeRemaining = -1;
            DisplayTime(_extraTime, true);
        }
    }

    public void CheckWinState()
    {
        if (timeRemaining <= _humanErrorMargin && _extraTime <= _humanErrorMargin)
        {
            _animator.SetBool("success", true);
            _gameTimer.MarkGameAsSuccess();
        }
        else
        {
            _animator.SetBool("fail", true);
            _gameTimer.MarkGameAsFailure();
        }
    }

    void DisplayTime(float timeToDisplay, bool extra = false)
    {
        // get the total full seconds.
        var t0 = (int) timeToDisplay;
        // get the number of minutes.
        var m = t0/60;
        // get the remaining seconds.
        var s = (t0 - m*60);
        // get the 2 most significant values of the milliseconds.
        var ms = (int)( (timeToDisplay - t0)*100);

        _timeText.text = $"{m:00}:{s:00}:{ms:00}";
        if (extra)
            _timeText.text = "-" + _timeText.text;
        
    }
    
    
}
