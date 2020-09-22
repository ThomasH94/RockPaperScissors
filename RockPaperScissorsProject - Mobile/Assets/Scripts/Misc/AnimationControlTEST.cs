using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationControlTEST : MonoBehaviour
{

    Animator anim;
    int counter = 0;

	// Use this for initialization
	void Start ()
    {
        anim = GetComponent<Animator>();
        //anim.SetTrigger("PlayerChoice");
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
        
	}

    public void CheckHand(bool rock = false, bool paper = false, bool scissors = false)
    {
        anim.SetBool("PlayerChoice", true);
        if(rock)
        {
            anim.SetTrigger("Rock");
        }
        else if (paper)
        {
            anim.SetTrigger("Paper");
        }
        else if (scissors)
        {
            anim.SetTrigger("Scissors");
        }
    }
}
