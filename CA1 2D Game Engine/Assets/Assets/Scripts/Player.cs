
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour
{

    //Declaring Variables
    public float speed; //To assign the speed of the player.
    private Animator animator; //To assign animations to the player as they move.
    private Rigidbody2D rb; //Assigning a rigidbody2d  variable.
    private Vector2 startPosition;
    public GameObject projectilePrefab;
    private int scrapCount = 15;
    [SerializeField] public UIManager ui;
    private bool isPowerUp = false;
    private float PowerUpTimeRemaining = 5;
    private float DefaultPowerUpTime = 5;
    public int lives = 5;
    private AudioSource _audio;
    public bool isPlaying = true;
    public AudioClip collectSound;
    public AudioClip hitSound;
    public AudioClip boostSound;
    public AudioClip deathSound;
    private MainMenu menu; //To cut to the menu.

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Calling the components.
        animator = GetComponent<Animator>(); //Linking to the animator.
        rb = GetComponent<Rigidbody2D>(); //Linking to the Rigidbody2D
        startPosition = transform.position;
        _audio = GetComponent<AudioSource>();
        menu = GetComponent<MainMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        //Moving the player
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector2 position = transform.position;
        position.x += Time.deltaTime* moveHorizontal * speed;

        position.y += Time.deltaTime * moveVertical * speed;
        transform.position = position;
        updateAnimator(moveHorizontal, moveVertical);

        //Audio
        if(moveHorizontal != 0 || moveVertical != 0)
        {
            _audio.Play();
            isPlaying = true;
        }
        else if(moveHorizontal == 0 && moveVertical == 0)
        {
            _audio.Pause();
            isPlaying = false;
        }

        //Projectiles.
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject projectile = Instantiate(projectilePrefab, rb.position, Quaternion.identity);
            Projectile prX = projectile.GetComponent<Projectile>();
            prX.Launch(new Vector2(animator.GetInteger("DirectionX"), 0), 300);
            //Projectile prY = projectile.GetComponent<Projectile>();
            //prY.Launch(new Vector2(animator.GetInteger("DirectionY"), 0), 300);
        }

        if (isPowerUp)
        {

            PowerUpTimeRemaining -= Time.deltaTime;
            if (PowerUpTimeRemaining < 0)
            {

                isPowerUp = false;
                PowerUpTimeRemaining = DefaultPowerUpTime;
                animator.speed /= 2;
                speed /= 2;

            }

            if (lives <= 0)
            {
                _audio.PlayOneShot(deathSound);
                SceneManager.LoadSceneAsync(2);
            }

        }

    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(!isPowerUp && collision.gameObject.tag == "Speed_Boost")
        {
            Destroy(collision.gameObject);
            speed *= 2;
            isPowerUp=true;
            animator.speed *= 2;
            _audio.PlayOneShot(boostSound);
        }

        if (!isPowerUp && collision.gameObject.tag == "Health_Boost")
        {
            Destroy(collision.gameObject);
            if (lives < 5)
            {
                lives = 5;
            }
            Debug.Log(lives);
            _audio.PlayOneShot(boostSound);
        }

        if(collision.gameObject.name.Contains("EnemyProjectile"))
        {
            lives--;
            Debug.Log(lives);
            ui.UpdateLives(lives);
            _audio.PlayOneShot(hitSound);
        }

    }


    //Creating a method to update the animator as the player moves.
    private void updateAnimator(float moveX, float moveY)
    {
        animator.SetFloat("MoveX", moveX);
        animator.SetFloat("MoveY", moveY);
        if (moveX > 0)
        {
            animator.SetInteger("DirectionX", 1);
        }
        else if (moveY > 0)
        {
            animator.SetInteger("DirectionY", 1);
        }
        else if (moveX < 0)
        {
            animator.SetInteger("DirectionX", -1);
        }
        else if(moveY < 0)
        {
            animator.SetInteger("DirectionY", -1);
        }

    }

    //Calling a method to count the scraps collected.
    public void CollectScrap()
    {
        menu = GetComponent<MainMenu>();
        scrapCount--; //Subtracing 1 every time a scrap is collected.
        ui.setCount(scrapCount);
        _audio.PlayOneShot(collectSound);
        if(scrapCount <= 0)
        {
            SceneManager.LoadSceneAsync(2);
        }
    }
}
