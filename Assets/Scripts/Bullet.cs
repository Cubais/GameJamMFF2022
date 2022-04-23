using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rigidbody;
    public float damage = 20f;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = transform.right * speed;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        NPCControler npc = collision.GetComponent<NPCControler>();
        if (npc != null)
        {
            npc.TakeDamage(damage);
        }

        Destroy(gameObject);
    }
}
