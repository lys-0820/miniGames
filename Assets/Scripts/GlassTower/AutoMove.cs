using UnityEngine;
using System.Collections.Generic;

public class AutoMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float leftBoundary = -7.5f;
    public float rightBoundary = 7.5f;
    public float gravity = 9.8f;

    private int direction = 1;
    private bool isClicked = false;
    private float yVelocity = 0f;

    private static List<AutoMove> allObjects = new List<AutoMove>();

    void Start()
    {
        allObjects.Add(this);
    }

    void OnDestroy()
    {
        allObjects.Remove(this);
    }

    void Update()
    {
        if (!isClicked)
        {
            // 未被点击时继续水平移动
            Vector3 movement = new Vector3(direction, 0, 0) * moveSpeed * Time.deltaTime;
            transform.Translate(movement);

            // 到达左右边界时改变方向
            if (transform.position.x >= rightBoundary)
            {
                direction = -1;
            }
            else if (transform.position.x <= leftBoundary)
            {
                direction = 1;
            }
        }
        else
        {
            // 被点击后下落
            yVelocity += gravity * Time.deltaTime;
            transform.Translate(new Vector3(0, -yVelocity * Time.deltaTime, 0));

            // 检查碰撞和堆叠
            float groundLevel = -4.5f; // 假设地面位置，根据实际情况调整
            AutoMove objectBelow = allObjects.Find(other => 
                other != this &&
                other.transform.position.y < transform.position.y &&
                transform.position.x < other.transform.position.x + 1f &&
                transform.position.x + 1f > other.transform.position.x
            );

            if (objectBelow != null)
            {
                transform.position = new Vector3(transform.position.x, objectBelow.transform.position.y + 1f, transform.position.z);
                yVelocity = 0;
            }
            else if (transform.position.y <= groundLevel)
            {
                transform.position = new Vector3(transform.position.x, groundLevel, transform.position.z);
                yVelocity = 0;
            }
        }
    }

    void OnMouseDown()
    {
        if (!isClicked)
        {
            Debug.Log("OnMouseDown");
            isClicked = true;
            moveSpeed = 0; // 停止水平移动
        }
    }
}