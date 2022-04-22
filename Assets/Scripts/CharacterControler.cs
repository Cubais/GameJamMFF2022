using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControler : MonoBehaviour
{
    public float maxSpeed = 5f;
    public Rigidbody2D rigidbody;

    Vector2 movement;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");
    }

    void FixedUpdate()
    {
        rigidbody.MovePosition(rigidbody.position + movement * maxSpeed * Time.fixedDeltaTime);
    }
}
