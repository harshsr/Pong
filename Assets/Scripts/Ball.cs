using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    [SerializeField]
    public float MovementSpeed = 15.0f;
    private Vector2 MoveDirection = Vector2.zero;
    // Start is called before the first frame update
    void Start()
    {
        MoveDirection = RandomInitialDirection();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        var transformPosition = transform.position;
        transformPosition.x += MoveDirection.x * Time.deltaTime * MovementSpeed;
        transformPosition.y += MoveDirection.y * Time.deltaTime * MovementSpeed;
        transform.position = transformPosition;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("ReflectWall"))
        {
            Debug.Log("Reflection collision");
            MoveDirection.y = -MoveDirection.y;
        }
        else if (other.gameObject.CompareTag("Player"))
        {
            MoveDirection = MoveDirection - 2*(Vector2.Dot(MoveDirection, other.contacts[0].normal)) * other.contacts[0].normal;
        }
        else if (other.gameObject.CompareTag("DeathWallLeft"))
        {
            Debug.Log("PlayerLeft Lost");
        }
        else if (other.gameObject.CompareTag("DeathWallRight"))
        {
            Debug.Log("PlayerRight Lost");
        }
    }
    

    Vector2 RandomInitialDirection()
    {
        float RandomAngle = Random.Range(0f,2*Mathf.PI);
        return new Vector2(Mathf.Cos(RandomAngle), Mathf.Sin(RandomAngle));
    }
}
