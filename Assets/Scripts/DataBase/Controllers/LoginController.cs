using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoginController : AuthorizeController
{
    private AuthorizeForm _form;

    private void Start()
    {
        _form = GetComponent<AuthorizeForm>();
    }

    public void LoginPlayer()
    {
        if (IsDataCorrect())
        {
            var md5 = new MD5CryptoServiceProvider();

            _form.Login = _login.text;
            _form.Password = Encrypt(_password.text);

            StartCoroutine(AuthorizePlayer(_form, "http://wallpainter/login.php"));
        }
    }

    protected override void LoadScene(AuthorizeForm form)
    {
        if (requestText == form.Login)
        {
            _initialGameParameters.Login = form.Login;
            SceneManager.LoadSceneAsync("MainMenu");
        }
        else
        {
            _alert.text = "пользователя нет";
        }
    }

    protected override bool IsDataCorrect()
    {
        if (_login.text == "" || _password.text == "")
        {
            _alert.text = "Заполните все поля";
            return false;
        }

        return true;
    }
}
