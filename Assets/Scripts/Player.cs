using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Player : MonoBehaviour
{
    [SerializeField]
    private SpriteRenderer playerSprite;

    [SerializeField]
    private SpriteRenderer volumeSprite;

    [SerializeField]
    private List<Color> colorList;

    private float playerSize;
    private bool isGrounded;

    private float moveSpeed = 2000.0f;
    private float jumpSpeed = 20000.0f;
    private bool canJump;

    private bool isBottomHit;
    private bool isTopHit;
    private bool isLeftHit;
    private bool isRightHit;

    private Vector2 velocity;
    private float drag = 0.9f;

    private Rigidbody2D rigidBody2d;

    private void Awake() {
        isGrounded = false;
        canJump = false;

        isBottomHit = false;
        isTopHit = false;
        isLeftHit = false;
        isRightHit = false;

        velocity = Vector2.zero;

        playerSize = playerSprite.size.x * 0.9f;

        rigidBody2d = GetComponent<Rigidbody2D>();
    }

    // Start is called before the first frame update
    private void Start()
    {
        //CycleColors();
    }

    // Update is called once per frame
    private void Update() {
        UpdateVolume();
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
            //rigidBody2d.AddForce(new Vector2(0f, 10000.0f * MicInput.MicLoudness));
            //Debug.Log(size);
        }

        volumeSprite.transform.localScale = new Vector3(size, size, 0);
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


        Vector2 direction = -Vector2.up * 0.401f;
        // Cast a ray straight down.
        RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, 0.401f);

        Debug.DrawRay(transform.position, direction, Color.green);

        if (hit.collider != null) {
            if (hit.collider.gameObject.layer == LayerMask.NameToLayer("Obstacle")) {
                canJump = true;
            }
        }

    }

    private void MoveLeft() {
        if (!isLeftHit) {
            rigidBody2d.AddForce(Vector2.left * moveSpeed);
        }
    }
    private void MoveRight() {
        if (!isRightHit) {
            rigidBody2d.AddForce(Vector2.left * -moveSpeed);
        }
    }

    private void Jump() {
        Debug.Log("jump");
        if (canJump) {
            canJump = false;
            rigidBody2d.AddForce(new Vector2(0f, jumpSpeed));

            transform.SetParent(null);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.tag == "Moving Platform") {
            transform.SetParent(collision.transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.tag == "Moving Platform") {
            transform.SetParent(null);
        }
    }

}
