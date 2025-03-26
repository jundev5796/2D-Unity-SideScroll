//using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

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

    // ¹Ù´Ú¸ÕÁö
    //public GameObject Dust;
    public GameObject jumpDust;




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
                JumpDust();
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



    // Èë¸ÕÁö
    public void RandDust(GameObject dust)
    {
        Instantiate(dust, transform.position + new Vector3(-0.114f, -0.467f, 0), Quaternion.identity);
    }

    public void JumpDust()
    {
        Instantiate(jumpDust, transform.position, Quaternion.identity);
    }
}
