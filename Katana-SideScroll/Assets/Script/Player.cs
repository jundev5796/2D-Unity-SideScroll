//using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class Player : MonoBehaviour
{
    [Header("Player Stat")]
    public float speed = 5;
    public float jumpUp = 1;
    public float power = 5;
    public Vector3 direction;
    public GameObject slash;

    // shadow
    public GameObject Shadow1;
    List<GameObject> sh = new List<GameObject>();

    // hit effect
    public GameObject hit_lazer;

    bool bJump = false;
    Animator anim;
    Rigidbody2D rb;
    SpriteRenderer sp;

    // 바닥먼지
    //public GameObject Dust;
    public GameObject jumpDust;

    // 벽점프
    public Transform wallChk;
    public float wallChkDistance;
    public LayerMask wLayer;
    bool isWall;
    public float slidingSpeed;
    public float wallJumpPower;
    public bool isWallJump;
    float isRight = 1;




    void Start()
    {
        direction = Vector2.zero;
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        sp = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (!isWallJump)
        {
            KeyInput();
            Move();
        }

        isWall = Physics2D.Raycast(wallChk.position, Vector2.right * isRight, wallChkDistance, wLayer);
        anim.SetBool("Grab", isWall);

        if (Input.GetKeyDown(KeyCode.W))
        {
            if (anim.GetBool("Jump") == false)
            {
                Jump();
                anim.SetBool("Jump", true);
                JumpDust();
            }
        }

        if (isWall)
        {
            isWallJump = false;
            // 벽점프 상태
            rb.linearVelocity = new Vector2(rb.linearVelocityX, rb.linearVelocityY * slidingSpeed);
            // 벽을 잡고있는 상태에서 점프
            if (Input.GetKeyDown(KeyCode.W))
            {
                isWallJump = true;
                // 벽점프 먼지

                Invoke("FreezeX", 0.3f);

                // 물리
                rb.linearVelocity = new Vector2(-isRight * wallJumpPower, 0.9f * wallJumpPower);

                sp.flipX = sp.flipX == false ? true : false;
                isRight = -isRight;
            }
        }
    }

    void FreezeX()
    {
        isWallJump = false;
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
            // left
            sp.flipX = true;
            anim.SetBool("Run", true);

            // 점프 벽잡기 방향


            // shadowflip
            for (int i = 0; i < sh.Count; i++)
            {
                sh[i].GetComponent<SpriteRenderer>().flipX = sp.flipX;
            }
        }

        else if (direction.x > 0)
        {
            sp.flipX = false;
            anim.SetBool("Run", true);

            isRight = 1;
        }

        else if (direction.x == 0)
        {
            anim.SetBool("Run", false);

            for (int i = 0; i < sh.Count; i++)
            {
                Destroy(sh[i]); // destroy game object
                sh.RemoveAt(i); // destroy game object list
            }
        }

        if (Input.GetMouseButtonDown(0))
        {
            anim.SetTrigger("Attack");
            Instantiate(hit_lazer, transform.position, Quaternion.identity);
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
        // player right
        if (sp.flipX == false)
        {
            rb.AddForce(Vector2.right * power, ForceMode2D.Impulse);

            GameObject go = Instantiate(slash, transform.position, Quaternion.identity); // right
            //go.GetComponent<SpriteRenderer>().flipX = sp.flipX;
        }
        else
        {
            rb.AddForce(Vector2.left * power, ForceMode2D.Impulse);

            GameObject go = Instantiate(slash, transform.position, Quaternion.identity); // left
            //go.GetComponent<SpriteRenderer>().flipX = sp.flipX;
        }
    }

    public void RunShadow() 
    {
        if (sh.Count < 6)
        {
            GameObject go = Instantiate(Shadow1, transform.position, Quaternion.identity);
            go.GetComponent<Shadow>().TwSpeed = 10 - sh.Count;
            sh.Add(go);
        }
    }



    // 흙먼지
    public void RandDust(GameObject dust)
    {
        Instantiate(dust, transform.position + new Vector3(-0.114f, -0.467f, 0), Quaternion.identity);
    }

    public void JumpDust()
    {
        Instantiate(jumpDust, transform.position, Quaternion.identity);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(wallChk.position, Vector2.right * isRight * wallChkDistance);
    }
}
