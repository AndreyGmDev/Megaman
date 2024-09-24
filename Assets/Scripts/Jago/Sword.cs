using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private List<Collider2D> detectedEnemies = new List<Collider2D>(); // Inimigos que entraram na area da espada
    public Collider2D swordArea; // Aréa de alcance da espada
    public Animator anim;
    float attackCooldown;
    private void Update()
    {
        attackCooldown -= Time.deltaTime;

        anim.SetBool("SwordAttack", false);

        if (Input.GetKeyDown(KeyCode.Mouse1) && attackCooldown<0)
        {
            if (!anim.GetBool("Side")) anim.Play("SwordAttack");
            attackCooldown = 1.5f;
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy") && other.IsTouching(swordArea))
        {
            detectedEnemies.Add(other);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            detectedEnemies.Remove(other);
        }     
    }

    public void DamageSword()
    {
        // Aqui você pode aplicar dano ou qualquer outra lógica com os inimigos detectados
        foreach (var enemy in detectedEnemies)
        {
            Collider2D enemyCollider = enemy.GetComponent<Damage>().colliderDamage;
            if (enemyCollider.IsTouching(swordArea))
                enemy.GetComponent<Damage>().DamageSword();
        }
    }
}
