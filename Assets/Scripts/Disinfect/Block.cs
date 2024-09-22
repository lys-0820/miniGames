using UnityEngine;

public class Block : MonoBehaviour
{
    [SerializeField] private int correctRotation = 0;
    [SerializeField] public bool canRandomRotate = true;
    [SerializeField] private AudioSource clickSound;

    private int rotationState;
    private AudioSource audioSource;

    void Start()
    {
        // 根据当前旋转设置初始 rotationState
        rotationState = Mathf.RoundToInt(transform.rotation.eulerAngles.z / 90) % 4;
        audioSource = GetComponent<AudioSource>();
        Debug.Log($"Block {name} initialized with rotationState {rotationState}");
    }

    void OnMouseDown()
    {
        Debug.Log("点击了方块");
        RotateBlock();
        PlayClickSound();
        PuzzleManager.Instance.CheckIfPuzzleSolved();
    }

    public void RotateBlock()
    {
        rotationState = (rotationState + 1) % 4;
        transform.rotation = Quaternion.Euler(0, 0, rotationState * 90);
        Debug.Log($"方块旋转: 新的旋转状态 = {rotationState}");
    }

    private void PlayClickSound()
    {
        if (audioSource != null && clickSound != null)
        {
            audioSource.PlayOneShot(clickSound.clip);
        }
    }

    public bool IsCorrectRotation()
    {
        return rotationState == correctRotation;
    }

    public void SetColor(Color color)
    {
        SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            spriteRenderer.color = color;
        }
    }
}