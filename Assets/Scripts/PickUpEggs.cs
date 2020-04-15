using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
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
        if (!wonGame)
            timer += Time.deltaTime;

        string minutes = Mathf.Floor(timer / 60).ToString("00");
        string seconds = Mathf.Floor(timer % 60).ToString("00");
       
        timerText.text = minutes + ":" + seconds;

        if (eggsCollected >= eggsSpawned)
        {
            wonGame = true;
            // Won game

            if (!winPanel.activeInHierarchy)
            {
                winPanel.SetActive(true);

                winPanel.GetComponentInChildren<TextMeshProUGUI>().text = "You collected all of the eggs in "
                       + (int.Parse(minutes) > 0 ? minutes + (int.Parse(minutes) == 1 ? " minute and " : " minutes and ") : "") 
                            + (int.Parse(seconds) != 0 ? seconds + (int.Parse(seconds) == 1 ? " seconds!" : " seconds!") : "");
            }

            if (int.Parse(minutes) <= 1)
            {
               
            } else
            {

            }
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            restartClicks++;
            if (restartClicks > 5)
            {
                restartClicks = 0;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        //if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        //{
        if (Input.GetMouseButton(0) && nearEgg)
            {
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, 10, eggMask))
                {
                    AddEgg(hit.point, hit.collider.transform);
                }
            }
        //} else if (Application.platform == RuntimePlatform.Android)
        //{
        //    if (Input.touchCount > 0 && nearEgg)
        //    {
        //        if (Input.GetTouch(0).phase == TouchPhase.Began)
        //        {
        //            Vector2 touchPos = Input.GetTouch(0).position;

        //            Ray ray = mainCamera.ScreenPointToRay(touchPos);

        //            RaycastHit hit;

        //            if (Physics.Raycast(ray, out hit, 15, eggMask))
        //            {
        //                hit.collider.gameObject.SetActive(false);

        //                AddEgg(hit.point, hit.collider.transform);
        //            }
        //        }
                
        //    }
        //}
    }

    void AddEgg(Vector3 position, Transform egg)
    {
        egg.gameObject.SetActive(false);

        if (egg.CompareTag("GoldenEgg"))
        {
            timer -= 20;
            timer = Mathf.Clamp(timer, 0, 100000);
            eggsCollected++;
            GameObject confetti = Instantiate(goldenEggConfetti, egg.position, Quaternion.identity);
            Destroy(confetti, 10);
        } else
        {
            eggsCollected++;
            GameObject confetti = Instantiate(confettiPrefab, egg.position, Quaternion.identity);
            Destroy(confetti, 16);
        }

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


