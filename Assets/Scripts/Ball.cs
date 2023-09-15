using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Ball : MonoBehaviour
{
    [SerializeField] public string PlayerRightTag = "PlayerRight";
    [SerializeField] public string PlayerLeftTag = "PlayerLeft";
    [SerializeField] public string ReflectWallTag = "ReflectWall";
    [SerializeField] public string DeathWallLeftTag = "DeathWallLeft";
    [SerializeField] public string DeathWallRightTag = "DeathWallRight";

    [SerializeField] public float MovementSpeed = 10.0f;

    [SerializeField] public TextMeshProUGUI PlayerLeftScoreText;
    [SerializeField] public TextMeshProUGUI PlayerRightScoreText;
    [SerializeField] public Button RestartButton;
    [SerializeField] public TextMeshProUGUI RestartText;
    [SerializeField] public TextMeshProUGUI MatchTimerText;

    private int PlayerLeftScore = 0;
    private int PlayerRightScore = 0;

    [SerializeField] public float MatchTime = 60.0f;
    private float CurrentMatchTime = 0.0f;

    //private bool bLastPlayerHitWasRight = false;
    private bool bJustCollided = false;
    private float CollisionDisableGoal = 0.1f;
    private float CollisionDisableTimer = 0.0f;
    
    private float ResetMatchGoal = 1f;
    private float ResetMatchTimer = 0.0f;
    private bool bResetMatch = true;

    private Vector2 MoveDirection = Vector2.zero;

    // Start is called before the first frame update
    void Start()
    {
        CurrentMatchTime = MatchTime;
        MoveDirection = RandomInitialDirection();

        RestartButton.enabled = false;
        RestartButton.GetComponent<Image>().enabled = false;
        RestartText.enabled = false;
        RestartButton.onClick.AddListener(RestartMatch);
    }

    // Update is called once per frame
    void Update()
    {
        if (bJustCollided)
        {
            CollisionDisableTimer += Time.deltaTime;
            if (CollisionDisableTimer > CollisionDisableGoal)
            {
                bJustCollided = false;
                CollisionDisableTimer = 0.0f;
                this.GetComponent<CircleCollider2D>().enabled = true;
            }
        }

        if (bResetMatch)
        {
            ResetMatchTimer += Time.deltaTime;
            if (ResetMatchTimer > ResetMatchGoal)
            {
                bResetMatch = false;
                ResetMatchTimer = 0.0f; 
            }
        }

        CurrentMatchTime -= Time.deltaTime;
        MatchTimerText.text = CurrentMatchTime.ToString("F2");
        if (CurrentMatchTime < 0)
        {
            EndMatch();
        }
    }

    private void EndMatch()
    {
        if (PlayerLeftScore>PlayerRightScore)
        {
            PlayerLeftScoreText.text = "Won. Score: " + PlayerLeftScore.ToString();
            PlayerRightScoreText.text = "Lost. Score: " + PlayerRightScore.ToString();
        }
        else if (PlayerLeftScore<PlayerRightScore)
        {
            PlayerRightScoreText.text = "Won. Score: " + PlayerRightScore.ToString();
            PlayerLeftScoreText.text = "Lost. Score: " + PlayerLeftScore.ToString();
        }
        else if (PlayerLeftScore==PlayerRightScore)
        {
            PlayerRightScoreText.text = "Tie. Score: " + PlayerRightScore.ToString();
            PlayerLeftScoreText.text = "Tie. Score: " + PlayerLeftScore.ToString();
        }
        
        Time.timeScale = 0;
        RestartButton.enabled = true;
        RestartButton.GetComponent<Image>().enabled = true;
        RestartText.enabled = true;
    }

    private void FixedUpdate()
    {
        if (!bResetMatch)
        {
            transform.Translate(MoveDirection * Time.deltaTime * MovementSpeed);
            // var transformPosition = transform.position;
            // transformPosition.x += MoveDirection.x * Time.deltaTime * MovementSpeed;
            // transformPosition.y += MoveDirection.y * Time.deltaTime * MovementSpeed;
            // transform.position = transformPosition;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag(ReflectWallTag))
        {
            MoveDirection.y = -MoveDirection.y;
            if (MoveDirection.x > 0)
            {
                MoveDirection.x += Random.Range(0.1f, 0.05f);
            }
            else
            {
                MoveDirection.x -= Random.Range(0.1f, 0.05f);
            }

            MoveDirection = MoveDirection.normalized;
        }
        else if (other.gameObject.CompareTag(PlayerRightTag))
        {
            MoveDirection = MoveDirection -
                            2 * (Vector2.Dot(MoveDirection, other.contacts[0].normal)) * other.contacts[0].normal;
            //bLastPlayerHitWasRight = true;
        }
        else if (other.gameObject.CompareTag(PlayerLeftTag))
        {
            MoveDirection = MoveDirection -
                            2 * (Vector2.Dot(MoveDirection, other.contacts[0].normal)) * other.contacts[0].normal;
            //bLastPlayerHitWasRight = false;
        }
        else if (other.gameObject.CompareTag(DeathWallLeftTag))
        {
            PlayerRightScore++;
            if (PlayerRightScoreText != null)
            {
                PlayerRightScoreText.text = PlayerRightScore.ToString();
            }
            ResetMatch();
        }
        else if (other.gameObject.CompareTag(DeathWallRightTag))
        {
            PlayerLeftScore++;
            if (PlayerLeftScoreText != null)
            {
                PlayerLeftScoreText.text = PlayerLeftScore.ToString();
            }
            ResetMatch();
        }
        //bJustCollided = true;
        //this.GetComponent<CircleCollider2D>().enabled = false;
    }


    Vector2 RandomInitialDirection()
    {
        float RandomAngle = Random.Range(0f, 2 * Mathf.PI);
        return new Vector2(Mathf.Cos(RandomAngle), Mathf.Sin(RandomAngle));
    }

    public void RestartMatch()
    {
        Time.timeScale = 1;
        PlayerLeftScore = 0;
        PlayerRightScore = 0;
        PlayerRightScoreText.text = "0";
        PlayerLeftScoreText.text = "0";
        CurrentMatchTime = MatchTime;
        MatchTimerText.text = CurrentMatchTime.ToString("F2");
        RestartButton.enabled = false;
        RestartButton.GetComponent<Image>().enabled = false;
        RestartText.enabled = false;
        
        ResetMatch();
    }

    private void ResetMatch()
    {
        bResetMatch = true;
        var transformPosition = transform.position;
        transformPosition.x = 0;
        transformPosition.y = 0;
        transform.position = transformPosition;
        GameObject.FindWithTag(PlayerLeftTag).GetComponent<Player>().Restart();
        GameObject.FindWithTag(PlayerRightTag).GetComponent<Player>().Restart();
        MoveDirection = RandomInitialDirection();
    }
}