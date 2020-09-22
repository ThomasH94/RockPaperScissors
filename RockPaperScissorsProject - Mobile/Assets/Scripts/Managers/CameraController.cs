using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {

    public Camera mainCam;
    [SerializeField]
    float defaultSize = 6.0f;
	
	// Update is called once per frame
	void Update ()
    {
		if(Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(PlayResults());
        }
	}

    public IEnumerator PlayResults()
    {
       //Set the camera too zoom in
       //Then wait
       //Then repeat
       //Do this 3 times

        for(int i = 0; i < 3; i++)
        {
            //zoom, then yield
            Time.timeScale = 0.6f;
            mainCam.orthographicSize -= 0.5f;
            yield return new WaitForSeconds(0.75f);
            if(i == 3)
            {
                break;
            }
        }
    }

    public void ResetCam()
    {
        mainCam.orthographicSize = defaultSize;
    }
}
