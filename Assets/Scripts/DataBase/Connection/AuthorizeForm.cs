using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AuthorizeForm : BaseForm
{
    public string Login { get; set; }
    public string Password { get; set; }

    public override WWWForm SetForm()
    {
        WWWForm form = new WWWForm();
        form.AddField("Password", Password);
        form.AddField("Login", Login);

        return form;
    }
}
