using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGround : MonoBehaviour
{
    public Animator anim;
    public ParticleSystem shoot;
    [SerializeField] int life;
    [SerializeField] float delay;
    float countDelay;
    bool attack;

    // Update is called once per frame
    void Start()
    {
        countDelay = delay;
    }
    void Update()
    {
        Detect(); // Identifica quando o player está na visão do inimigo.
        ShootDelay();
    }

    void ShootDelay()
    {
        countDelay -= Time.deltaTime;
        if (countDelay < 0)
            attack = true;
    }

    private void Detect()
    {
        // Faz um raycast para identificar se o player está na frente do inimigo.
        RaycastHit2D sawPlayerFront;
        sawPlayerFront = Physics2D.Raycast(transform.position + transform.right * 0.5f + Vector3.up * 0.4f, transform.right, 6.5f);
        if (sawPlayerFront)
        {
            if (sawPlayerFront.collider.CompareTag("Player") && attack)
            {
                anim.SetBool("Attack",true);
            }
        }

        // Faz um raycast para identificar se o player está atrás do inimigo.
        RaycastHit2D sawPlayerBack;
        sawPlayerBack = Physics2D.Raycast(transform.position - transform.right * 0.5f + Vector3.up * 0.4f, -transform.right, 6.5f);
        if (sawPlayerBack)
        {
            if (sawPlayerBack.collider.CompareTag("Player"))
            {
                transform.rotation = Quaternion.Euler(0, (transform.rotation.eulerAngles.y + 180) % 360, 0); // Rotaciona o inimigo para a direção do player.
            }
        }
    }

    private void Attack()
    {
        shoot.Emit(1);
        attack = false;
        countDelay = delay;
        anim.SetBool("Attack",false);
    }

    private void OnParticleCollision(GameObject other)
    {
        if (other.CompareTag("BulletPlayer"))
        {
            life--;
        }
        if (life <= 0)
        {
            Destroy(gameObject);
        }
    }
}
