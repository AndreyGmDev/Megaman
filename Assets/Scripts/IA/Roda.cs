using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Roda : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;
    public GameObject wheelOut; // GameObject da parte de fora da Roda.
    [SerializeField] float minSpeed;
    float speed,speedBoost;
    bool attack;
    public enum RodaState {walkR, walkL, stop, attackR, attackL};
    public RodaState rodaState;

    // Start is called before the first frame update
    void Start()
    {
        rodaState = RodaState.walkR;
        speed = minSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        
        
    }

    private void FixedUpdate()
    {
        print(speed);
        Rotation();
        switch (rodaState)
        {
            case RodaState.walkR:
                anim.SetBool("SawPlayer", false);
                DetectGroundRight();
                Move();
                break;
            case RodaState.walkL:
                anim.SetBool("SawPlayer", false);
                DetectGroundLeft();
                Move();
                break;
            case RodaState.attackR:
                Attack();
                DetectGroundRight();
                break;
            case RodaState.attackL:
                Attack();
                DetectGroundLeft();
                break;
        };
    }

    private void DetectGroundRight()
    {
        // Faz um raycast para identificar se o player está na frente do inimigo.
        RaycastHit2D sawGround;
        sawGround = Physics2D.Raycast(transform.position + Vector3.right * 0.6f, Vector3.down);
        Debug.DrawLine(transform.position + Vector3.right * 0.6f, sawGround.point,Color.red);
        if (sawGround)
        {
            if (sawGround.distance >= 0.4f)
            {
                speed = -minSpeed;
                rodaState = RodaState.walkL;
            }
        }

        // Faz um raycast para identificar se o player está atrás do inimigo.
        RaycastHit2D sawWall;
        sawWall = Physics2D.Raycast(transform.position + Vector3.right * 0.37f, Vector3.right);
        Debug.DrawLine(transform.position + Vector3.right * 0.37f, sawWall.point, Color.yellow);
        if (sawWall)
        {
            if (sawWall.distance <= 0.01f)
            {
                speed = -minSpeed;
                rodaState = RodaState.walkL;
            }
            else if (sawWall.collider.CompareTag("Player"))
            {
                speedBoost = 1;
                rodaState = RodaState.attackR;
            }

        }
    }
    private void DetectGroundLeft()
    {
        // Faz um raycast para identificar se o player está na frente do inimigo.
        RaycastHit2D sawGround;
        sawGround = Physics2D.Raycast(transform.position - Vector3.right * 0.6f, Vector3.down);
        Debug.DrawLine(transform.position - Vector3.right * 0.6f, sawGround.point, Color.red);
        if (sawGround)
        {
            if (sawGround.distance >= 0.4f)
            {
                speed = minSpeed;
                rodaState = RodaState.walkR;
            }
        }

        // Faz um raycast para identificar se o player está atrás do inimigo.
        RaycastHit2D sawWall;
        sawWall = Physics2D.Raycast(transform.position - Vector3.right * 0.37f, Vector3.left);
        Debug.DrawLine(transform.position - Vector3.right * 0.37f, sawWall.point, Color.yellow);
        if (sawWall)
        {
            if (sawWall.distance <= 0.01f)
            {
                speed = minSpeed;
                rodaState = RodaState.walkR;
            }
            else if (sawWall.collider.CompareTag("Player"))
            {
                speedBoost = -1;
                rodaState = RodaState.attackL;
            }

        }
    }

    private void Move()
    {
        rb.velocity = new Vector2(speed, 0) * Time.fixedDeltaTime;
    }

    private void Attack()
    {
        anim.SetBool("SawPlayer", true);
        speed = Mathf.Clamp(speed + speedBoost, -300, 300);
        rb.velocity = new Vector2(speed,0) * Time.fixedDeltaTime;
    }

    private void Rotation()
    {
        if(rb.velocity.x < 0) transform.Rotate(-transform.forward, (speed * 2.5f) * Time.fixedDeltaTime);
        if (rb.velocity.x > 0) transform.Rotate(-transform.forward, (speed * 2.5f) * Time.fixedDeltaTime);
    }
}
