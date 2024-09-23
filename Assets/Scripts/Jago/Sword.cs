using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword : MonoBehaviour
{
    private List<Collider2D> detectedEnemies = new List<Collider2D>(); // Inimigos que entraram na area da espada
    public Collider2D swordArea; // Ar�a de alcance da espada
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
        // Aqui voc� pode aplicar dano ou qualquer outra l�gica com os inimigos detectados
        foreach (var enemy in detectedEnemies)
        {
            Collider2D enemyCollider = enemy.GetComponent<Damage>().colliderDamage;
            if (enemyCollider.IsTouching(swordArea))
                enemy.GetComponent<Damage>().DamageSword();
        }
    }
}
