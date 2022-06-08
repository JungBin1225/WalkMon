using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Globalization;
using System;
using UnityEngine;

public class DataController : MonoBehaviour
{
    static GameObject _container;
    static GameObject Container
    {
        get
        {
            return _container;
        }
    }
    static DataController _instance;
    public static DataController Instance
    {
        get
        {
            if (!_instance)
            {
                _container = new GameObject();
                _container.name = "DataController";
                _instance = _container.AddComponent(typeof(DataController)) as DataController;
                DontDestroyOnLoad(_container);
            }
            return _instance;
        }
    }

    public string GameDataFileName = "Data.json";

    public GameData _gameData;
    public GameData gameData
    {
        get
        {
            if(_gameData == null)
            {
                LoadGameData();
                SaveGameData();
            }
            return _gameData;
        }
    }

    void Start()
    {
        LoadGameData();
        SaveGameData();
    }

    public void LoadGameData()
    {
        string filePath = Application.persistentDataPath + GameDataFileName;

        if(File.Exists(filePath))
        {
            Debug.Log("불러오기 성공");
            string FromJsonData = File.ReadAllText(filePath);
            _gameData = JsonUtility.FromJson<GameData>(FromJsonData);

            GameManager.gameManager.status = _gameData.status;
            GameManager.gameManager.grade = _gameData.grade;
            GameManager.gameManager.monsterName = _gameData.monsterName;
            GameManager.gameManager.monsterGrade = _gameData.monsterGrade;

            FindObjectOfType<Pedometer>().steps = _gameData.steps;
            FindObjectOfType<HpManager>().maxHP = _gameData.maxHP;
            FindObjectOfType<HpManager>().nowHP = _gameData.nowHP;
            FindObjectOfType<HpManager>().cumHP = _gameData.cumHP;
            FindObjectOfType<HpManager>().IncreaseCum(0);

            DateTime lastAccess = DateTime.ParseExact(_gameData.accessDate, "yyyy-MM-dd-HH-mm-ss", CultureInfo.InvariantCulture);
            TimeSpan timeCal = DateTime.Now - lastAccess;
            Debug.Log(timeCal);
            Debug.Log((float)timeCal.TotalSeconds);
            FindObjectOfType<HpManager>().nowHP -= (float)timeCal.TotalSeconds;
        }
        else
        {
            Debug.Log("새로운 파일 생성");
            _gameData = new GameData();
        }
    }

    public void SaveGameData()
    {
        gameData.status = GameManager.gameManager.status;
        gameData.grade = GameManager.gameManager.grade;
        gameData.monsterName = GameManager.gameManager.monsterName;
        gameData.monsterGrade = GameManager.gameManager.monsterGrade;

        gameData.steps = FindObjectOfType<Pedometer>().steps;
        gameData.maxHP = FindObjectOfType<HpManager>().maxHP;
        gameData.nowHP = FindObjectOfType<HpManager>().nowHP;
        gameData.cumHP = FindObjectOfType<HpManager>().cumHP;

        gameData.accessDate = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss");

        string ToJsonData = JsonUtility.ToJson(gameData);
        string filePath = Application.persistentDataPath + GameDataFileName;

        File.WriteAllText(filePath, ToJsonData);
    }

    void Update()
    {
        SaveGameData();
    }
}
