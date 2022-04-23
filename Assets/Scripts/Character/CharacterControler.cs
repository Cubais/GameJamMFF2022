using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControler : MonoBehaviour, ICharacterController
{
    [Header("Character Controler Params")]
    public float maxSpeed = 5f;
    public float meeleyAttackDamage = 40f;
    public float rangeAttackDamage = 5f;

    [Header("Melee Combat")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemiesLayers;

    [Header("Health")]
    public float maxHealth = 100;

    [Header("Range Combat")]
    public Transform shootPoint;
    public GameObject frisbeePrafab;

    private Vector2 movement;
    private bool rangeAttack = false;
    private bool radioAttack = false;
    private bool meleeAttack = false;

    private float currentHealth;

    private Animator animator;
    private Rigidbody2D rb;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Flipping character
        var localScale = transform.localScale;
        localScale.x = (movement.x < 0f) ? transform.localScale.y : -transform.localScale.y;
        transform.localScale = localScale;
        
        meleeAttack = Input.GetMouseButtonDown(0);
        rangeAttack = Input.GetMouseButtonDown(1);        

        //UpdateAnimator();
    }

    void FixedUpdate()
    {
        Move(false);

        if (rangeAttack)
            RangeAttack(rangeAttackDamage);

        if (meleeAttack)
            MeleeAttack(meeleyAttackDamage);
    }

    private void UpdateAnimator()
    {
        animator.SetBool("Walking", movement != Vector2.zero);
        animator.SetBool("MeleeAttack", meleeAttack);
        animator.SetBool("RangeAttack", rangeAttack);
    }

    public void Move(bool follow)
    {
        rb.MovePosition(rb.position + movement * maxSpeed * Time.fixedDeltaTime);
    }

    public void MeleeAttack(float damage)
    {
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemiesLayers);

        if (enemies.Length > 0)
        {
            foreach (Collider2D enemy in enemies)
            {
                enemy.GetComponent<NPCControler>().TakeDamage(damage);
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
        Instantiate(frisbeePrafab, shootPoint.position, shootPoint.rotation);
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
