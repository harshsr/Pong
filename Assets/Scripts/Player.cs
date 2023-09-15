using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    [SerializeField]
    public InputAction PlayerMove;
    [SerializeField]
    public float MovementSpeed = 10.0f;
    private float MoveInputAmount = 0f;
    
    // Start is called before the first frame update
    void Start()
    {
        PlayerMove.Enable();

    }

    // Update is called once per frame
    void Update()
    {
        MoveInputAmount = PlayerMove.ReadValue<float>();
        if (MoveInputAmount!=0)
        {
            //Debug.Log(MoveInputAmount);
        }
    }

    private void FixedUpdate()
    {
        var transformPosition = transform.position;
        float CurrentY = transformPosition.y + MoveInputAmount * Time.deltaTime * MovementSpeed;
        if (CurrentY>4.0f)
        {
            CurrentY = 4.0f;
        }
        else if (CurrentY < -4.0f)
        {
            CurrentY = -4.0f;
        }

        transformPosition.y = CurrentY;
        transform.position = transformPosition;
    }

    void OnDisable()
    {
        PlayerMove.Disable();
    }
}
