using UnityEngine;
using UnityEngine.UI;


public class Score : MonoBehaviour
{
    public Text scoreText;
    public AudioSource scoreUpdate;
    public int currentScore = 0;
    
    public void UpdateScore(int score)
    {
        currentScore += score;
        scoreUpdate.Play();
        scoreText.text = "SCORE: " + currentScore.ToString();
    }

    public int getScore()
    {
        return currentScore;
    }
}
