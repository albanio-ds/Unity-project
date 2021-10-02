using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScript : MonoBehaviour
{

    public bool isPlaying = false;
    public GameObject score;
    public GameObject maxScore;
    public GameObject button;
    public GameObject falls;
    public GameObject canvas;
    public float maxScoreNb = 0.0f;
    public bool ended = false;
    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Score"))
        {
            maxScoreNb = PlayerPrefs.GetFloat("Score");
            maxScore.GetComponent<UnityEngine.UI.Text>().text = "Max Score : " + maxScoreNb;
            //PlayerPrefs.DeleteAll();
        }
    }

    public void resetWorld(bool valeur, float scorePlayer, bool pause)
    {
        
        if (pause == false)
        {
            /*foreach (GameObject element in animations)
            {
                element.SetActive(valeur);
            }*/
            ended = true;
            if (scorePlayer < maxScoreNb || maxScoreNb == 0.0f)
            {
                maxScore.GetComponent<UnityEngine.UI.Text>().text = "Max Score : " + Mathf.Round(scorePlayer * 100f) / 100f;
                maxScoreNb = Mathf.Round(scorePlayer * 100f) / 100f;
                PlayerPrefs.SetFloat("Score", maxScoreNb);
            }
            button.SetActive(true);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            canvas.SetActive(!canvas.active);
        }
    }

}
