using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermovement : MonoBehaviour
{
    public float speed;
    public float jump_force;
    private float move_input;
    public bool ismoving;
    private Rigidbody2D rb;

    private bool facingRight = true;


    private bool is_grounded;
    public Transform ground_check;
    public float check_radius;
    public LayerMask what_is_ground;

    private float jump_time_counter;
    public float jump_time;
    private bool is_jumping;
    private bool canjump;
    private bool has_jumped = false;

    private Animator anim;

    private Vector2 colliderSize;
    private Vector2 slope_normal_perp;
    private CapsuleCollider2D cc;
    public float slope_check_distance;
    private float slope_down_angle;
    private bool is_on_slope;
    private float slope_down_angle_old;
    private float slope_side_angle;
    public PhysicsMaterial2D nofriction;
    public PhysicsMaterial2D friction;



    private void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();
        colliderSize = cc.size;
    }





    // Update is called once per frame
    void FixedUpdate()
    {
        move();
        SlopeCheck();

    }

    void Update()
    {
        checkground();
        jump();

    }




private void checkground()
    {
        is_grounded = Physics2D.OverlapCircle(ground_check.position, check_radius, what_is_ground);

        if (is_grounded && rb.velocity.y <= 0.01f)
        {
            canjump = true;
        }
    }

    private void move()
    {
        is_grounded = Physics2D.OverlapCircle(ground_check.position, check_radius, what_is_ground);



        move_input = Input.GetAxis("Horizontal");

        rb.velocity = new Vector2(move_input * speed, rb.velocity.y);
        
        if (is_grounded && !is_on_slope && !is_jumping)
        {
            rb.velocity = new Vector2(move_input * speed, 0.0f);
            ismoving = true;
        }
        else if (is_grounded && is_on_slope && !is_jumping)
        {
            rb.velocity = new Vector2(speed * slope_normal_perp.x * - move_input, speed * slope_normal_perp.y * -move_input);
            ismoving = true;
        }
        else if (!is_grounded)
        {
            rb.velocity = new Vector2(move_input * speed, rb.velocity.y);
            ismoving = true;
        }

        if (move_input == 0)
        {
            anim.SetBool("isRunning", false);
        }
        else
        {
            anim.SetBool("isRunning", true);
        }

        if (facingRight == false && move_input > 0)
        {
            flip();
        }
        else if (facingRight == true && move_input < 0)
        {
            flip();
        }
    }


    private void jump()
    {
        if (is_grounded && rb.velocity.y <= 0.01f)
        {
            has_jumped = false;
            canjump = true;
        }

        if (canjump && Input.GetKeyDown(KeyCode.Space))
        {
            anim.SetTrigger("takeoff");
            is_jumping = true;
            jump_time_counter = jump_time;
            rb.velocity = Vector2.up * jump_force;
            canjump = false;
            has_jumped = true;
        }
        else if (Input.GetKey(KeyCode.Space) && is_jumping && jump_time_counter > 0)
        {
            jump_time_counter -= Time.deltaTime;
            rb.velocity = Vector2.up * jump_force;
        }
        else
        {
            is_jumping = false;
            jump_time_counter = 0;
        }

        if (!Input.GetKey(KeyCode.Space))
        {
            has_jumped = false;
        }

        if (is_grounded)
        {
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", false);
            anim.SetBool("landed", true);
        }
        else if (rb.velocity.y < 0 && !is_jumping)
        {
            anim.SetTrigger("playerfell");
            anim.SetBool("isJumping", false);
            anim.SetBool("isFalling", true);
            anim.SetBool("landed", false);
        }
        else
        {
            anim.SetBool("isJumping", true);
            anim.SetBool("isFalling", false);
        }
    }

    private void flip()
    {
        facingRight = !facingRight;

        Vector3 Scaler = transform.localScale;

        Scaler.x *= -1;

        transform.localScale = Scaler;
    }

    private void SlopeCheck()
    {
        Vector2 checkpos = transform.position - new Vector3(0.0f, colliderSize.y / 2);
        SlopeCheckHoriz(checkpos);
        SlopeCheckVert(checkpos);
    }
    private void SlopeCheckHoriz(Vector2 checkpos)
    {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkpos, transform.right, slope_check_distance, what_is_ground);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkpos, -transform.right, slope_check_distance, what_is_ground);

        if (slopeHitFront)
        {
            is_on_slope = true;
            slope_side_angle = Vector2.Angle(slopeHitFront.normal, Vector2.up);
        }
        else if (slopeHitBack)
        {
            is_on_slope = true;
            slope_side_angle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
        }
        else
        {
            slope_side_angle = 0.0f;
            is_on_slope = false;
        }

    }
    private void SlopeCheckVert(Vector2 checkpos)
    {
        RaycastHit2D hit = Physics2D.Raycast(checkpos, Vector2.down, slope_check_distance, what_is_ground);

        if (hit)
        {
            slope_normal_perp = Vector2.Perpendicular(hit.normal).normalized;

            slope_down_angle = Vector2.Angle(hit.normal, Vector2.up);

            if (slope_down_angle != slope_down_angle_old)
            {
                is_on_slope = true;
            }

            slope_down_angle_old = slope_down_angle;

            Debug.DrawRay(hit.point, slope_normal_perp, Color.red);
            Debug.DrawRay(hit.point, hit.normal, Color.green);
        }


        if (is_on_slope && move_input == 0.0f)
        {
            rb.sharedMaterial = friction;
        }
        else
        {
            rb.sharedMaterial = nofriction;
        }

    }

}
