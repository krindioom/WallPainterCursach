using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float LerpSpeed = 10f;

    public PlayerMovement Player;

    private void Start()
    {
        Player = FindObjectOfType<PlayerMovement>();
    }

    private void FixedUpdate()
    {
        Follow();
    }

    private void Follow()
    {
        Vector3 target = new Vector3(Player.transform.position.x, Player.transform.position.y, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, target, LerpSpeed);
    }
}
