using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class NewMonoBehaviourScript : MonoBehaviour
{

    //Declaring Variables
    public float speed; //To assign the speed of the player.
    private Animator animator; //To assign animations to the player as they move.
    private int scrapCount = 0; //To count the scrap the player picks up.
    [SerializeField] private UIManager ui; //To link the scrapCount to the counter in UI.

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>(); //Linking to the animator.
    }

    // Update is called once per frame
    void Update()
    {

    }

    //Calling a method to count the scraps collected.
    public void CollectScrap()
    {
        scrapCount++;
        ui.SetScore(scrapCount);
    }
}
