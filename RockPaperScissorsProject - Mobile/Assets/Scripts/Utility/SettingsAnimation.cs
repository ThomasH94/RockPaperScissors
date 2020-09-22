using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsAnimation : MonoBehaviour {

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
        {
            anim.SetTrigger("Disabled");
        }
    }

}
