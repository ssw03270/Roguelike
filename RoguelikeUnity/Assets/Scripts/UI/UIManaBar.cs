using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManaBar : MonoBehaviour
{
    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    /// <summary>
    /// 마나바를 설정하는 함수
    /// 최대 마나를 바탕으로 마나바의 최대치와 현재 마나바를 설정한다.
    /// </summary>
    /// <param name="mana">주어진 최대 마나</param>
    public void SetMaxMana(int mana)
    {
        try
        {
            slider.maxValue = mana;
            slider.value = mana;
        }
        catch
        {
            Debug.Log("Error while set the resource bar.");
        }
    }

    /// <summary>
    /// 현재 마나바를 설정하는 함수
    /// 현재 마나를 바탕으로 현재 마나바를 설정한다.
    /// </summary>
    /// <param name="mana">현재 마나</param>
    public void SetCurrentMana(int mana)
    {
        slider.value = mana;
    }
}
