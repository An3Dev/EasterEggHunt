using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PickUpEggs : MonoBehaviour
{

    bool nearEgg = false;

    Camera mainCamera;

    public LayerMask eggMask;

    public TextMeshProUGUI eggText;

    int eggsCollected;

    int eggsSpawned = 0;

    public GameObject confettiPrefab;

    public TextMeshProUGUI timerText;
    public float timer;
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = GetComponentInChildren<Camera>();
        eggsSpawned = EggSpawner.Instance.numOfEggs;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;

        string minutes = Mathf.Floor(timer / 60).ToString("00");
        string seconds = Mathf.Floor(timer % 60).ToString("00");

       
        timerText.text = minutes + ":" + seconds;

        if (eggsCollected >= eggsSpawned)
        {
            // Won game

        }

        //if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.WindowsEditor)
        //{
            if (Input.GetMouseButton(0) && nearEgg)
            {
                RaycastHit hit = new RaycastHit();
                if (Physics.Raycast(mainCamera.transform.position, mainCamera.transform.forward, out hit, 10, eggMask))
                {
                    hit.collider.gameObject.SetActive(false);

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

        if (egg.CompareTag("GoldenEgg"))
        {
            eggsCollected += 5;
        } else
        {
            eggsCollected++;
        }

        eggText.text = eggsCollected.ToString();

        // spawn confetti effect
        GameObject confetti = Instantiate(confettiPrefab, egg.position, Quaternion.identity);
        Destroy(confetti, 31);
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


