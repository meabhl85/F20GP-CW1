using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;
using System.Collections.Generic;

using UnityEngine.AI;



public class GameManager : MonoBehaviour
{
    //References to other objects
    public GameOverScreen gameOverScreen;
    public GameWonScreen gameWonScreen;
    public MouseLook mouseLook;
    public FpsPlayerMovement player;
    public EnemyCount zombieCount;
    public Score score;
    public Gun gun;
    
    //
    public List<GameObject> listOfEnemies = new List<GameObject>();
    public int totalEnemies;
    public int deadCount = 0;
    bool gameEnd = false;
    public float wait = 1f;

    void Start()
    {
        //Creates a list of all enemies in a scene
        listOfEnemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        totalEnemies = listOfEnemies.Count;
    }

    public void EndGame(int endScore)
    {
        if (gameEnd == false)
        {
            gameEnd = true;
            GameOver(endScore);
        }
    }
    
    public void GameOver(int endScore)
    {
        Cursor.lockState = CursorLockMode.None;
        gun.gunEnabled = false;
        mouseLook.mouseEnabled = false;
        player.playerEnabled = false;

        //Show game over screen
        gameOverScreen.Setup(endScore);
    }
    
    public void GameWin(int endScore)
    { 
        Cursor.lockState = CursorLockMode.None;
        gun.gunEnabled = false;
        mouseLook.mouseEnabled = false;
        player.playerEnabled = false;

        //Show game win screen
        gameWonScreen.Setup(endScore);
    }

    public void KilledEnemy(GameObject enemy)
    {
        deadCount += 1;

        //Update enemy list
        if (listOfEnemies.Contains(enemy))
        {
            listOfEnemies.Remove(enemy);
            zombieCount.EnemyCountUpdate(deadCount, totalEnemies);
        }

        //When alll enemies are dead
        if (listOfEnemies.Count <= 0)
        {
            GameWin(score.currentScore);
        }
    }
}
