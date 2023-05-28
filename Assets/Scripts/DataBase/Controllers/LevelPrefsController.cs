using System.Collections;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class LevelPrefsController : MonoBehaviour
{
    [SerializeField] private InitialGameParameters _gameParrameters;
    private LevelPrefsForm _form;
    private LevelPrefsIO _levelPrefsIO;

    private void Start()
    {
        _form = GetComponent<LevelPrefsForm>();
        _levelPrefsIO = new LevelPrefsIO();
    }

    public void SendData() => StartCoroutine(SendToServer());
    public void SendData(string remoteAplyer) => StartCoroutine(SendToServer(remoteAplyer));

    private IEnumerator SendToServer()
    {
        var jsonText = File.ReadAllText(Application.streamingAssetsPath + '/' + "LevelPrefirenses.json");

        _form.Login = _gameParrameters.Login;
        _form.LevelId = _gameParrameters.Level.Id;
        _form.LevelPrefs = jsonText;

        var request = UnityWebRequest.Post("http://wallpainter/levelPrefs.php", _form.SetForm()); //

        yield return request.SendWebRequest();

        Debug.Log(request.downloadHandler.text);

        if(request.downloadHandler.text != "")
        {
            _levelPrefsIO.SaveToFile(request.downloadHandler.text);
        }
    }

    private IEnumerator SendToServer(string remoteAplyer)
    {
        var jsonText = File.ReadAllText(Application.streamingAssetsPath + '/' + "LevelPrefirenses.json");

        _form.Login = _gameParrameters.Login;
        _form.LevelId = _gameParrameters.Level.Id;
        _form.LevelPrefs = jsonText;

        var request = UnityWebRequest.Post(remoteAplyer, _form.SetForm()); //"http://wallpainter/levelPrefs.php"

        yield return request.SendWebRequest();

        Debug.Log(request.downloadHandler.text);

        if (request.downloadHandler.text != "")
        {
            _levelPrefsIO.SaveToFile(request.downloadHandler.text);
        }
    }
}