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

        cumText.text = "���� Hp: " + (int)(cumHP / 60) + "�� " + (int)(cumHP % 60) + "��";
    }

    public void IncreaseHP(float second)
    {
        nowHP += second;
        hpText.text = (int)(nowHP / 60) + "�� " + (int)(nowHP % 60) + "��" + " / " + (int)(maxHP / 60) + "�� " + (int)(maxHP % 60) + "��";
    }

    public void DecreaseHP(float second)
    {
        nowHP -= second;
        hpText.text = (int)(nowHP / 60) + "�� " + (int)(nowHP % 60) + "��" + " / " + (int)(maxHP / 60) + "�� " + (int)(maxHP % 60) + "��";
    }

    public void IncreaseCum(float second)
    {
        cumHP += second;
        if(cumHP > maxHP * 2)
        {
            statusText.text = "���Ͱ� ��ȭ�� �� �ֽ��ϴ�!";
        }
        else if(cumHP > maxHP * 1.8f)
        {
            statusText.text = "���ݸ� ��!";
        }
        else if (cumHP > maxHP * 1.5f)
        {
            statusText.text = "����? ������ ���°�?";
        }
        else if (cumHP > maxHP * 1.2f)
        {
            statusText.text = "�̻��� ���� ���͸� ���Ѵ�.";
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
