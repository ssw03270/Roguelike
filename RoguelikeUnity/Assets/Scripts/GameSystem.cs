using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class GameSystem : MonoBehaviour
{
    public List<List<string>> skillList = new List<List<string>>();     // 스킬 리스트
    public List<List<string>> enemyList = new List<List<string>>();     // 적 리스트
    public List<List<string>> itemList = new List<List<string>>();      // 아이템 리스트
    // Start is called before the first frame update
    void Start()
    {
        CsvReader("skillList");                                         // 스킬 리스트 정보 저장
        CsvReader("enemyList");                                         // 적 리스트 정보 저장
        CsvReader("itemList");                                          // 아이템 리스트 정보 저장 
    }

    /// <summary>
    /// csv 파일을 읽는 함수
    /// 주어진 filename에 해당하는 csv 파일의 정보를 읽어 리스트에 저장한다.
    /// </summary>
    /// <param name="fileName">csv 파일 이름</param>
    void CsvReader(string fileName)
    {
        var reader = new StreamReader(File.OpenRead(@"../RoguelikeUnity/Assets/Resources/CSV/" + fileName + ".csv"));

        while (!reader.EndOfStream)
        {
            var line = reader.ReadLine();
            var values = line.Split(',');
            List<string> list = new List<string>();
            foreach (var content in values)
            {
                list.Add(content);
            }

            if(fileName == "skillList")
            {
                skillList.Add(list);
            }
            else if(fileName == "enemyList")
            {
                enemyList.Add(list);
            }
            else if (fileName == "itemList")
            {
                itemList.Add(list);
            }
        }
    }
}
