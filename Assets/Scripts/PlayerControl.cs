using System;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PlayerControl : MonoBehaviour
{
    public float moveSpeed = 7f;
    public float jumpForce = 14f;
    private Rigidbody2D rigidBody;
    private Animator animator;
    private GameObject[] spawns;
    private float gravitySlot = 0;
    private bool isOnCloud = false;
    private bool wasOnCloud = false;
    private bool isOnGrounded = false;
    private bool isFalling = false;
    private bool isFacingRight = true;
    [SerializeField]
    private int lives = 3;
    public float fenceHeight = 1f;
    private bool isFenceReached = false;

    private bool IsFenceReached
    {
        get => isFenceReached;
        set
        {
            if (value == isFenceReached)
            {
                return;
            }
            isFenceReached = value;
            OnPlayerFenceReached?.Invoke(isFenceReached);
            Debug.Log("Fence reached: " + isFenceReached);
        }
    }

    public delegate void PlayerFenceReached(bool isReached);
    public event PlayerFenceReached OnPlayerFenceReached;

    public delegate void PlayerDeath(int lives);
    public event PlayerDeath OnPlayerDeath;

    public void Die()
    {
        lives--;
        OnPlayerDeath?.Invoke(lives);
        if (lives == 0)
        {
            SceneManager.LoadScene("GameOver");
        }
        else
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        int spawnIndex = UnityEngine.Random.Range(0, spawns.Length);
        transform.position = spawns[spawnIndex].transform.position;
        IsFenceReached = false;
        wasOnCloud = false;
    }

    void Jump()
    {
        rigidBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        isOnGrounded = false;
    }

    private void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spawns = GameObject.FindGameObjectsWithTag("Spawnpoint");
        gravitySlot = rigidBody.gravityScale;
    }

    private void Update()
    {
        animator.SetBool("IsRunning", rigidBody.velocity.x != 0);
        animator.SetBool("IsJumping", !isOnGrounded);
        if (Math.Sign(rigidBody.velocity.x) * (isFacingRight ? 1 : -1) < 0)
        {
            Flip();
        }
    }

    private void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal");

        isOnGrounded = rigidBody.velocity.y == 0;
        IsFenceReached = transform.position.y >= fenceHeight;

        if (isOnCloud)
        {
            rigidBody.velocity = new Vector2(moveX, Input.GetAxis("Vertical")) * moveSpeed;
        }
        else if (isOnGrounded)
        {
            var moveY = Input.GetAxisRaw("Vertical");
            if (!isFalling && moveY > 0)
            {
                Jump();
            }
            isFalling = moveY < 0;
        }

        if (moveX != 0)
        {
            rigidBody.velocity = new Vector2(moveX * moveSpeed, rigidBody.velocity.y);
        }
    }

    private void Flip()
    {
        isFacingRight = !isFacingRight;
        transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
    }

    private void SetGravity(bool enable)
    {
        rigidBody.gravityScale = enable ? gravitySlot : 0;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Cloud"))
        {
            wasOnCloud = true;
            isOnCloud = true;
            SetGravity(false);
            transform.SetParent(other.transform.parent, true);
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Spike"))
        {
            isFalling = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Cloud"))
        {
            isOnCloud = false;
            if (Input.GetAxisRaw("Vertical") > 0)
            {
                Jump();
            }
            SetGravity(true);
            transform.SetParent(null, true);
        }
    }


    private void OnCollisionStay2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Platform"))
        {
            if (wasOnCloud)
            {
                Respawn();
            }
            else if (isFalling)
            {
                other.gameObject.GetComponent<Passable>().Pass();
            }
        }
    }

}
