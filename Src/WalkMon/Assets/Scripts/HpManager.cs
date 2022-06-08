using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpManager : MonoBehaviour
{
    public float maxHP;
    public float cumHP;
    public Text hpText;
    public Text cumText;
    public Text statusText;

    public float nowHP;
    private Image hpBar;

    void Start()
    {
        hpBar = GetComponent<Image>();
    }


    void Update()
    {
        if (GameManager.gameManager.status != "Egg")
        {
            DecreaseHP(Time.deltaTime);
        }

        hpBar.fillAmount = nowHP / maxHP;

        cumText.text = "누적 Hp: " + (int)(cumHP / 60) + "분 " + (int)(cumHP % 60) + "초";
    }

    public void IncreaseHP(float second)
    {
        nowHP += second;
        hpText.text = (int)(nowHP / 60) + "분 " + (int)(nowHP % 60) + "초" + " / " + (int)(maxHP / 60) + "분 " + (int)(maxHP % 60) + "초";
    }

    public void DecreaseHP(float second)
    {
        nowHP -= second;
        hpText.text = (int)(nowHP / 60) + "분 " + (int)(nowHP % 60) + "초" + " / " + (int)(maxHP / 60) + "분 " + (int)(maxHP % 60) + "초";
    }

    public void IncreaseCum(float second)
    {
        cumHP += second;
        if(cumHP > maxHP * 2)
        {
            statusText.text = "몬스터가 진화할 수 있습니다!";
        }
        else if(cumHP > maxHP * 1.8f)
        {
            statusText.text = "조금만 더!";
        }
        else if (cumHP > maxHP * 1.5f)
        {
            statusText.text = "오잉? 몬스터의 상태가?";
        }
        else if (cumHP > maxHP * 1.2f)
        {
            statusText.text = "이상한 빛이 몬스터를 감싼다.";
        }
        else
        {
            statusText.text = "";
        }
    }

    public bool isMax()
    {
        return nowHP > maxHP;
    }
}
