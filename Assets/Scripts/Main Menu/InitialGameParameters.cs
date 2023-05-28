using UnityEngine;


[CreateAssetMenu(fileName = "InitialGameParameters", menuName = "GameParameters")]
public class InitialGameParameters : ScriptableObject
{
    public string Login { get; set; }

    public LevelParameters Level { get; set; }

    public GunParameters Gun { get; set; }
}
