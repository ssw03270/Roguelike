using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIItemBox : MonoBehaviour
{
    private GameObject canvas;                      // 캔버스
    public GameObject itemBoxObj;                   // 아이템 박스 UI 오브젝트
    RectTransform itemBoxUI;                        // 캔버스에서의 아이템 박스 UI
    int essenceCount;
    private TextMeshProUGUI essenceCountText;           // 아이템 박스에 표기할 정수 갯수 
    private PlayerController playerController;      // 플레이어 컨트롤러 클래스
    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        itemBoxUI = Instantiate(itemBoxObj, canvas.transform).GetComponent<RectTransform>();
        essenceCountText = itemBoxUI.GetChild(0).GetComponent<TextMeshProUGUI>();
        essenceCount = Random.Range(1, 5);
        essenceCountText.text = essenceCount.ToString();
        Vector3 itemBoxPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y - 0.75f, 0f));
        itemBoxUI.position = itemBoxPos;
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
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
                Destroy(this.gameObject);
            }
        }
    }
}
