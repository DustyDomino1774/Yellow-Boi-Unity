using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Predator[] predators; //players/ghosts
    public Prey prey; //pacman/ai/yellow boi
    public Transform pellets;

    public int score { get; private set; }
    public int lives { get; private set; }

    private void Start()
    {
        NewGame();
    }

    private void Update()
    {
        if ( (this.lives <= 0) &&(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))) {
            NewGame();
        } 
    }

    private void NewGame()
    {
        SetScore(0);
        SetLives(3);
        NewRound();
    }
    
    private void NewRound()
    {
        foreach (Transform pellet in this.pellets){
            pellet.gameObject.SetActive(true);
        }

        for (int i = 0; i < this.predators.Length; i++)
            this.predators[i].gameObject.SetActive(true);

        this.prey.gameObject.SetActive(true);
    }

    private void ResetState()
    {
        for (int i = 0; i < this.predators.Length; i++)
            this.predators[i].gameObject.SetActive(true);

        this.prey.gameObject.SetActive(true);
    }

    private void GameOver()
    {
        for (int i = 0; i < this.predators.Length; i++)
            this.predators[i].gameObject.SetActive(false);

        this.prey.gameObject.SetActive(false);
    }

    private void SetScore(int score)
    {
        this.score = score;
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
    }

    public void PredatorBecomesPrey(Predator predator)
    {
        SetScore(this.score + predator.points);
    }

    public void PreyCaught()
    {
        this.prey.gameObject.SetActive(false);

        SetLives(this.lives - 1);

        if (this.lives > 0){
            Invoke(nameof(ResetState), 3.0f);
        }
        else
            GameOver();
    }
}
