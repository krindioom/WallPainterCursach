using System.Collections;
using System.Collections.Generic;
using Unity.Core;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour
{
    public float Speed;
    public float RotateOffset;
    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
    }

    private void Move()
    {
        float xDirection = Input.GetAxis("Horizontal");
        float yDirection = Input.GetAxis("Vertical");

        Vector2 direction = new Vector2(xDirection, yDirection) * Speed;

        _rigidbody.velocity = direction;
    }

    private void Rotate()
    {
        Vector3 diference = Camera.main.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        float zRotation = Mathf.Atan2(diference.y, diference.x) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, 0, zRotation + RotateOffset);
    }
}
