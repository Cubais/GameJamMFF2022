using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    public Rigidbody2D rigidbody;
    public float damage = 20f;
    GameObject effect;

    public void Shoot(Vector2 direction)
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = direction * speed;
    }

    public void Throw(Vector2 direction, GameObject throwEffect)
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = direction * speed;
        effect = throwEffect;
    }

    public Vector2 Granade(Vector2 direction, GameObject fireEffect, Vector2 playerPosition)
    {
        rigidbody = GetComponent<Rigidbody2D>();
        rigidbody.velocity = direction * 8;
        effect = fireEffect;

        StartCoroutine(GranadeAsynch(playerPosition - new Vector2(0, 1)));

        return playerPosition;
    }

    private IEnumerator GranadeAsynch(Vector2 groud)
    {
        while (Mathf.Abs(transform.position.y - groud.y) > 0.05f)
        {           
            transform.position = Vector2.MoveTowards(transform.position, groud, 3 * Time.fixedDeltaTime);
            yield return null;
        }

        rigidbody.velocity = Vector2.zero;
        rigidbody.gravityScale = 0;

        yield return new WaitForSeconds(0.5f);

        if (effect)
            Instantiate(effect, transform);

        transform.gameObject.GetComponent<SpriteRenderer>().enabled = false;

        yield return new WaitForSeconds(1f);

        Destroy(transform.gameObject);
    }

    private IEnumerator FallAsynch(Vector2 groud)
    {
        Debug.Log(transform.position.y - groud.y);
        while (Mathf.Abs(transform.position.y - groud.y) > 0.05f)
        {
            Debug.Log(transform.position.y - groud.y);
            transform.position = Vector2.MoveTowards(transform.position, groud, 3 * Time.fixedDeltaTime);         
            yield return null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.name);
        BossControler boss = null;
        NPCControler npc = null;

        if (collision.tag == "Boss")
            boss = collision.GetComponent<BossControler>();
        else
            npc = collision.GetComponent<NPCControler>();

        if (npc != null)
        {
            npc.TakeDamage(damage);
            Instantiate(effect, transform);
        }
        else if (boss != null)
        {
            boss.TakeDamage(damage);
            Instantiate(effect, transform);
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
