using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{
    private Slider slider;

    private void Start()
    {
        slider = GetComponent<Slider>();
    }

    /// <summary>
    /// 체력바를 설정하는 함수
    /// 최대 체력을 바탕으로 체력바의 최대치와 현재 체력바를 설정한다.
    /// </summary>
    /// <param name="health">최대 체력</param>
    public void SetMaxHealth(int health)
    {

        slider.maxValue = health;
        slider.value = health;
    }

    /// <summary>
    /// 현재 체력바를 설정하는 함수
    /// 현재 체력을 바탕으로 현재 체력바를 설정한다.
    /// </summary>
    /// <param name="health">현재 체력</param>
    public void SetCurrentHealth(int health)
    {
        slider.value = health;
    }
}
