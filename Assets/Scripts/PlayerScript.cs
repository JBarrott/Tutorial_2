using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public GameObject playerGameObject;
    public float speed;
    public Text winText;
    public Text nameText;
    public Text score;
    public Text lives;
    public AudioClip musicClipOne;
    public AudioClip musicClipTwo;
    public AudioSource musicSource;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;
    Animator anim;
    private Rigidbody2D rd2d;
    private int scoreValue = 0;
    private int livesValue = 3;
    private bool facingRight = true;
    private bool isOnGround;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        score.text = scoreValue.ToString();
        SetScoreText ();
        lives.text = livesValue.ToString();
        SetLivesText ();
        winText.text = "";
        nameText.text = "";
        musicSource.clip = musicClipOne;
        musicSource.Play();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float hozMovement = Input.GetAxis("Horizontal");
        float vertMovement = Input.GetAxis("Vertical");
        rd2d.AddForce(new Vector2(hozMovement * speed, vertMovement * speed));
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);
        if (facingRight == false && hozMovement > 0)
            {
                Flip();
            }
        else if (facingRight == true && hozMovement < 0)
            {
                Flip();
            }
        if (Input.GetKey("escape"))
            {
                Application.Quit();
            }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = scoreValue.ToString();
            Destroy(collision.collider.gameObject);
            SetScoreText ();
        }
        if (collision.collider.tag == "Enemy")
        {
            livesValue -= 1;
            lives.text = livesValue.ToString();
            Destroy(collision.collider.gameObject);
            SetLivesText ();
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }

    void SetScoreText ()
    {
        score.text = "Score: " + scoreValue;
        if (scoreValue == 4)
            {
                transform.position = new Vector3(24.0f, 1.0f, 0.0f);
            }
        if (scoreValue == 8)
            {
                musicSource.clip = musicClipTwo;
                musicSource.Play();
                transform.position = new Vector3(24.0f, 23.0f, 0.0f);
                winText.text = "You Win!";
                nameText.text = "Game created by Jordan Barrott";
            }
    }
    void SetLivesText ()
    {
        lives.text = "Lives: " + livesValue;
        if (scoreValue < 4)
            {
            if (livesValue <= 0)
                {
                    Destroy(playerGameObject);
                    winText.text = "You Lose";
                    nameText.text = "Game created by Jordan Barrott";
                }
            }
        if (scoreValue == 4)
            {
            if (livesValue <= 0)
                {
                    livesValue += 3;
                    transform.position = new Vector3(24.0f, 1.0f, 0.0f);
                }
            }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        anim.SetInteger("State", 0);
        if (collision.collider.tag == "Ground" && isOnGround)
            {
                if (Input.GetKeyDown(KeyCode.W))
                    {
                        rd2d.AddForce(new Vector2(0, 8), ForceMode2D.Impulse);
                        anim.SetInteger("State", 2);
                    }
                if (Input.GetKeyDown(KeyCode.A))
                    {
                        anim.SetInteger("State", 1);
                    }
                if (Input.GetKeyDown(KeyCode.D))
                    {
                        anim.SetInteger("State", 1);
                    }
            }
    }
}