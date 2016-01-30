using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class CubeMove : MonoBehaviour {
    private string direction = "forward";

    // Convoying
    private bool convoying = false;
    private string directionBeforeConvoying;

    // Teleport
    private bool teleporting = false;
    private Vector3 teleportingArrivalPosition;

	private string[] directionOrder = new string[] { "forward", "right", "back", "left" };

    private float lastCycle;
    private Vector3 currentPosition;
	private Vector3 nextPosition;
	private Vector3 rotationAxis;
	private Vector3 directionAxis;
	private int waitCycle = 0; // Nombre de cycle à attendre
    public string color;
    public float speed;
    
    // Use this for initialization
    void Start() {
        //Move("forward");
    }


    // Update is called once per frame
    void Update() {
		bool isNotNewCycle = Mathf.Floor(Metronome.Instance.time) == Mathf.Floor(lastCycle);
		bool notWaiting = !isWaiting();

		if (isNotNewCycle) {
			if (notWaiting) {
                if (!convoying) {
                    doNormalMovement();
                } else {
                    doConvoyingMovement();
                }
			}
		} else {
            if (convoying) {
                resetConvoying();
                Move(calculNextMovement());
                if (direction != "down") waitCycle++;
            } else {
                if (isWaiting()) {
                    waitCycle--;
                } else {
                    reajustElevation();
                    Move(calculNextMovement());
                }
            }
        }

        lastCycle = Metronome.Instance.time;
    }


	/**
	 * Indique vers quel direction aller
	 * @param string de la direction (right, left, forward, backward)
	 */
    public void Move(string _direction) {
        direction = _direction;

        currentPosition = transform.position;
        nextPosition = transform.position;

        switch (direction) {
            case "right":
                nextPosition.z += 1;
                rotationAxis = Vector3.right;
				directionAxis = Vector3.forward;
                break;
            case "left":
                nextPosition.z -= 1;
				rotationAxis = Vector3.left;
				directionAxis = Vector3.back;
                break;
            case "forward":
                nextPosition.x -= 1;
				rotationAxis = Vector3.forward;
				directionAxis = Vector3.left;
                break;
            case "back":
                nextPosition.x += 1;
				rotationAxis = Vector3.back;
				directionAxis = Vector3.right;
                break;
            case "down":
                nextPosition.y -= 1;
                break;
            default:
                throw new Exception("Incorrect direction in Move function");
                break;
        }
    }


    /**
     * Effectu le mouvement normal du cube
     */
    void doNormalMovement() {
        // Rotation & Position horizontale
        transform.rotation = Quaternion.AngleAxis(Mathf.Lerp(0, 90, Metronome.Instance.timeNormalized), rotationAxis);
        transform.position = Vector3.Lerp(currentPosition, nextPosition, Metronome.Instance.timeNormalized);

        // Elevation de la position
        float half_hypotenuse = Mathf.Sqrt(1 * 1 + 1 * 1) / 2;
        float cubeElevation = half_hypotenuse - 0.5f;
        transform.position = new Vector3(transform.position.x, (float)Math.Floor(transform.position.y) + 0.5f + Mathf.Sin(Mathf.Lerp(0, Mathf.PI, Metronome.Instance.timeNormalized)) * cubeElevation, transform.position.z);
    }


    /**
     * Effectu un mouvement de convoyage
     */
    void doConvoyingMovement() {
        transform.position = Vector3.Lerp(currentPosition, nextPosition, Metronome.Instance.timeNormalized);
    }


    /**
     * Fonction pour désactiver le convoyage
     */
    void resetConvoying() {
        convoying = false;
        direction = directionBeforeConvoying;
        transform.position = new Vector3(Mathf.Round(transform.position.x), (float)Math.Floor(transform.position.y) + 0.5f, Mathf.Round(transform.position.z));
    }


    /**
     * Securité pour réajuster l'elevation du cube par rapport au sol
     */
    void reajustElevation () {
        transform.position = new Vector3(Mathf.Round(transform.position.x), (float)Math.Floor(transform.position.y) + 0.5f, Mathf.Round(transform.position.z));
    }


    /**
     * Si le cube est en train d'attendre
     */
    bool isWaiting() {
        return (waitCycle > 0);
    }


	/**
	 * Donne la prochaine direction du cube
	 * @return nom de la direction suivante
	 */
	string calculNextMovement() {
		Collider colliderUnderMe = getColliderUnderMe();

        if (isCubeBehindMe()) {
            GameManager.Instance.loseGame();
            return direction;
        }

        if (!isGroundUnderMe()) {
            convoying = true;
            directionBeforeConvoying = direction;
            return "down";
        }

        if (colliderUnderMe == null) return direction;

        if (colliderUnderMe.tag == "DeathZone") {
            GameManager.Instance.loseGame();
            return direction;
        }

        if (colliderUnderMe.tag == "Arrow") {
            string newDirection = colliderUnderMe.GetComponent<Arrow>().direction.ToString().ToLower();
            if (newDirection == getDirectionInverse(direction)) GameManager.Instance.loseGame();
            return newDirection;
        }

        if (colliderUnderMe.tag == "Stopper") {
            waitCycle++;
            return direction;
        }

        if (colliderUnderMe.tag == "Split") {
            return colliderUnderMe.GetComponent<Split>().getDirection();
        }

        if (colliderUnderMe.tag == "Convoyor") {
            string newDirection = colliderUnderMe.GetComponent<Conveyor>().direction.ToString().ToLower();
            convoying = true;
            directionBeforeConvoying = direction;
            if (newDirection == getDirectionInverse(direction)) GameManager.Instance.loseGame();
            return newDirection;
        }

        if (colliderUnderMe.tag == "Target") {
            if (colliderUnderMe.GetComponent<Target>().targetColor.ToString().ToLower() == color) validCube();
        }

        if (colliderUnderMe.tag == "Teleporter") {
            waitCycle++;
            teleporting = true;
            teleportingArrivalPosition = colliderUnderMe.GetComponent<Teleporter>().getLinkedTeleporterPosition();
            transform.position = teleportingArrivalPosition;
            transform.position = new Vector3(Mathf.Round(transform.position.x), (float)Math.Floor(transform.position.y) + 0.5f, Mathf.Round(transform.position.z));
        }

        if (lookForWallTowardMeInDirectionAxis(directionAxis)) {
            if (lookForWallTowardMeInDirectionAxis(Quaternion.AngleAxis(90, Vector3.up) * directionAxis)) {
                waitCycle += 2;
                return getDirectionInverse(direction);
            }
            waitCycle += 2;
            return getRightDirection();
        }

        return direction;
	}


	/**
	 * Récupère l'objet en dessous du cube (Si il y en a un, sinon return null)
	 * @return 
	 */
	 Collider getColliderUnderMe () {
		RaycastHit hitInfo;

		if(Physics.Raycast(nextPosition, Vector3.down, out hitInfo)) {
			if (hitInfo.distance <= 0.5f) {
				return hitInfo.collider;
			}
		}

		return null;
	}


    /**
	 * Permet de savoir si il y a un wall autour du cube
     * @param Vector3 Direction du check
	 * @return bool - true/false
	 */
    bool lookForWallTowardMeInDirectionAxis(Vector3 axis) {
        bool _isWall = false;
        RaycastHit hitInfo;

        if (Physics.Raycast(nextPosition, axis, out hitInfo)) {
            if (hitInfo.collider.tag == "Wall" && hitInfo.distance < 1.0f) {
                _isWall = true;
            }
        }

        return _isWall;
    }


    /**
	 * Permet de savoir si il y a un cube devant le cube
	 * @return bool - true/false
	 */
    bool isCubeBehindMe() {
        bool _isCube = false;
        RaycastHit hitInfo;

        if (Physics.Raycast(nextPosition, directionAxis, out hitInfo)) {
            if (hitInfo.collider.tag == "Cube" && hitInfo.distance < 1.0f) {
                _isCube = true;
            }
        }

        return _isCube;
    }


    /**
	 * Permet de savoir si il y a un sol en dessous du cube
	 * @return bool - true/false
	 */
    bool isGroundUnderMe() {
        bool _isGround = false;
        RaycastHit hitInfo;

        if (Physics.Raycast(transform.position, Vector3.down, out hitInfo)) {
            if (hitInfo.distance <= 0.51f) {
                _isGround = true;
            }
        }

        return _isGround;
    }



    /**
	 * Permet de donner le nom de la direction à droite de la direction actuel. (ex: forward -> right)
	 * @return la direction à droite de la direction actuel
	 */
    string getRightDirection() {
		if (directionOrder[directionOrder.Length - 1] == direction) {
			return directionOrder[0];
		}

		for (int i = 0; i < directionOrder.Length; i++) {
			if (directionOrder[i] == direction) {
				return directionOrder[i + 1];
			}
		}

		return "";
	}


    /**
     * Permet de récupérer la direction inverse de la direction entré en paramètre
     * @param direction
     */
    string getDirectionInverse(string direction) {
        if (direction == "forward") return "back";
        if (direction == "back") return "forward";
        if (direction == "right") return "left";
        if (direction == "left") return "right";
        return null;
    }
    

    /**
     * Valide le cube
     */
    void validCube () {
        GameManager.Instance.addScore();
        Destroy(gameObject);
    }
}
