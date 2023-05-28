using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public abstract class AuthorizeController : MonoBehaviour
{
    [SerializeField]
    protected TMP_InputField _login;
    
    [SerializeField]
    protected TMP_InputField _alert;

    [SerializeField]
    protected TMP_InputField _password;

    [SerializeField]
    protected InitialGameParameters _initialGameParameters;

    protected string requestText;

    protected abstract bool IsDataCorrect();

    protected IEnumerator AuthorizePlayer(AuthorizeForm form, string connectionPath)
    {
        var request = UnityWebRequest.Post(connectionPath, form.SetForm());

        yield return request.SendWebRequest();

        requestText = request.downloadHandler.text;
        request.Dispose();

        LoadScene(form);
    }

    protected abstract void LoadScene(AuthorizeForm form);

    protected string Encrypt(string input)
    {
        using (MD5 md5 = MD5.Create())
        {
            byte[] inputBytes = Encoding.UTF8.GetBytes(input);
            byte[] hashedBytes = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hashedBytes.Length; i++)
            {
                sb.Append(hashedBytes[i].ToString("x2"));
            }

            return sb.ToString();
        }
    }
}
