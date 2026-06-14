using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TMP_Text player1ScoreText;
    [SerializeField] private TMP_Text player2ScoreText;
    [SerializeField] private TMP_Text timerText;
    [SerializeField] private TMP_Text winnerText;

    private void Update()
    {
        if (GameManager.Instance == null)
            return;

        player1ScoreText.text =
            "P1: " +
            GameManager.Instance.player1Score.Value;

        player2ScoreText.text =
            "P2: " +
            GameManager.Instance.player2Score.Value;

        timerText.text =
            Mathf.CeilToInt(
                GameManager.Instance.timer.Value
            ).ToString();

        winnerText.text =
            GameManager.Instance.winnerMessage.Value.ToString();
    }
}