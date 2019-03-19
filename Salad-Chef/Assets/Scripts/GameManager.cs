using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    public static GameManager Instance { get; set; }

    public bool Paused { get; set; }

    public bool gameOver;
    
    public GameObject PlayCanvas;
    public GameObject GameOverCanvas;

    Player[] playersCurrentlyPlaying;
    List<Player> playersLost = new List<Player>();

    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        Paused = true;
        playersCurrentlyPlaying = FindObjectsOfType<Player>();
    }

    public void PlayerLost(Player p)
    {
        playersLost.Add(p);
        //Debug.Log("sd: " + playersLost.Count)
        if(playersLost.Count == playersCurrentlyPlaying.Length)
        {
            GameOverCanvas.SetActive(true);
            Paused = true;
        }
    }


    public void Play()
    {
        Paused = false;
        PlayCanvas.SetActive(false);
    }

    public void TryAgain()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
}
