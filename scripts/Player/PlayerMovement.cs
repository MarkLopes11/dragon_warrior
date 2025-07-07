using UnityEngine;

public class Playermovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpPower;
    [SerializeField] private LayerMask groundlayer;
    [SerializeField] private LayerMask walllayer;
    private Rigidbody2D body;
    private Animator anim;
    //private bool grounded;
    private BoxCollider2D BoxCollider;
    private float wallJumpCooldown;
    private float HorizontalInput;
    private int wallSide = 0; // -1 for left wall, 1 for right wall

    private void Awake()
    {
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        BoxCollider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
        HorizontalInput = Input.GetAxis("Horizontal");

        //flip the player direction when moving left/right
        if (HorizontalInput > 0.01f)
            transform.localScale = Vector3.one;
        else if (HorizontalInput < -0.01f)
            transform.localScale = new Vector3(-1, 1, 1);

        //set animator parameters
        anim.SetBool("run",HorizontalInput != 0);
        anim.SetBool("grounded", isGrounded());

        if (wallJumpCooldown > 0.2f)
        {
            

            body.linearVelocity = new Vector2(HorizontalInput * speed, body.linearVelocity.y);

            if (onWall() && !isGrounded())
            {
                body.gravityScale = 0;
                body.linearVelocity = Vector2.zero;
            }
            else
            {
                body.gravityScale = 3;
                // Only move if not pressing toward the wall
                if ((wallSide == 1 && HorizontalInput > 0) || (wallSide == -1 && HorizontalInput < 0))
                {
                    // Prevent movement into wall
                    body.linearVelocity = new Vector2(0, body.linearVelocity.y);
                }
                else
                {
                    body.linearVelocity = new Vector2(HorizontalInput * speed, body.linearVelocity.y);
                }
            }

            if (Input.GetKey(KeyCode.Space))
                jump();
        }
        else wallJumpCooldown += Time.deltaTime;

        //print(onWall());
    }

    private void jump()
    {
        if (isGrounded())
        {
            body.linearVelocity = new Vector2(body.linearVelocity.x, jumpPower);
            //grounded = false; 
            anim.SetTrigger("jump");
        }
        else if(onWall() && !isGrounded())
        {

            bool facingWall = Mathf.Sign(transform.localScale.x) == wallSide;

            if (facingWall)
            {
                // Jump away from the wall
                body.linearVelocity = new Vector2(-wallSide * 3, 6);
                transform.localScale = new Vector3(-wallSide, 1, 1); // Flip to face away
            }
            else
            {
                // Jump in direction player is already facing
                float direction = Mathf.Sign(transform.localScale.x);
                body.linearVelocity = new Vector2(direction * 3, 6);
            }

            wallJumpCooldown = 0;
            anim.SetTrigger("jump");

        }
        
    }

    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    if (collision.gameObject.tag == "Ground")
    //    {
    //        grounded = true;
    //    }
    //}

    private bool isGrounded()
    {
        RaycastHit2D rayCastHit = Physics2D.BoxCast(BoxCollider.bounds.center, BoxCollider.bounds.size, 0, Vector2.down, 0.1f, groundlayer); 
        return rayCastHit.collider != null;
    }

    private bool onWall()
    {

        RaycastHit2D hitLeft = Physics2D.BoxCast(BoxCollider.bounds.center, BoxCollider.bounds.size, 0, Vector2.left, 0.2f, walllayer);
        RaycastHit2D hitRight = Physics2D.BoxCast(BoxCollider.bounds.center, BoxCollider.bounds.size, 0, Vector2.right, 0.2f, walllayer);

        if (hitLeft.collider != null)
        {
            wallSide = -1;
            return true;
        }
        else if (hitRight.collider != null)
        {
            wallSide = 1;
            return true;
        }

        wallSide = 0;
        return false;
    }

    public bool canAttack()
    {
        return true;
            //HorizontalInput == 0;
            //&& isGrounded() && !onWall();
        
    }
}