using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerScript : MonoBehaviour
{
    private Rigidbody2D rd2d;

    public Transform groundcheck;
    
    public float checkRadius;
    
    public LayerMask allGround;

    public float speed;

    public float jumpForce;

    public Text scoreText;

    public Text gameoverText;

    public Text livesText;

    private int score;

    private int lives;

    private bool facingRight = true;

    private bool isOnGround;

    private bool gameOver;

    public AudioClip musicClipOne;

    public AudioClip musicClipTwo;

    public AudioSource musicSource;

    Animator anim;
    
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();

        score = 0;

        SetScoreText ();

        lives = 3;

        SetLivesText ();

        gameoverText.text = "";

        musicSource.clip = musicClipOne;

        musicSource.Play();

        gameOver = false;

        anim = GetComponent<Animator>();
    }

    void Flip()
    {
        facingRight = !facingRight;
        
        Vector2 Scaler = transform.localScale;
        
        Scaler.x = Scaler.x * -1;
        
        transform.localScale = Scaler;
    }

    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        
        float vertMovement = Input.GetAxis("Vertical");

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }

        if (hozMovement == 0 && isOnGround == true)
        {
            anim.SetInteger("State", 0);
        }

        if (hozMovement > 0 && isOnGround == true)
        {
            anim.SetInteger("State", 1);
        }

        if (hozMovement < 0 && isOnGround == true)
        {
            anim.SetInteger("State", 1);
        }

        if (isOnGround == false)
        {
            anim.SetInteger("State", 2);
        }

        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));

        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);
    }
    
    private void OnCollisionEnter2D(Collision2D collision)
    {
       if (collision.collider.tag == "Coin")
        {
            score = score +1;
            
            SetScoreText();
            
            collision.collider.gameObject.SetActive(false);
        }

        else if (collision.collider.tag == "Enemy")
        {
            lives = lives - 1;
            
            SetLivesText();
            
            collision.collider.gameObject.SetActive(false);
        }
    }
    void Update()
    {
        if (score == 4)
        {
            transform.position = new Vector2(20f, 0.0f);

            score = score + 1;
            
            SetScoreText();

            lives = 3;

            SetLivesText();
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey("up"))
            {
                rd2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }

            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            }
        }
    }

    void SetScoreText()
    {
        scoreText.text = "Score: " + score.ToString();

        if (score >=9)
        {
            gameOver = true;
        }
        
        if (gameOver = true && score >=9)
        {
            gameoverText.text = "You Win! - A Game by Alejandro Morales";

            musicSource.clip = musicClipTwo;

            musicSource.Play();

            speed = 0;

            jumpForce = 0;

            rd2d.constraints = RigidbodyConstraints2D.FreezeAll;
        }
    }

    void SetLivesText()
    {
        livesText.text = "Lives: " + lives.ToString();

        if (lives <= 0)
        {
            gameOver = true;
        }

        if (gameOver = true && lives <= 0)
        {
            gameoverText.text = "You Lose! - Try Again?";
            
            gameObject.SetActive(false);
        }
    }
}