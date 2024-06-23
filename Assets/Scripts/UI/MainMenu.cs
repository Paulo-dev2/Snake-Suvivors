using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject homeMenu;
    [SerializeField] private GameObject optionMenu;
    [SerializeField] private GameObject infoMenu;
    
    public void LoadGame()
    {
        SceneManager.LoadScene(1);
    }

    public void OpenOptions(){
        homeMenu.SetActive(false);
        optionMenu.SetActive(true);
    }

    public void CloseOptions(){
        homeMenu.SetActive(true);
        optionMenu.SetActive(false);
    }

    public void OpenInfo(){
        homeMenu.SetActive(false);
        infoMenu.SetActive(true);
    }

    public void CloseInfo(){
        homeMenu.SetActive(true);
        infoMenu.SetActive(false);
    }

    public void ExitGame(){
        Debug.Log("Exit to Game");
        Application.Quit();
    }

}

