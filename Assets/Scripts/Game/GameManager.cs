using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {
    private static GameManager _instance;

    private GameManager() { }

    private int totalScore = 0;
    private int score = 0;

    public static GameManager Instance {
        get {
            return _instance;
        }
    }

    void Awake() {
        _instance = this;
    }


    void Start() {
        GameObject.Find("/Canvas/Lose").GetComponent<Text>().enabled = false;
        GameObject.Find("/Canvas/Win").GetComponent<Text>().enabled = false;
    }


    void Update() {
        
    }

    public void addScore() {
        if (score == totalScore -1) winGame();
        score++;
    }

    public void addAmountToTotalScore(int amount) {
        totalScore += amount;
    }

    public void loseGame() {
        Debug.Log("Game Lose");
        GameObject.Find("/Canvas/Lose").GetComponent<Text>().enabled = true;
        Metronome.Instance.Pause();
    }

    public void winGame() {
        Debug.Log("Game Win");
        GameObject.Find("/Canvas/Win").GetComponent<Text>().enabled = true;
    }
}