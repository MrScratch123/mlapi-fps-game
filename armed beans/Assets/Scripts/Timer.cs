using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MLAPI;
using MLAPI.NetworkVariable;
public class Timer : NetworkBehaviour
{
    [SerializeField]
    int TimerLength;

    [SerializeField]
    Text timerText;

    NetworkVariable<float> timeLeft = new NetworkVariable<float>();

    

    bool timerStarted = false;
    public void StartTimer()
    {
        if (IsServer)
        {
            timeLeft.Value = TimerLength;
            timerStarted = true;
        }
    }

    void Update()
    {
        if (timerStarted && IsServer)
        {
            timeLeft.Value -= Time.deltaTime;
            if (timeLeft.Value <= 0)
            {
                VotingHandler.activeVotingHandler.TimerEnded();
            }
        }

        timerText.text = Mathf.RoundToInt(timeLeft.Value).ToString();

    }
}
