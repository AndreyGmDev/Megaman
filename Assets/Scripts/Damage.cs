using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField]
    private int lives=4;

    int initiallives;
    [SerializeField]
    ParticleSystem smoke;
    [SerializeField]
    ParticleSystem explosion;
    [SerializeField] Vector3 smokePosition;

    // Start is called before the first frame update
    void Start()
    {
        initiallives = lives;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnParticleCollision(GameObject particle)
    {
        CallDamage(particle,null,2);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        CallDamage(null,collision,4);
    }

    // Função que causa dano ao Inimigo.
    void CallDamage(GameObject particle, Collision2D collision, int damage)
    {
        // Verifica qual é o responsável pela colisão com o Inimigo.
        bool isBulletPlayer = particle != null && particle.CompareTag("BulletPlayer"); 
        bool isSwordPlayer = collision != null && collision.collider.CompareTag("SwordPlayer");

        // Se for o tiro ou a espada do Player, o Inimigo recebe dano.
        if (isBulletPlayer || isSwordPlayer)
        {
            StartCoroutine(Blink());

            lives -= damage;
            if (lives < initiallives / 2)
            {
                CreateandPlay(smoke);
            }

            if (lives < 1)
            {
                CreateandPlay(explosion);

                SpriteRenderer renderer = GetComponent<SpriteRenderer>();
                if (!renderer)
                {
                    renderer = GetComponentInChildren<SpriteRenderer>();
                }
                renderer.enabled = false;
                Destroy(gameObject, 0.8f);
            }
        }
    }

    /// <summary>
    /// cria uma particula e liga ela
    /// </summary>
    /// <param name="particle"> colocar aqui a referencia da particula (prefab)</param>
    void CreateandPlay(ParticleSystem particle)
    {
        if (particle)
        {
            GameObject ob = Instantiate(particle.gameObject, transform.position + smokePosition, Quaternion.identity);
            ob.transform.parent = gameObject.transform;
            ob.GetComponent<ParticleSystem>().Play();
        }
    }

    IEnumerator Blink()
    {

        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        if (!renderer)
        {
            renderer = GetComponentInChildren<SpriteRenderer>();
        }
        for (int i = 0; i < 5; i++)
        {
            renderer.color = new Color(1, 0, 0);

            yield return new WaitForSeconds(0.1f);

            renderer.color = new Color(1, 1, 1);

            yield return new WaitForSeconds(0.1f);
        }
    }

    
}
