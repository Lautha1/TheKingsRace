using System.Collections;
using System.Collections.Generic;
using MLAPI;
using UnityEngine;

public class GameHandler : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start() {
        StartCoroutine(introCutscene());
    }

    IEnumerator introCutscene() {
        // Get the time to complete the intro camera fly through
        float cameraLerpTime = Camera.main.GetComponent<CPC_CameraPath>().playOnAwakeTime;

        // Wait for that many seconds - allows for time to complete the "cutscene"
        yield return new WaitForSecondsRealtime(cameraLerpTime);

        // Remove the main camera and give camera to local players
        Camera.main.gameObject.SetActive(false);

        // Get all players in the scene - runners and king
        GameObject[] playableCharacters = GameObject.FindGameObjectsWithTag("Player");

        // Iterate over them and if their network object equals the call to the rpc, enable that camera
        GameObject localPlayer = null;
        foreach (GameObject character in playableCharacters) {
            if (character.GetComponent<NetworkObject>().OwnerClientId == NetworkManager.Singleton.LocalClientId) {
                localPlayer = character;
                FindGameObjectInChildWithTag(character, "PlayerCam").SetActive(true);
            }
        }

        //TODO: Do a 3.2.1 countdown

        // Re-enable the player movement
        localPlayer.GetComponentInChildren<PlayerMovement>().enabled = true;

        //TODO: Start the game timer
    }

    // Only works for 1st generation children
    private static GameObject FindGameObjectInChildWithTag(GameObject parent, string tag) {
        Transform t = parent.transform;

        for (int i = 0; i < t.childCount; i++) { 
            if (t.GetChild(i).gameObject.tag == tag) {
                return t.GetChild(i).gameObject;
            }
        }

        // Couldn't find child with tag
        return null;
    }
}
