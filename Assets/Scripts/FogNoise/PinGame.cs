using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PinGame : MonoBehaviour
{
    public Transform bar;
    public Transform pin;
    public Transform targetArea;
    public Button stopButton;
    public Button restartButton;
    public TextMeshProUGUI resultText;
    public Animator frogAnimator; // 青蛙动画控制器
    public Collider2D frogCollider; // 青蛙的碰撞体

    public float pinSpeed = 5f; // 针的移动速度

    private bool isMoving = true;
    private bool movingRight = true;
    private float minX, maxX; // 针的移动范围

    void Start()
    {
        stopButton.onClick.AddListener(StopPin);
        restartButton.onClick.AddListener(RestartGame);
        restartButton.gameObject.SetActive(false);
        CalculateMovementRange();
        SetupGame();
        frogCollider.enabled = true; // 确保碰撞体开启
    }

    void CalculateMovementRange()
    {
        // 获取 bar 的边界
        Renderer barRenderer = bar.GetComponent<Renderer>();
        if (barRenderer != null)
        {
            Vector3 barMin = barRenderer.bounds.min;
            Vector3 barMax = barRenderer.bounds.max;
            minX = barMin.x;
            maxX = barMax.x;
        }
        else
        {
            Debug.LogError("Bar 没有 Renderer 组件！");
        }

        // 考虑 pin 的宽度，调整移动范围
        Renderer pinRenderer = pin.GetComponent<Renderer>();
        if (pinRenderer != null)
        {
            float pinWidth = pinRenderer.bounds.size.x;
            minX += pinWidth / 2;
            maxX -= pinWidth / 2;
        }
        else
        {
            Debug.LogError("Pin 没有 Renderer 组件！");
        }

        Debug.Log($"计算的移动范围：minX = {minX}, maxX = {maxX}");
    }

    void Update()
    {
        if (isMoving && Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (frogCollider.OverlapPoint(mousePosition))
            {
                StopPin();
            }
        }

    }

    void SetupGame()
    {
        isMoving = true;
        movingRight = true;
        resultText.text = "";

        // 设置针的初始位置
        pin.position = new Vector2(minX, pin.position.y);

        // 随机设置目标区域的位置
        float targetX = Random.Range(minX + targetArea.localScale.x / 2, maxX - targetArea.localScale.x / 2);
        targetArea.position = new Vector2(targetX, targetArea.position.y);

        Debug.Log($"游戏设置 - minX: {minX}, maxX: {maxX}, 初始 pin 位置: {pin.position}");
    }

    void MovePin()
    {
        Debug.Log("MovePin");
        float newX = pin.position.x + (movingRight ? pinSpeed : -pinSpeed) * Time.deltaTime;

        // 使用 Mathf.Clamp 确保 pin 不会超出范围
        newX = Mathf.Clamp(newX, minX, maxX);

        if (newX >= maxX)
        {
            movingRight = false;
        }
        else if (newX <= minX)
        {
            movingRight = true;
        }

        pin.position = new Vector2(newX, pin.position.y);
    }

    void StopPin()
    {
        isMoving = false;
        frogAnimator.SetTrigger("Click");
        frogCollider.enabled = false;

        // 使用协程来延迟检查结果，给动画播放的时间
        StartCoroutine(DelayedCheckResult());
    }

    System.Collections.IEnumerator DelayedCheckResult()
    {
        yield return new WaitForSeconds(0.5f); // 等待动画播放
        CheckResult();
        restartButton.gameObject.SetActive(true);
    }

    void CheckResult()
    {
        float pinCenter = pin.position.x;
        float targetMin = targetArea.position.x - targetArea.localScale.x / 2;
        float targetMax = targetArea.position.x + targetArea.localScale.x / 2;

        if (pinCenter >= targetMin && pinCenter <= targetMax)
        {
            resultText.text = "Win";
            resultText.color = Color.green;
            GameTimer.Instance.MarkGameAsSuccess();
        }
        else
        {
            resultText.text = "Fail";
            resultText.color = Color.red;
            GameTimer.Instance.MarkGameAsFailure();
        }
    }

    public void RestartGame()
    {
        SetupGame();
        isMoving = true;
        restartButton.gameObject.SetActive(false);
        //stopButton.gameObject.SetActive(true);
        // ... 其他重置代码 ...
        frogCollider.enabled = true; // 重新启用碰撞体
        frogAnimator.SetTrigger("Reset"); // 重置青蛙动画状态
    }
}