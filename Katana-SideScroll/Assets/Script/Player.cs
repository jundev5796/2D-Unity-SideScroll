using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5;
    public float jumpUp = 1;
    public Vector3 direction;
    public GameObject slash;

    bool bJump = false;
    Animator anim;
    Rigidbody2D rb;
    SpriteRenderer sp;

    void Start()
    {
        direction = Vector2.zero;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        KeyInput();
        Move();

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (anim.GetBool("Jump") == false)
            {
                Jump();
                anim.SetBool("Jump", true);
            }
        }
    }

    private void FixedUpdate()
    {
        Debug.DrawRay(rb.position, Vector3.down, new Color(0, 1, 0));

        RaycastHit2D rayHit = Physics2D.Raycast(rb.position, Vector3.down, 1, LayerMask.GetMask("Ground"));

        if (rb.linearVelocityY < 0)
        {
            if (rayHit.collider != null)
            {
                if(rayHit.distance < 0.7f)
                {
                    anim.SetBool("Jump", false);
                }
            }
        }
    }

    void KeyInput()
    {
        direction.x = Input.GetAxisRaw("Horizontal");

        if (direction.x < 0)
        {
            sp.flipX = true;
            anim.SetBool("Run", true);
        }
        else if (direction.x > 0)
        {
            sp.flipX = false;
            anim.SetBool("Run", true);
        }
        else if (direction.x == 0)
        {
            anim.SetBool("Run", false);
        }

        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Attack");
        }
    }

    public void Move()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    public void Jump()
    {
        rb.linearVelocity = Vector2.zero;

        rb.AddForce(new Vector2(0, jumpUp), ForceMode2D.Impulse);
    }

    public void AtkSlash()
    {
        Instantiate(slash, transform.position, Quaternion.identity); // right
    }
}
