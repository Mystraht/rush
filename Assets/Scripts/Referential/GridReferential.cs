using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Vectrosity;
using TPMath;

public class GridReferential : MonoBehaviour
{

	static int mainCounter = 0;
	int index = 0;
	VectorLine gridXY = null;
	VectorLine gridXZ = null;
	VectorLine gridYZ = null;
	VectorLine axisX = null;
	VectorLine axisY = null;
	VectorLine axisZ = null;
	VectorLine axisNegX = null;
	VectorLine axisNegY = null;
	VectorLine axisNegZ = null;
	VectorLine letterX = null;
	VectorLine letterY = null;
	VectorLine letterZ = null;
	public Texture textureAxisX;
	public Texture textureAxisNegX;
	public Texture textureAxisY;
	public Texture textureAxisNegY;
	public Texture textureAxisZ;
	public Texture textureAxisNegZ;
	public Texture textureGridXY;
	public Texture textureGridXZ;
	public Texture textureGridYZ;
	public int gridNSteps = 10;
	public float gridDelta = 1f;
	public float gridThickness = 1f;
	public float axisLength = 20f;
	public float axisThickness = 2f;
	public float sphericalEndsSize = .5f;
	public GameObject[] tracers;
	public bool displayGridXZ = true;
	public bool displayGridXY = true;
	public bool displayGridYZ = true;
	public bool displayXYZ = true;
	public bool displayNegativeAxis = true;
	public bool displaySphericalEnds = true;
	public Material materialSphericalEnds;
	public bool traceXEndPos;
	public bool traceYEndPos;
	public bool traceZEndPos;
	bool showReferential = false;
	List<Transform> sphericalEnds ;
	
	void OnGUI ()
	{
		//showReferential = GUI.Toggle (new Rect (Screen.width - 200, Screen.height - 30 * (index + 1), 200, 30), showReferential, name + " Show Ref.");
	}

	void Awake ()
	{
		mainCounter = 0;
	}

	GameObject CreateSphere (Color color, Vector3 localPos, float size)
	{
		GameObject sphGO = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		Destroy (sphGO.GetComponent<Collider> ());
		sphGO.transform.parent = transform;
		sphGO.transform.localPosition = localPos;
		sphGO.transform.localScale = Vector3.one * size;
		Material newMaterial = new Material (materialSphericalEnds);
		newMaterial.color = color;
		sphGO.GetComponent<Renderer> ().material = newMaterial;
		sphGO.layer = LayerMask.NameToLayer ("layer1");

		return sphGO;
	}

