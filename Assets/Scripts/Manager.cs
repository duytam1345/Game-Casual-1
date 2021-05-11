using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager : MonoBehaviour
{
    public static Manager manager;

    public int curLevel;

    public bool enableCamFollowPlayer;

    public Transform tMap;

    public int scoreDiamond;
    public int scoreAdd;
    public Text textScoreDiamond;
    public Text textScoreAdd;

    public Image[] imgShowLevel;

    public enum StateGame
    {
        PressToPlay,//hien thi so level hien tai, map hien tai va cham de bat dau choi
        Playing,//dang choi
        PauseGame,//tam dung
        WinLose,//
    }

    public StateGame stateGame;


    public Text textTouchToPlay;


    public GameObject showLevelUI;
    public GameObject barUI;

    public GameObject winPanel;
    public Text textMultiScore;
    public Text textToalScore;

    public GameObject losePanel;

    public float zPosEndgame;

    public Transform tTemp;

    public Image fillAmount;
    public Text textLevelCur;

    private void Awake()
    {
        if (!manager) {
            manager = this;
        }
    }

    private void Start()
    {
        SetStartLevel();
    }
    public void SetStartLevel()
    {
        textLevelCur.text = curLevel.ToString();

        scoreAdd = 0;
        textScoreAdd.text = scoreAdd.ToString();


        foreach (Transform item in tTemp) {
            Destroy(item.gameObject);
        }

        enableCamFollowPlayer = true;

        textTouchToPlay.gameObject.SetActive(true);

        winPanel.SetActive(false);
        losePanel.SetActive(false);

        stateGame = StateGame.PressToPlay;

        if (tMap.childCount > 0) {
            Destroy(tMap.GetChild(0).gameObject);
        }

        for (int i = 0; i < curLevel; i++) {
            imgShowLevel[i].color = new Color(0, .5f, 1, 1);
        }

        GameObject g = Instantiate(Resources.Load("Level/" + "Level " + curLevel) as GameObject, tMap);
    }

    public void ToPlay()
    {
        stateGame = StateGame.Playing;

        textTouchToPlay.gameObject.SetActive(false);

        showLevelUI.SetActive(false);
        barUI.SetActive(true);
    }

    public void WinGame()
    {
        enableCamFollowPlayer = false;

        curLevel++;

        if(curLevel == 4) {
            curLevel = 1;
        }

        stateGame = StateGame.WinLose;

        barUI.SetActive(false);
        showLevelUI.SetActive(false);
        winPanel.SetActive(true);
    }

    public void LoseGame()
    {
        enableCamFollowPlayer = false;

        stateGame = StateGame.WinLose;

        barUI.SetActive(false);
        showLevelUI.SetActive(false);
        losePanel.SetActive(true);
    }
}
