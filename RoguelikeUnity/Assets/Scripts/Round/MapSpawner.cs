using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSpawner : MonoBehaviour
{

    public List<GameObject> mapList = new List<GameObject>();
    private PlayerController playerController;

    // Start is called before the first frame update
    void Start()
    {
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerController.isWarp)
        {
            for(int i = 0; i < transform.childCount; i++)
            {
                Destroy(transform.GetChild(i).gameObject);
            }
            Debug.Log(playerController.roundNum);
            playerController.roundNum += 1;
            playerController.isWarp = false;
            playerController.ResetPosition();
            Instantiate(mapList[Random.Range(0, mapList.Count)], transform.position, transform.rotation).transform.parent = this.gameObject.transform;
        }
    }
}