	// Use this for initialization
	void Start ()
	{
		index = mainCounter;
		mainCounter++;

		
		List<Vector3> ptsXY = new List<Vector3> ();
		List<Vector3> ptsXZ = new List<Vector3> ();
		List<Vector3> ptsYZ = new List<Vector3> ();

		//GRID XY
		if (displayGridXY) {
			for (int i = 1; i < gridNSteps; i++) {
				ptsXY.Add (new Vector3 (0, i * gridDelta, 0));
				ptsXY.Add (new Vector3 ((gridNSteps + 1) * gridDelta, i * gridDelta, 0));
				ptsXY.Add (new Vector3 (i * gridDelta, 0, 0));
				ptsXY.Add (new Vector3 (i * gridDelta, (gridNSteps + 1) * gridDelta, 0));
			}
			gridXY = new VectorLine ("gridXY", ptsXY, textureGridXY, gridThickness);
		}
		if (displayGridXZ) {
			//GRID XZ
			for (int i = 1; i < gridNSteps; i++) {
				ptsXZ.Add (new Vector3 (i * gridDelta, 0, 0));
				ptsXZ.Add (new Vector3 (i * gridDelta, 0, (gridNSteps + 1) * gridDelta));
				ptsXZ.Add (new Vector3 (0, 0, i * gridDelta));
				ptsXZ.Add (new Vector3 ((gridNSteps + 1) * gridDelta, 0, i * gridDelta));
			}
			gridXZ = new VectorLine ("gridXZ", ptsXZ, textureGridXZ, gridThickness);
		}

		if (displayGridYZ) {
			//GRID YZ
			for (int i = 1; i < gridNSteps; i++) {
				ptsYZ.Add (new Vector3 (0, i * gridDelta, 0));
				ptsYZ.Add (new Vector3 (0, i * gridDelta, (gridNSteps + 1) * gridDelta));
				ptsYZ.Add (new Vector3 (0, 0, i * gridDelta));
				ptsYZ.Add (new Vector3 (0, (gridNSteps + 1) * gridDelta, i * gridDelta));
			}
			gridYZ = new VectorLine ("gridYZ", ptsYZ, textureGridYZ, gridThickness);
		}

		axisX = new VectorLine ("axisX", new List<Vector3> () {
						new Vector3 (0, 0, 0),
						new Vector3 (axisLength, 0, 0)
				}, textureAxisX, axisThickness);
		axisY = new VectorLine ("axisY", new List<Vector3> (){
						new Vector3 (0, 0, 0),
						new Vector3 (0, axisLength, 0)
				}, textureAxisY, axisThickness);
		axisZ = new VectorLine ("axisZ", new List<Vector3> () {
						new Vector3 (0, 0, 0),
						new Vector3 (0, 0, axisLength)
		}, textureAxisZ, axisThickness);

		if (displayNegativeAxis) {
			axisNegX = new VectorLine ("axisNegX", new List<Vector3> (){
								new Vector3 (0, 0, 0),
								new Vector3 (-axisLength, 0, 0)
						}, textureAxisNegX, axisThickness);
			axisNegY = new VectorLine ("axisNegY", new List<Vector3> () {
								new Vector3 (0, 0, 0),
								new Vector3 (0, -axisLength, 0)
			}, textureAxisNegY, axisThickness);
			axisNegZ = new VectorLine ("axisNegZ", new List<Vector3> () {
								new Vector3 (0, 0, 0),
								new Vector3 (0, 0, -axisLength)
			}, textureAxisNegZ, axisThickness);
		}

		if (displayXYZ) {
			letterX = new VectorLine ("letterX", new List<Vector3> () {
						new Vector3 (0, 0, 0),
						new Vector3 (axisLength, 0, 0)
				}, textureAxisX, 1f);
			letterX.MakeText ("X", new Vector3 (axisLength + .5f, .5f, 0), 1);
		
			letterY = new VectorLine ("letterY", new List<Vector3> () {
						new Vector3 (0, 0, 0),
						new Vector3 (axisLength, 0, 0)
			}, textureAxisY, 1f);
			letterY.MakeText ("Y", new Vector3 (-.25f, axisLength + 1.5f, 0), 1);
		
			letterZ = new VectorLine ("letterZ", new List<Vector3> (){
						new Vector3 (0, 0, 0),
						new Vector3 (axisLength, 0, 0)
			}, textureAxisZ, 1f);
			letterZ.MakeText ("Z", new Vector3 (0, .5f, axisLength + .5f), 1);
		}

		if (displaySphericalEnds) {
			sphericalEnds = new List<Transform> ();
				
			//on s'occupe de l'axe Ox
			sphericalEnds.Add (CreateSphere (Color.red, Vector3.right * axisLength, .5f).transform);
			if (traceXEndPos) {
				GameObject tracerGO = Instantiate (tracers [0]) as GameObject;
				tracerGO.transform.parent = sphericalEnds [sphericalEnds.Count - 1];
				tracerGO.transform.localPosition = Vector3.zero;
			}

			//on s'occupe de l'axe Oy
			sphericalEnds.Add (CreateSphere (Color.green, Vector3.up * axisLength, .5f).transform);
			if (traceYEndPos) {
				GameObject tracerGO = Instantiate (tracers [1]) as GameObject;
				tracerGO.transform.parent = sphericalEnds [sphericalEnds.Count - 1];
				tracerGO.transform.localPosition = Vector3.zero;
			}

			//on s'occupe de l'axe Oz
			sphericalEnds.Add (CreateSphere (Color.blue, Vector3.forward * axisLength, .5f).transform);
			if (traceZEndPos) {
				GameObject tracerGO = Instantiate (tracers [2]) as GameObject;
				tracerGO.transform.parent = sphericalEnds [sphericalEnds.Count - 1];
				tracerGO.transform.localPosition = Vector3.zero;
			}
		}
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
		
		axisX.active = showReferential;
		axisY.active = showReferential;
		axisZ.active = showReferential;

		if (displayNegativeAxis) {
			axisNegX.active = showReferential;
			axisNegY.active = showReferential;
			axisNegZ.active = showReferential;
		}
		if (displayGridXY)
			gridXY.active = showReferential;
		if (displayGridXZ)
			gridXZ.active = showReferential;
		if (displayGridYZ)
			gridYZ.active = showReferential;

		if (displayXYZ) {
			letterX.active = showReferential;
			letterY.active = showReferential;
			letterZ.active = showReferential;
		}
				
		if (displaySphericalEnds) {
			foreach (var item in sphericalEnds) {
				item.gameObject.SetActive (showReferential);
			}
		}

		if (showReferential) {
			axisX.drawTransform = transform;
			axisY.drawTransform = transform;
			axisZ.drawTransform = transform;
			axisX.Draw3D ();
			axisY.Draw3D ();
			axisZ.Draw3D ();
		
			if (displayNegativeAxis) {
				axisNegX.drawTransform = transform;
				axisNegY.drawTransform = transform;
				axisNegZ.drawTransform = transform;
				axisNegX.Draw3D ();
				axisNegY.Draw3D ();
				axisNegZ.Draw3D ();
			}

			if (displayGridXY) {
				gridXY.drawTransform = transform;
				gridXY.Draw3D ();
			}
			if (displayGridXZ) {
				gridXZ.drawTransform = transform;
				gridXZ.Draw3D ();
			}
			if (displayGridYZ) {
				gridYZ.drawTransform = transform;
				gridYZ.Draw3D ();
			}
			if (displayXYZ) {
				letterX.drawTransform = transform;
				letterY.drawTransform = transform;
				letterZ.drawTransform = transform;
				letterX.Draw3D ();
				letterY.Draw3D ();
				letterZ.Draw3D ();
			}
		}
	}
}
