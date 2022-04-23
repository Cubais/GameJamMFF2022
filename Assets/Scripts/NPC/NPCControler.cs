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
    public float meleeAttackDamage = 5f;
    public float rangeAttackDamage = 5f;

    [Header("Distances Params")]
    public float meleeDistance = 3f;
    public float rangeDistance = 2f;
    public float rangeRunAmount = 10f;

    [Header("Type of NPC")]
    public NPCTypeEnum npcTypeEnum;

    [Header("Health")]
    public int maxHealth = 100;

    [Header("Range Combat")]
    public Transform shootPoint;
    public GameObject firePrefab;
    public GameObject rangeHitEffect;

    [Header("Melee Combat")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemiesLayers;

    [Header("Animations length")]
    [SerializeField] private float rangeAttackLenght;
    [SerializeField] private float meleeAttackLenght;

    private Vector2 movement;
    private Vector2 oldPost;
    private bool rangeAttack = false;
    private bool radioAttack = false;
    private bool meleeAttack = false;
    private bool npcType = false;
    private float currentHealth;

    private Transform player;
    private Animator animator;
    private bool inAttack = false;
    private bool isWalking = false;
    private bool movingRight = false;

    void Start()
    {
        animator = GetComponent<Animator>();
        player = GameManager.instance.playerCharacter.transform;
        npcType = npcTypeEnum == NPCTypeEnum.Melee ? true : false;
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        oldPost = transform.position;

        /*if (rangeAttack)
            RangeAttack(rangeAttackDamage);*/

        /*if (meleeAttack)
            MeleeAttack(meleeAttackDamage);*/

        UpdateAnimator();
    }

    void FixedUpdate()
    {
        if (!inAttack)
            Move(npcType);
    }

    private void UpdateAnimator()
    {
        animator.SetBool("Walking", isWalking && !inAttack);
        animator.SetBool("MeleeAttack", meleeAttack);
    }

    private IEnumerator ResetIsInAttackAsync(float time)
    {
        yield return new WaitForSeconds(time);

        inAttack = false;
    }

    public void Move(bool follow)
    {
        Vector2 newPost = player.position;
        if (follow)
        {
            if (Vector2.Distance(transform.position, player.position) > meleeDistance)
            {
                isWalking = true;
                meleeAttack = false;
                transform.position = Vector2.MoveTowards(transform.position, newPost, maxSpeed * Time.fixedDeltaTime);
            }
            else
            {
                meleeAttack = true;
                isWalking = false;
                MeleeAttack(meleeAttackDamage);
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, player.position) < rangeDistance)
            {
                isWalking = true;
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
            else
            {
                isWalking = false;
                RangeAttack(rangeAttackDamage);
            }

            if (Mathf.Abs(transform.position.y - player.position.y) > 0.05f)
            {

                newPost = player.position;
                if (transform.position.y - player.position.y > 0)
                {
                    newPost.y -= rangeRunAmount;
                }
                else
                {
                    newPost.y += rangeRunAmount;
                }

                isWalking = true;
                transform.position = Vector2.MoveTowards(transform.position, newPost, maxSpeed * Time.fixedDeltaTime);
            }
        }

        var localScale = transform.localScale;
        if (isWalking)
            localScale.x = (transform.position.x < newPost.x) ? transform.localScale.y : -transform.localScale.y;

        transform.localScale = localScale;
    }

    public void MeleeAttack(float damage)
    {
        if (inAttack)
            return;

        inAttack = false;
        StartCoroutine(ResetIsInAttackAsync(meleeAttackLenght));

        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemiesLayers);

        if (enemies.Length > 0)
        {
            foreach (Collider2D enemy in enemies)
            {
                enemy.GetComponent<CharacterControler>().TakeDamage(damage);
            }
        }

        Debug.Log("Melee");
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }

    public void RangeAttack(float damage)
    {
        if (inAttack || rangeAttack)
            return;

        inAttack = true;
        animator.SetBool("RangeAttack", true);
        if (firePrefab != null)
        {
            PrefabRangeAttack(damage);
            rangeAttack = true;
        }
        else
        {
            rangeAttack = true;
            StartCoroutine(ShootCastAsynch(1.25f));
        }

        StartCoroutine(ResetIsInAttackAsync(rangeAttackLenght));

        //Instantiate(firePrefab, shootPoint.position, shootPoint.rotation);        
    }

    void PrefabRangeAttack(float damage)
    {
        
    }

    IEnumerator ShootCastAsynch(float circleCastWait)
    {
        yield return null;
        animator.SetBool("RangeAttack", false);
        yield return new WaitForSeconds(circleCastWait);
        RaycastRangeAttack();

        yield return new WaitForSeconds(Random.Range(1, 5));
        rangeAttack = false;
    }

    void RaycastRangeAttack()
    {
        RaycastHit2D hitInfo = Physics2D.CircleCast(shootPoint.position, 0.5f,shootPoint.right);

        if (hitInfo)
        {
            CharacterControler player = hitInfo.transform.GetComponent<CharacterControler>();
            Debug.Log(hitInfo.transform.name);
            if (player != null)
            {
                player.TakeDamage(rangeAttackDamage);
                Debug.Log("RANGE HIT");

                if (rangeHitEffect)
                {
                    Instantiate(rangeHitEffect, hitInfo.point, Quaternion.identity);
                }
            }
        }
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
