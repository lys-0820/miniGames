using UnityEngine;

public class AutoMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float leftBoundary = -7.5f;
    public float rightBoundary = 7.5f;
    public float clickAreaLeft;
    public float clickAreaRight;
    public float stackThreshold = 0.5f; // 堆叠允许的水平距离阈值

    private int direction = 1;
    private bool isClicked = false;
    private bool isStacked = false;
    private bool IsRegistered = false;
    private Rigidbody2D rb;
    private AutoMove stackTarget; // 下方的目标物体

    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    void Update()
    {
        if (!isClicked && !isStacked)
        {
            // 水平移动
            Vector3 movement = new Vector3(direction, 0, 0) * moveSpeed * Time.deltaTime;
            transform.Translate(movement);

            // 到达边界时改变方向
            if (transform.position.x >= rightBoundary)
            {
                direction = -1;
            }
            else if (transform.position.x <= leftBoundary)
            {
                direction = 1;
            }
        }
    }

    void OnMouseDown()
    {
        
        if (!isClicked && !isStacked && !IsRegistered)
        {
            GlassManager.Instance.RegisterAutoMove(this);
            IsRegistered = true;
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (mousePosition.x >= clickAreaLeft && mousePosition.x <= clickAreaRight)
            {
                isClicked = true;
                rb.gravityScale = 1; // 启用重力
                stackTarget = GlassManager.Instance.GetStackTarget(this);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (isClicked && !isStacked)
        {
            if (stackTarget != null)
            {
                float horizontalDistance = Mathf.Abs(transform.position.x - stackTarget.transform.position.x);
                if (horizontalDistance <= stackThreshold)
                {
                    // 成功堆叠
                    isStacked = true;
                    rb.gravityScale = 0;
                    rb.velocity = Vector2.zero;
                    transform.position = new Vector3(transform.position.x, stackTarget.transform.position.y + 1.5f, transform.position.z);
                    GlassManager.Instance.NotifyStacked(this);
                }
                else
                {
                    // 堆叠失败
                    Debug.Log("游戏失败：堆叠不成功");
                    //Destroy(gameObject, 2f); // 2秒后销毁物体
                    GlassManager.Instance.SetWinCondition(false);
                    GlassManager.Instance.SetGameOverCondition(true);
                    GlassManager.Instance.ClearObject();
                    GameTimer.Instance.MarkGameAsFailure();
                }
            }
            else if (collision.gameObject.CompareTag("Table"))
            {
                // 第一个物体固定在桌子上
                isStacked = true;
                rb.gravityScale = 0;
                rb.velocity = Vector2.zero;
                GlassManager.Instance.NotifyStacked(this);
            }
        }
    }
    void FixedUpdate()
    {
        if (isClicked && isStacked)
        {
            // 确保物体完全静止
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            
            // 如果有堆叠目标，确保物体保持在正确的位置
            if (stackTarget != null)
            {
                // Vector3 correctPosition = new Vector3(
                //     stackTarget.transform.position.x,
                //     stackTarget.transform.position.y + 1f,
                //     transform.position.z
                // );
               // transform.position = correctPosition;
            }
        }
    }
}