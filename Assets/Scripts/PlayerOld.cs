using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerOld : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer playerSprite;

    [SerializeField]
    private SpriteRenderer volumeSprite;

    [SerializeField]
    private List<Color> colorList;

    private float playerSize;

    private float gravity = 19.8f;
    private bool isGrounded;

    private float moveSpeed = 5.0f;
    private float jumpSpeed = 20.0f;
    private bool canJump;

    private bool isBottomHit;
    private bool isTopHit;
    private bool isLeftHit;
    private bool isRightHit;

    private Vector2 velocity;
    private float drag = 0.9f;

    private void Awake() {
        isGrounded = false;
        canJump = false;

        isBottomHit = false;
        isTopHit = false;
        isLeftHit = false;
        isRightHit = false;

        velocity = Vector2.zero;

        playerSize = playerSprite.size.x * 0.9f;
    }

    // Start is called before the first frame update
    private void Start()
    {
        //CycleColors();
    }

    // Update is called once per frame
    private void Update() {
        UpdateVolume();
        CheckCollision();
        ApplyGravity();
        HandleMovement();
    }

    private float size = 0.0f;

    private void CycleColors() {
        volumeSprite.DOBlendableColor(GetRandomColor(), 0.1f).SetEase(Ease.Linear).OnComplete(CycleColors);
    }

    private Color GetRandomColor() {
        return colorList[Random.Range(0, colorList.Count)];
    }

    private void UpdateVolume() {

        //Debug.Log(MicInput.MicLoudness);
        if (MicInput.MicLoudness <= 0.005) {
            //Debug.Log("Not talking");
            Mathf.SmoothDamp(0.0f, 0, ref size, 0.25f, 8.0f);

            //volumeSprite.DOFade(0.0f, 0.25f);

        } else {
            if (size <= 0) {
                volumeSprite.DOFade(1.0f, 0.05f);
                volumeSprite.transform.localScale = Vector3.zero;
                size = 0;
            }

            Mathf.SmoothDamp(0.0f, MicInput.MicLoudness * 10, ref size, 0.75f, 5.0f);
            //Debug.Log(size);
        }

        volumeSprite.transform.localScale = new Vector3(size, size, 0);
    }

    private void ApplyGravity() {
        if (!isGrounded) {
            //transform.Translate(Vector3.up * -gravity * Time.deltaTime, Space.Self);
            if (velocity.y >= 0) {
                velocity.y -= gravity * Time.deltaTime * 20;
            } else { 
        }
        } else {
            Debug.Log("Grounded");
            velocity.y = 0;
        }
    }

    private void HandleMovement() {
        if (Input.GetKey(KeyCode.LeftArrow)) {
            MoveLeft();
        }

        if (Input.GetKey(KeyCode.RightArrow)) {
            MoveRight();
        }

        if (Input.GetKey(KeyCode.UpArrow)) {
            Jump();
        }

        transform.Translate(velocity * Time.deltaTime, Space.Self);

        velocity.x *= drag * Time.deltaTime * 60;
    }

    private void MoveLeft() {
        if (!isLeftHit) {
            //transform.Translate(Vector3.left * moveSpeed * Time.deltaTime, Space.Self);
            velocity.x = -moveSpeed;
        }
    }
    private void MoveRight() {
        if (!isRightHit) {
            //transform.Translate(Vector3.left * -moveSpeed * Time.deltaTime, Space.Self);
            velocity.x = moveSpeed;
        }
    }

    private void Jump() {
        if (!isTopHit && canJump) {
            canJump = false;
            velocity.y = jumpSpeed;
        }
    }

    private void CheckCollision() {
        float offset = playerSize / 4;
        float raycastLength = playerSize / 4;

        // Bottom
        Vector3 bottomLeft = new Vector3(transform.position.x - offset, transform.position.y - offset, transform.position.z);
        Vector3 bottomRight = new Vector3(transform.position.x + offset, transform.position.y - offset, transform.position.z);
        Vector3 bottomCenter = new Vector3(transform.position.x, transform.position.y - offset, transform.position.z);

        RaycastHit2D bottomLeftHit = Physics2D.Raycast(bottomLeft, -Vector2.up, raycastLength);
        RaycastHit2D bottomRightHit = Physics2D.Raycast(bottomRight, -Vector2.up, raycastLength);
        RaycastHit2D bottomCenterHit = Physics2D.Raycast(bottomCenter, -Vector2.up, raycastLength);

        Debug.DrawRay(bottomLeft, -Vector2.up * raycastLength, bottomLeftHit.collider == null ? Color.green : Color.red);
        Debug.DrawRay(bottomRight, -Vector2.up * raycastLength, bottomRightHit.collider == null ? Color.green : Color.red);
        Debug.DrawRay(bottomCenter, -Vector2.up * raycastLength, bottomCenterHit.collider == null ? Color.green : Color.red);

        // Top
        Vector3 topLeft = new Vector3(transform.position.x - offset, transform.position.y + offset, transform.position.z);
        Vector3 topRight = new Vector3(transform.position.x + offset, transform.position.y + offset, transform.position.z);
        Vector3 topCenter = new Vector3(transform.position.x, transform.position.y + offset, transform.position.z);

        RaycastHit2D topLeftHit = Physics2D.Raycast(topLeft, Vector2.up, raycastLength);
        RaycastHit2D topRightHit = Physics2D.Raycast(topRight, Vector2.up, raycastLength);
        RaycastHit2D topCenterHit = Physics2D.Raycast(topCenter, Vector2.up, raycastLength);

        Debug.DrawRay(topLeft, Vector2.up * raycastLength, topLeftHit.collider == null ? Color.green : Color.red);
        Debug.DrawRay(topRight, Vector2.up * raycastLength, topRightHit.collider == null ? Color.green : Color.red);
        Debug.DrawRay(topCenter, Vector2.up * raycastLength, topCenterHit.collider == null ? Color.green : Color.red);

        // Left
        Vector3 leftTop = new Vector3(transform.position.x - offset, transform.position.y - offset, transform.position.z);
        Vector3 leftBottom = new Vector3(transform.position.x - offset, transform.position.y + offset, transform.position.z);
        Vector3 leftCenter = new Vector3(transform.position.x - offset, transform.position.y, transform.position.z);

        RaycastHit2D leftTopHit = Physics2D.Raycast(leftTop, Vector2.left, raycastLength);
        RaycastHit2D leftBottomHit = Physics2D.Raycast(leftBottom, Vector2.left, raycastLength);
        RaycastHit2D leftCenterHit = Physics2D.Raycast(leftCenter, Vector2.left, raycastLength);

        Debug.DrawRay(leftTop, Vector2.left * raycastLength, leftTopHit.collider == null ? Color.green : Color.red);
        Debug.DrawRay(leftBottom, Vector2.left * raycastLength, leftBottomHit.collider == null ? Color.green : Color.red);
        Debug.DrawRay(leftCenter, Vector2.left * raycastLength, leftCenterHit.collider == null ? Color.green : Color.red);
        
        // Right
        Vector3 rightTop = new Vector3(transform.position.x + offset, transform.position.y - offset, transform.position.z);
        Vector3 rightBottom = new Vector3(transform.position.x + offset, transform.position.y + offset, transform.position.z);
        Vector3 rightCenter = new Vector3(transform.position.x + offset, transform.position.y, transform.position.z);

        RaycastHit2D rightTopHit = Physics2D.Raycast(bottomLeft, -Vector2.left, raycastLength);
        RaycastHit2D rightBottomHit = Physics2D.Raycast(bottomRight, -Vector2.left, raycastLength);
        RaycastHit2D rightCenterHit = Physics2D.Raycast(bottomCenter, -Vector2.left, raycastLength);

        Debug.DrawRay(rightTop, -Vector2.left * raycastLength, rightTopHit.collider == null ? Color.green : Color.red);
        Debug.DrawRay(rightBottom, -Vector2.left * raycastLength, rightBottomHit.collider == null ? Color.green : Color.red);
        Debug.DrawRay(rightCenter, -Vector2.left * raycastLength, rightCenterHit.collider == null ? Color.green : Color.red);

        // If left hit something or right hit something or center hit something
        if (bottomLeftHit.collider != null || bottomRightHit.collider != null || bottomCenterHit.collider != null) {
            isBottomHit = true;
            isGrounded = true;
            canJump = true;
            Debug.Log("hit");
        } else {
            isBottomHit = false;
            isGrounded = false;
        }

        if (topLeftHit.collider != null || topRightHit.collider != null || topCenterHit.collider != null) {
            isTopHit = true;
        } else {
            isTopHit = false;
        }

        if (leftTopHit.collider != null || leftBottomHit.collider != null || leftCenterHit.collider != null) {
            isLeftHit = true;
            velocity.x = 0;
        } else {
            isLeftHit = false;
        }

        if (rightTopHit.collider != null || rightBottomHit.collider != null || rightCenterHit.collider != null) {
            isRightHit = true;
            velocity.x = 0;
        } else {
            isRightHit = false;
        }
    }
}
