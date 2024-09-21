using UnityEngine;
using System.Collections;

public class ShakeChampagne : MonoBehaviour
{
    private Vector3 originalPosition;
    public float shakeAmount = 0.1f;
    public float shakeDuration = 0.2f;
    public GameObject bottleCap; // 香槟瓶盖的游戏对象
    public Vector2 capEjectForce = new Vector2(0, 500f); // 瓶盖弹出的力
    public float capSpinTorque = 200f; // 瓶盖旋转的扭矩
    public static int clickCount = 0; // 作为全局静态变量
    private bool capEjected = false; // 瓶盖是否已弹出

    private void Start()
    {
        // 记录瓶子的初始位置
        originalPosition = transform.localPosition;
    }
    private void OnEnable()
    {
        clickCount = 0;
    }
    private void OnMouseDown()
    {

        // 每次点击时增加点击次数
        clickCount++;

        // 如果点击次数少于20次，继续摇晃
        if (clickCount < 20)
        {
            StartCoroutine(Shake());
        }

        // 如果点击次数达到20次且瓶盖未弹出，则弹出瓶盖
        if (clickCount >= 20 && !capEjected)
        {
            EjectCap();
            GameTimer.Instance.MarkGameAsSuccess();
        }
    }

    private IEnumerator Shake()
    {
        float elapsedTime = 0f;

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

        // 恢复瓶子的原始位置
        transform.localPosition = originalPosition;
    }

    private void EjectCap()
    {
        // 瓶盖弹出
        if (bottleCap != null)
        {
            Rigidbody2D rb = bottleCap.GetComponent<Rigidbody2D>();
            
            if (rb != null)
            {
                rb.gravityScale = 1.0F;
                // 施加向上的力，并稍微向右的水平力使瓶盖弹出
                rb.AddForce(capEjectForce);

                // 添加旋转扭矩，使瓶盖在弹出时旋转
                rb.AddTorque(capSpinTorque);
                //TODO: play sound and animation
            }
        }

        capEjected = true; // 标记瓶盖已弹出
    }
}