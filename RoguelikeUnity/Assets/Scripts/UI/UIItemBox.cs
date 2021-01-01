using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIItemBox : MonoBehaviour
{
    private GameObject canvas;                      // 캔버스
    public GameObject itemBoxObj;                   // 아이템 박스 UI 오브젝트
    RectTransform itemBoxUI;                        // 캔버스에서의 아이템 박스 UI
    private TextMeshProUGUI essenceCount;           // 아이템 박스에 표기할 정수 갯수 
    // Start is called before the first frame update
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        itemBoxUI = Instantiate(itemBoxObj, canvas.transform).GetComponent<RectTransform>();
        essenceCount = itemBoxUI.GetChild(0).GetComponent<TextMeshProUGUI>();
        essenceCount.text = Random.Range(1, 5).ToString();
        Vector3 itemBoxPos = Camera.main.WorldToScreenPoint(new Vector3(transform.position.x, transform.position.y - 0.75f, 0f));
        itemBoxUI.position = itemBoxPos;
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnDestroy()
    {
        Destroy(itemBoxUI.gameObject);
    }
}
