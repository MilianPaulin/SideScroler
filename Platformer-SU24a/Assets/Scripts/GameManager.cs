using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField]int Score = 0;
    [SerializeField] TextMeshProUGUI scoreText;

    private void Awake()
    {
        scoreText.text = "Score: " + Score.ToString();
    }

    public void AddToScore(int pointsToAdd)
    {
        Score += pointsToAdd;
        scoreText.text = "Score: " + Score.ToString();
    }
}
