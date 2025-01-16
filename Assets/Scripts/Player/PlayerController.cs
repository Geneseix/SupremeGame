using Unity.VisualScripting;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    [SerializeField] float movementSpeed = 3f;
    [SerializeField] float jumpForce = 3f;
    [SerializeField] private Animator animator;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] Transform groundCheck;
    [SerializeField] Transform attackPoint;
    [SerializeField] float attackRange = 0.5f;
    [SerializeField] LayerMask enemyLayers;
    [SerializeField] int attackDamage = 40;


    private bool isJumping = false;
    private bool isGrounded = true;
    private float movementHorizontal;
    private Rigidbody2D playerRigidBody;
    private SpriteRenderer spriteRenderer;
    private float groundCheckRadius = 0.2f; 

    void Start() {
        playerRigidBody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update() { 
        movementHorizontal = Input.GetAxisRaw("Horizontal") * movementSpeed;

        animator.SetFloat("Speed", Mathf.Abs(movementHorizontal));


        if (Input.GetKeyDown(KeyCode.Space) && isGrounded) {
            isJumping = true;
            animator.SetBool("IsJumping", true); 
        }


        if (movementHorizontal < 0) {
            spriteRenderer.flipX = true;
        } else if (movementHorizontal > 0) {
            spriteRenderer.flipX = false; 
        }

        if (Input.GetKeyDown(KeyCode.J)) {
            Attack();
        }
    }

    void FixedUpdate() {
        playerRigidBody.linearVelocity = new Vector2(movementHorizontal, playerRigidBody.linearVelocityY);

        if (isJumping) {
            playerRigidBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isJumping = false;
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        animator.SetBool("IsJumping", !isGrounded); 
    }

    private void Attack() {
        animator.SetTrigger("Attack");
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayers);
    
        foreach(Collider2D enemy in hitEnemies) {
            enemy.GetComponent<Enemy>().TakeDamage(attackDamage);
        }
    }
    
    private void OnDrawGizmosSelected() {
        if(attackPoint == null)
            return;

        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
