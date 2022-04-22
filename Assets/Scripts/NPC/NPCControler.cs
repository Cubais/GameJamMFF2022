using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCControler : MonoBehaviour, ICharacterController
{
    public float maxSpeed = 5f;
    public float meeleyAttackDamage = 5f;
    public float rangeAttackDamage = 5f;

    Vector2 movement;
    bool rangeAttack = false;
    bool radioAttack = false;
    bool meeleyAttack = false;

    Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {
        Move();

        if (Vector2.Distance(transform.position, player.position) > 3)
        {
            Move();
        }
        else
        {
            if (rangeAttack)
                RangeAttack(rangeAttackDamage);

            if (meeleyAttack)
                MeleeAttack(meeleyAttackDamage);
        }

    }

    public void Move()
    {
            transform.position = Vector2.MoveTowards(transform.position, player.position, maxSpeed * Time.fixedDeltaTime);  
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
