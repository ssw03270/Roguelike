using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{
    private Animator animator;
    private Camera camera;

    public List<bool> skillCondition = new List<bool>();   // 스킬 습득 여부 확인 리스트
    private float moveSpeed = 3f;                           // 플레이어 이동 속도
    private bool isPlayerMove;                              // 플레이어가 이동 여부 확인
    private bool isPlayerAttack;                            // 플레이어 공격 여부 확인
    public Vector2 lastMove;                                // 플레이어 마지막 움직임 방향

    public int currentHealth;                               // 플레이어 현재 체력
    public int currentMana;                                 // 플레이어 현재 마나
    public int maxHealth;                                   // 플레이어 최대 체력
    public int maxMana;                                     // 플레이어 최대 마나
    public float delaySkill;                                // 스킬 딜레이 타임
    private int essenceCount;                               // 정수 갯수

    public GameObject FireBall;                             // 스킬 1번 : 화염구
    public GameObject Lightning;                            // 스킬 1번 : 번개작렬
    public SkillFireBall skillFireBall;                     // 스킬 화염구 관리 클래스
    public SkillLightning skillLightning;                   // 스킬 번개작렬 관리 클래스

    public UIHealthBar uIHealthBar;                         // 체력바 클래스
    public UIManaBar uIManaBar;                             // 마나바 클래스
    public GameSystem gameSystem;                           // 게임 시스템 클래스

    private SpriteRenderer spriteRenderer;                  // 플레이어 스프라이트 정보
    private Color playerColor;                              // 플레이어 색
    private bool changeOpacity;                             // 투명도 조절 방향 설정, true 증가, false 감소
    public bool isInvincible;                               // 플레이어 무적 여부
    public float invincibleTime;                            // 남은 무적 시간

    public TextMeshProUGUI itemCount_01;                    // 아이템 슬롯 01 갯수 UI
    public TextMeshProUGUI itemCount_02;                    // 아이템 슬롯 02 갯수 UI
    public TextMeshProUGUI itemCount_03;                    // 아이템 슬롯 03 갯수 UI

    private Dictionary<string, int> itemSlot = new Dictionary<string, int>();   // 아이템 슬롯에 있는 아이템 종류             
    private Dictionary<string, int> itemCount = new Dictionary<string, int>();  // 아이템 슬롯에 있는 아이템 갯수             

    public TextMeshProUGUI essenceCountText;                // 정수 갯수 UI 

    public int stageNum;
    public bool isWarp;

    // Start is called before the first frame update
    private void Start()
    {
        animator = GetComponent<Animator>();                // 애니메이터 컴포넌트 설정
        camera = GameObject.Find("Main Camera").GetComponent<Camera>();

        currentHealth = maxHealth;                          // 현재 체력 = 최대 체력
        currentMana = maxMana;                              // 현재 마나 = 최대 마나

        spriteRenderer = GetComponent<SpriteRenderer>();
        playerColor = spriteRenderer.color;

        itemSlot.Add("Z", 1);                               // 체력 포션
        itemSlot.Add("X", 2);                               // 마나 포션
        itemSlot.Add("C", 0);                               // 공백

        itemCount.Add("Z", 3);                              // 첫 번째 슬롯 갯수
        itemCount.Add("X", 3);                              // 두 번째 슬롯 갯수
        itemCount.Add("C", 0);                              // 세 번째 슬롯 갯수

        stageNum = 0;
        isWarp = true;
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

        itemCount_01.text = itemCount["Z"].ToString();
        itemCount_02.text = itemCount["X"].ToString();
        itemCount_03.text = itemCount["C"].ToString();

        essenceCountText.text = essenceCount.ToString();
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
        if (Input.GetKeyDown(KeyCode.Q) && delaySkill <= 0)        // 화염구 스킬 사용
        {
            Instantiate(FireBall, transform.position + new Vector3(lastMove.x / 2, lastMove.y / 2, 0f), transform.rotation);
        }
        if (Input.GetKeyDown(KeyCode.E) && delaySkill <= 0)       // 번개작렬 스킬 사용
        {
            Vector3 targetPos = camera.ScreenToWorldPoint(Input.mousePosition);
            targetPos.z = 0;
            Instantiate(Lightning, targetPos, transform.rotation);
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
        if (Input.GetKeyDown(KeyCode.Z) && itemCount["Z"] > 0)
        {
            int itemCode = itemSlot["Z"];
            itemCount["Z"] -= 1;
            CheckItemType(itemCode);
        }
        if (Input.GetKeyDown(KeyCode.X) && itemCount["X"] > 0)
        {
            int itemCode = itemSlot["X"];
            itemCount["X"] -= 1;
            CheckItemType(itemCode);
        }
        if (Input.GetKeyDown(KeyCode.C) && itemCount["C"] > 0)
        {
            int itemCode = itemSlot["C"];
            itemCount["C"] -= 1;
            CheckItemType(itemCode);
        }
    }

    /// <summary>
    /// 사용한 아이템 종류 확인 함수
    /// 어떤 종류의 아이템을 사용했는지 확인하고 그에 맞는 효과를 부여한다.
    /// </summary>
    /// <param name="itemCode">사용한 아이템 코드</param>
    private void CheckItemType(int itemCode)
    {
        switch (gameSystem.itemList[itemCode][2])
        {
            case "health":
                currentHealth += int.Parse(gameSystem.itemList[itemCode][3]);
                break;
            case "mana":
                currentMana += int.Parse(gameSystem.itemList[itemCode][3]);
                break;
            default:
                Debug.Log("no item");
                break;
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
        if (collision.name.Equals("WarpGate"))
        {
            isWarp = true;
        }
        if (collision.name.Equals("Essence(Clone)"))
        {
            Destroy(collision.gameObject);
            essenceCount += 1;
        }
    }
}
