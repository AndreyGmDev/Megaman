using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPlayer : MonoBehaviour
{
    public Animator anima; // Referência ao Animator do personagem.
    float xmov; // Variável para guardar o movimento horizontal.
    public Rigidbody2D rdb; // Referência ao Rigidbody2D do personagem.
    bool jump, doubleJump, jumpAgain; // Flags para controle de pulo e pulo duplo.
    float jumpTime, jumpTimeSide; // Controla a duração dos pulos.
    public ParticleSystem fire; // Sistema de partículas para o efeito de fogo.

    void Start()
    {
        // Método para inicializações. 
        jumpAgain = true;
    }

    void Update()
    {
        // Captura o movimento horizontal do jogador.
        xmov = Input.GetAxis("Horizontal"); 

        // Verifica se o botão de pulo foi pressionado e controla o pulo duplo.
        if (Input.GetButtonDown("Jump"))
            doubleJump = true;

        if (Input.GetButtonUp("Jump"))
            jumpAgain = true;
        
        // Define o estado de pulo com base na entrada do usuário.
        if (Input.GetButton("Jump") && jumpAgain)
        {
            jump = true;
        }
        else
        {
            jump = false;
            doubleJump = false;
            jumpTime = 0;
            jumpTimeSide = 0;
        }

        // Desativa o estado de "Fire" no Animator.
        anima.SetBool("Fire", false);

        // Ativa o efeito de fogo e ativa o estado "Fire" no Animator quando o botão de fogo é pressionado.
        if (Input.GetButtonDown("Fire1"))
        {
            fire.Emit(1);
            anima.SetBool("Fire", true);
        }

        PhisicalReverser(); // Chama a função que inverte o personagem.
    }

    void FixedUpdate()
    {
        anima.SetFloat("Velocity", Mathf.Abs(xmov)); // Define a velocidade no Animator.
        rdb.AddForce(new Vector2(xmov * 20 / (rdb.velocity.magnitude + 1), 0)); // Adiciona uma força para mover o personagem.

        // Faz um raycast para baixo para detectar o chão para a animação de Pulo.
        RaycastHit2D hit;
        hit = Physics2D.Raycast(transform.position - new Vector3(0, 0.5f, 0), Vector2.down);
        
        
        if (hit)
        {
            anima.SetFloat("Height", hit.distance);
            if (jumpTimeSide < 0.1)
                JumpRoutine(hit); // Chama a rotina de pulo.
        }

        // Faz um raycast para a direita para detectar paredes.
        /*RaycastHit2D hitright;
        hitright = Physics2D.Raycast(transform.position + Vector3.up * 0.5f, transform.right, 1);
        if (hitright)
        {
            if (hitright.distance < 0.3f && hit.distance > 0.5f)
            {
                JumpRoutineSide(hitright); // Chama a rotina de pulo lateral.
            }
            Debug.DrawLine(hitright.point, transform.position + Vector3.up * 0.5f);
        }*/
    }

    // Rotina de pulo (parte física).
    private void JumpRoutine(RaycastHit2D hit)
    {
        // Verifica a distância do chão e aplica uma força de pulo se necessário.
        if (hit.distance < 0.1f)
        {
            jumpTime = 1;
        }

        if (jump)
        {
            jumpTime = Mathf.Lerp(jumpTime, 0, Time.fixedDeltaTime * 10);
            rdb.AddForce(Vector2.up * jumpTime, ForceMode2D.Impulse);

            // Impedir de pular enquanto segura a tecla de pulo
            if (rdb.velocity.y < 0)
            {
                jumpAgain = false;
            }
        }

    }

    // Rotina de pulo lateral.
    private void JumpRoutineSide(RaycastHit2D hitside)
    {
        jumpTimeSide = 6;

        if (doubleJump)
        {
            // PhisicalReverser();
            jumpTimeSide = Mathf.Lerp(jumpTimeSide, 0, Time.fixedDeltaTime * 10);
            rdb.AddForce((hitside.normal + Vector2.up) * jumpTimeSide, ForceMode2D.Impulse);
        }
    }


    // Função para inverter a direção do personagem.
    void PhisicalReverser()
    {
        if (rdb.velocity.x > 0.1f) transform.rotation = Quaternion.Euler(0, 0, 0);
        if (rdb.velocity.x < -0.1f) transform.rotation = Quaternion.Euler(0, 180, 0);
    }

    // Detecção de colisão com objetos marcados com a tag "Damage" ou "Enemy".
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Damage") || collision.collider.CompareTag("Enemy"))
        {
            LevelManager.instance.LowDamage(); // Chama a função para aplicar dano.
        }
    }
}
