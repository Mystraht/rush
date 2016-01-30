using UnityEngine;

using System.Collections;


public class Metronome : MonoBehaviour {
    private static Metronome _instance;

    private Metronome() { }
    private bool paused = false;

    private int cycleCount = 0;
    public float time = 0.0f;
    public float timeNormalized = 0.0f;
    public float timeStep = 3.0f;

    public static Metronome Instance {
        get {
            return _instance;
        }
    }

    void Awake() {
        _instance = this;
    }

    void Start() {

    }


    void Update() {
        if (!paused) {
            time += timeStep * Time.deltaTime;
            timeNormalized = time % 1;
            cycleCount = (int)Mathf.Floor(time);
        }
    }

    public void Pause() {
        paused = true;
    }

    public void Resume() {
        paused = false;
    }
}