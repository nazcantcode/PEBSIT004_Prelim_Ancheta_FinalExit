using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed = 350f;

    [Header("Room Boundaries")]
    public float minX = -800f;
    public float maxX = 800f;
    public float minY = -430f;
    public float maxY = 130f;

    [Header("Doors")]
    public RectTransform doorA;
    public RectTransform doorB;
    public RectTransform doorC;

    [Header("Game Manager")]
    public GameManager gameManager;

    private RectTransform playerRect;

    private bool canMove = true;

    // Mobile movement
    private Vector2 mobileDirection = Vector2.zero;

    // 0 = no door selected
    // 1 = Door A
    // 2 = Door B
    // 3 = Door C
    private int selectedDoor = 0;

    void Start()
    {
        playerRect = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (!canMove)
            return;

        MovePlayer();

        CheckDoorSelection();
    }

    void MovePlayer()
    {
        float horizontal = 0f;
        float vertical = 0f;

        // LEFT
        if (Input.GetKey(KeyCode.A) ||
            Input.GetKey(KeyCode.LeftArrow))
        {
            horizontal = -1f;
        }

        // RIGHT
        if (Input.GetKey(KeyCode.D) ||
            Input.GetKey(KeyCode.RightArrow))
        {
            horizontal = 1f;
        }

        // UP
        if (Input.GetKey(KeyCode.W) ||
            Input.GetKey(KeyCode.UpArrow))
        {
            vertical = 1f;
        }

        // DOWN
        if (Input.GetKey(KeyCode.S) ||
            Input.GetKey(KeyCode.DownArrow))
        {
            vertical = -1f;
        }

        Vector2 keyboardDirection =
            new Vector2(horizontal, vertical);

        Vector2 movement =
            keyboardDirection + mobileDirection;

        if (movement.magnitude > 1f)
        {
            movement.Normalize();
        }

        playerRect.anchoredPosition +=
            movement * moveSpeed * Time.deltaTime;

        KeepPlayerInsideRoom();
    }

    void KeepPlayerInsideRoom()
    {
        Vector2 position = playerRect.anchoredPosition;

        position.x = Mathf.Clamp(
            position.x,
            minX,
            maxX
        );

        position.y = Mathf.Clamp(
            position.y,
            minY,
            maxY
        );

        playerRect.anchoredPosition = position;
    }

    // Detect which door the player is touching
    void CheckDoorSelection()
    {
        if (IsTouching(playerRect, doorA))
        {
            selectedDoor = 1;
        }
        else if (IsTouching(playerRect, doorB))
        {
            selectedDoor = 2;
        }
        else if (IsTouching(playerRect, doorC))
        {
            selectedDoor = 3;
        }
        else
        {
            selectedDoor = 0;
        }
    }

    // Called by the CHOOSE button
    public void ConfirmSelection()
    {
        if (!canMove)
            return;

        if (selectedDoor == 1)
        {
            gameManager.ChooseDoorA();
        }
        else if (selectedDoor == 2)
        {
            gameManager.ChooseDoorB();
        }
        else if (selectedDoor == 3)
        {
            gameManager.ChooseDoorC();
        }
        else
        {
            // Player is not touching any door
            return;
        }

        ResetPosition();
        selectedDoor = 0;
    }

    bool IsTouching(
        RectTransform first,
        RectTransform second)
    {
        Vector3[] firstCorners = new Vector3[4];
        Vector3[] secondCorners = new Vector3[4];

        first.GetWorldCorners(firstCorners);
        second.GetWorldCorners(secondCorners);

        Rect firstRect = new Rect(
            firstCorners[0].x,
            firstCorners[0].y,
            firstCorners[2].x - firstCorners[0].x,
            firstCorners[2].y - firstCorners[0].y
        );

        Rect secondRect = new Rect(
            secondCorners[0].x,
            secondCorners[0].y,
            secondCorners[2].x - secondCorners[0].x,
            secondCorners[2].y - secondCorners[0].y
        );

        return firstRect.Overlaps(secondRect);
    }

    // =========================
    // MOBILE MOVEMENT
    // =========================

    public void PressUp()
    {
        mobileDirection = Vector2.up;
    }

    public void PressDown()
    {
        mobileDirection = Vector2.down;
    }

    public void PressLeft()
    {
        mobileDirection = Vector2.left;
    }

    public void PressRight()
    {
        mobileDirection = Vector2.right;
    }

    public void ReleaseDirection()
    {
        mobileDirection = Vector2.zero;
    }

    public void ResetPosition()
    {
        playerRect.anchoredPosition =
            new Vector2(0f, -360f);

        selectedDoor = 0;
    }

    public void SetMovement(bool value)
    {
        canMove = value;

        if (!value)
        {
            mobileDirection = Vector2.zero;
            selectedDoor = 0;
        }
    }
}