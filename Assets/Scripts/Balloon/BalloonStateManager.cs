using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OkapiKit;

public class BalloonStateManager : MonoBehaviour
{
    [SerializeField] private Hypertag _winTag;
    [SerializeField] private GameTimer _gameTimer;

    void Start()
    {
    }
    
    private void OnTriggerEnter2D(Collider2D collider)
    {
        Debug.Log(collider.gameObject.name);
        if (collider.gameObject.HasHypertag(_winTag))
        {
            _gameTimer.MarkGameAsSuccess();
        }
    }
}
