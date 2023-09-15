using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour, IPlayerCommunication
{
    [SerializeField] public InputAction PlayerMove;
    [SerializeField] public float MovementSpeed = 10.0f;
    private float MoveInputAmount = 0f;

    private Vector3 InitialPosition = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        PlayerMove.Enable();
        InitialPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        MoveInputAmount = PlayerMove.ReadValue<float>();
        if (MoveInputAmount != 0)
        {
            //Debug.Log(MoveInputAmount);
        }
    }

    private void FixedUpdate()
    {
        Vector2 transformPosition = transform.position;
        float CurrentY = transformPosition.y + MoveInputAmount * Time.deltaTime * MovementSpeed;
        if (CurrentY > 4.0f)
        {
            CurrentY = 4.0f;
        }
        else if (CurrentY < -4.0f)
        {
            CurrentY = -4.0f;
        }

        transformPosition.y = CurrentY;
        //transform.Translate(transformPosition);
        transform.position = transformPosition;
    }

    public void Restart()
    {
        transform.position = InitialPosition;
    }

    void OnDisable()
    {
        PlayerMove.Disable();
    }
}