using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum NPCTypeEnum
{
    Melee,
    Range
}

public class NPCControler : MonoBehaviour, ICharacterController
{
    [Header("Character Controler Params")]
    public float maxSpeed = 5f;
    public float meeleyAttackDamage = 5f;
    public float rangeAttackDamage = 5f;

    [Header("Distances Params")]
    public float meleeDistance = 3f;
    public float rangeDistance = 5f;
    public float rangeRunAmount = 10f;

    [Header("Type of NPC")]
    public NPCTypeEnum npcTypeEnum;

    [Header("Health")]
    public int maxHealth = 100;

    [Header("Range Combat")]
    public Transform shootPoint;
    public GameObject firePrefab;

    Vector2 movement;
    Vector2 oldPost;
    bool rangeAttack = false;
    bool radioAttack = false;
    bool meeleyAttack = false;
    bool npcType = false;
    public float currentHealth;

    Transform player;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        npcType = npcTypeEnum == NPCTypeEnum.Melee ? true : false;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        oldPost = transform.position;
    }

    void FixedUpdate()
    {
        Move(npcType);

        if (rangeAttack)
            RangeAttack(rangeAttackDamage);

        if (meeleyAttack)
            MeleeAttack(meeleyAttackDamage);
    }

    public void Move(bool follow)
    {   
        if (follow)
        {
            if (Vector2.Distance(transform.position, player.position) > meleeDistance)
            {
                transform.position = Vector2.MoveTowards(transform.position, player.position, maxSpeed * Time.fixedDeltaTime);
            }
        }
        else
        {
            Vector2 newPost = player.position;
            if (Vector2.Distance(transform.position, player.position) < rangeDistance)
            {
                if (player.position.y > transform.position.y)
                {
                    if (oldPost.y != newPost.y)
                    {
                        newPost.y -= rangeRunAmount;
                    }
                    else 
                    {
                        newPost.y += rangeRunAmount;
                    }
                }
                else
                {
                    if (oldPost.y != newPost.y)
                    {
                        newPost.y += rangeRunAmount;
                    }
                    else
                    {
                        newPost.y -= rangeRunAmount;
                    }
                }

                newPost.x *= -1;

                transform.position = Vector2.MoveTowards(transform.position, newPost, maxSpeed * Time.fixedDeltaTime);
            }
        }
    }

    public void MeleeAttack(float damage)
    {
        Debug.Log("MeeleyAttack");
    }

    public void RangeAttack(float damage)
    {
        Instantiate(firePrefab, shootPoint.position, shootPoint.rotation);
        Debug.Log("Range");
    }

    public void RadioAttack(float damage)
    {
        throw new System.NotImplementedException();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("DEAD");
    }
}
