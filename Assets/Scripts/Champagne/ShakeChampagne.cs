using UnityEngine;
using System.Collections;

public class ShakeChampagne : MonoBehaviour
{
    private Vector3 originalPosition;
    public float shakeAmount = 0.1f;
    public float shakeDuration = 0.2f;
    public GameObject bottleCap; // 引用瓶盖子物体
    public Vector2 capEjectForce = new Vector2(0, 500f); // 瓶盖喷出的力
    public float capSpinTorque = 200f; // 瓶盖旋转的力矩
    public static int clickCount = 0; // 设为全局静态变量
    private bool capEjected = false; // 瓶盖是否已喷出


    private void Start()
    {
        // 记录瓶子最初的位置
        originalPosition = transform.localPosition;
    }

    private void OnMouseDown()
    {
        // 每次点击时增加点击次数
        clickCount++;

        // 如果点击次数少于50次，继续晃动
        if (clickCount < 10)
        {
            StartCoroutine(Shake());
        }

        // 当点击次数达到50次并且瓶盖尚未喷出时，喷出瓶盖
        if (clickCount >= 10 && !capEjected)
        {
            EjectCap();
        }
    }

    private IEnumerator Shake()
    {
        float elapsedTime = 0f;

        while (elapsedTime < shakeDuration)
        {
            // 随机产生一个晃动的位置
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
        // 瓶盖喷出
        if (bottleCap != null)
        {
            
            Rigidbody2D rb = bottleCap.GetComponent<Rigidbody2D>();
            
            if (rb != null)
            {
                rb.gravityScale = 1.0F;
                // 添加向上的力和稍微向右的水平力，让瓶盖喷出
                rb.AddForce(capEjectForce);

                // 添加旋转力矩，让瓶盖在喷出时旋转
                rb.AddTorque(capSpinTorque);
            }
        }

        capEjected = true; // 标记瓶盖已喷出
    }
}
