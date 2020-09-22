using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;     //For Delegates but might not be used
using UnityEngine.UI;               //UI for icons and buttons
using UnityEngine.SceneManagement;  //For updating and restarting our scene

public class Combat : MonoBehaviour {

    //Using a Singleton
    //Might not be used
    private static Combat _instance;
    public static Combat Instance
    {
        get
        {
            if (_instance == null)
            {
                GameObject go = new GameObject("Combat");
                go.AddComponent<Combat>();
            }

            return _instance;
        }
    }

    //Each players animator controller
    //Use these to trigger the animations for the players
    public Animator playerOneHandController, playerTwoHandController;

    //1. This is a list of the possible hands you can select from
    //2. We are using enums to call each hand by name
    enum Hands
    {
        Rock,
        Paper,
        Scissors,
        Empty
    }

    //3. We create new instances of our enum for each player to use
    Hands playerOneHand;
    Hands playerTwoHand;

    //4. We add a turn counter to decide which player has made a selection
    int turnCounter;

    //5. This int is to decide which selection the player has made so it may be compared to the other players selection
    int playerOneSelection, playerTwoSelection;

    //Used to display the score of the player who just scored
    int playerOneScore, playerTwoScore;

    //This bool decides whether or not we play another round
    bool gameOver = false;

    //This holds all of the buttons to disable/enable
    public Button[] playerOneButtons, playerTwoButtons;
    public GameObject[] endGameButtons;
    public Button[] uiButtons;

    //Stores the value of the winner, gets reset after the winner has their score updated
    int winner;

    //These are the player scores to be updated during a game
    //These will NOT be saved anywhere
    int playerOnescore = 0 , player2Score = 0;

    //The text displayed to show each players score
    public Text playerOneScoreText, playerTwoScoreText;
    //The text displayed for when a player wins
    public Text winnerText;

    bool isAgainstComputer;

    //The Camera and the controller script attached to it
    public Camera mainCam;
    CameraController camControl;
    public ParticleSystem confetti;
    public GameObject confet;

    AudioManager am;


    // Use this for initialization
    void Start ()
    {
        //5. We update this amount when a player makes a selection and reset it when player 2 has made a decision
        turnCounter = 1;
        //gameEnd += GameOver;

        //Subscribe the GameOver Method to the Game End event

        foreach (GameObject g in endGameButtons)
        {
            g.SetActive(false);
        }

        playerOneScore = 0;
        playerTwoScore = 0;

        playerOneHandController = playerOneHandController.GetComponent<Animator>();
        winnerText.enabled = false;

        camControl = mainCam.GetComponent<CameraController>();
        confetti.Stop();

        //Cache the audio manager
        am = FindObjectOfType<AudioManager>();

    }
	
    //Enables and Disables buttons depending on whose turn it is
    //Send the number of the players buttons to disable
    void DisableButtons(int playerButtonsToDisable = 0)
    {
        if(playerButtonsToDisable == 1)
        {
            foreach (Button button in playerOneButtons)
            {
                button.interactable = false;
            }
        }

        else if (playerButtonsToDisable == 2)
        {
            foreach (Button button in playerTwoButtons)
            {
                button.interactable = false;
            }
        }
        else
        {
            foreach(Button button in uiButtons)
            {
                button.interactable = false;
            }
        }

    }

    void EnableButtons()
    {
        foreach(Button button in uiButtons)
        {
            button.interactable = true;
        }
    }


	// Update is called once per frame
	void Update ()
    {
        if (turnCounter > 2)
        {
            CheckResults();
        }

        if(Input.GetKeyDown(KeyCode.Space))
        {
            confetti.Stop();
            confet.SetActive(false);
        }

    }

    //This is called when a button is selected for player 1
    //Removing the turn counter lets either player choose
    public void Player1Choices()
    {

        if(EventSystem.current.currentSelectedGameObject.name == "P1_Rock")
        {
            playerOneHand = Hands.Rock;
        }
        else if(EventSystem.current.currentSelectedGameObject.name == "P1_Paper")
        {
            playerOneHand = Hands.Paper;
        }
        else if (EventSystem.current.currentSelectedGameObject.name == "P1_Scissors")
        {
            playerOneHand = Hands.Scissors;
        }

        //playerOneHandController.SetTrigger("Paper");
        playerOneHandController.SetTrigger(playerOneHand.ToString());

        DisableButtons(1);

        //After either choice, update the turn counter
        turnCounter++;


    }


    //This is called for player 2's buttons
    public void Player2Choices()
    {
        //Check to see if you're against the computer
        //If you are, computer picks randomly
        //Otherwise the second player makes a decision
        if(isAgainstComputer)
        {
            //playerTwoHand = (Hands)Random.Range(0, 3);
            playerTwoHand = (Hands)CallRandom();

        }

        else
        {
            if (EventSystem.current.currentSelectedGameObject.name == "P1_Rock")
            {
                playerTwoHand = Hands.Rock;
            }
            else if (EventSystem.current.currentSelectedGameObject.name == "P2_Paper")
            {
                playerTwoHand = Hands.Paper;
            }
            else if (EventSystem.current.currentSelectedGameObject.name == "P2_Scissors")
            {
                playerTwoHand = Hands.Scissors;
            }
        }

        playerTwoHandController.SetTrigger(playerTwoHand.ToString());

        DisableButtons(2);

        //After either choice, update the turn counter
        turnCounter++;

    }

    //Used for the AI's turn
    int CallRandom()
    {
        int random = Random.Range(0, 4);
        return random;
    }

