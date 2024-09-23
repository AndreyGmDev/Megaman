using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Damage : MonoBehaviour
{
    [SerializeField]
    private int lives = 4;

    int initiallives;
    [SerializeField]
    ParticleSystem smoke;
    [SerializeField]
    ParticleSystem explosion;
    [SerializeField] Vector3 smokePosition;
    public Collider2D colliderDamage;

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
        // Caso a particula com a Tag BulletPlayer acerte o Inimigo a fun��o CallDamage � chamada.
        if (particle.CompareTag("BulletPlayer")) CallDamage(2);
    }

    public void DamageSword()
    {
        // Caso o Inimigo estiver dentro da area de alcance da espada a fun��o CallDamage � chamada.
        CallDamage(4);

    }

    // Fun��o que causa dano ao Inimigo.
    void CallDamage(int damage)
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