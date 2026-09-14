using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public int countdownTime = 3;
    public int gameTime = 10;
    public TMP_Text ui_timer;
    public TMP_Text ui_countDown;
    private PointScorer ps;

    public static Action GameStart;
    public static Action TimerEnded;


    void Awake()
    {
        ui_countDown.text = countdownTime.ToString();
        ps = GetComponent<PointScorer>();
        ui_timer.text = gameTime.ToString();
    }
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(StartGameCountdown),1,1);
    }

    private void StartGameCountdown()
    {
        countdownTime--;
        ui_countDown.text = countdownTime.ToString();

        if(countdownTime == 0)
        {
            ui_countDown.text = "Start!";

            GameStart?.Invoke();
            InvokeRepeating(nameof(GameTimer),1,1);
            CancelInvoke(nameof(StartGameCountdown));
        }
    }


    private void GameTimer()
    {
        ui_countDown.text = "";

        gameTime--;
        ui_timer.text = gameTime.ToString();
        if(gameTime == 0)
        {
            ps.CheckCollection();

            TimerEnded?.Invoke();

            CancelInvoke(nameof(GameTimer));
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
