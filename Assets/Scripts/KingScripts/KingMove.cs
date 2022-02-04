using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingMove : MonoBehaviour
{
    public float speed = 240.0f;

    private Vector3 newPos;
    private Vector3 MountCent = new Vector3(-4500, 620, 510);
    public GameObject Grid;
    // Update is called once per frame
    void Update() {
        float translation = Input.GetAxis("KingMove") * speed;

        // Make it move 10 meters per second instead of 10 meters per frame...
        translation *= Time.deltaTime;

        // Move translation along the object's z-axis
        if (transform.position.x <= 350 && transform.position.x >= -4500) {
            transform.Translate(translation, 0, 0);
            Grid.GetComponent<GridReveal>().DynGridReveal(transform.position.x, translation); //Makes the grid reveal itself as the King moves
            transform.rotation = Quaternion.Euler(0, 180, 0);
        }
        else if (transform.position.x > 350) {
            transform.position = new Vector3(350, 625, 1130);//Keeps them from going too far left
        }
        else if (transform.position.x < -4500) {//Once they rech a certain point they begin to cirlce around the mountain (radius of 1050, (x+4500)^2+(z-510)^2=620^2)
            float z = transform.position.z;
            z -= translation; //Moves the player's X forward slightly
            float x = -Mathf.Sqrt((620 * 620) - ((z-510) * (z-510))) - 4500; //Snaps the player onto a circle that is around the mountain, so the player orbits it smoothly
            newPos = new Vector3(x, 620, z); //make a Vector3 out of the new X and Z
            transform.position = newPos;//Sets the player's new position on the cirlce
            transform.LookAt(MountCent);//Rotates the player as they move along the circumfurance
            if (z > 1130) {//Snaps the player back onto the horizontal track
                transform.position = new Vector3(-4500, 625, 1130);
            }
        }
        else {//If somehow the player disappears into the void, resets them
            Debug.Log("Aw, Beans");
            transform.position = new Vector3(350, 625, 1130);
        }
    }
}
