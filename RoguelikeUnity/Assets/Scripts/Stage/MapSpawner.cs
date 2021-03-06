﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawner : MonoBehaviour
{

    public List<GameObject> mapList = new List<GameObject>();       // 예시 맵 저장된 리스트
    private PlayerController playerController;                      // 플레이어 컨트롤러 클래스
    private GameObject beforeMap;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    /// <summary>
    /// 새로운 맵 생성하는 함수
    /// 플레이어가 워프를 한 경우 새로운 맵을 생성한다.
    /// </summary>
    void Update()
    {
        if (playerController.isWarp)
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            playerController.stageNum += 1;
            playerController.isWarp = false;
            playerController.ResetPosition();


            GameObject nowMap = null;
            if (playerController.stageNum > 0 && playerController.stageNum <= 20)
            {
                nowMap = mapList[Random.Range(0, 5)];
                while (beforeMap == nowMap)
                {
                    nowMap = mapList[Random.Range(0, 5)];
                }
                if (playerController.stageNum == 5 || playerController.stageNum == 15)
                {
                    nowMap = mapList[5];
                }
                else if (playerController.stageNum == 10)
                {
                    nowMap = mapList[6];
                }
                else if (playerController.stageNum == 20)
                {
                    nowMap = mapList[7];
                }
            }
            Instantiate(nowMap, transform.position, transform.rotation).transform.parent = this.gameObject.transform;
            beforeMap = nowMap;
        }
    }
}
