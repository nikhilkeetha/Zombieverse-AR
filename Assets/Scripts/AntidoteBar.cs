using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AntidoteBar : MonoBehaviour
{
    private Image Bar;

    public int totalTime,currentTime;
    public float perce;
    gameManager manager;

    private void Start()
    {
        Bar =GetComponent<Image>();
        manager=GameObject.Find("UI").GetComponent<gameManager>();
        totalTime=manager.andidoteTime;;
    }

    public void Update()
    {
        currentTime=manager.andidoteTime;
        perce=(float)currentTime/totalTime * 100f;
        Bar.fillAmount=perce/100f;
    }
}
