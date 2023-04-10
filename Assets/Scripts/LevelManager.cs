using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{

    public int asteroidCount;
    public int levelNumber;
    public GameObject asteroid1;
    public GameObject asteroid2;
    

    public Text textLevel;

    void Start(){
        textLevel.text = "Level number : " + levelNumber;
    }
    // Start is called before the first frame update
    public void UpdateAsteroidCount(int change){
        asteroidCount += change;
        if(asteroidCount<=0){
            Invoke("StartLevel" , 3f);
        }
    }
    void StartLevel(){
        levelNumber ++;
        textLevel.text = "Level number : " + levelNumber;
        for (int i = 0; i < levelNumber*2; i++)
        {
            asteroidCount++;
            Vector2 position = new Vector2(Random.Range(0f,Screen.width),Screen.height);
            int x = Random.Range(2, 4);
            if (x==2){
                Instantiate(asteroid2,position,Quaternion.identity);
            }
            else{
                Instantiate(asteroid1,position,Quaternion.identity);
            }
            
        }
    }
}
