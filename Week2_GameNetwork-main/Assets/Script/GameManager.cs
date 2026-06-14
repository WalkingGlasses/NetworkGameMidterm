using System;
using Unity.Netcode;
using UnityEngine;
using Unity.Collections;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;
    public NetworkVariable<int> player1Score =
    new NetworkVariable<int>();

    public NetworkVariable<int> player2Score =
        new NetworkVariable<int>();

    public NetworkVariable<float> timer =
    new NetworkVariable<float>(60);

    public NetworkVariable<FixedString64Bytes>
    winnerMessage =
        new NetworkVariable<FixedString64Bytes>("");

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (!IsServer) return;

        //servertimc checker/debugger
        //Debug.Log("SERVER TIMER: " + timer.Value);

        if (timer.Value > 0)
        {
            timer.Value -= Time.deltaTime;

            if (timer.Value <= 0)
            {
                EndGame();
            }
        }
    }

    public void AddScore(ulong shooterID)
    {
        if (shooterID == 0)
        {
            player1Score.Value++;
        }
        else
        {
            player2Score.Value++;
        }
    }

    private void EndGame()
    {
        timer.Value = 0;

        if (player1Score.Value >
           player2Score.Value)
        {
            winnerMessage.Value =
                "PLAYER 1 WINS!";
        }
        else if (player2Score.Value >
                player1Score.Value)
        {
            winnerMessage.Value =
                "PLAYER 2 WINS!";
        }
        else
        {
            winnerMessage.Value =
                "DRAW!";
        }
    }

}
