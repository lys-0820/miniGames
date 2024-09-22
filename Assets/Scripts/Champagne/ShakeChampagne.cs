using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class ShakeChampagne : MonoBehaviour
{
    [SerializeField] Animator _kidAnimator;
    private Vector3 originalPosition;
    public float shakeAmount = 0.1f;
    public float shakeDuration = 0.2f;
    public int clickCount = 0; // 作为全局静态变量
    private bool capEjected = false; // 瓶盖是否已弹出
    public Animator animator;
    [SerializeField] private AudioSource successMusic;
    //change player face
    [SerializeField] private SpriteRenderer targetImage;
    [SerializeField] private Sprite originalSprite;
    [SerializeField] private Sprite successSprite;
    [SerializeField] private Sprite failureSprite;
    [SerializeField] private Sprite shakingSprite;
    //progress bar
    [SerializeField] private Image progressBar;
    [SerializeField] private RectTransform movingBottle;
    private float progressPerClick; // 每次点击增加的进度
    private Vector2 imageStartPosition; // 图像的起始位置

    private void Start()
    {
        // 记录瓶子的初始位置
        originalPosition = transform.localPosition;
        targetImage.sprite = originalSprite;
        // progress bar init
        progressPerClick = 1f / 20f; // 每次点击增加5%的进度
        imageStartPosition = movingBottle.anchoredPosition;
        
    }
    private void OnEnable()
    {
        clickCount = 0;
    }
    private void Update(){
        if(GameTimer.Instance.GetRemainingTime() <= 0.02f && !GameTimer.Instance.GetGameSuccess()){
            _kidAnimator.SetBool("lose", true);
            //targetImage.sprite = failureSprite;
        }
    }
    private void OnMouseDown()
    {
        // 每次点击时增加点击次数
        clickCount++;
        // 如果点击次数少于20次，继续摇晃
        if (clickCount < 20)
        {
            //update progress bar
            UpdateProgressBar();
            StartCoroutine(Shake());
        }

        // 如果点击次数达到20次且瓶盖未弹出，则弹出瓶盖
        if (clickCount >= 20 && !capEjected)
        {
            //update progress bar
            UpdateProgressBar();
            EjectCap();
            GameTimer.Instance.MarkGameAsSuccess();
        }
    }

    private IEnumerator Shake()
    {
        float elapsedTime = 0f;
        _kidAnimator.SetTrigger("shake");
        //targetImage.sprite = shakingSprite;
        while (elapsedTime < shakeDuration)
        {
            // 生成一个随机摇晃位置
            Vector3 randomPoint = originalPosition + Random.insideUnitSphere * shakeAmount;
            randomPoint.z = originalPosition.z;  // 保持Z轴不变
            // 更新瓶子的位置
            transform.localPosition = randomPoint;

            elapsedTime += Time.deltaTime;
            yield return null;
        }
        //targetImage.sprite = originalSprite;
        // 恢复瓶子的原始位置
        transform.localPosition = originalPosition;
    }

    private void EjectCap()
    {
        // 瓶盖弹出
        if (animator != null)
        {
            animator.SetTrigger("IsEject");
        }
        _kidAnimator.SetBool("win", true);
        //targetImage.sprite = successSprite;
        //TODO: play sound
        if (successMusic != null)
        {
            successMusic.Play();
        }


        capEjected = true; // 标记瓶盖已弹出

    }
    private void UpdateProgressBar()
    {
        if (progressBar != null)
        {
            progressBar.fillAmount = clickCount * progressPerClick;
        }
        if (movingBottle != null)
        {
            Vector2 newPosition = imageStartPosition + new Vector2(clickCount * 39.5f, 0);
            movingBottle.anchoredPosition = newPosition;
        }
    }
}