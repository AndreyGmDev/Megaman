using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    public GameObject wheel1,wheel2;
    SpriteRenderer spriteWhell1,spriteWhell2;
    Rigidbody2D rigidbody1,rigidbody2;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody1 = wheel1.GetComponent<Rigidbody2D>();
        rigidbody2 = wheel2.GetComponent<Rigidbody2D>();
        spriteWhell1 = wheel1.GetComponentInChildren<SpriteRenderer>();
        spriteWhell2 = wheel2.GetComponentInChildren<SpriteRenderer>();
        spriteWhell1.enabled = false;
        spriteWhell2.enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        
    }

   

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            spriteWhell1.enabled = true;
            spriteWhell2.enabled = true;
            rigidbody1.drag = 1f;
            rigidbody2.drag = 1f;
        }

    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            spriteWhell1.enabled = false;
            spriteWhell2.enabled = false;
            rigidbody1.drag = 100000f;
            rigidbody2.drag = 100000f;
        }
    }

}
