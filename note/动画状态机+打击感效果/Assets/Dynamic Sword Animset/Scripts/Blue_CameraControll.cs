using UnityEngine;
using System.Collections;

public class Blue_CameraControll : MonoBehaviour
{

	public GameObject new_Position;
	private Transform from_iniPos;
	private Transform to_iniPos;
	public Vector3 offset_Vector;
	public float smooth_speed;

	// Use this for initialization
	void Start()
	{
		from_iniPos = new_Position.transform;
		to_iniPos = transform;
	}
	
	// Update is called once per frame
	void Update()
	{
		transform.position = Vector3.Lerp(to_iniPos.position, (from_iniPos.position + offset_Vector), smooth_speed);

	}
}
