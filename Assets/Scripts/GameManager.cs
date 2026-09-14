using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private PointScorer ps;
    
    [Header("UI References")]
    public TMP_Text ui_timer;
    public TMP_Text ui_countDown;

    [Header("Timers")]
    public int countdownTime = 3;
    public int gameTime = 10;

    [Header("Events")]
    public static Action GameStart;
    public static Action TimerEnded;

    //Initialize variables
    void Awake()
    {
        ps = GetComponent<PointScorer>();
        ui_countDown.text = countdownTime.ToString();
        ui_timer.text = gameTime.ToString();
    }
    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating(nameof(StartGameCountdown), 1, 1);
    }

    //Set up the first countdown for players to move and control their crosshair without being able to shoot. The prep phase.
    private void StartGameCountdown()
    {
        countdownTime--;
        ui_countDown.text = countdownTime.ToString();

        if (countdownTime == 0)
        {
            ui_countDown.text = "Start!";

            GameStart?.Invoke();
            InvokeRepeating(nameof(GameTimer), 1, 1);
            CancelInvoke(nameof(StartGameCountdown));
        }
    }

    //Set up the gameplay timer once this ends scoring begins.
    private void GameTimer()
    {
        ui_countDown.text = "";

        gameTime--;
        ui_timer.text = gameTime.ToString();
        if (gameTime == 0)
        {
            ps.CheckCollection(); //Begin scoring through the point scorer.

            TimerEnded?.Invoke(); //Sends this information to the CrosshairBehavior basically preventing the player from acting while scoring happens. Also tells all flying cannonballs to be destroyed.

            CancelInvoke(nameof(GameTimer));
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
