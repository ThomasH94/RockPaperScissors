using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour {

    private static LevelLoader instance;

    public static LevelLoader Instance
    {
        get
        {
            if (instance != null)
            {
                instance = GameObject.FindObjectOfType<LevelLoader>();

                if (instance == null)
                {
                    instance = new GameObject().AddComponent<LevelLoader>();
                }
            }

            return instance;
        }
    }

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        else
        {
            Destroy(gameObject);
        }
    }


    // Update is called once per frame
    void Update ()
    {
		if(Input.GetKeyDown(KeyCode.P))
        {
            LoadScene("_Main");
        }
	}

    public void LoadScene(string levelToLoad)
    {
        FindObjectOfType<AudioManager>().StopMusic();

        if (levelToLoad == "_Main")
        {
            SceneManager.LoadScene("_Main");
            FindObjectOfType<AudioManager>().Play("CombatMusic");
        }

        else if (levelToLoad == "_Menu")
        {
            SceneManager.LoadScene("_Menu");
            FindObjectOfType<AudioManager>().Play("MenuMusic");
        }




    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
