using UnityEditor.PackageManager;
using UnityEngine;
using Vuforia;

public class AvatarSpawnChecker : MonoBehaviour
{
    public GameObject avatar; 
    private ContentPositioningBehaviour contentPositioningBehaviour;
    private bool isAvatarSpawned = false;
    public GameObject planeFinder;
    public Interaction interaction;


    void Start()
    {
        // Get the Content Positioning Behaviour from the Plane Finder
        contentPositioningBehaviour = planeFinder.GetComponent<ContentPositioningBehaviour>();

        // Subscribe to the OnContentPlaced event
        if (contentPositioningBehaviour != null)
        {
            contentPositioningBehaviour.OnContentPlaced.AddListener(OnContentPlaced);
        }

        // Initially check if avatar exists in the scene
        //CheckAvatarStatus();
    }

    // Called when content is placed by Vuforia
    private void OnContentPlaced(GameObject placedObject)
    {
        Debug.Log("Content placed: " + placedObject.name);
        // Check if the avatar is now in the scene
        CheckAvatarStatus();
    }

    // Check if the avatar is spawned/active
    private void CheckAvatarStatus()
    {
        if (avatar != null && avatar.activeInHierarchy)
        {
            isAvatarSpawned = true;
            SessionManager.isAvatarSpawned = true;
            Debug.Log("Avatar is spawned and active in the scene!");
            interaction.Greet();
        }
        else
        {
            isAvatarSpawned = false;
            SessionManager.isAvatarSpawned = false;
            Debug.Log("Avatar is not spawned or inactive.");
        }
    }

    // Public method to query avatar spawn status
    public bool IsAvatarSpawned()
    {
        return isAvatarSpawned;
    }

}