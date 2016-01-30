using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEditor;

public class Spawner : MonoBehaviour {

	private float lastCycle;

    public int totalCubeToGenerate = 5;
    private int cubeSpawned = 0;
	private int cycleBeforeSpawnCount = 10;
	private int cycleBeforeSpawn = 0;
    public GameObject cubePrefab;

	public enum SpawnerColors
	{
		Green,
		Red,
		Blue,
		Yellow
	}

    public enum SpawnerDirection {
        Forward,
        Right,
        Back,
        Left
    }

    public SpawnerColors spawnerColor;
    public SpawnerDirection spawnerDirection;
	
	// Use this for initialization
	void Start() {
		GetComponent<ParticleSystem>().startColor = Utils.Instance.getRGBAByEnum(spawnerColor);
        GameManager.Instance.addAmountToTotalScore(totalCubeToGenerate);
    }
	
	
	// Update is called once per frame
	void Update() {
		bool isNewCycle = Mathf.Floor(Metronome.Instance.time) > Mathf.Floor(lastCycle);

		if (isNewCycle) {
			if (cycleBeforeSpawn != 0) cycleBeforeSpawn--;
			else {
				cycleBeforeSpawn = cycleBeforeSpawnCount;

                if (cubeSpawned < totalCubeToGenerate) {
                    createNewCube();
                }

                cubeSpawned++;
            }
		}

		lastCycle = Metronome.Instance.time;
	}


	/**
	 * Crée un nouveau cube de la couleur du spawner
	 */
	void createNewCube () {
		GameObject cube = (GameObject)Instantiate(cubePrefab, transform.position, Quaternion.identity);
		cube.GetComponent<CubeMove>().Move(spawnerDirection.ToString().ToLower());
        cube.GetComponent<CubeMove>().color = spawnerColor.ToString().ToLower();
        cube.GetComponent<Renderer>().material.color = Utils.Instance.getColorByString(spawnerColor.ToString().ToLower());
        cube.transform.position.Set(transform.position.x, transform.position.y + 3.45f, transform.position.z);
        cube.tag = "Cube";
    }
}


