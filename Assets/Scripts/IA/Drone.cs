using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    public Animator anim; // Referência ao Animator do Drone.
    public ParticleSystem shoot;
    public SpriteRenderer shootSprite;
    public Rigidbody2D rdb; // Referência ao Rigidbody2D do Drone.
    public Collider2D colliderTrigger, colliderDamage;
    GameObject targetPlayer;// Referência ao GameObject do Player.
    
    [SerializeField] float delay;
    float countDelay;
    bool attack;
    float colorAlpha = 1;
    Vector3 startPostion;

    void Start()
    {
        startPostion = transform.position; 
    }
    void Update()
    {
        ShootDelay();
        Detect();
    }

    void FixedUpdate()
    {
        if (anim.GetBool("SawPlayer")) Move();
    }

    void ShootDelay()
    {
        if (countDelay < 0)
            attack = true;
        else
            countDelay -= Time.deltaTime;

        colorAlpha += Time.deltaTime*0.5f;
        colorAlpha = Mathf.Clamp(colorAlpha,0,1);
        shootSprite.color = new Color(shootSprite.color.r, shootSprite.color.g, shootSprite.color.b, colorAlpha);
    }

    void Detect()
    {
        // Faz um raycast para identificar se o player está na frente do inimigo.
        RaycastHit2D sawPlayerDown;
        sawPlayerDown = Physics2D.Raycast(transform.position - transform.up * 0.5f, -transform.up);
        Debug.DrawLine(transform.position - transform.up * 0.5f, sawPlayerDown.point);
        if (sawPlayerDown)
        {
            if (sawPlayerDown.collider.CompareTag("Player") && attack==true)
            {
                anim.SetBool("Attack", true);
            }
            else
            {
                anim.SetBool("Attack", false);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            targetPlayer = collision.gameObject;
            anim.SetBool("SawPlayer", true);
        }
        
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        // Verifica se o ColliderTrigger está collidindo com o Player.
        if (collision.CompareTag("Player") && !colliderTrigger.IsTouching(collision))
        {
            targetPlayer = null;
            anim.SetBool("SawPlayer", false);
        }
    }

    void Move()
    {
        rdb.AddForce(new Vector3(targetPlayer.transform.position.x - transform.position.x, 0, 0) * Time.fixedDeltaTime * 90);
        //if(targetPlayer.transform.position.x>transform.position.x) transform.position += Vector3.right * Time.fixedDeltaTime * 2.5f;
        //else if (targetPlayer.transform.position.x < transform.position.x) transform.position -= Vector3.right * Time.fixedDeltaTime * 2.5f;
    }

    void Attack()
    {
        shootSprite.color = new Color(shootSprite.color.r, shootSprite.color.g, shootSprite.color.b, 0);
        colorAlpha = 0;
        shoot.Emit(1);
        countDelay = delay;
        attack = false;
        anim.SetBool("Attack", false);
    }
}
