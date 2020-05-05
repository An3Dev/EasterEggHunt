using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class FPS : MonoBehaviour
{

    public TextMeshProUGUI text;

    // Update is called once per frame
    void Update()
    {
        // sets the fps text to the frames per second
        text.text = (1 / Time.deltaTime).ToString();
    }
}
