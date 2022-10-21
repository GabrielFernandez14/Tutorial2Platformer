using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerScript : MonoBehaviour
{
    public float speed;
    public bool gameOver;

    public Text score;
    public Text lives;

    public GameObject winTextObject;
    public GameObject loseTextObject;

    public AudioSource musicSource;
    public AudioClip backgroundMusic;
    public AudioClip winMusic;

    private Rigidbody2D rd2d;
    private int scoreValue = 0;
    private int livesValue = 3;
    private float hozMovement;
    private float verMovement;
    private bool facingRight = true;

    private bool isOnGround;
    public Transform groundcheck;
    public float checkRadius;
    public LayerMask allGround;

    Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rd2d = GetComponent<Rigidbody2D>();

        anim = GetComponent<Animator>();

        score.text = scoreValue.ToString();
        lives.text = livesValue.ToString();

        winTextObject.SetActive(false);
        loseTextObject.SetActive(false);

        musicSource.clip = backgroundMusic;
        musicSource.Play();

        gameOver = false;
    }

    void FixedUpdate()
    {
        hozMovement = Input.GetAxis("Horizontal");
        verMovement = Input.GetAxis("Vertical");

        rd2d.AddForce(new Vector2(hozMovement * speed, verMovement * speed));
        isOnGround = Physics2D.OverlapCircle(groundcheck.position, checkRadius, allGround);
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.A))
        {
            anim.SetInteger("State", 1);
        }

        else if (Input.GetKeyUp(KeyCode.A))
        {
            anim.SetInteger("State", 0);
        }

        else if (Input.GetKeyDown(KeyCode.D))
        {
            anim.SetInteger("State", 1);
        }

        else if (Input.GetKeyUp(KeyCode.D))
        {
            anim.SetInteger("State", 0);
        }

        if (Input.GetKeyDown(KeyCode.W)) 
        {
            anim.SetInteger("State", 2);
        }
        
        if (Input.GetKeyUp(KeyCode.W)) 
        {
            anim.SetInteger("State", 0);
        }

        if (facingRight == false && hozMovement > 0)
        {
            Flip();
        }
        else if (facingRight == true && hozMovement < 0)
        {
            Flip();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.collider.tag == "Coin")
        {
            scoreValue += 1;
            score.text = scoreValue.ToString();
            Destroy(collision.collider.gameObject);
            CheckLevel2();
            CheckWin();
        }

        if(collision.collider.tag == "Enemy")
        {
            if (gameOver == false)
            {
                livesValue -= 1;
                lives.text = livesValue.ToString();
                Destroy(collision.collider.gameObject);
                CheckLose();
            }
            else
            {
                Destroy(collision.collider.gameObject);
            }
        }
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.collider.tag == "Ground" && isOnGround)
        {
            if (Input.GetKey(KeyCode.W))
            {
                rd2d.AddForce(new Vector2(0, 3), ForceMode2D.Impulse);
            }
        }
    }

    void Flip()
    {
        facingRight = !facingRight;
        Vector2 Scaler = transform.localScale;
        Scaler.x = Scaler.x * -1;
        transform.localScale = Scaler;
    }

    void CheckLevel2()
    {
        if (scoreValue == 4)
        {   
            transform.position = new Vector3(50, 0, 0);
            livesValue = 3;
            lives.text = livesValue.ToString();
        }
    }

    void CheckWin()
    {
        if (scoreValue == 8)
        {   
            winTextObject.SetActive(true);
            musicSource.clip = winMusic;
            musicSource.Play();
            gameOver = true;
        }
    }

    void CheckLose()
    {
        if (livesValue == 0)
        {   
            loseTextObject.SetActive(true);
            Destroy(gameObject);
        }
    }
}
