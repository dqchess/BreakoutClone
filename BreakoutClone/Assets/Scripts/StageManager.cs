﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour {
    public Ball ball;
    public Platform platform;
    public string nextStage;
    public bool finalStage = false;

    [Header("UI Elements")]
    public Text textLifeCount;
    public Text textScore;
    public Canvas gameOverScreen;
    public Canvas victoryScreen;
    public Canvas pauseScreen;

    private int brickCount;

    public static StageManager Instance { get; set; }
    public bool IsFirstLaunch { get; set; }
    public bool IsPaused { get{ return Time.timeScale == 0; } }

    private int _score;
    public int Score { 
        get
        {
            return _score;
        } 
        set
        {
            _score = value;

            SetScoreText(value);
        } 
    }
    public Transform Barrier { get; set; }


    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(gameObject);

        //DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        brickCount = GameObject.FindGameObjectsWithTag("Brick").Length;
        Barrier = transform.Find("Barrier");

        IsFirstLaunch = true;

        SetScoreText(Score);
        SetLifeCountText(GameManager.Instance.CurrentGame.LifeCount);

        Cursor.visible = false;
        Time.timeScale = 1;
    }

    void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            if(IsPaused)
                UnPause();
            else
                Pause();
        }
    }

    public void AddScore(int score)
    {
        if(GameManager.Instance.CurrentGame.Mode == GameMode.Hardcore)
            score *= 2;

        StageManager.Instance.Score += score;
    }

    public void ResetBall()
    {
        PowerUpHolder.Instance.Clear();
        RemovePowerUpsAtScreen();

        platform.ResetPosition();
        platform.ResetSize();
        ball.ResetPosition();

        IsFirstLaunch = true;
    }

    public void GameOver()
    {
        AudioManager.Instance.Play(AudioManager.Instance.sfxGameOver);

        Cursor.visible = true;
        Time.timeScale = 0;
        gameOverScreen.gameObject.SetActive(true);

        GameManager.Instance.CurrentGame.Score += StageManager.Instance.Score;
        GameManager.Instance.AddScoreToLeaderBoard(GameManager.Instance.CurrentGame.Score);
    }

    public void Victory()
    {
        //TODO: Tocar som para vitória!

        Cursor.visible = true;
        Time.timeScale = 0;
        victoryScreen.gameObject.SetActive(true);
    }

    public void Pause()
    {
        Cursor.visible = true;
        Time.timeScale = 0;
        pauseScreen.gameObject.SetActive(true);
    }

    public void UnPause()
    {
        Cursor.visible = false;
        Time.timeScale = 1;
        pauseScreen.gameObject.SetActive(false);
    }

    public void RemovePowerUpsAtScreen()
    {
        GameObject[] bricks = GameObject.FindGameObjectsWithTag("BrickPowerUp");

        foreach(GameObject obj in bricks)
        {
            Destroy(obj);
        }
    }

    public void SetScoreText(int score)
    {
        if(textScore != null)
            textScore.text = score.ToString();
    }

    public void SetLifeCountText(int lifeCount)
    {
        if(textLifeCount != null)
            textLifeCount.text = lifeCount.ToString();

        if(lifeCount <= 0)
            GameOver();
    }

    public void DecreaseBrick()
    {
        brickCount--;

        if(brickCount <= 0)
            Victory();
    }

    void OnDestroy()
    {
        Cursor.visible = true;
    }
}
