using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


/*The purpose of this class is to enable and disable the settings menu from any UI that will interact with the settings*/
public class EnableSettings : MonoBehaviour {

    //This is where we will get our canvas and determine if it can be enabled/disabled
    public GameObject canvas;
    private Animator anim;
    //Singleton
    int loadedCount = 0;
    //Check if this exists and create if it doesn't
    void Awake()
    {
       
    }

	// Use this for initialization
	void Start ()
    {
        anim = canvas.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update ()
    {
        //Debugging
	    if(Input.GetKeyDown(KeyCode.K))
        {
            EnableCanvas();
        }        
	}

    //Diables and enables the canvas
    public void EnableCanvas()
    {
        //Doesn't need to set the anim because it's the default state
        if(canvas != null && canvas.activeSelf == false)
        {
            StartCoroutine(SlowDown());
        }
    }

    public void DisableCanvas()
    {
        if (canvas != null && canvas.activeSelf == true)
        {
            StartCoroutine(SlowDown());
        }
    }

    //Checks if the canvas is enabled 
    //Plays an animation and makes you wait for it to finish
    public IEnumerator SlowDown()
    {
        //anim.SetTrigger("Enabled");
        yield return new WaitForSeconds(0.1f);

        //Enable/Disabled based on the method that calls this
        if (canvas.activeSelf == true)
        {
            canvas.SetActive(false);
        }

        else if (canvas.activeSelf == false)
        {
            canvas.SetActive(true);
        }
    }
}
