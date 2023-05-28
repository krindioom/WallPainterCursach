
using UnityEngine;

public class LevelPrefsForm : BaseForm
{
    public string Login { get; set; }
    public int LevelId { get; set; }
    public string LevelPrefs { get; set; }

    public override WWWForm SetForm()
    {
        WWWForm form = new WWWForm();
        form.AddField("Login", Login);
        form.AddField("LevelId", LevelId);
        form.AddField("JsonFile", LevelPrefs);

        return form;
    }
}
