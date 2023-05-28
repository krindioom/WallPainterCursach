using UnityEngine;

public class StartParametersHandler : MonoBehaviour
{
    [SerializeField]
    private InitialGameParameters _parameters;

    public void SelectWeapon(GunUI gun)
    {
        _parameters.Gun = gun.Gun;
        Debug.Log(_parameters.Gun);
    }

    public void SelectLevel(LevelParameters level)
    {
        _parameters.Level = level;
    }
}
