using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuNPC : MonoBehaviour
{
    private Animator animator;
    private Rigidbody2D rb;
    private Vector2 xBounds = new Vector2(2f, 6f);
    private Vector2 yBounds = new Vector2(-1f, -3.8f);

    private Vector3 currentDir;
    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        GetComponent<CapsuleCollider2D>().isTrigger = true;
        transform.Find("HealthBarCanvas").gameObject.SetActive(false);

        currentDir = ChangeDirection();
        animator.SetBool("Walking", true);
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.x < xBounds.x || transform.position.x > xBounds.y || transform.position.y > yBounds.x || transform.position.y < yBounds.y)
        {
            currentDir = ChangeDirection();           
        }

        rb.MovePosition(transform.position + currentDir * Time.deltaTime * 5f);
    }

    Vector3 ChangeDirection()
    {
        var xMin = (transform.position.x < xBounds.x) ? 0f : -1f;
        var xMax = (transform.position.x > xBounds.y) ? 0f : 1f;
        var yMax = (transform.position.y > yBounds.x) ? 0f : 1f;
        var yMin = (transform.position.y < yBounds.y) ? 0f : -1f;

        var dir = new Vector2(Random.Range(xMin, xMax), Random.Range(yMin, yMax));

        var scale = transform.localScale;
        scale.x = (currentDir.x < 0) ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
        transform.localScale = scale;

        return dir.normalized;
    }

    private void OnDisable()
    {
        transform.Find("HealthBarCanvas").gameObject.SetActive(true);
        GetComponent<CapsuleCollider2D>().isTrigger = false;
    }
}
