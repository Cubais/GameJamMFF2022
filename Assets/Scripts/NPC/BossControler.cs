using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossControler : MonoBehaviour
{
    [Header("Character Controler Params")]
    public float maxSpeed = 5f;
    public float meleeAttackDamage = 5f;
    public float rangeAttackDamage = 5f;
    public float flashAttackDamage = 5f;
    public float superAttackDamage = 5f;

    [Header("Distances Params")]
    public float meleeDistance = 3f;
    public float meleeWalkingDistance = 2f;
    public float rangeAvoidDistance = 2f;
    public float rangeRunAmount = 10f;

    [Header("Health")]
    public int maxHealth = 1000;
    public HealthBar healthSlider;

    [Header("Combat Anim Wait")]
    public float animAttackWait = 0.5f;

    [Header("Range Combat")]
    public Transform shootPoint;
    public GameObject rangeHitEffect;
    public GameObject meleeHitEffect;
    public GameObject superHitEffect;

    [Header("Melee Combat")]
    public Transform attackPoint;
    public float attackRange = 0.5f;
    public float flashRange = 10f;
    public float superRange = 20f;
    public LayerMask enemiesLayers;

    [Header("Animations length")]
    [SerializeField] private float rangeAttackLenght;
    [SerializeField] private float meleeAttackLenght;

    [Header("Death")]
    public Transform deathPoint;
    public GameObject deathEffect;

    private Vector2 movement;
    private Vector2 oldPost;
    private bool rangeAttack = false;
    private bool meleeAttack = false;
    private float currentHealth;

    private Transform player;
    private Animator animator;
    private bool inAttack = false;
    private bool isWalking = false;
    private bool movingRight = false;
    private bool flashAttackk = false;
    private bool superAttackk = false;
    private Rigidbody2D rigidbody;
    private bool isFlash = false;
    private int flashAttack;
    private bool superAttack;
    private bool isFlashCorutine = false;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        rigidbody = transform.GetComponent<Rigidbody2D>();
        rigidbody.velocity = Vector2.zero;

        player = GameManager.instance.playerCharacter.transform;
        currentHealth = maxHealth;
        //healthSlider.SetMaxSliderValue(currentHealth);
    }

    // Update is called once per frame
    void Update()
    {
        rigidbody.velocity = Vector2.zero;
        UpdateAnimator();
    }

    private IEnumerator ResetIsInAttackAsync(float time)
    {
        yield return new WaitForSeconds(time);

        inAttack = false;
    }

    private void UpdateAnimator()
    {
        Debug.Log(isWalking && !inAttack);
        animator.SetBool("isWalking", isWalking && !inAttack);
        animator.SetBool("MeleeAttack", meleeAttack);
        animator.SetBool("RangeAttack", rangeAttack);
        animator.SetBool("FlashAttack", flashAttackk);
        animator.SetBool("SuperAttack", superAttackk);
    }

    void FixedUpdate()
    {
        if (!isFlashCorutine)
            StartCoroutine(FlashAsynch());

        if (currentHealth <= 750 || currentHealth <= 500 || currentHealth <= 250 || currentHealth <= 100)
        {
            superAttack = true;
            SuperAttack();
        }
        else if (isFlash)
        {
            FlashAttack();
            isFlash = false;
            flashAttackk = true;
        }
        else if (!inAttack)
            Move();
    }

    IEnumerator FlashAsynch()
    {
        flashAttack = Random.Range(0, 100_000);

        if (flashAttack % 7 == 0)
            isFlash = true;

        isFlashCorutine = true;

        yield return new WaitForSeconds(15);

        isFlashCorutine = false;
    }

    void Move()
    {
        Debug.Log("MOVE");
        Vector2 newPost = player.position;
        var distance = Vector2.Distance(transform.position, player.position);
        //Follow + Melee
        if (distance > meleeDistance && distance < rangeAvoidDistance)
        {
            isWalking = true;
            meleeAttack = false;
            rangeAttack = false;
            superAttack = false;
            transform.position = Vector2.MoveTowards(transform.position, newPost, maxSpeed * Time.fixedDeltaTime);
        }
        else if (distance <= meleeDistance)
        {
            meleeAttack = true;
            rangeAttack = false;
            isWalking = false;
            superAttack = false;
            MeleeAttack();
        }
        else if (Mathf.Abs(transform.position.y - player.position.y) > 1f)
        {

            newPost = new Vector2(transform.position.x, player.position.y + Random.Range(-0.2f, 0.2f));
            meleeAttack = false;
            rangeAttack = false;
            isWalking = true;
            superAttack = false;
            transform.position = Vector2.MoveTowards(transform.position, newPost, maxSpeed * Time.fixedDeltaTime);

            Debug.Log("RANGE ADJUST");
        }
        else
        {
            meleeAttack = false;
            rangeAttack = true;
            isWalking = false;
            superAttack = false;
            RangeAttack();
            Debug.Log("RANGE ATTACK");
        }

        var localScale = transform.localScale;
        if (isWalking)
        {
            localScale.x = (transform.position.x < GameManager.instance.playerCharacter.transform.position.x) ? transform.localScale.y : -transform.localScale.y;
            var rotation = (transform.position.x < GameManager.instance.playerCharacter.transform.position.x) ? 0 : 180;
            shootPoint.eulerAngles = new Vector3(0, shootPoint.eulerAngles.y + rotation, 0);
        }

        transform.localScale = localScale;
    }

    void MeleeAttack()
    {
        if (inAttack || !meleeAttack)
            return;

        inAttack = true;

        StartCoroutine(Melee()); 
        StartCoroutine(ResetIsInAttackAsync(1.5f));
    }

    IEnumerator Melee()
    {
        yield return null;
        animator.SetBool("MeleeAttack", false);
        yield return new WaitForSeconds(1f);
        meleeAttack = false;
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemiesLayers);

        if (enemies.Length > 0)
        {
            enemies[0].GetComponent<CharacterControler>().TakeDamage(meleeAttackDamage);
            yield return StartCoroutine(EffectsAsynch(meleeHitEffect, attackPoint));
        }

        yield return new WaitForSeconds(1.5f);
    }

    IEnumerator EffectsAsynch(GameObject effect, Transform point)
    {
        Instantiate(effect, point);

        yield return new WaitForSeconds(0.3f);

        Instantiate(effect, point);
    }

    void RangeAttack()
    {
        if (inAttack)
            return;

        inAttack = true;

        StartCoroutine(ShootCastAsynch(1f));
        StartCoroutine(ResetIsInAttackAsync(1.5f));
    }

    IEnumerator ShootCastAsynch(float circleCastWait)
    {
        yield return null;
        //rangeAttack = false;
        yield return new WaitForSeconds(circleCastWait);
        RaycastRangeAttack();

        yield return new WaitForSeconds(Random.Range(1, 5));
        //rangeAttack = false;
    }

    void RaycastRangeAttack()
    {
        RaycastHit2D hitInfo = Physics2D.CircleCast(shootPoint.position, 0.5f, shootPoint.right);
        Debug.LogError("RAYCAST SHOOT");
        if (hitInfo)
        {
            Debug.Log(hitInfo.transform.name);
            CharacterControler playerp = hitInfo.transform.GetComponent<CharacterControler>();
            Debug.Log(hitInfo.transform.name);
            if (playerp != null)
            {
                playerp.TakeDamage(rangeAttackDamage);
                Instantiate(rangeHitEffect, hitInfo.point, Quaternion.identity);
            }
        }
    }

    void FlashAttack()
    {
        if (inAttack || !isFlash)
            return;

        inAttack = true;

        StartCoroutine(Flash());
        StartCoroutine(ResetIsInAttackAsync(1.5f));
        
    }

    IEnumerator Flash()
    {
        yield return null;
        flashAttackk = false;
        yield return new WaitForSeconds(1f);
        meleeAttack = false;
        Collider2D[] enemies = Physics2D.OverlapCircleAll(deathPoint.position, flashRange, enemiesLayers);

        if (enemies.Length > 0)
        {
            enemies[0].GetComponent<CharacterControler>().TakeDamage(meleeAttackDamage);
            ScreenManager.instance.SetScreen(ScreenType.Flash, ScreenType.World);
        }

        yield return new WaitForSeconds(1.5f);
    }

    void SuperAttack()
    {
        if (inAttack)
            return;

        inAttack = true;
        StartCoroutine(Super());
        StartCoroutine(ResetIsInAttackAsync(1.5f));
    }

    IEnumerator Super()
    {
        yield return null;
        animator.SetBool("SuperAttack", false);
        yield return new WaitForSeconds(1f);
        meleeAttack = false;
        Collider2D[] enemies = Physics2D.OverlapCircleAll(attackPoint.position, superRange, enemiesLayers);

        if (enemies.Length > 0)
        {
            enemies[0].GetComponent<CharacterControler>().TakeDamage(superAttackDamage);
            yield return StartCoroutine(EffectsAsynch(superHitEffect, attackPoint));
        }

        yield return new WaitForSeconds(1.5f);
    }


    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        healthSlider.SetSliderValue(currentHealth);
        Debug.LogWarning("DAMAGE");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Transform point = deathPoint;
        point.parent = deathPoint.parent.parent;
        Instantiate(deathEffect, point);
        StartCoroutine(DieAsynch());
    }

    IEnumerator DieAsynch()
    {
        gameObject.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
