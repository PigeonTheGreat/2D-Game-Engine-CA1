using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    Animator _animator;
    int directionX = 1;
    int directionY = 1;
    float timeInDirection;
    public float distanceTime;
    public float speed;
    public int health;
    bool isDead = false;
    float dieTime = 2;
    bool isIdle = false;
    public float idleTime = 2;
    [SerializeField] float fireTimer = 0.5f;
    float fireCountdown = 0;
    [SerializeField] GameObject projectilePrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponent<Animator>();
        timeInDirection = distanceTime;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isDead)
        {
            if(isIdle && idleTime <0)
            {
                directionX = directionX * -1;
                _animator.SetInteger("DirectionX", directionX);
                directionY = directionY * -1;
                _animator.SetInteger("DirectionY", directionY);
                timeInDirection = distanceTime;
                _animator.SetFloat("MoveX", 1);
                _animator.SetFloat("MoveY", 1);
                isIdle = false;
            }
            else if(!isIdle && timeInDirection < 0)
            {
                idleTime = 2;
                isIdle = true;
                _animator.SetFloat("MoveX", 0);
                _animator.SetFloat("MoveY", 0);
            }

            if (!isIdle)
            {
                Vector2 pos = transform.position;
                pos.x = pos.x + (speed * Time.deltaTime * directionX);
                pos.y = pos.y + (speed * Time.deltaTime * directionY);
                transform.position = pos;
                timeInDirection -= Time.deltaTime;
            }
            else 
            {
                idleTime -= Time.deltaTime;
            }

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
            if (dieTime < 0)
            {
                Destroy(this.gameObject);
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
            projectile.Launch(new Vector2(directionY, 0), 300);
            Debug.Log(directionX); 
            Debug.Log(directionY);
        }
    }

}
