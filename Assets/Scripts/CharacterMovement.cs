using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class CharacterMovement : MonoBehaviour
{
    [Header("playerMovement Info")]
    [SerializeField] private float InputVecX;
    public float moveSpeed;
    [SerializeField] private float jumpPower;
    [SerializeField] private bool isFlip;
    public bool isHit;
    public float HitTime;   //�ǰ� �����ð� (�ǰݹ����ð�)

    [Header("Dash Info")]
    [SerializeField] private bool canDash;
    [SerializeField] private bool isDashing;
    [SerializeField] private float dashPower;
    [SerializeField] private float dashDuration;
    [SerializeField] private float dashCooltime;

    [Header("PlayerJumpData")]
    [SerializeField] private int maxJumpCount;
    [SerializeField] private int curJumpCount;

    [Header("PlatformCheckData")]
    [SerializeField] private Vector2 footPosition;  //�ٴ� üũ�� ���� ����ġ
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool isGrounded;
    [Header("CheckWall")]
    [SerializeField] private bool isWallSliding;
    [SerializeField] private float wallSlideSpeed = 2f;
    [SerializeField] private Vector2 FrontCheckPos; //�� Ž�� Ȯ�� ��ġ
    [SerializeField] private LayerMask wallLayer;   //�� ���̾�
    [Header("WallJumpInfo")]
    [SerializeField] private bool isWallJumping;
    [SerializeField] private float wallJumpingDirection;
    [SerializeField] private float wallJumpingTime = 0.2f;
    [SerializeField] private float wallJumpingCounter;
    [SerializeField] private float wallJumpingDuration = 0.4f;
    [SerializeField] private Vector2 wallJumpingPower;

    [Header("Gravity Settings")]
    public float normalGravity = 1f;   // �⺻ �߷�
    public float fallGravity = 2.5f;   // �ϰ� �� �߷�

    [Header("Particle&Trail")]
    [SerializeField] private TrailRenderer trailObj;

    [Header("�м��ִ� ��ü�� ���� ����")]
    [SerializeField] private float pushObjeOriginalMass;
    [SerializeField] private bool isCollidingWithPushObject = false;
    [SerializeField] private GameObject collisionGameObj;

    [Header("��������")]
    [SerializeField] private GameObject AttackRange;
    [SerializeField] private float attackInterval;
    [SerializeField] private bool canAttack;
    [SerializeField] private float attackMoveDistance = 0.5f; // ���� �� �̵� �Ÿ�
    [SerializeField] private float attackMoveSpeed = 5f;      // ���� �� �̵� �ӵ�

    //�������� Componenets
    private Collider2D col;
    private Rigidbody2D rb;
    private SpriteRenderer playerSprite;
    [HideInInspector]public Animator playerAnimator;

    //�ڷ�ƾ ����
    private Coroutine DodgeCoroutine;
    private Coroutine PlayerHit;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<Collider2D>();
        playerSprite = GetComponentInChildren<SpriteRenderer>();
        playerAnimator = GetComponent<Animator>();
        canAttack = true;
    }
    private void OnEnable()
    {
        //�ʱ� �߷� ����
        rb.gravityScale = normalGravity;
        canDash = true;
    }

    // �浹 �� ���� mass ���� �� �浹 ���� ������Ʈ
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PushObject"))
        {
            isCollidingWithPushObject = true;
            collisionGameObj = collision.gameObject;
            pushObjeOriginalMass = collisionGameObj.GetComponent<Rigidbody2D>().mass;
        }
    }

    // �浹�� ������ ���� mass ���� �� ���� �ʱ�ȭ
    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PushObject"))
        {
            isCollidingWithPushObject = false;
            Rigidbody2D rb = collisionGameObj.GetComponent<Rigidbody2D>();
            rb.mass = pushObjeOriginalMass;
            rb.velocity = Vector2.zero;
            collisionGameObj = null;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Trap"))
        {
            PlayerHit = StartCoroutine(Hit());
        }
        else if (collision.CompareTag("DeadZone"))
        {
            Retire();
        }
        else if (collision.CompareTag("Item"))
        {
            collision.gameObject.GetComponent<Item>().GetItems();
        }
        else if(collision.CompareTag("EnemyBullet"))
        {
            PlayerHit = StartCoroutine(Hit());
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(GameManager.instance.isDead == false)
        {
            //�뽬 ����, �ƾ�, ��ȭ Ȱ��ȭ �� ������ �Ұ�
            if (isDashing == true || StartCutScene.isCutSceneOn == true || GameManager.instance.isDialogStart == true)
            {
                return;
            }

            GravitySet();

            InputVecX = Input.GetAxisRaw("Horizontal");

            playerAnimator.SetFloat("Speed", Mathf.Abs(InputVecX));

            if (Input.GetButtonDown("Jump"))
            {
                Jump();
            }

            if (Input.GetKeyDown(KeyCode.LeftShift) && canDash == true)
            {
                DodgeCoroutine = StartCoroutine(Dodge()); // ��� �ڷ�ƾ ����
            }

            WallSlide();
            WallJump();
            if (!isWallJumping)
            {
                Flip();
            }
            if (InputVecX == 0)  // Ű �Է��� ���� �� ���콺 �������� �ø�
            {
                FlipToMouse();
            }

            if (isCollidingWithPushObject)
            {
                GameObject pushObject = collisionGameObj;
                if (pushObject != null)
                {
                    Rigidbody2D rb = pushObject.GetComponent<Rigidbody2D>();

                    // LeftControl Ű�� ������ mass�� 100���� ����
                    if (Input.GetKey(KeyCode.LeftControl))
                    {
                        rb.mass = 100;
                    }
                    // LeftControl Ű�� ���� ���� mass�� �����ϰ� velocity�� 0���� ����
                    else
                    {
                        rb.mass = pushObjeOriginalMass;
                        rb.velocity = Vector2.zero;
                    }
                }
            }
            if(Input.GetMouseButtonDown(0) && canAttack == true)
            {
                StartCoroutine(Attack());
            }

        }
    }

    private void FixedUpdate()
    {
        if (isDashing == true)
        {
            return;
        }

        if (!isWallJumping)
        {

            //�÷��� üũ �� �̵� 
            Move_CheckPlatform();
        }
    }
    private IEnumerator Attack()
    {
        canAttack = false;
        AttackRange.SetActive(true);
        playerAnimator.SetTrigger("Attack");
        yield return new WaitForSeconds(attackInterval);
        AttackRange.SetActive(false);
        canAttack = true;
    }
    private void Move_CheckPlatform()
    {
        //�÷��̾� ������Ʈ�� Collider2D min,Center, Max ��ġ ����
        Bounds bounds = col.bounds;
        //�÷��̾��� �� ��ġ ����
        footPosition = new Vector2(bounds.center.x, bounds.min.y);
        //�÷��̾��� �� �ʿ� Ž�� ������Ʈ ����, ������Ʈ�� �ٴ��� ����ִٸ� isGrounded = true
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(footPosition, 0.05f, groundLayer);
        //�̵� ����
        rb.velocity = new Vector2(InputVecX * moveSpeed, rb.velocity.y);
        
        //�ٴ� Ž���� ���� ���� ī��Ʈ �ʱ�ȭ
        if (isGrounded && wasGrounded == false)
        {
            //���� ī��Ʈ �ʱ�ȭ
            curJumpCount = 0;
        }
    }
    private bool isWalled()
    {
        // �÷��̾� ������Ʈ�� Collider2D min,Center, Max ��ġ ����
        Bounds bounds = col.bounds;

        // �� Ž�� ������ ���
        float wallCheckOffset = (transform.localScale.x > 0) ? 0.5f : -0.5f; // �������� ��� +0.5f, ������ ��� -0.5f
        FrontCheckPos = new Vector2(bounds.center.x + wallCheckOffset, bounds.center.y);

        // �� üũ
        return Physics2D.OverlapBox(FrontCheckPos, new Vector2(0.5f, 0.5f), 0, wallLayer);
    }

    private void WallSlide()
    {
        if (isWalled() && isGrounded == false && InputVecX != 0)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -wallSlideSpeed, float.MaxValue));
        }
        else
            isWallSliding = false;
    }
    private void WallJump()
    {
        if (isWallSliding)
        {
            isWallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        }
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if (Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallJumping = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if (transform.localScale.x != wallJumpingDirection)
            {
                isFlip = !isFlip;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingDuration);
        }
    }

    private void StopWallJumping()
    {
        isWallJumping = false;
    }

    private void Flip()
    {
        if (isFlip && InputVecX > 0f || !isFlip && InputVecX < 0f)
        {
            isFlip = !isFlip;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    private void FlipToMouse()
    {
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        float direction = mousePosition.x - transform.position.x;

        // ���콺�� �����ʿ� ���� �� ĳ���Ͱ� �������� �ٶ󺸵���, �ݴ��� �� ������ �ٶ󺸵��� ����
        if (direction > 0 && transform.localScale.x < 0 || direction < 0 && transform.localScale.x > 0)
        {
            isFlip = !isFlip;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    private void GravitySet()
    {
        if (rb.velocity.y < 0 && !isGrounded)
        {
            // �ϰ� ���� �� �߷� ����
            rb.gravityScale = fallGravity;

            //���� ���� ����
            //Highter gravity if falling

            //Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds

        }
        else if (isGrounded || isWallJumping == true)
        {
            // ���� ����� �� �߷��� ������� ����
            rb.gravityScale = normalGravity;
        }
    }

    private void Jump()
    {
        if (curJumpCount < maxJumpCount) // ���� ī��Ʈ üũ
        {
            playerAnimator.SetTrigger("Jumping");
            rb.gravityScale = normalGravity;
            curJumpCount++;
            print("�����߽��ϴ�.");
            rb.AddForce(Vector2.up * jumpPower, ForceMode2D.Impulse);
        }
    }

    private IEnumerator Dodge()
    {
        canDash = false;
        isDashing = true;
        float original = rb.gravityScale;
        rb.gravityScale = 0;
        playerAnimator.SetTrigger("Dodge");
        gameObject.layer = 7;
        // �̵� ���⿡ ���� ���
        float dashDirection = transform.localScale.x; // ���� ���⿡ ���� ��� ���� ����
        rb.velocity = new Vector2(dashDirection * dashPower * 10, 0f);

        //trail ����
        trailObj.gameObject.SetActive(true);
        yield return new WaitForSeconds(dashDuration);
        rb.gravityScale = original;
        isDashing = false;
        trailObj.gameObject.SetActive(false);
        gameObject.layer = 3;
        yield return new WaitForSeconds(dashCooltime);
        canDash = true;
    }
    public IEnumerator Hit()
    {
        if (GameManager.instance.hp > 1)
        {
            isHit = true;
            GameManager.instance.hp--;
            playerAnimator.SetTrigger("Hit");
            gameObject.layer = 7;
            yield return new WaitForSeconds(HitTime);   //HitTime��ŭ �ǰݻ���(�Ͻ����� ����)
            isHit = false;
            gameObject.layer = 3;   //PlayerLayer�� ����   
        }
        else
        //
        {
            GameManager.instance.hp--;
            Retire();
        }
    }
    //GameOver
    private void Retire()
    {
        if (GameManager.instance.hp < 0)
        {
            //ü�� ������ ��ȯ�Ǵ� �� ����
            GameManager.instance.hp = 0;
        }

        GameManager.instance.hp = 0;
        GameManager.instance.isDead = true;
        playerAnimator.SetTrigger("Retire");

        //��Ÿ�̾� �� ���� ����

    }
  
    private void OnDrawGizmos()
    {
        // �ٴ� üũ ��ġ
        Gizmos.color = isGrounded ? Color.green : Color.red; // �ٴڿ� ������ �ʷϻ�, ������ ������
        Gizmos.DrawWireSphere(footPosition, 0.05f); // �ٴ� üũ ����
 
    }

}
