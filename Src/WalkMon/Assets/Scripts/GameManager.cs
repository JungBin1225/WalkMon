using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager gameManager;

    public string status;
    public string grade;
    public string monsterName;
    public string monsterGrade;

    private Vector3 touchPosition;
    private bool input;

    private void Awake()
    {
        if (gameManager == null)
            gameManager = this;

        else if (gameManager != this)
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        if (status == "")
        {
            status = "Egg";
        }

        if(grade == "")
        {
            grade = "Low";
        }

        if(monsterName == "")
        {
            monsterName = "¾Ë";
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
