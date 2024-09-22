using UnityEngine;

public class AutoMove : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float leftBoundary = -7.5f;
    public float rightBoundary = 7.5f;
    public float clickAreaLeft;
    public float clickAreaRight;
    public float stackThreshold = 0.5f; // threshold for stacking

    private int direction = 1;
    private bool isClicked = false;
    private bool isStacked = false;
    private bool IsRegistered = false;
    private Rigidbody2D rb;
    private AutoMove stackTarget; // target for stacking
    [SerializeField] private AudioSource breakSound;
    void Start()
    {
        
        rb = GetComponent<Rigidbody2D>();
        rb.gravityScale = 0;
    }

    void Update()
    {
        if (!isClicked && !isStacked)
        {
            // move horizontally
            Vector3 movement = new Vector3(direction, 0, 0) * moveSpeed * Time.deltaTime;
            transform.Translate(movement);

            // change direction when reaching boundaries
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
            isClicked = true;
            rb.gravityScale = 1; // active gravity
            stackTarget = GlassManager.Instance.GetStackTarget(this);
            // Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            // if (mousePosition.x >= clickAreaLeft && mousePosition.x <= clickAreaRight)
            // {
            //     isClicked = true;
            //     rb.gravityScale = 1; // active gravity
            //     stackTarget = GlassManager.Instance.GetStackTarget(this);
            // }
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
                    // success
                    GlassManager.Instance.ActiveNextGlass();
                    isStacked = true;
                    rb.gravityScale = 0;
                    rb.velocity = Vector2.zero;
                    //transform.position = new Vector3(transform.position.x, stackTarget.transform.position.y + 2.0f, transform.position.z);
                    //GlassManager.Instance.ActiveNextGlass();
                    GlassManager.Instance.NotifyStacked(this);
                }
                else
                {
                    // failure
                    // glass breaking sound
                    if(breakSound != null){
                        breakSound.Play();
                    }
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
                GlassManager.Instance.ActiveNextGlass();
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