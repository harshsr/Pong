using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    [SerializeField]
    public string PlayerRightTag = "PlayerRight";
    [SerializeField]
    public string PlayerLeftTag = "PlayerLeft";
    [SerializeField]
    public string ReflectWallTag = "ReflectWall";
    [SerializeField]
    public string DeathWallLeftTag = "DeathWallLeft";
    [SerializeField]
    public string DeathWallRightTag = "DeathWallRight";
    
    [SerializeField]
    public float MovementSpeed = 15.0f;
    
    [SerializeField]
    public TextMeshProUGUI PlayerLeftScoreText;
    [SerializeField]
    public TextMeshProUGUI PlayerRightScoreText;
    
    private int PlayerLeftScore = 0;
    private int PlayerRightScore = 0;
    
    private bool LastPlayerHitWasRight = false;
    
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
        if (other.gameObject.CompareTag(ReflectWallTag))
        {
            MoveDirection.y = -MoveDirection.y;
            if (MoveDirection.x>0)
            {
                MoveDirection.x+= Random.Range(0.05f, 0.01f);
            }
            else
            {
                MoveDirection.x-= Random.Range(0.05f, 0.01f);
            }
            MoveDirection = MoveDirection.normalized;
        }
        else if (other.gameObject.CompareTag(PlayerRightTag))
        {
            MoveDirection = MoveDirection - 2*(Vector2.Dot(MoveDirection, other.contacts[0].normal)) * other.contacts[0].normal;
            PlayerRightScore++;
            if (PlayerRightScoreText!=null)
            {
                PlayerRightScoreText.text = PlayerRightScore.ToString();
            }
            LastPlayerHitWasRight = true;
        }
        else if (other.gameObject.CompareTag(PlayerLeftTag))
        {
            MoveDirection = MoveDirection - 2*(Vector2.Dot(MoveDirection, other.contacts[0].normal)) * other.contacts[0].normal;
            PlayerLeftScore++;
            if (PlayerLeftScoreText!=null)
            {
                Debug.Log("PlayerLeftScore: " + PlayerLeftScore);
                PlayerLeftScoreText.text = PlayerLeftScore.ToString();
            }
            LastPlayerHitWasRight = false;
        }
        else if (other.gameObject.CompareTag(DeathWallLeftTag))
        {
            if (!LastPlayerHitWasRight)
            {
                PlayerLeftScore--;
            }
            PlayerLeftScoreText.text = "Left Lost. Score: " + PlayerLeftScore.ToString();
            Time.timeScale = 0;
        }
        else if (other.gameObject.CompareTag(DeathWallRightTag))
        {
            if (LastPlayerHitWasRight)
            {
                PlayerRightScore--;
            }
            PlayerRightScoreText.text = "Right Lost. Score: " + PlayerRightScore.ToString();
            Time.timeScale = 0;
            
        }
    }
    

    Vector2 RandomInitialDirection()
    {
        float RandomAngle = Random.Range(0f,2*Mathf.PI);
        return new Vector2(Mathf.Cos(RandomAngle), Mathf.Sin(RandomAngle));
    }
}
