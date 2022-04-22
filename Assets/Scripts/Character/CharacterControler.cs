using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControler : MonoBehaviour, ICharacterController
{
    public float maxSpeed = 5f;
    public Rigidbody2D rigidbody;
    public float meeleyAttackDamage = 5f;
    public float rangeAttackDamage = 5f;

    Vector2 movement;
    bool rangeAttack = false;
    bool radioAttack = false;
    bool meeleyAttack = false;

    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxis("Horizontal");
        movement.y = Input.GetAxis("Vertical");

        if (Input.GetMouseButtonDown(0))
        {
            meeleyAttack = true;
        }
        else if (Input.GetMouseButtonUp(0))
        {
            meeleyAttack = false;
        }

        if (Input.GetMouseButtonDown(1))
        {
            rangeAttack = true;
        }
        else if (Input.GetMouseButtonUp(1))
        {
            rangeAttack = false;
        }
    }

    void FixedUpdate()
    {
        Move();

        if (rangeAttack)
            RangeAttack(rangeAttackDamage);

        if (meeleyAttack)
            MeleeAttack(meeleyAttackDamage);

    }

    public void Move()
    {
        rigidbody.MovePosition(rigidbody.position + movement * maxSpeed * Time.fixedDeltaTime);
    }

    public void MeleeAttack(float damage)
    {
        Debug.Log("MeeleyAttack");
    }

    public void RangeAttack(float damage)
    {
        Debug.Log("RangeAttack");
    }

    public void RadioAttack(float damage)
    {
        throw new System.NotImplementedException();
    }
}
