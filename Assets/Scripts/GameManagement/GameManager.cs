using UnityEngine;
using UnityEngine.SceneManagement;

using System.Collections;
using System.Collections.Generic;

using UnityEngine.AI;
using Assets.Scripts.EnemyBehaviour;

public class GameManager : MonoBehaviour
{
    //References to other objects
    public GameOverScreen gameOverScreen;
    public GameWonScreen gameWonScreen;
    public MouseLook mouseLook;
    public FpsPlayerMovement player;
    public EnemyCount enemyCount;
    public Score score;
    public Gun gun;
    public SquadParent squadParent;
    
    //
    public List<GameObject> listOfEnemies = new List<GameObject>();
    public int totalEnemies;
    public int deadCount = 0;
    bool gameEnd = false;
    public float wait = 1f;
    bool init = true;

    void Start()
    {
        
    }

    private void Update()
    {
        if (init == true)
        {
            //Creates a list of all enemies in a scene
            listOfEnemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
            totalEnemies = listOfEnemies.Count;
            Debug.Log(totalEnemies);
            init = false;
        }
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
        init = true;

        //Show game over screen
        gameOverScreen.Setup(endScore);
    }
    
    public void GameWin(int endScore)
    { 
        Cursor.lockState = CursorLockMode.None;
        gun.gunEnabled = false;
        mouseLook.mouseEnabled = false;
        player.playerEnabled = false;
        init = true;

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
            enemyCount.EnemyCountUpdate(deadCount, totalEnemies);
        }

        //When alll enemies are dead
        if (listOfEnemies.Count <= 0)
        {
            GameWin(score.currentScore);
        }
    }

    public int EnemyCurrentCount()
    {
        List<GameObject> listOfEnemies = new List<GameObject>();
        //Creates a list of all enemies in a scene
        listOfEnemies.AddRange(GameObject.FindGameObjectsWithTag("Enemy"));
        totalEnemies = listOfEnemies.Count;
        Debug.Log(totalEnemies);
        //init = false;

        return totalEnemies;
    }
}
