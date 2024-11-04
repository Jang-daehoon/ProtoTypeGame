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
    public float HitTime;   //피격 유지시간 (피격무적시간)

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
    [SerializeField] private Vector2 footPosition;  //바닥 체크를 위한 발위치
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private bool isGrounded;
    [Header("CheckWall")]
    [SerializeField] private bool isWallSliding;
    [SerializeField] private float wallSlideSpeed = 2f;
    [SerializeField] private Vector2 FrontCheckPos; //벽 탐지 확인 위치
    [SerializeField] private LayerMask wallLayer;   //벽 레이어
    [Header("WallJumpInfo")]
    [SerializeField] private bool isWallJumping;
    [SerializeField] private float wallJumpingDirection;
    [SerializeField] private float wallJumpingTime = 0.2f;
    [SerializeField] private float wallJumpingCounter;
    [SerializeField] private float wallJumpingDuration = 0.4f;
    [SerializeField] private Vector2 wallJumpingPower;

    [Header("Gravity Settings")]
    public float normalGravity = 1f;   // 기본 중력
    public float fallGravity = 2.5f;   // 하강 중 중력

    [Header("Particle&Trail")]
    [SerializeField] private TrailRenderer trailObj;

    [Header("밀수있는 물체에 관한 정보")]
    [SerializeField] private float pushObjeOriginalMass;
    [SerializeField] private bool isCollidingWithPushObject = false;
    [SerializeField] private GameObject collisionGameObj;

    [Header("공격정보")]
    [SerializeField] private GameObject AttackRange;
    [SerializeField] private float attackInterval;
    [SerializeField] private bool canAttack;
    [SerializeField] private float attackMoveDistance = 0.5f; // 공격 시 이동 거리
    [SerializeField] private float attackMoveSpeed = 5f;      // 공격 시 이동 속도

    //보유중인 Componenets
    private Collider2D col;
    private Rigidbody2D rb;
    private SpriteRenderer playerSprite;
    [HideInInspector]public Animator playerAnimator;

    //코루틴 정보
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
        //초기 중력 설정
        rb.gravityScale = normalGravity;
        canDash = true;
    }

    // 충돌 시 원래 mass 저장 및 충돌 상태 업데이트
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("PushObject"))
        {
            isCollidingWithPushObject = true;
            collisionGameObj = collision.gameObject;
            pushObjeOriginalMass = collisionGameObj.GetComponent<Rigidbody2D>().mass;
        }
    }

    // 충돌이 끝나면 원래 mass 복구 및 상태 초기화
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
            //대쉬 도중, 컷씬, 대화 활성화 시 움직임 불가
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
                DodgeCoroutine = StartCoroutine(Dodge()); // 대시 코루틴 시작
            }

            WallSlide();
            WallJump();
            if (!isWallJumping)
            {
                Flip();
            }
            if (InputVecX == 0)  // 키 입력이 없을 때 마우스 방향으로 플립
            {
                FlipToMouse();
            }

            if (isCollidingWithPushObject)
            {
                GameObject pushObject = collisionGameObj;
                if (pushObject != null)
                {
                    Rigidbody2D rb = pushObject.GetComponent<Rigidbody2D>();

                    // LeftControl 키를 누르면 mass를 100으로 설정
                    if (Input.GetKey(KeyCode.LeftControl))
                    {
                        rb.mass = 100;
                    }
                    // LeftControl 키를 떼면 원래 mass로 복구하고 velocity를 0으로 설정
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

            //플랫폼 체크 및 이동 
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
        //플레이어 오브젝트의 Collider2D min,Center, Max 위치 정보
        Bounds bounds = col.bounds;
        //플레이어의 발 위치 설정
        footPosition = new Vector2(bounds.center.x, bounds.min.y);
        //플레이어의 발 쪽에 탐지 오브젝트 생성, 오브젝트와 바닥이 닿아있다면 isGrounded = true
        bool wasGrounded = isGrounded;
        isGrounded = Physics2D.OverlapCircle(footPosition, 0.05f, groundLayer);
        //이동 연산
        rb.velocity = new Vector2(InputVecX * moveSpeed, rb.velocity.y);
        
        //바닥 탐지로 인한 점프 카운트 초기화
        if (isGrounded && wasGrounded == false)
        {
            //점프 카운트 초기화
            curJumpCount = 0;
        }
    }
    private bool isWalled()
    {
        // 플레이어 오브젝트의 Collider2D min,Center, Max 위치 정보
        Bounds bounds = col.bounds;

        // 벽 탐지 오프셋 계산
        float wallCheckOffset = (transform.localScale.x > 0) ? 0.5f : -0.5f; // 오른쪽일 경우 +0.5f, 왼쪽일 경우 -0.5f
        FrontCheckPos = new Vector2(bounds.center.x + wallCheckOffset, bounds.center.y);

        // 벽 체크
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

        // 마우스가 오른쪽에 있을 때 캐릭터가 오른쪽을 바라보도록, 반대일 때 왼쪽을 바라보도록 설정
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
            // 하강 중일 때 중력 증가
            rb.gravityScale = fallGravity;

            //추후 개선 내역
            //Highter gravity if falling

            //Caps maximum fall speed, so when falling over large distances we don't accelerate to insanely high speeds

        }
        else if (isGrounded || isWallJumping == true)
        {
            // 땅에 닿았을 때 중력을 원래대로 복구
            rb.gravityScale = normalGravity;
        }
    }

    private void Jump()
    {
        if (curJumpCount < maxJumpCount) // 점프 카운트 체크
        {
            playerAnimator.SetTrigger("Jumping");
            rb.gravityScale = normalGravity;
            curJumpCount++;
            print("점프했습니다.");
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
        // 이동 방향에 따라 대시
        float dashDirection = transform.localScale.x; // 현재 방향에 따라 대시 방향 설정
        rb.velocity = new Vector2(dashDirection * dashPower * 10, 0f);

        //trail 생성
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
            yield return new WaitForSeconds(HitTime);   //HitTime만큼 피격상태(일시적인 무적)
            isHit = false;
            gameObject.layer = 3;   //PlayerLayer로 변경   
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
            //체력 음수로 변환되는 것 방지
            GameManager.instance.hp = 0;
        }

        GameManager.instance.hp = 0;
        GameManager.instance.isDead = true;
        playerAnimator.SetTrigger("Retire");

        //리타이어 시 게임 종료

    }
  
    private void OnDrawGizmos()
    {
        // 바닥 체크 위치
        Gizmos.color = isGrounded ? Color.green : Color.red; // 바닥에 있으면 초록색, 없으면 빨간색
        Gizmos.DrawWireSphere(footPosition, 0.05f); // 바닥 체크 범위
 
    }

}
