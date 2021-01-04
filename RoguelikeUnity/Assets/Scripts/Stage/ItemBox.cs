using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ItemBox : MonoBehaviour
{
    private GameObject canvas;                          // 캔버스
    RectTransform itemBoxUI;                            // 캔버스에서의 아이템 박스 UI
    public GameObject itemBoxObj;                       // 아이템 박스 UI 오브젝트

    int essenceCount;                                   // 랜덤하게 정해질 아이템의 가격 == 정수 갯수

    private TextMeshProUGUI essenceCountText;           // 아이템 박스에 표기할 정수 갯수 
    private PlayerController playerController;          // 플레이어 컨트롤러 클래스

    public int itemBoxType;                             // 아이템 박스 타입, 0 == 랜덤박스, 1 == 체력포션, 2 == 마나포션

    private GameSystem gameSystem;                      // 게임 시스템 클래스

    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        gameSystem = GameObject.Find("GameSystem").GetComponent<GameSystem>();

        itemBoxUI = Instantiate(itemBoxObj, canvas.transform).GetComponent<RectTransform>();

        essenceCountText = itemBoxUI.GetChild(0).GetComponent<TextMeshProUGUI>();
        essenceCount = Random.Range(1, 5);
        essenceCountText.text = essenceCount.ToString();

        Vector3 itemBoxPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y - 0.75f, 0f));
        itemBoxUI.position = itemBoxPos;

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnDestroy()
    {
        try
        {
            Destroy(itemBoxUI.gameObject);
        }
        catch
        {
            Debug.Log("Error while destoy the itemBox UI.");
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.name == "Player")
        {
            if(playerController.essenceCount >= essenceCount)
            {
                playerController.essenceCount -= essenceCount;
                switch (itemBoxType)
                {
                    case 0:
                        int itemCode = Random.Range(0, gameSystem.itemList.Count - 1);
                        GameObject randomItem = Resources.Load<GameObject>("Prefabs/Item/" + gameSystem.itemList[itemCode][1]);
                        Instantiate(randomItem, transform.position + new Vector3(0f, 1f, 0f), transform.rotation).transform.parent = transform.parent;
                        break;
                    case 1:
                        playerController.healthPotionCount += 1;
                        break;
                    case 2:
                        playerController.manaPotionCount += 1;
                        break;
                }
                Destroy(this.gameObject);
            }
        }
    }
}
