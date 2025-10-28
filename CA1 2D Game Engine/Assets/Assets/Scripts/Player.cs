
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
public class Player : MonoBehaviour
{

    //Declaring Variables
    public float speed; //To assign the speed of the player.
    private Animator animator; //To assign animations to the player as they move.
    //private int scrapCount = 0; //To count the scrap the player picks up.
    private Rigidbody2D rb; //Assigning a rigidbody2d  variable.
    //[SerializeField] private UIManager ui; //To link the scrapCount to the counter in UI.
    private Vector2 startPosition;
    public GameObject projectilePrefab;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Calling the components.
        animator = GetComponent<Animator>(); //Linking to the animator.
        rb = GetComponent<Rigidbody2D>(); //Linking to the Rigidbody2D
        startPosition = transform.position;
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
        //updateAnimator(moveHorizontal, moveVertical);

        //Projectiles.
        if (Input.GetKeyDown(KeyCode.Space))
        {

            GameObject projectile = Instantiate(projectilePrefab, rb.position, Quaternion.identity);
            Projectile pr = projectile.GetComponent<Projectile>();
            pr.Launch(new Vector2(animator.GetInteger("Direction"), 0), 300);

        }

    }

    //Calling a method to count the scraps collected.
    //public void CollectScrap()
    //{
    //    scrapCount++;
    //    ui.SetScore(scrapCount);
    //}
}
