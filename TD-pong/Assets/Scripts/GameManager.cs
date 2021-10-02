using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject Player1;

    [SerializeField]
    private GameObject Player2;

    [SerializeField]
    private Vector3 startP1 = new Vector3(-10.0f, 0.5f, 0.0f);

    private float p1X;
    private float p2X;

    [SerializeField]
    private Vector3 startP2 = new Vector3(10.0f, 0.5f, 0.0f);

    [SerializeField]
    private GameObject ball;

    [SerializeField]
    private GameObject ballPosition;

    [SerializeField]
    private Text MaxEchanges;

    [SerializeField]
    private Text Score;

    [SerializeField]
    private GameObject PlayB;

    [SerializeField]
    private Text pause;

    [SerializeField]
    private BotFollowScript botFollowScript;

    [SerializeField]
    private GameObject buttonBot;

    [SerializeField]
    private GameObject randomPower;

    [SerializeField]
    private Text textBot;

    private int mode = 0;
    private int record = 1;

    private int score1;
    private int score2;
    private int maxScore = 5;

    private bool spawned = false;

    //spaawn ball et dictionnzry, dico score player, enum plzyers
    //txt olayerd scores value

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Record"))
        {
            record = PlayerPrefs.GetInt("Record");
        }
        p1X = Player1.transform.position.x;
        p2X = Player2.transform.position.x;
        //PlayerPrefs.DeleteKey("Record");
        PlayB.SetActive(true);
        buttonBot.SetActive(true);
        botUpdateButton();
        //int scores a 0
    }

    private void resetPlayers()
    {
        Player1.SetActive(true);
        Player2.SetActive(true);
        //Player1.transform.position = new Vector3(p1X, 0.5f, 0);
        //Player2.transform.position = new Vector3(p2X, 0.5f, 0);
        //Player1.transform.SetPositionAndRotation(startP1, Quaternion.identity);
        //Player2.transform.SetPositionAndRotation(startP2, Quaternion.identity);
    }

    public void botClick()
    {
        mode++;
        if (mode == 6) mode = 0;
        botUpdateButton();
    }
    private void botUpdateButton()
    {
        if (mode == 0)
        {
            Player2.GetComponent<PlayerMovementScript>().enabled = true;
            Player2.GetComponent<BotFollowScript>().enabled = false;
            textBot.text = "vs Player";
        }
        if (mode == 1)
        {
            Player2.GetComponent<PlayerMovementScript>().enabled = false;
            Player2.GetComponent<BotFollowScript>().enabled = true;
            botFollowScript.setSpeed(3.0f);
            textBot.text = "vs Clo Bot";
        }
        if (mode == 2)
        {
            Player2.GetComponent<PlayerMovementScript>().enabled = false;
            Player2.GetComponent<BotFollowScript>().enabled = true;
            botFollowScript.setSpeed(8.0f);
            textBot.text = "vs Max Bot";
        }
        if (mode == 3)
        {
            Player2.GetComponent<PlayerMovementScript>().enabled = false;
            Player2.GetComponent<BotFollowScript>().enabled = true;
            botFollowScript.setSpeed(13.0f);
            textBot.text = "vs Dayt Bot";
        }
        if (mode == 4)
        {
            Player2.GetComponent<PlayerMovementScript>().enabled = false;
            Player2.GetComponent<BotFollowScript>().enabled = true;
            botFollowScript.setSpeed(18.0f);
            textBot.text = "vs Albanio Bot";
        }
        if (mode == 5)
        {
            Player2.GetComponent<PlayerMovementScript>().enabled = false;
            Player2.GetComponent<BotFollowScript>().enabled = true;
            botFollowScript.setSpeed(23.0f);
            textBot.text = "vs Nagan Bot";
        }
    }

    IEnumerator modeLocked()
    {
        textBot.text = "Locked";
        yield return new WaitForSeconds(1);
        botUpdateButton();
    }
        public void playButton()
    {
        if (mode > record)
        {
            StartCoroutine(modeLocked());
            return;
        }
        buttonBot.SetActive(false);
        Time.timeScale = 1;
        resetPlayers();
        PlayB.SetActive(false);
        score1 = 0;
        score2 = 0;
        //Player1.transform.position = startP1;
        //Player2.transform.position = startP2;
        Score.text = score1 + "-" + score2 + " ";
        MaxEchanges.text = "";
        spawnBall();
    }

    private void spawnBall()
    {
        //echanges = 0;
        botFollowScript.setBall( Instantiate<GameObject>(ball, ballPosition.transform.position, Quaternion.identity) );
        Player1.transform.position = new Vector3(p1X, 0.5f, 0);
        Player2.transform.position = new Vector3(p2X, 0.5f, 0);
    }

    private void destroyBonus()
    {
        var gameObjectsList = GameObject.FindGameObjectsWithTag("Bonus");
        foreach (GameObject obj in gameObjectsList)
        {
            Destroy(obj);
        }
    }

    private void EndGame()
    {
        PlayB.SetActive(true);
        buttonBot.SetActive(true);
        destroyBonus();
        Time.timeScale = 0;
    }

    public void ScoreGoal( String name, GameObject gameObject)
    {
        //++scorePalyer[owner]++;
        //update score UI
        if (name == "GoalP1")
        {
            score2++;
        } else
        {
            score1++;
        }
        Score.text = score1 + "-" + score2 + " ";

        Destroy(gameObject);
        if (score1 == maxScore - 1 || score2 == maxScore - 1) { MaxEchanges.text = "Match Point"; MaxEchanges.color = Color.black; }
        if (score1 == maxScore)
        {
            MaxEchanges.text = "Player 1 win !";
            MaxEchanges.color = Color.blue;
            if (mode == record)
            {
                record++;
                PlayerPrefs.SetInt("Record", record);
            }
            Player2.SetActive(false);
            EndGame();
            return;
        } else if (score2 == maxScore)
        {
            MaxEchanges.text = "Player 2 win !";
            MaxEchanges.color = Color.red;
            Player1.SetActive(false);
            EndGame();
            return;
        }
        spawnBall();
        //spawn

    }


    //update score uUI : tewt = ScoreByPlr[Player,playeer1].tostring;
    //pareil p2
    // Update is called once per frame
   
    void Update()
    {
        spawnRandomPower();
        lookPause();
    }


    private void lookPause()
    {
        if (PlayB.activeInHierarchy) return;
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (Time.timeScale == 0)
                {
                    Time.timeScale = 1;
                    pause.text = "";
                } else
                {
                    Time.timeScale = 0;
                    pause.text = "Pause";
                }
            }
        }
    }

    IEnumerator spawnTimeSetter()
    {
        spawned = true;
        yield return new WaitForSeconds(10.0f);
        spawned = false;
    }

   
    private void spawnRandomPower()
    {
        if (spawned || PlayB.activeInHierarchy) return;
        if (UnityEngine.Random.Range(0.0f, 10.0f) < 8) return;
        StartCoroutine(spawnTimeSetter());
        float x = UnityEngine.Random.Range(-7.0f,7.0f);
        float z = UnityEngine.Random.Range(-3.0f, 3.0f);
        Instantiate<GameObject>(randomPower, new Vector3(x, 0.5f, z), Quaternion.identity);
    }
}
