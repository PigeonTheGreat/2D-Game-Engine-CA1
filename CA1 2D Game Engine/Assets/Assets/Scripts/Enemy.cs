using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    private Animator _animator;
    int directionX = 1;
    int directionY = 1;
    int idleTime;
    public int health;
    bool isDead = false;
    bool isIdle;
    float dieTime = 2;
    [SerializeField] float fireTimer = 0.5f;
    float fireCountdown = 0;
    [SerializeField] GameObject projectilePrefab;
    private AudioSource audio;
    public AudioClip deathSound;
    public AudioClip hitSound;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDead)
        {
            RaycastHit2D hitX = Physics2D.Raycast(transform.position, new Vector2(directionX, 0), 5f, LayerMask.GetMask("Player"));
            RaycastHit2D hitY = Physics2D.Raycast(transform.position, new Vector2(directionY, 0), 5f, LayerMask.GetMask("Player"));
            if (hitX.collider != null)
            {
                if (hitX.collider.GetComponent<Player>() != null)
                {
                    fire();
                }
            }

            if (hitY.collider != null)
            {
                if (hitY.collider.GetComponent<Player>() != null)
                {
                    fire();
                }
            }
            fireCountdown -= Time.deltaTime;

        }
        else
        {
            dieTime -= Time.deltaTime;
            audio.PlayOneShot(deathSound);
            if (dieTime < 0)
            {
                Destroy(this.gameObject);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "PlayerProjectile")
        {
            audio.PlayOneShot(hitSound);
            health--;
            //Debug.Log(health);
            
            if(health <= 0)
            {
                isDead = true;
            }

        }
    }

    private void fire()
    {
        if (fireCountdown < 0)
        {
            fireCountdown = fireTimer;
            GameObject projectileObject = Instantiate(projectilePrefab, GetComponent<Rigidbody2D>().position, Quaternion.identity);
            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.Launch(new Vector2(directionX, 0), 300);
            //projectile.Launch(new Vector2(directionY, 0), 300);
            //Debug.Log(directionX); 
            //Debug.Log(directionY);
        }
    }

}
