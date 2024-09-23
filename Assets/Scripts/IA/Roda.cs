using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Roda : MonoBehaviour
{
    public Animator anim;
    public Rigidbody2D rb;
    public GameObject wheelOut; // GameObject da parte de fora da Roda.
    [SerializeField] float speed;
    bool attack;
    public enum RodaState {walkR, walkL, stop};
    public RodaState rodaState;

// Start is called before the first frame update
void Start()
    {
        rodaState = RodaState.walkR;
    }

    // Update is called once per frame
    void Update()
    {
        DetectPlayer();
        print(rb.velocity.x);
        switch (rodaState)
        {
            case RodaState.walkR:
                DetectGroundRight();
                MoveRight();
                break;
            case RodaState.walkL:
                DetectGroundLeft();
                MoveLeft();
                break;
            case RodaState.stop:
                DetectGroundRight();
                DetectGroundLeft();
                break;
        };
        
    }

    private void DetectPlayer()
    {
        // Faz um raycast para identificar se o player está na frente do inimigo.
        RaycastHit2D sawPlayerFront;
        sawPlayerFront = Physics2D.Raycast(transform.position + transform.right * 0.5f + Vector3.up * 0.4f, transform.right, 6.5f);
        if (sawPlayerFront)
        {
            if (sawPlayerFront.collider.CompareTag("Player") && attack)
            {   
                anim.SetBool("Attack", true);
            }
        }

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
                rodaState = RodaState.walkL;
            }
        }

        // Faz um raycast para identificar se o player está atrás do inimigo.
        RaycastHit2D sawWall;
        sawWall = Physics2D.Raycast(transform.position + Vector3.right * 0.3f, Vector3.right);
        Debug.DrawLine(transform.position + Vector3.right * 0.3f, sawWall.point, Color.yellow);
        if (sawWall)
        {
            if (sawWall.distance <= 0.1f)
            {
                rodaState = RodaState.walkL;
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
                rodaState = RodaState.walkR;
            }
        }

        // Faz um raycast para identificar se o player está atrás do inimigo.
        RaycastHit2D sawWall;
        sawWall = Physics2D.Raycast(transform.position - Vector3.right * 0.3f, Vector3.left);
        Debug.DrawLine(transform.position - Vector3.right * 0.3f, sawWall.point, Color.yellow);
        if (sawWall)
        {
            if (sawWall.distance <= 0.1f)
            {
                rodaState = RodaState.walkR;
            }
        }
    }

    private void MoveRight()
    {
        rb.velocity = new Vector2(speed, 0) * Time.deltaTime;
        transform.Rotate(-transform.forward, (speed / 1.5f) * Time.deltaTime);

    }
    private void MoveLeft()
    {
        rb.velocity = new Vector2(-speed, 0) * Time.deltaTime;
        transform.Rotate(transform.forward, (speed / 1.5f) * Time.deltaTime);
    }

}
