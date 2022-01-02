using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Control : MonoBehaviour
{
    public float MovementSpeed = 2f;
    public float JumpForce = 1f;
    public float movement;

    public Transform attackPoint;
    public float attackRange = 0.5f;
    public LayerMask enemyLayers;
    public int attackDamage = 20;
    public float attackRate =2f;
    float nextAttackTime = 0f;

    private bool facingRight = true;
    private Animator myAnimator;
    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    // Update is called once per frame
    private void Update()
    {   
        if(Time.time >= nextAttackTime){    
            if(Input.GetKeyDown(KeyCode.Z)){

                Ataque();
                nextAttackTime = Time.time + 1f / attackRate;
            }
        }
        movement = Input.GetAxisRaw("Horizontal");
        if (Input.GetButtonDown("Jump") && Mathf.Abs(_rigidbody.velocity.y) < 0.001f)
        {
            _rigidbody.AddForce(new Vector2(0,JumpForce),ForceMode2D.Impulse);
        }
    }

    private void FixedUpdate() {
        _rigidbody.velocity = new Vector2(movement*MovementSpeed,_rigidbody.velocity.y);
        Flip(movement);
        myAnimator.SetFloat("speed",Mathf.Abs(movement));
    }
    private void Flip(float horizontal){
        if(horizontal < 0 && facingRight || horizontal > 0 && !facingRight){
            facingRight = !facingRight;

            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
    }
    void Ataque(){
        myAnimator.SetTrigger("Ataque");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position,attackRange,enemyLayers);
        foreach(Collider2D enemy in hitEnemies){
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
            Debug.Log("We hit" + enemy.name);
        }
    }
    void OnDrawGizmosSelected(){
        if(attackPoint == null){
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position,attackRange);
    }
}
