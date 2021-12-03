using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlowTerrain : MonoBehaviour
{
    //This will need to change to be dynamic (for things like roller skates)
    //Also maybe add an isGrounded check?
    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Slower!");
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerStats>().MaxVel = 10.0f; //if player enters trigger slow them down
            Debug.Log("Slower");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            other.GetComponent<PlayerStats>().MaxVel = 25.0f;//if player exits trigger return speed to default
        }
    }
}
