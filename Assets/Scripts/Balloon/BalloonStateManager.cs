using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OkapiKit;

public class BalloonStateManager : MonoBehaviour
{
    [SerializeField] private Hypertag _obstacleTag;
    [SerializeField] private GameTimer _gameTimer;

    void Start()
    {
        _gameTimer.MarkGameAsSuccess();
    }
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log(collider.gameObject.name);
        if (collider.gameObject.HasHypertag(_obstacleTag))
        {
            _gameTimer.MarkGameAsFailure();
        }
    }
}
