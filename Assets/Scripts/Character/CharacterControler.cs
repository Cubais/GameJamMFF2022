using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControler : MonoBehaviour, ICharacterController
{
    [Header("Character Controler Params")]
    public float maxSpeed = 5f;
    public float meeleyAttackDamage = 5f;
    public float rangeAttackDamage = 5f;

    private Vector2 movement;
    private bool rangeAttack = false;
    private bool radioAttack = false;
    private bool meleeAttack = false;

    private Animator animator;
    private Rigidbody2D rb;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
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

        UpdateAnimator();
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
