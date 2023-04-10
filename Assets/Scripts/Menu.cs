using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject panel;
    // Start is called before the first frame update
    public void startGame(){
        SceneManager.LoadScene("SampleScene");
    }
    public void quitGame(){
        Application.Quit();
        Debug.Log("QUIT");
        }

    public void Controls(){
        panel.SetActive(true);
    }
    public void Close(){
        panel.SetActive(false);
    }
}
