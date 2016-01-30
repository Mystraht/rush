using UnityEngine;

using System.Collections;


public class Utils : MonoBehaviour {
	private static Utils _instance;
	
	private Utils() { }

	public static Utils Instance {
		get {
			if (_instance == null) {
				print("Creating GM Instance");
				GameObject go = new GameObject("Utils");
				go.AddComponent<Utils>();
				_instance = go.GetComponent<Utils>();
			}
			
			return _instance;
		}
	}
	
	
	void Start() {
		
	}
	
	
	void Update() {

	}


	public Color getRGBAByEnum(Spawner.SpawnerColors colors) {
		switch (colors) {
			case Spawner.SpawnerColors.Green:
				return Color.green;
				break;
			case Spawner.SpawnerColors.Blue:
				return Color.blue;
				break;
			case Spawner.SpawnerColors.Red:
				return Color.red;
				break;
			case Spawner.SpawnerColors.Yellow:
				return Color.yellow;
				break;
			default:
				break;
		}

		return Color.green;
	}


    public Color getColorByString(string color) {
        switch (color) {
            case "green":
                return Color.green;
                break;
            case "blue":
                return Color.blue;
                break;
            case "red":
                return Color.red;
                break;
            case "yellow":
                return Color.yellow;
                break;
            default:
                break;
        }

        return Color.green;
    }
}