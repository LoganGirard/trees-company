using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    public GameObject Board;
    public float RestartDelay = 5.0f;
    public Text FinalScoreText;

    private Animator Anim;
    private BoardManager BoardManager;
    private float RestartTimer = 0.0f;


    // Start is called before the first frame update
    void Awake()
    {
        Anim = GetComponent<Animator>();
    }

    void Start()
    {
        BoardManager = Board.GetComponent<BoardManager>();
    }

    // Update is called once per frame
    void Update()
    {
        FinalScoreText.text = BoardManager.HighScore.ToString();
        CheckGameOver();
    }

    void CheckGameOver()
    {
        if (BoardManager.Population <= 0)
        {
            Anim.SetTrigger("GameOver");
            SoundManager.instance.Transition(SoundManager.instance.gameOverMusicSource);
            Board.SetActive(false);


            RestartTimer += Time.deltaTime;

            if (RestartTimer >= RestartDelay)
            {
                RestartTimer = 0;
                Application.LoadLevel(Application.loadedLevel);
            }

        }
    }
}

