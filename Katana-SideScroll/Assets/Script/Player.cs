using UnityEngine;

public class Player : MonoBehaviour
{
    public float speed = 5;
    public float jumpUp = 1;
    public Vector2 direction;

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
        
    }

    void KeyInput()
    {
        direction.x = Input.GetAxisRaw("Horizontal");

        if (direction.x < 0)
        {
            sp.flipX = true;
        }
        else if (direction.x > 0)
        {
            sp.flipX = false;
        }
        else if (direction.x == 0)
        {

        }
    }
}
