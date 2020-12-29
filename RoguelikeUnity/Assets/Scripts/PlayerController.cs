using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Animator animator;

    private List<bool> skillCondition = new List<bool>();   // 스킬 습득 여부 확인 리스트
    private float moveSpeed = 3f;                           // 플레이어 이동 속도
    private bool isPlayerMove;                              // 플레이어가 이동 여부 확인
    private bool isPlayerAttack;                            // 플레이어 공격 여부 확인
    public Vector2 lastMove;                                // 플레이어 마지막 움직임 방향

    public int currentHealth;                               // 플레이어 현재 체력
    public int currentMana;                                 // 플레이어 현재 마나
    public int maxHealth;                                   // 플레이어 최대 체력
    public int maxMana;                                     // 플레이어 최대 마나
    public float delaySkill;                                // 스킬 딜레이 타임

    public GameObject FireBall;                             // 스킬 1번 : 화염구
    public SkillFireBall skillFireBall;                 // 스킬 관리 클래스

    public UIHealthBar uIHealthBar;                         // 체력바 클래스
    public UIManaBar uIManaBar;                             // 마나바 클래스


    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();                // 애니메이터 컴포넌트 설정

        skillCondition.Add(true);                           //  스킬 화염구 추가 (임시)

        currentHealth = maxHealth;                          // 현재 체력 = 최대 체력
        currentMana = maxMana;                              // 현재 마나 = 최대 마나

        uIHealthBar.SetMaxHealth(maxHealth);                // 체력바 설정
        uIManaBar.SetMaxMana(maxMana);                      // 마나바 설정

    }


    /// <summary>
    /// 벽 충돌 시 진동하는 듯한 움직임 방지하기 위한 조치
    /// </summary>
    private void FixedUpdate()                              
    {
        Move();                                             
    }

    /// <summary>
    /// 업데이트 함수
    /// </summary>
    private void Update()
    {
        // TakeDamage(10);
        UseSkill();                                         
        SetAnimation();                                     
    }

    /// <summary>
    /// 플레이어 움직임 관련 함수
    /// 평상시에는 isPlayerMove을 false로 설정해두지만 입력이 감지되면 true로 전환시킨다.
    /// 또한 입력에 따라 lastMove 값을 변경해준다.
    /// </summary>
    private void Move()                                     
    {
        isPlayerMove = false;                               
        
        if (Input.GetAxisRaw("Horizontal") != 0f && delaySkill <= 0)           
        {
            transform.Translate(new Vector3(Input.GetAxisRaw("Horizontal") * moveSpeed * Time.deltaTime, 0f, 0f));
            isPlayerMove = true;                            
            lastMove = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
        }
        if (Input.GetAxisRaw("Vertical") != 0f && delaySkill <= 0)
        {
            transform.Translate(new Vector3(0f, Input.GetAxisRaw("Vertical") * moveSpeed * Time.deltaTime, 0f));
            isPlayerMove = true;
            lastMove = new Vector2(0f, Input.GetAxisRaw("Vertical"));
        }
    }

    /// <summary>
    /// 플레이어의 애니메이션과 관련된 함수
    /// 애니메이션 파라미터를 설정해준다.
    /// </summary>
    private void SetAnimation()
    {
        animator.SetFloat("MoveX", Input.GetAxisRaw("Horizontal"));
        animator.SetFloat("MoveY", Input.GetAxisRaw("Vertical"));
        animator.SetBool("Moving", isPlayerMove);
        animator.SetBool("Attacking", isPlayerAttack);
        animator.SetFloat("LastMoveX", lastMove.x);
        animator.SetFloat("LastMoveY", lastMove.y);
    }

    /// <summary>
    /// 스킬 사용에 관한 함수
    /// 평상시에는 isPlayerAttack가 false로 설정되어 있으나 스킬을 사용할 경우 true로 변한다.
    /// 또한 각 스킬에 맞는 딜레이 시간이 지나면 다시 false로 설정한다.
    /// </summary>
    private void UseSkill()
    {
        isPlayerAttack = false;
        if (delaySkill > 0)
        {
            delaySkill -= Time.deltaTime;
            isPlayerAttack = true;
        }
        if (Input.GetKeyDown(KeyCode.Q) && currentMana >= skillFireBall.usedMana && delaySkill <= 0)       // 화염구 스킬 사용
        {
            Instantiate(FireBall, transform.position + new Vector3(lastMove.x, lastMove.y, 0f), transform.rotation);
        }
    }

    /// <summary>
    /// 플레이어가 입는 데미지에 관한 함수
    /// 아직은 작성만 해둔다.
    /// </summary>
    /// <param name="damage">플레이어가 받는 데미지</param>
    private void TakeDamage(int damage)
    {
        currentHealth -= damage;
        uIHealthBar.SetCurrentHealth(currentHealth);
    }
}
