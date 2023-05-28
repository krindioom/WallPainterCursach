using System.Collections;
using System.Security.Cryptography;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;

public class RegisterController : AuthorizeController
{
    [SerializeField]
    private TMP_InputField _confirmPassword;

    private AuthorizeForm _form;

    private void Start()
    {
        _form = GetComponent<AuthorizeForm>();
    }

    public void CreatePlayer()
    {
        if (IsDataCorrect())
        {
            var md5 = new MD5CryptoServiceProvider();
            _form.Login = _login.text;
            _form.Password = Encrypt(_password.text);

            StartCoroutine(AuthorizePlayer(_form, "http://wallpainter/register.php"));
        }
    }

    protected override void LoadScene(AuthorizeForm form)
    {
        if (requestText != form.Login)
        {
            _initialGameParameters.Login = form.Login;
            SceneManager.LoadSceneAsync("MainMenu");
        }
        else
        {
            _alert.text = "пользователь уже есть";
        }
    }

    protected override bool IsDataCorrect()
    {
        if (_login.text == "" || _password.text == "" || _confirmPassword.text == "")
        {
            _alert.text = "Заполните все поля";
            return false;
        }

        if (_password.text != _confirmPassword.text)
        {
            _alert.text = "Пароли не совпадают";
            return false;
        }

        return true;
    }

}
