using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelUI : MonoBehaviour
{
    private LevelPrefsIO _levelPrefs;
    [SerializeField] private LevelConfigurer _levelConfigurer;

    private void Start()
    {
        _levelPrefs = GetComponent<LevelPrefsIO>();
    }

    public void PauseGame()
    {
        Time.timeScale = 0; 
    }

    public void ResumeGame()
    {
        Time.timeScale = 1;
        Debug.Log(Time.timeScale);
    }

    public void QuitGame()
    {
        _levelConfigurer.SaveData();
        SceneManager.LoadScene("MainMenu");
    }


}
