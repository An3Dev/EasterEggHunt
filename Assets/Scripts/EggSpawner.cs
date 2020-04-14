using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EggSpawner : MonoBehaviour
{

    public GameObject goldenEggPrefab, eggPrefab, egg2Prefab;

    public int numOfEggs;

    public int range = 60;

    public LayerMask mask;

    public static EggSpawner Instance;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        for (int i = 0; i < numOfEggs; i++)
        {
            int random = Random.Range(0, 1);
            GameObject egg = null;

            Vector3 randomPosition;
            randomPosition = new Vector3(transform.position.x + Random.Range(-range, range), 30, transform.position.z + Random.Range(-range, range));

            if (random == 1)
            {
                egg = Instantiate(eggPrefab, randomPosition, Quaternion.identity);
            } else
            {
                egg = Instantiate(egg2Prefab, randomPosition, Quaternion.identity);
            }

            RaycastHit hit;

            if (Physics.Raycast(egg.transform.position, Vector3.down, out hit, mask))
            {
                egg.transform.position = hit.point + Vector3.up * 0.2f;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
    }
}
