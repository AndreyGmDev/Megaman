using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : MonoBehaviour
{
    public Animator anim; // Referência ao Animator do Drone.
    public ParticleSystem shoot;
    public SpriteRenderer shootSprite;
    public Rigidbody2D rdb; // Referência ao Rigidbody2D do Drone.
    public Collider2D colliderTrigger;
    GameObject targetPlayer;// Referência ao GameObject do Player.
    
    [SerializeField] float delay, speed;
    float countDelay;
    bool attack;
    float colorAlpha = 1;
    Vector3 startPostion;

    void Start()
    {
        startPostion = transform.position; // Pega a posição inicial do Drone.
    }
    void Update()
    {
        ShootDelay();
        Detect();
    }

    void FixedUpdate()
    {
        if (anim.GetBool("SawPlayer")) Move(); // Move o Drone em direção ao Player, caso o Player entre em sua área.
        else transform.position += (startPostion - transform.position) * Time.fixedDeltaTime; // Volta o Drone para posição inicial, caso o Player saia de sua área ou morra.
    }

    // Tempo entre os ataques do Drone.
    void ShootDelay()
    {
        if (countDelay < 0)
            attack = true;
        else
            countDelay -= Time.deltaTime;

        // Mostra visualmente na cena o ataque do Drone carregado.
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
                anim.SetBool("Attack", true); // Inicia a animação de ataque se o Player estiver colidindo com o Raycast e o cooldown do ataque tiver zerado.
            }
            else
            {
                anim.SetBool("Attack", false);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Seta o Player como alvo do Drone.
        if (collision.CompareTag("Player"))
        {
            targetPlayer = collision.gameObject;
            anim.SetBool("SawPlayer", true);
        }
        
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        // Seta null como alvo do Drone.
        if (collision.CompareTag("Player") && !colliderTrigger.IsTouching(collision)) // Verifica se o ColliderTrigger do Drone está collidindo com o Player.
        {
            targetPlayer = null;
            anim.SetBool("SawPlayer", false);
        }
    }

    // Move o Drone para o Player.
    void Move()
    {
        rdb.AddForce(new Vector3(targetPlayer.transform.position.x - transform.position.x, 0, 0) * Time.fixedDeltaTime * speed);
    }

    // Função ataque é chamada em um frame da animação de ataque.
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
