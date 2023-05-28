using TMPro;
using UnityEngine;

public class UILogin : MonoBehaviour
{
    [SerializeField]
    private TMP_Text _text;

    [SerializeField]
    private InitialGameParameters _gameParameters;

    private void Start()
    {
        _text.text = _gameParameters.Login;
    }
}
