using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this class is entirely made by me
public class EggSpawner : MonoBehaviour
{
    public GameObject goldenEggPrefab, eggPrefab, egg2Prefab;

    public int numOfEggs;

    public int range = 60;

    public LayerMask mask;

    public static EggSpawner Instance;

    public GameObject goldenEggSpawnContainer;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update and is called every time the scene is loaded.
    void Start()
    {
        // this number chooses a random index from an array of spawn points for the golden egg. 
        int randomNum = Random.Range(0, goldenEggSpawnContainer.transform.childCount - 1);
        // spawns the golden egg at the position of the child of the array 
        //GameObject goldenEgg = Instantiate(goldenEggPrefab, goldenEggSpawnContainer.transform.GetChild(randomNum).position, goldenEggSpawnContainer.transform.GetChild(randomNum).rotation);
        GameObject goldenEgg = SpawnEgg(goldenEggPrefab, goldenEggSpawnContainer.transform.GetChild(randomNum).position, goldenEggSpawnContainer.transform.GetChild(randomNum).rotation);
        
        // spawns a certain number of eggs
        for (int i = 0; i < numOfEggs; i++)
        {
            // random number to choose a random egg. There are only 2 types eggs
            int random = Random.Range(0, 1);
            GameObject egg = null;

            Vector3 randomPosition;

            // chooses random position between the range, which is set in the Unity inspector
            randomPosition = new Vector3(transform.position.x + Random.Range(-range, range), 30, transform.position.z + Random.Range(-range, range));

            // if the random number is equal to 1, spawn the first egg, or else, spawn the variant.
            if (random == 1)
            {
                egg = SpawnEgg(eggPrefab, randomPosition, Quaternion.identity);
                //egg = Instantiate(eggPrefab, randomPosition, Quaternion.identity);
            } else
            {
                egg = SpawnEgg(egg2Prefab, randomPosition, Quaternion.identity);
                //egg = Instantiate(egg2Prefab, randomPosition, Quaternion.identity);
            }

            RaycastHit hit;

            // the eggs are spawned above all of the trees and land so that they don't intersect with the ground.
            // I raycast from the position of the egg down. The egg is moved to the point where the raycast hit.
            // This will make sure that the egg is spawned on top of the object that is below it.
            if (Physics.Raycast(egg.transform.position, Vector3.down, out hit, mask))
            {
                // places the egg slightly above the point that the raycast hit.
                egg.transform.position = hit.point + Vector3.up * 0.2f;
            }
        }
    }

    GameObject SpawnEgg(GameObject egg, Vector3 position, Quaternion rotation)
    {      
        return Instantiate(eggPrefab, position, rotation) as GameObject;
    }
}
