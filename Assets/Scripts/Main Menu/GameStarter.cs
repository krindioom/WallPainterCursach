using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameStarter : MonoBehaviour
{
    [SerializeField]
    private InitialGameParameters _parameters;

    public void StartGame()
    {
        if (SceneManager.GetSceneByName(_parameters.Level.Name) == null)
        {
            return;
        }
            
        SceneManager.LoadScene(_parameters.Level.Name);
    }
}
