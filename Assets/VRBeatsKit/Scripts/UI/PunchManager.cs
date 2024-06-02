using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Punch Area击打区域
/// Perfect，Good判断
/// </summary>
public class PunchManager : MonoBehaviour
{
    private Text m_Lable;

    private string[] Lables = { "Miss", "Good", "Pefect" };

    private void Awake()
    {
        m_Lable = gameObject.GetComponentInChildren<Text>(true);
    }

    public void OnPunchArea(int area)
    {
        m_Lable.text = Lables[area];
    }
}