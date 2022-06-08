using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Pedometer : MonoBehaviour
{
    public Text statusText, stepsText;
    public GameObject hpImage, hpText, cumText, statText;
    public HpManager hpManager;
    public GameObject hatchButton, evolButton, releaseButton, restartButton, quitButton;
    public GameObject hatchPopup, evolPopup, releasePopup, restartPopup, quitPopup;
    public GameObject gameOverPopup;
    public float lowLimit = 0.005f;//slow
    public float highLimit = 0.1f;//peaks and valleys when walking
    public float vertHighLimit = 0.25f;//The peak and valley when jumping
    private bool isHigh = false;//status
    private float filterCurrent = 10.0f;//filter parameters to get the fitted value
    private float filterAverage = 0.1f;//Filter parameters to get the average
    private float accelerationCurrent = 0f;//fitting value
    private float accelerationAverage = 0f;//Average
    public int steps;//number of steps
    private int oldSteps;
    private float deltaTime = 0f;//Timer
    private int jumpCount = 0;//jump count
    private int oldjumpCount = 0;
    private float time;
    private HpManager hp;

    private bool startTimer = false;//Start timing
    private bool isWalking = false;
    private bool isJumping = false;

    void Awake()
    {
        hp = FindObjectOfType<HpManager>();
        accelerationAverage = Input.acceleration.magnitude;
        oldSteps = steps;
        oldjumpCount = jumpCount;

        time = 0;
    }

    void Update()
    {
        checkWalkingAndJumping();//Check whether to walk

        if(GameManager.gameManager.status == "Egg")
        {
            stepsText.gameObject.SetActive(true);
            hatchButton.SetActive(true);
            restartButton.SetActive(true);
            hpImage.GetComponent<Image>().color = new Color(1, 1, 1, 0);
            hpImage.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0);
            hpText.SetActive(false);
            cumText.SetActive(false);
            statText.SetActive(false);
            evolButton.SetActive(false);
            releaseButton.SetActive(false);

            if (steps < 100)
            {
                GameManager.gameManager.grade = "Low";
            }
            else if (steps < 300)
            {
                GameManager.gameManager.grade = "Middle";
            }
            else
            {
                GameManager.gameManager.grade = "High";
            }
        }
        else if(GameManager.gameManager.status == "First")
        {
            stepsText.gameObject.SetActive(false);
            hatchButton.SetActive(false);
            restartButton.SetActive(false);
            hpImage.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            hpImage.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
            hpText.SetActive(true);
            cumText.SetActive(true);
            statText.SetActive(true);
            evolButton.SetActive(true);
            releaseButton.SetActive(true);
        }
        else
        {
            stepsText.gameObject.SetActive(false);
            hatchButton.SetActive(false);
            restartButton.SetActive(false);
            hpImage.GetComponent<Image>().color = new Color(1, 1, 1, 1);
            hpImage.transform.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 1);
            hpText.SetActive(true);
            cumText.SetActive(false);
            statText.SetActive(false);
            evolButton.SetActive(false);
            releaseButton.SetActive(true);
        }

        if(GameManager.gameManager.status == "First" || GameManager.gameManager.status == "Second")
        {
            if(hpManager.nowHP <= 0)
            {
                gameOverPopup.SetActive(true);
                gameOverPopup.transform.GetChild(1).GetComponent<Text>().text = "최대 HP: " + hpManager.maxHP + "\n진화정도: ";
                if(GameManager.gameManager.status == "Egg")
                {
                    gameOverPopup.transform.GetChild(1).GetComponent<Text>().text += "0단계";
                }
                else if (GameManager.gameManager.status == "First")
                {
                    gameOverPopup.transform.GetChild(1).GetComponent<Text>().text += "1단계";
                }
                else
                {
                    gameOverPopup.transform.GetChild(1).GetComponent<Text>().text += "2단계";
                }
                gameOverPopup.transform.GetChild(1).GetComponent<Text>().text += "\n몬스터 종류: " + GameManager.gameManager.monsterName + "\n플레이어 등급: " + GameManager.gameManager.monsterGrade;

                Time.timeScale = 0;
            }
        }

        if(time < 10)
        {
            time += Time.deltaTime;
        }

        stepsText.text = "걸음수: " + steps;

        /*if (isWalking)
        {
            statusText.text = ("Status: Walking");

        }
        else if (!isWalking)
        {
            statusText.text = ("Status: Not Moving");
        }

        if (isJumping)
        {
            statusText.text = ("Status: Jumping");
        }*/
    }

    void FixedUpdate()
    {

        //Filter Input.acceleration.magnitude (acceleration scalar sum) through Lerp
        //The linear interpolation formula used here is exactly the EMA one-time exponential filtering y[i]=y[i-1]+(x[i]-y[i])*k=(1-k)*y[i] +kx[i]
        accelerationCurrent = Mathf.Lerp(accelerationCurrent, Input.acceleration.magnitude, Time.deltaTime * filterCurrent);
        accelerationAverage = Mathf.Lerp(accelerationAverage, Input.acceleration.magnitude, Time.deltaTime * filterAverage);
        float delta = accelerationCurrent - accelerationAverage;//Get the difference, that is, the slope

        if (!isHigh)
        {
            if (delta > highLimit && time >= 10)//Go high
            {
                isHigh = true;
                if(GameManager.gameManager.status == "Egg")
                {
                    steps++;
                }
                if(hp.isMax())
                {
                    if(GameManager.gameManager.status == "First")
                    {
                        hp.IncreaseCum(10);
                    }
                }
                else
                {
                    hp.IncreaseHP(10);
                }
            }
            if (delta > vertHighLimit)
            {
                isHigh = true;
                jumpCount++;
                //stepsText.text = "Number of steps: " + steps + "\nNumber of jumps:" + jumpCount;
            }
        }
        else
        {
            if (delta < lowLimit)//lower
            {

                isHigh = false;
            }
        }
    }


    private void checkWalkingAndJumping()
    {
        if ((steps != oldSteps) || (oldjumpCount != jumpCount))
        {
            startTimer = true;
            deltaTime = 0f;
        }

        if (startTimer)//Timer, make it slower to update the UI, because you can’t walk in only one frame QAQ
        {
            deltaTime += Time.deltaTime;

            if (deltaTime != 0)
            {
                if (oldjumpCount != jumpCount)//Check if it is a jump
                    isJumping = true;
                else
                    isWalking = true;

            }
            if (deltaTime > 2)
            {
                deltaTime = 0F;
                startTimer = false;
            }
        }
        else if (!startTimer)
        {
            isWalking = false;
            isJumping = false;
        }
        oldSteps = steps;
        oldjumpCount = jumpCount;
    }

    public void Hatch()
    {
        hpManager.maxHP = steps * 60;
        hpManager.nowHP = steps * 60;
        hpManager.cumHP = 0;
        switch(GameManager.gameManager.grade)
        {
            case "Low":
                GameManager.gameManager.monsterName = "노말 슬라임";
                GameManager.gameManager.monsterGrade = "호크아이";
                break;

            case "Middle":
                GameManager.gameManager.monsterName = "병사 슬라임";
                GameManager.gameManager.monsterGrade = "팔콘";
                break;

            case "High":
                GameManager.gameManager.monsterName = "씨앗 슬라임";
                GameManager.gameManager.monsterGrade = "비전";
                break;
        }
        steps = 0;
        GameManager.gameManager.status = "First";
        hatchPopup.SetActive(false);
    }

    public void HatchPopup()
    {
        if(!hatchPopup.activeSelf && !evolPopup.activeSelf && !gameOverPopup.activeSelf && !releasePopup.activeSelf && !restartPopup.activeSelf && !quitPopup.activeSelf)
        {
            hatchPopup.SetActive(true);
        }
    }

    public void HatchPopDown()
    {
        hatchPopup.SetActive(false);
    }

    public void Evolution()
    {
        if(hpManager.cumHP >= hpManager.maxHP * 2)
        {
            hpManager.maxHP = hpManager.cumHP;
            hpManager.nowHP = hpManager.maxHP;
            hpManager.cumHP = 0;
            switch (GameManager.gameManager.grade)
            {
                case "Low":
                    GameManager.gameManager.monsterName = "킹 슬라임";
                    GameManager.gameManager.monsterGrade = "아이언맨";
                    break;

                case "Middle":
                    GameManager.gameManager.monsterName = "전사 슬라임";
                    GameManager.gameManager.monsterGrade = "토르";
                    break;

                case "High":
                    GameManager.gameManager.monsterName = "풀 슬라임";
                    GameManager.gameManager.monsterGrade = "헐크";
                    break;
            }
            steps = 0;
            GameManager.gameManager.status = "Second";
            evolPopup.SetActive(false);
        }
    }

    public void EvolutionPopUp()
    {
        if (!hatchPopup.activeSelf && !evolPopup.activeSelf && !gameOverPopup.activeSelf && !releasePopup.activeSelf && !restartPopup.activeSelf && !quitPopup.activeSelf)
        {
            if (hpManager.cumHP >= hpManager.maxHP * 2)
            {
                evolPopup.SetActive(true);
            }
        }
    }

    public void EvolutionPopDown()
    {
        evolPopup.SetActive(false);
    }

    public void Release()
    {
        releasePopup.SetActive(false);
        hpManager.nowHP = 0;
    }

    public void ReleasePopUp()
    {
        if (!hatchPopup.activeSelf && !evolPopup.activeSelf && !gameOverPopup.activeSelf && !releasePopup.activeSelf && !restartPopup.activeSelf && !quitPopup.activeSelf)
        {
            releasePopup.SetActive(true);
        }
    }

    public void ReleasePopDown()
    {
        releasePopup.SetActive(false);
    }

    public void Restart()
    {
        restartPopup.SetActive(false);
        steps = 0;
    }

    public void RestartPopUp()
    {
        if (!hatchPopup.activeSelf && !evolPopup.activeSelf && !gameOverPopup.activeSelf && !releasePopup.activeSelf && !restartPopup.activeSelf && !quitPopup.activeSelf)
        {
            restartPopup.SetActive(true);
        }
    }

    public void RestartPopDown()
    {
        restartPopup.SetActive(false);
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void QuitPopUp()
    {
        if (!hatchPopup.activeSelf && !evolPopup.activeSelf && !gameOverPopup.activeSelf && !releasePopup.activeSelf && !restartPopup.activeSelf && !quitPopup.activeSelf)
        {
            quitPopup.SetActive(true);
        }
    }

    public void QuitPopDown()
    {
        quitPopup.SetActive(false);
    }

    public void GameOver()
    {
        GameManager.gameManager.status = "Egg";
        GameManager.gameManager.grade = "Low";
        GameManager.gameManager.monsterName = "알";
        GameManager.gameManager.monsterGrade = "";
        Time.timeScale = 1;
        SceneManager.LoadScene("Main");
    }
}
