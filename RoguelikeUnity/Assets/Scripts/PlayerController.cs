using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private Camera camera;

    public Dictionary<int, int> skillCondition = new Dictionary<int, int>();      // 스킬 습득 여부 및 스킬 레벨 확인 딕셔너리
    public List<int> skillConditionChecker = new List<int>();      // 스킬 습득 여부 확인 리스트

    private float moveSpeed = 3f;                           // 플레이어 이동 속도
    private bool isPlayerMove;                              // 플레이어가 이동 여부 확인
    private bool isPlayerAttack;                            // 플레이어 공격 여부 확인
    public Vector2 lastMove;                                // 플레이어 마지막 움직임 방향

    public int currentHealth;                               // 플레이어 현재 체력
    public int currentMana;                                 // 플레이어 현재 마나
    public int maxHealth;                                   // 플레이어 최대 체력
    public int maxMana;                                     // 플레이어 최대 마나
    public float delaySkill;                                // 스킬 딜레이 타임
    public int essenceCount;                                // 정수 갯수

    public GameObject FireBall;                             // 스킬 0번 : 화염구
    public GameObject Lightning;                            // 스킬 1번 : 번개작렬
    public GameObject Healing;                              // 스킬 1번 : 초회복
    public SkillFireBall skillFireBall;                     // 스킬 화염구 관리 클래스
    public SkillLightning skillLightning;                   // 스킬 번개작렬 관리 클래스
    public SkillHealing skillHealing;                       // 스킬 초회복 관리 클래스

    public UIHealthBar uIHealthBar;                         // 체력바 클래스
    public UIManaBar uIManaBar;                             // 마나바 클래스
    public GameSystem gameSystem;                           // 게임 시스템 클래스

    private SpriteRenderer spriteRenderer;                  // 플레이어 스프라이트 정보
    private Color playerColor;                              // 플레이어 색
    private bool changeOpacity;                             // 투명도 조절 방향 설정, true 증가, false 감소
    public bool isInvincible;                               // 플레이어 무적 여부
    public float invincibleTime;                            // 남은 무적 시간

    public TextMeshProUGUI slotText_01;                     // 아이템 슬롯 01 갯수 UI
    public TextMeshProUGUI slotText_02;                     // 아이템 슬롯 02 갯수 UI
    public TextMeshProUGUI slotText_03;                     // 스킬 슬롯 UI
    public Image slotSkillImage;                             // 스킬 슬롯 이미지

    public int healthPotionCount;                          // 체력 포션 갯수
    public int manaPotionCount;                            // 마나 포션 갯수
    public int currentSkillNumber;                         // 현재 등록된 스킬 번호

    public int healthPotionPower;                           // 체력 포션 효과
    public int manaPotionPower;                             // 체력 포션 효과

    public TextMeshProUGUI essenceCountText;                // 정수 갯수 UI 

    public int stageNum;
    public bool isWarp;

    public TextMeshProUGUI stageNumText;                    // 스테지이 번호 UI

    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();                // 애니메이터 컴포넌트 설정
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();

        currentHealth = maxHealth;                          // 현재 체력 = 최대 체력
        currentMana = maxMana;                              // 현재 마나 = 최대 마나

        spriteRenderer = GetComponent<SpriteRenderer>();
        playerColor = spriteRenderer.color;

        stageNum = 0;
        isWarp = true;

        skillCondition.Add(0, 1);
        skillConditionChecker.Add(0);
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
        UseSkill();
        UseItem();
        SetAnimation();
        SetResourceUI();
        CheckInvincible();
        CheckState();
        SetResourceBar();
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
    /// 플레이어의 정보를 UI에 설정하는 함수
    /// 플레이어가 가진 자원을 바탕으로 UI를 수정한다.
    /// </summary>
    private void SetResourceUI()
    {
        uIHealthBar.SetMaxHealth(maxHealth);                
        uIManaBar.SetMaxMana(maxMana);                      
        uIHealthBar.SetCurrentHealth(currentHealth);                
        uIManaBar.SetCurrentMana(currentMana);

        slotText_01.text = healthPotionCount.ToString();
        slotText_02.text = manaPotionCount.ToString();
        slotText_03.text = gameSystem.skillList[currentSkillNumber][3];

        essenceCountText.text = essenceCount.ToString();

        stageNumText.text = stageNum.ToString();

        slotSkillImage.sprite = Resources.Load<Sprite>("Sprites/Skills/" + skillConditionChecker[currentSkillNumber].ToString());

    }

    /// <summary>
    /// 스킬 사용에 관한 함수
    /// 평상시에는 isPlayerAttack가 false로 설정되어 있으나 스킬을 사용할 경우 true로 변한다.
    /// 또한 각 스킬에 맞는 딜레이 시간이 지나면 다시 false로 설정한다.
    /// 추가로 마우스 휠을 움직임에 따라 currentSkillNumber 값이 증가하며 최대치에 도달하면 다시 0으로 초기화된다.
    /// </summary>
    private void UseSkill()
    {
        isPlayerAttack = false;
        if (delaySkill > 0)
        {
            delaySkill -= Time.deltaTime;
            isPlayerAttack = true;
        }
        if(Input.GetKeyDown(KeyCode.C) && delaySkill <= 0)
        {
            Vector3 mousePos = camera.ScreenToWorldPoint(Input.mousePosition);
            mousePos.z = 0;
            switch (skillConditionChecker[currentSkillNumber])
            {
                case 0:
                    mousePos -= transform.position;
                    float angle = Mathf.Atan2(mousePos.y, mousePos.x) * Mathf.Rad2Deg;
                    Quaternion mouseRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                    Instantiate(FireBall, transform.position, mouseRotation);
                    break;
                case 1:
                    Instantiate(Lightning, mousePos, transform.rotation);
                    break;
                case 2:
                    Instantiate(Healing, transform.position, transform.rotation);
                    break;
            }
        }
        int maxSkillNumber = skillCondition.Count;
        currentSkillNumber += Mathf.RoundToInt(Input.GetAxis("Mouse ScrollWheel") * 10);
        if(currentSkillNumber >= maxSkillNumber)
        {
            currentSkillNumber = 0;
        }
        if(currentSkillNumber < 0)
        {
            currentSkillNumber = maxSkillNumber - 1;
        }

    }

    /// <summary>
    /// 체력바와 마나바를 설정하는 함수
    /// 체력바와 마나바의 위치 및 자원 정보를 바탕으로 값을 설정한다.
    /// </summary>
    private void SetResourceBar()
    {
        uIHealthBar.SetMaxHealth(maxHealth);
        uIManaBar.SetMaxMana(maxMana);
        uIHealthBar.SetCurrentHealth(currentHealth);
        uIManaBar.SetCurrentMana(currentMana);
    }

    /// <summary>
    /// 플레이어 상태 체크 함수
    /// 최대 체력이나 마나를 벗어날 경우, 바로 잡아준다.
    /// </summary>
    private void CheckState()
    {
        if(currentHealth >= maxHealth)
        {
            currentHealth = maxHealth;
        }
        if (currentHealth <= 0)
        {
            currentHealth = 0;
        }
        if(currentMana >= maxMana)
        {
            currentMana = maxMana;
        }
        if (currentMana <= 0)
        {
            currentMana = 0;
        }
    }

    /// <summary>
    /// 플레이어 아이템 사용 함수
    /// 키 입력의 종류와 해당 키에 등록되어 있는 아이템에 따른 효과가 발생한다.
    /// </summary>
    private void UseItem()
    {
        if (Input.GetKeyDown(KeyCode.Z) && healthPotionCount > 0)
        {
            healthPotionCount -= 1;
            currentHealth += healthPotionPower;
        }
        if (Input.GetKeyDown(KeyCode.X) && manaPotionCount > 0)
        {
            manaPotionCount -= 1;
            currentMana += manaPotionPower;
        }
    }


    public void ResetPosition()
    {
        transform.position = new Vector3(0f, -3.5f, 0f);
    }

    /// <summary>
    /// 플레이어의 무적 여부 확인 함수
    /// invincibleTime이 0보다 크면 무적 시간이고 그렇지 않으면 무적이 아니다.
    /// invincibleTime이 0보다 크다면 그렇지 않게 만든다.
    /// </summary>
    private void CheckInvincible()
    {
        if (isInvincible)
        {
            if (changeOpacity)
            {
                if (playerColor.a >= 1f)
                {
                    changeOpacity = false;
                }
                else
                {
                    playerColor.a += 0.1f;
                    spriteRenderer.color = playerColor;
                }
            }
            else
            {
                if (playerColor.a <= 0.5f)
                {
                    changeOpacity = true;
                }
                else
                {
                    playerColor.a -= 0.1f;
                    spriteRenderer.color = playerColor;
                }
            }
        }
        else
        {
            playerColor.a = 1f;
            spriteRenderer.color = playerColor;
        }
        if(invincibleTime > 0)
        {
            isInvincible = true;
            invincibleTime -= Time.deltaTime;
        }
        else if(invincibleTime <= 0)
        {
            isInvincible = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag.Equals("WarpGate"))
        {
            isWarp = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag.Equals("Skill"))
        {
            for (int i = 0; i < gameSystem.skillList.Count; i++)
            {
                if ((gameSystem.skillList[i][1] + "(Clone)").Equals(collision.name) && gameSystem.skillList[i][2].Equals("Support"))
                {
                    int skillCode = int.Parse(gameSystem.skillList[i][0]);
                    currentHealth += int.Parse(gameSystem.skillList[i][5]) * skillCondition[skillCode];
                    break;
                }
            }
        }
    }
}
