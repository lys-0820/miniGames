using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Numerics;
using Vector2 = UnityEngine.Vector2;

public class CatGameManager : MonoBehaviour
{
    [Header("Cursor Settings")]
    [SerializeField] private Texture2D _cursorTexture;
    [SerializeField] Vector2 hotSpot;

    [Header("Slider Values")]
    [SerializeField] float _moveDelay;  
    private bool _forward;
    [SerializeField] float _slideDuration;

    [Header("Scene References")]
    [SerializeField] private Slider _slider;
    [SerializeField] private Animator _catAnimator;
    [SerializeField] CatLifeManager _catLifeManager;
    [SerializeField] private GameTimer _timer;
    [Header("Game Conditions")]
    [SerializeField] private float _minCorrectRange;
    [SerializeField] private float _maxCorrectRange;

    [SerializeField] private bool _isDevilActive;

    [Header("Sound Effects")] [SerializeField]
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _catPurrSound;
    [SerializeField] private AudioClip _catSuccessSound;
    [SerializeField] private AudioClip _catFailSound;
    public void CheckWinState(float value)
    {
        if (value > _minCorrectRange && value < _maxCorrectRange)
        {
            _catLifeManager.GainLife();
            _audioSource.PlayOneShot(_catPurrSound);
            if (_catLifeManager.GetGameState()) GameWon();
            else StartCoroutine(ResetGameCycle());
            return;
        }
        GameLost();
    }
    public void GameWon()
    {
        Debug.Log("Game Won");
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        _isDevilActive = false;
        _audioSource.PlayOneShot(_catSuccessSound);
        _catLifeManager.LoseLives();
        _catAnimator.SetBool("success", true);
        _timer.MarkGameAsSuccess();
    }

    public void GameLost()
    {
        _audioSource.PlayOneShot(_catFailSound);
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);

        _catAnimator.SetBool("fail", true);
        _slider.gameObject.SetActive(false);
        _isDevilActive = false;
        _timer.MarkGameAsFailure();
    }
    
    void Start()
    {
        _slider.gameObject.SetActive(false);
        Cursor.SetCursor(_cursorTexture, hotSpot, CursorMode.Auto);
        StartCoroutine(StartGameCycle());   
    }

    IEnumerator StartGameCycle()
    {
        yield return new WaitForSeconds(0.3f);
        _isDevilActive = true;
        _catAnimator.SetBool("play", true);
        StartCoroutine(StartBar());
    }

    IEnumerator ResetGameCycle()
    {
        _catAnimator.SetBool("play", false);
        _isDevilActive = false;
        _slider.gameObject.SetActive(false);
        var delayTimer = Random.Range(0.5f, 1f);
        yield return new WaitForSeconds(delayTimer);
        StartCoroutine(StartGameCycle()); 
    }

    void Update()
    {
        if (_isDevilActive && Input.GetMouseButtonDown(0))
        {
            CheckWinState(_slider.value);
        }
    }
    
    IEnumerator StartBar()
    {
        _slider.gameObject.SetActive(true);
        _forward = true;
        while (_isDevilActive)
        {        
            var step = 1 / _slideDuration;
            if (_forward)
            {
                if (_slider.value >= 1) _forward = false;
                _slider.value += step;
                yield return new WaitForSecondsRealtime(_moveDelay);
            }
            else
            { 
                if (_slider.value <= 0) _forward = true;
                _slider.value -= step;
                yield return new WaitForSecondsRealtime(_moveDelay);
            }
            yield return null;
        }
        
    }
}