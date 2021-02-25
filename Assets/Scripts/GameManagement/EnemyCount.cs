using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCount : MonoBehaviour
{
    public GameManager gameManager;
    public Text enemyText;

    private float timer;
    private bool loopActive = true;

    void Start()
    {
        StartCoroutine(FlashText());
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
    }

    void Update()
    {
        if (loopActive)
        {
            timer = Time.time;
            if (timer >= 5)
            {
                GameManager gm = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
                loopActive = false;
                EnemyCountUpdate(gameManager.deadCount, gm.EnemyCurrentCount());
            }
        }
    }

    public void EnemyCountUpdate(int deadCount, int totalCount)
    {
        enemyText.text = deadCount + " / " + totalCount + "  enemies dead";
    }

    public IEnumerator FlashText()
    {
        while (loopActive)
        {
            //Flip the active state of goldText
            enemyText.enabled = !enemyText.enabled; 
            yield return new WaitForSeconds(0.5f);
        }
        enemyText.enabled = true;
    }
}
