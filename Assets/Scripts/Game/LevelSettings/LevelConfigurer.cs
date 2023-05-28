using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

public class LevelConfigurer : MonoBehaviour
{
    private LevelPrefsIO _levelPrefs;
    private LevelPrefsController _levelPrefsController;

    [SerializeField]
    private InitialGameParameters _initialGameParameters;

    [SerializeField]
    private PlayerMovement _player;

    [SerializeField] 
    private CameraMovement _camera;

    [SerializeField]
    private WallMap _map;

    [SerializeField]
    private Temp _temp;

    private PlayerMovement _instatiatedPlayer;

    //private Transform _playerPosition;

    private void Awake()
    {
        _levelPrefs = GetComponent<LevelPrefsIO>();
        _levelPrefsController = GetComponent<LevelPrefsController>();
        _levelPrefs.LoadFromFile();
        _instatiatedPlayer = Instantiate(_player);

        foreach (var wall in _levelPrefs._paintedWalls)
        {
            _map.Positions.Add(wall);
        }

        _instatiatedPlayer.transform.position = new Vector3(_levelPrefs._playerPosition.x, _levelPrefs._playerPosition.y);

       // _playerPosition = FindObjectOfType<PlayerMovement>().transform;
    }

    private void Start()
    {
        _instatiatedPlayer.GetComponent<PlayerWeaponSlot>().Weapon = _initialGameParameters.Gun;
        _instatiatedPlayer.GetComponent<PlayerWeaponSlot>().Map = _map;
    }

    public void SaveData()
    {
        PlayerPosition position = new PlayerPosition()
        {
            x = _instatiatedPlayer.transform.position.x,
            y = _instatiatedPlayer.transform.position.y
        };

        _levelPrefs._playerPosition = position;

        _levelPrefs._paintedWalls = _map.Positions.ToList();

        _levelPrefs.SaveToFile();

        _levelPrefsController.SendData("http://wallpainter/levelPrefsSaver.php");
    }
}
