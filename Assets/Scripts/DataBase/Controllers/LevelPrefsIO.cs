using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class LevelPrefsIO : MonoBehaviour
{
    public PlayerPosition _playerPosition { get; set; }

    public int _enemyCount { get; set; }

    public List<TilePosition> _paintedWalls { get; set; }

    public string _fileName { get; set; } = "LevelPrefirenses.json";

    private void Start()
    {
        _paintedWalls = new List<TilePosition>();
        SaveToFileDefaults();
    }

    public LevelPrefs LoadFromFile()
    {
        /*if (!File.Exists(Application.streamingAssetsPath + '/' + _fileName))
        {
            Debug.Log("такого файла нет");
            return ;
        }*/

        string json = File.ReadAllText(Application.streamingAssetsPath + '/' + _fileName);
        LevelPrefs prefs = JsonUtility.FromJson<LevelPrefs>(json);

        _enemyCount = prefs.EnemyCount;
        _paintedWalls = prefs.PaintedWalls.ToList();
        _playerPosition = prefs.PlayerPosition;

        Debug.Log(prefs.PlayerPosition);

        return prefs;
    }

    public void SaveToFile()
    {
        LevelPrefs prefs = new LevelPrefs
        {
            PlayerPosition = _playerPosition,
            EnemyCount = _enemyCount,
            PaintedWalls = _paintedWalls.ToArray()
        };

        string json = JsonUtility.ToJson(prefs);

        File.WriteAllText(Application.streamingAssetsPath + '/' + _fileName, json);
    }

    public void SaveToFile(string jsonString)
    {
        Debug.Log(jsonString);
        LevelPrefs prefs = JsonUtility.FromJson<LevelPrefs>(jsonString);

        string json = JsonUtility.ToJson(prefs);

        File.WriteAllText(Application.streamingAssetsPath + '/' + _fileName, json);
    }

    private void SaveToFileDefaults()
    {
        LevelPrefs prefs = new LevelPrefs
        {
            PlayerPosition = new PlayerPosition { x = 0, y = 0},
            EnemyCount = 0
        };

        string json = JsonUtility.ToJson(prefs);

        File.WriteAllText(Application.streamingAssetsPath + '/' + _fileName, json);
    }

    public string ReadToString(string name)
    {
        return File.ReadAllText(Application.streamingAssetsPath + '/' + name);
    }
}
