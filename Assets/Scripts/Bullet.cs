using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rigidbody;
    public float damage = 20f;

    public void Shoot(Vector2 direction)
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = direction * speed;
    }

    private IEnumerator FallAsynch(Vector2 groud)
    {
        Debug.Log(transform.position.y - groud.y);
        while (transform.position.y - groud.y > 0.05f)
        {
            transform.position = Vector2.MoveTowards(transform.position, groud, 3 * Time.fixedDeltaTime);         
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);

        NPCControler npc = collision.GetComponent<NPCControler>();
        if (npc != null)
        {
            npc.TakeDamage(damage);
        }

        CharacterControler player = collision.GetComponent<CharacterControler>();
        if (player != null)
        {
            player.hasFrisbee = true;
            Destroy(gameObject);
        }

        rigidbody.velocity *= -1 / 15;
        Vector2 newPosition = transform.position - new Vector3(0, 1f, 0f);
        rigidbody.gravityScale = 0;
        StartCoroutine(FallAsynch(newPosition));
    }
}
