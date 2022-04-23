using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControler : MonoBehaviour, ICharacterController
{
    public const float PUNCH_ANIM_LENGHT = 0.5f;
    public const float THROW_ANIM_LENGHT = 0.8f;
    public const float THROW_OFFSET_ANIM_LENGHT = 0.7f;
    public const float RADIO_ANIM_LENGHT = 1.1f;

    [Header("Character Controler Params")]
    public float maxSpeed = 5f;
    public float meleeAttackDamage = 40f;
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
    public bool hasFrisbee = true;

    [Header("Health Bar")]
    public HealthBar healthSlider;

    private Vector2 movement;
    private bool rangeAttack = false;
    private bool radioAttack = false;
    private bool meleeAttack = false;

    private float currentHealth;
    private bool inAttack;

    private Animator animator;
    private Rigidbody2D rb;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        healthSlider.SetMaxSlidetValue(currentHealth);
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        // Flipping character
        var localScale = transform.localScale;
        if (movement.x != 0f)
            localScale.x = (movement.x < 0f) ? transform.localScale.y : -transform.localScale.y;

        transform.localScale = localScale;
        
        meleeAttack = Input.GetMouseButtonDown(0);
        rangeAttack = Input.GetMouseButtonDown(1);
        radioAttack = Input.GetKeyDown(KeyCode.Space);

        if (rangeAttack)
        {
            RangeAttack(rangeAttackDamage);

            if (hasFrisbee)
                hasFrisbee = false;
        }

        if (meleeAttack)
            MeleeAttack(meleeAttackDamage);

        if (radioAttack && !inAttack)
        {            
            StartCoroutine(PerformRadioAttackAsync(RADIO_ANIM_LENGHT));
        }

        UpdateAnimator();
    }

    void FixedUpdate()
    {
        if (!inAttack)
            Move(false);
    }

    private void UpdateAnimator()
    {
        animator.SetBool("Walking", movement != Vector2.zero && !inAttack);
        animator.SetBool("MeleeAttack", meleeAttack);
        animator.SetBool("RangeAttack", rangeAttack);        
    }

    private IEnumerator PerformRadioAttackAsync(float totalTime)
    {
        var time = 0f;
        var currentTime = Time.realtimeSinceStartup;
        animator.SetBool("RadioAttack", true);
        inAttack = true;

        while (Input.GetKey(KeyCode.Space) && time < totalTime)
        {
            time += (Time.realtimeSinceStartup - currentTime);
            currentTime = Time.realtimeSinceStartup;            
            yield return null;
        }

        if (time >= totalTime)
        {
            RadioAttack();
        }

        animator.SetBool("RadioAttack", false);
        inAttack = false;
    }

    private IEnumerator ResetIsInAttackAsync(float time)
    {
        yield return new WaitForSeconds(time);

        inAttack = false;
    }

    private IEnumerator ThrowFrisbeeAsync(float time)
    {
        yield return new WaitForSeconds(time);

        var bullet = Instantiate(frisbeePrafab, shootPoint.position, shootPoint.rotation).GetComponent<Bullet>();
        bullet.Shoot((transform.localScale.x < 0) ? transform.right : -transform.right);
    }

    public void Move(bool follow)
    {
        rb.MovePosition(rb.position + movement * maxSpeed * Time.fixedDeltaTime);
    }

    public void RadioAttack()
    {   
        
    }

    public void MeleeAttack(float damage)
    {
        if (inAttack)
            return;

        inAttack = true;
        StartCoroutine(ResetIsInAttackAsync(PUNCH_ANIM_LENGHT));

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
        if (inAttack || !hasFrisbee)
            return;

        inAttack = true;
        StartCoroutine(ResetIsInAttackAsync(THROW_ANIM_LENGHT));
        StartCoroutine(ThrowFrisbeeAsync(THROW_OFFSET_ANIM_LENGHT));
    }

    public void RadioAttack(float damage)
    {
        if (inAttack)
            return;

        inAttack = true;
        StartCoroutine(ResetIsInAttackAsync(RADIO_ANIM_LENGHT));
        throw new System.NotImplementedException();
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthSlider.SetSliderValue(currentHealth);

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log("PLAYER DEAD");
    }
}
