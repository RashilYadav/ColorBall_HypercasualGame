using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager MenuManagerInstance;

    public bool GameState;
    public GameObject[] menuElement = new GameObject[2];

    private void Awake()
    {
        MenuManagerInstance = this;
    }

    void Start()
    {
        GameState = false;
        menuElement[3].GetComponent<Text>().text = PlayerPrefs.GetInt("score").ToString();
    }

    void Update()
    {
        
    }

    public void StartTheGame()
    {
        GameState = true;
        menuElement[0].SetActive(false);
        GameObject.FindWithTag("particle").GetComponent<ParticleSystem>().Play();
    }

    public void Retry_btn()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
