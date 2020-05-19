using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

// this class is entirely made by me
public class PickUpEggs : MonoBehaviour
{
    bool nearEgg = false;

    Camera mainCamera;

    public LayerMask eggMask;

    public TextMeshProUGUI eggText;

    int eggsCollected;

    int eggsSpawned = 0;

    public GameObject confettiPrefab, goldenEggConfetti;

    public TextMeshProUGUI timerText;
    public float timer;

    public GameObject winPanel;

    int restartClicks = 0;

    bool wonGame = false;

    public Rigidbody dinosaur;

    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GetComponentInChildren<Camera>();
        eggsSpawned = EggSpawner.Instance.numOfEggs;
        Application.targetFrameRate = -1;

        eggText.text = eggsCollected.ToString() + "/" + eggsSpawned;
    }

    // Update is called once per frame
    void Update()
    {
        // if the player hasn't won yet, add to the timer.
        if (!wonGame)
        {
            timer += Time.deltaTime;
        }
        // formats the timer(which is in seconds)
        string minutes = Mathf.Floor(timer / 60).ToString("00");
        string seconds = Mathf.Floor(timer % 60).ToString("00");
       
        // set the timer text.
        timerText.text = (int.Parse(minutes) != 0 ? minutes + ":" + seconds : (timer % 60).ToString("00.0"));

        // if the user collected the required amount of eggs, they won.
        if (eggsCollected >= eggsSpawned)
        {
            wonGame = true;

            // enables the rigidbody for the dinosaur floating in the air.
            if (dinosaur.isKinematic)
            {
                dinosaur.isKinematic = false;

                dinosaur.transform.position = new Vector3(Random.Range(-10, 10), dinosaur.transform.position.y, Random.Range(-10, 10));
            }


            // if the win panel is not active
            if (!winPanel.activeInHierarchy)
            {
                // show the win panel UI
                winPanel.SetActive(true);

                // sets text of the win panel UI. If the player won in less than a minute, the minute value is not shown in the UI
                winPanel.GetComponentInChildren<TextMeshProUGUI>().text = "You collected all of the eggs in "
                       + (int.Parse(minutes) > 0 ? minutes + (int.Parse(minutes) == 1 ? " minute and " : " minutes and ") : "") 
                            + (int.Parse(seconds) != 0 ? seconds + (int.Parse(seconds) == 1 ? " seconds!" : " seconds!") : "");
            }

            //if the R key is pressed, the scene is restarted. The game starts over
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        // if the r key is pressed, increase the restartClick counter
        if (Input.GetKeyDown(KeyCode.R))
        {
            restartClicks++;
            // if the r key was clicked more than 5 times, restart the level.
            if (restartClicks > 5)
            {
                restartClicks = 0;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        // if the user clicks and the player trigger is activated(meaning that an egg is near)
        if (Input.GetMouseButton(0) && nearEgg)
            {
                CheckForEgg();
            }
    }

    void CheckForEgg()
    {
        RaycastHit hit = new RaycastHit();
        // raycasts from the center of the camera in the direction of the camera. It is only true if the ray hits a collider in the egg layer
        if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, 10, eggMask))
        {
            AddEggPoint(hit.point, hit.collider.transform);
        }
    }

    // adds one to the player score
    void AddEggPoint(Vector3 position, Transform egg)
    {
        // disables the egg because it was found
        egg.gameObject.SetActive(false);

        // if the tag of the egg that was clicked on is "Golden egg"
        if (egg.CompareTag("GoldenEgg"))
        {
            timer -= 20;
            // makes the timer have the minimum value of 0 and maximum value of 10 million
            timer = Mathf.Clamp(timer, 0, 100000000);
            eggsCollected++;

            // spawn golden confetti
            GameObject confetti = Instantiate(goldenEggConfetti, position, Quaternion.identity);
            Destroy(confetti, 10);
        } else // if the clicked egg is not a golden egg, it's a regular egg
        {
            eggsCollected++;
            // spawn regular confetti
            GameObject confetti = Instantiate(confettiPrefab, position, Quaternion.identity);
            // destroy the confetti 16 seconds after it is spawned
            Destroy(confetti, 16);
        }

        // sets the text of the eggs collected text
        eggText.text = eggsCollected.ToString() + "/" + eggsSpawned;  
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Egg"))
        {
            nearEgg = true;
        }    
    }

    void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Egg"))
        {
            nearEgg = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Egg"))
        {
            nearEgg = false;
        }
    }
}


