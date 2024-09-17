using UnityEngine;
using System.Collections;

public class ShakeChampagne : MonoBehaviour
{
    private Vector3 originalPosition;
    public float shakeAmount = 0.1f;
    public float shakeDuration = 0.2f;
    public GameObject bottleCap; // ����ƿ��������
    public Vector2 capEjectForce = new Vector2(0, 500f); // ƿ���������
    public float capSpinTorque = 200f; // ƿ����ת������
    public static int clickCount = 0; // ��Ϊȫ�־�̬����
    private bool capEjected = false; // ƿ���Ƿ������


    private void Start()
    {
        // ��¼ƿ�������λ��
        originalPosition = transform.localPosition;
    }

    private void OnMouseDown()
    {
        // ÿ�ε��ʱ���ӵ������
        clickCount++;

        // ��������������50�Σ������ζ�
        if (clickCount < 10)
        {
            StartCoroutine(Shake());
        }

        // ����������ﵽ50�β���ƿ����δ���ʱ�����ƿ��
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
            // �������һ���ζ���λ��
            Vector3 randomPoint = originalPosition + Random.insideUnitSphere * shakeAmount;
            randomPoint.z = originalPosition.z;  // ����Z�᲻��

            // ����ƿ�ӵ�λ��
            transform.localPosition = randomPoint;

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // �ָ�ƿ�ӵ�ԭʼλ��
        transform.localPosition = originalPosition;
    }

    private void EjectCap()
    {
        // ƿ�����
        if (bottleCap != null)
        {
            
            Rigidbody2D rb = bottleCap.GetComponent<Rigidbody2D>();
            
            if (rb != null)
            {
                rb.gravityScale = 1.0F;
                // ������ϵ�������΢���ҵ�ˮƽ������ƿ�����
                rb.AddForce(capEjectForce);

                // �����ת���أ���ƿ�������ʱ��ת
                rb.AddTorque(capSpinTorque);
            }
        }

        capEjected = true; // ���ƿ�������
    }
}
