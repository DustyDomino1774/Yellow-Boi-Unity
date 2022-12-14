using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public Predator[] predators; //players/ghosts
    public Prey prey; //pacman/ai/yellow boi
    public Transform pellets;

    public Text gameOverText;
    public Text scoreText;
    public Text livesText;

    public int predMultiplier { get; private set; } = 1;
    public int score { get; private set; }
    public int lives { get; private set; }
    public int livesDebug;

    private void Start()
    {
        //for controller debugging
        for (int i = 0; i < Gamepad.all.Count; i++)
        {
            Debug.Log(Gamepad.all[i].name);
        }
        NewGame();
    }

    private void Update()
    {
        livesDebug = lives;
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
        //disablel game over text at start of game
        gameOverText.enabled = false;

        foreach (Transform pellet in this.pellets){
            pellet.gameObject.SetActive(true);
        }

        ResetState();
    }

    private void ResetState()
    {
        for (int i = 0; i < this.predators.Length; i++)
            this.predators[i].ResetState();

        this.prey.ResetState();
    }

    private void GameOver()
    {
        gameOverText.enabled = true;
        for (int i = 0; i < this.predators.Length; i++)
            this.predators[i].gameObject.SetActive(false);

        this.prey.gameObject.SetActive(false);
    }

    private void SetScore(int score)
    {
        this.score = score;
        scoreText.text = score.ToString().PadLeft(2, '0');
    }

    private void SetLives(int lives)
    {
        this.lives = lives;
        livesText.text = "x" + lives.ToString();
    }

    public void PredatorCaught(Predator predator)
    {
        int points = predator.points * predMultiplier;
        SetScore(this.score + points);

        predMultiplier++;
    }

    public void PreyCaught()
    {
        prey.DeathSequence();

        this.prey.gameObject.SetActive(false);

        this.lives--;
        livesText.text = "x" + lives.ToString();

        if (this.lives > 0){
            Invoke(nameof(ResetState), 3.0f);
        }
        else
            GameOver();
    }

    public void EatPellet(Pellet pellet) {
        pellet.gameObject.SetActive(false);

        SetScore(score + pellet.points);
        if (!HasRemainingPellets())
        {
            prey.gameObject.SetActive(false);
            Invoke(nameof(NewRound), 3f);
        }
    }

    public void EatPowerPellet(PowerPellet pellet)
    {
        for (int i = 0; i < predators.Length; i++) {
            predators[i].frightened.Enable(pellet.duration);
        }

        EatPellet(pellet);
        CancelInvoke(nameof(ResetPredMultiplier));
        Invoke(nameof(ResetPredMultiplier), pellet.duration);
    }

    private bool HasRemainingPellets()
    {
        foreach (Transform pellet in pellets)
            if (pellet.gameObject.activeSelf)
                return true;

        return false;
    }

    private void ResetPredMultiplier()
    {
        predMultiplier = 1;
    }
}
