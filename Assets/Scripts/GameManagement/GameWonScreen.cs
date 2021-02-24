using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine;

public class GameWonScreen : MonoBehaviour
{
    public Text pointsText;

    public void Setup(int score)
    {
        gameObject.SetActive(true);
        pointsText.text = "Total points: " + score.ToString();
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