    //This will check to see who the winner of the selections are
    //After choosing a winner, reset the turn counters so the players can play again
    void CheckResults()
    {
        if(am != null)
        {
            StartCoroutine(am.FadeOut("CombatMusic", 0.3f));
        }
        
        //Set the hand Animatior Controllers
        //Disable all other button inputs
        DisableButtons();
        playerOneHandController.SetTrigger("PlayerChoice");
        playerTwoHandController.SetTrigger("PlayerChoice");
        StartCoroutine(camControl.PlayResults());
        StartCoroutine(PlaySounds());
        
        
        
        
        //Check each players selection
        //Reset the turn counter
        //Update the score
        if (turnCounter > 2)
        {
            playerOneSelection = (int)playerOneHand;
            playerTwoSelection = (int)playerTwoHand;
            //Using a switch statement to decide player 1's selection
            //After we find their seleciton, we compare it to player 2's
            switch (playerOneSelection)
            {
                //This case is for if the player selects rock
                //We will then compare it to each of player 2's choices with if statements
                //Case 0 is for Rock
                case 0:
                    if(playerTwoSelection == 0)
                    {
                        print("TIE!");
                        winner = 0;

                    }
                    
                    else if (playerTwoSelection == 1)
                    {
                        print("Player 2 Wins!");
                        winner = 2;
                    }

                    else if (playerTwoSelection == 2)
                    {
                        print("Player 1 Wins!");
                        winner = 1;
                    }
                    break;
                //Case 1 is for Paper
                case 1:
                    if (playerTwoSelection == 0)
                    {
                        print("Player 1 Wins!");
                        winner = 1;

                    }

                    else if (playerTwoSelection == 1)
                    {
                        print("TIE!");
                        winner = 0;
                    }

                    else if (playerTwoSelection == 2)
                    {
                        print("Player 2 Wins!");
                        winner = 2;
                    }
                    break;

                //These are for when player one selects scissors
                case 2:
                    if (playerTwoSelection == 0)
                    {
                        print("Player 2 Wins");
                        winner = 2;
                    }

                    else if (playerTwoSelection == 1)
                    {
                        print("Player 1 Wins!");
                        winner = 1;
                    }

                    else if (playerTwoSelection == 2)
                    {
                        print("TIE");
                        winner = 0;
                    }
                    break;

            }
            if (winner != 0)
            {
                confet.SetActive(true);
                confetti.Play();
            }

            turnCounter = 1;

            //Call the gameover method
            //Checks the GameController on the Game Manager and updates score
            //GameOver(winner);
            StartCoroutine(ResultsTimer());
        }

    }



    //This is called when the game ends
    //Update the score and put the game in an end state
    //Allow the user to play again or exit
    //This adds the score, so it waits until the animation is finished
    //You can add the score sooner in the timer method
    public void GameOver(int scoreToAdd)
    {
        print("Game is over");
        if(winner == 1)
        {
            playerOneScore++;
            playerOneScoreText.text = playerOneScore.ToString();
        }

        else if(winner == 2)
        {
            playerTwoScore++;
            playerTwoScoreText.text = playerTwoScore.ToString();
        }
        
        gameOver = true;
        foreach(GameObject g in endGameButtons)
        {
            g.SetActive(true);
        }
        
    }

    //This is called when you select play again
    //Re-Enable all buttons
    //Reset turn counter(until we remove it)
    //Set GameOver to False
    //Keep score but let players replay
    public void Reset()
    {
        //Stops the hands current animation
        //playerOneHandController.StopPlayback();
        //playerTwoHandController.StopPlayback();
        //Reload scene
        //Don't destroy score
        foreach (GameObject g in endGameButtons)
        {
            g.SetActive(false);
        }
        turnCounter = 1;
        camControl.ResetCam();
        playerOneHandController.SetTrigger("Reset");
        playerTwoHandController.SetTrigger("Reset");
        winnerText.enabled = false;
        confetti.Stop();
        confet.SetActive(false);

        foreach (Button button in playerOneButtons)
        {
            
            foreach(Button but in playerTwoButtons)
            {
                but.interactable = true;
            }

            button.interactable = true;
        }
        playerOneHand = Hands.Empty;
        

        //Call the Player 2 choices if Player Two is a computer
        if(isAgainstComputer)
        {
            playerTwoHand = Hands.Empty;
            Player2Choices();
        }

        gameOver = false;
        EnableButtons();
    }

    //Turns off player 2 and enables AI
    public void EnableAI()
    {
        isAgainstComputer = true;
        Reset();
    }

    //Disables AI and turns on Player 2
    public void EnablePlayer2()
    {
        isAgainstComputer = false;
        Reset();
    }


    //This is called after the RPS animation plays
    //This is used to transition into game over after a few seconds
    IEnumerator ResultsTimer()
    {
        yield return new WaitForSeconds(2.0f);
        winnerText.enabled = true;
        if(winner != 0)
        {
            winnerText.text = string.Format("Player {0} wins!", winner.ToString());
        }
        else
        {
            winnerText.text = string.Format("Draw!");
        }
        
        GameOver(winner);
    }

    //Checks if the audio manager is in the scene
    //If so, play the sounds
    IEnumerator PlaySounds()
    {
        for (int i = 0; i < 3; i++)
        {
            if(am != null)
            {
                am.Play("TickDown");
                yield return new WaitForSeconds(0.75f);
                if (i == 1)
                {
                    am.Play("TickDownFinal");
                    break;
                }
            }
            
        }

        yield return new WaitForSeconds(0.25f);
        if(am != null)
        {
            am.Play("Applause");
        }
        

    }
}
