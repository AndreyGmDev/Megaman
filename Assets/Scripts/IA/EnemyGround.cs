using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGround : MonoBehaviour
{
    public Animator anim;
    public ParticleSystem shoot;
    [SerializeField] int life;

    // Update is called once per frame
    void Update()
    {
        Detect(); // Identifica quando o player est� na vis�o do inimigo.
    }

    private void Detect()
    {
        // Faz um raycast para identificar se o player est� na frente do inimigo.
        RaycastHit2D sawPlayerFront;
        sawPlayerFront = Physics2D.Raycast(transform.position + transform.right * 0.5f + Vector3.up * 0.4f, transform.right, 10);
        if (sawPlayerFront)
        {
            if (sawPlayerFront.collider.CompareTag("Player"))
            {
                // anim.SetBool("Attack",true);
                Attack();
            }
        }

        // Faz um raycast para identificar se o player est� atr�s do inimigo.
        RaycastHit2D sawPlayerBack;
        sawPlayerBack = Physics2D.Raycast(transform.position - transform.right * 0.5f + Vector3.up * 0.4f, -transform.right, 10);
        if (sawPlayerBack)
        {
            if (sawPlayerBack.collider.CompareTag("Player"))
            {
                transform.rotation = Quaternion.Euler(0, (transform.rotation.eulerAngles.y + 180) % 360, 0); // Rotaciona o inimigo para a dire��o do player.
            }
        }
    }

    private void Attack()
    {
        shoot.Emit(1);
        // anim.SetBool("Attack",false);
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
