using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]

public class Animation_view : MonoBehaviour {

	private Animator anim;
	public Vector2 scrollPosition = Vector2.zero; 
//	private AnimatorStateInfo currentState;		// 
//	private AnimatorStateInfo previousState;	// 
	// Use this for initialization

	void Start () {
		anim = GetComponent<Animator> ();
//		currentState = anim.GetCurrentAnimatorStateInfo (0);
//		previousState = currentState;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	void OnGUI()
	{	
		GUI.Box(new Rect(10 , 10 ,170 , 300), "");
		scrollPosition = GUI.BeginScrollView(new Rect(20, 20, 150, 280), scrollPosition, new Rect(0, 0, 100, 975));
		if(GUI.Button(new Rect(0 , 0 ,130, 20), "Attack_01"))
			anim.SetBool ("Attack_01", true);
		if(GUI.Button(new Rect(0 , 25 ,130, 20), "Attack_02"))
			anim.SetBool ("Attack_02", true);
		if(GUI.Button(new Rect(0 , 50 ,130, 20), "Attack_03"))
			anim.SetBool ("Attack_03", true);
		if(GUI.Button(new Rect(0 , 75 ,130, 20), "Attack_04"))
			anim.SetBool ("Attack_04", true);
		if(GUI.Button(new Rect(0 , 100 ,130, 20), "Attack_05"))
			anim.SetBool ("Attack_05", true);
		if(GUI.Button(new Rect(0 , 125 ,130, 20), "Attack_06"))
			anim.SetBool ("Attack_06", true);
		if(GUI.Button(new Rect(0 , 150 ,130, 20), "Attack_07"))
			anim.SetBool ("Attack_07", true);
		if(GUI.Button(new Rect(0 , 175 ,130, 20), "Attack_08"))
			anim.SetBool ("Attack_08", true);
		if(GUI.Button(new Rect(0 , 200 ,130, 20), "Attack_09"))
			anim.SetBool ("Attack_09", true);
		if(GUI.Button(new Rect(0 , 225 ,130, 20), "Attack_10"))
			anim.SetBool ("Attack_10", true);
		if(GUI.Button(new Rect(0 , 250 ,130, 20), "Attack_11"))
			anim.SetBool ("Attack_11", true);
		if(GUI.Button(new Rect(0 , 275 ,130, 20), "Attack_12"))
			anim.SetBool ("Attack_12", true);
		if(GUI.Button(new Rect(0 , 300 ,130, 20), "Attack_13"))
			anim.SetBool ("Attack_13", true);
		if(GUI.Button(new Rect(0 , 325 ,130, 20), "Attack_14"))
			anim.SetBool ("Attack_14", true);
		if(GUI.Button(new Rect(0 , 350 ,130, 20), "Attack_15"))
			anim.SetBool ("Attack_15", true);
		if(GUI.Button(new Rect(0 , 375 ,130, 20), "Attack_16"))
			anim.SetBool ("Attack_16", true);
		if(GUI.Button(new Rect(0 , 400 ,130, 20), "Attack_17"))
			anim.SetBool ("Attack_17", true);
		if(GUI.Button(new Rect(0 , 425 ,130, 20), "Attack_18"))
			anim.SetBool ("Attack_18", true);
		if(GUI.Button(new Rect(0 , 450 ,130, 20), "Combo_01"))
			anim.SetBool ("Combo_01", true);
		if(GUI.Button(new Rect(0 , 475 ,130, 20), "Combo_02"))
			anim.SetBool ("Combo_02", true);
		if(GUI.Button(new Rect(0 , 500 ,130, 20), "Damage_01"))
			anim.SetBool ("Damage_01", true);
		if(GUI.Button(new Rect(0 , 525 ,130, 20), "Damage_02"))
			anim.SetBool ("Damage_02", true);
		if(GUI.Button(new Rect(0 , 550 ,130, 20), "Damage_Left_01"))
			anim.SetBool ("Damage_Left_01", true);
		if(GUI.Button(new Rect(0 , 575 ,130, 20), "Damage_Left_02"))
			anim.SetBool ("Damage_Left_02", true);
		if(GUI.Button(new Rect(0 , 600 ,130, 20), "Damage_Right_01"))
			anim.SetBool ("Damage_Right_01", true);
		if(GUI.Button(new Rect(0 , 625 ,130, 20), "Damage_Right_02"))
			anim.SetBool ("Damage_Right_02", true);
		if(GUI.Button(new Rect(0 , 650 ,130, 20), "Dash"))
			anim.SetBool ("Dash", true);
		if(GUI.Button(new Rect(0 , 675 ,130, 20), "Dead_01"))
			anim.SetBool ("Dead_01", true);
		if(GUI.Button(new Rect(0 , 700 ,130, 20), "Dead_02"))
			anim.SetBool ("Dead_02", true);
		if(GUI.Button(new Rect(0 , 725 ,130, 20), "Dead_03"))
			anim.SetBool ("Dead_03", true);
		if(GUI.Button(new Rect(0 , 750 ,130, 20), "Dead_04"))
			anim.SetBool ("Dead_04", true);
		if(GUI.Button(new Rect(0 , 775 ,130, 20), "Rolling_Back_01"))
			anim.SetBool ("Rolling_Back_01", true);
		if(GUI.Button(new Rect(0 , 800 ,130, 20), "Rolling_Back_02"))
			anim.SetBool ("Rolling_Back_02", true);
		if(GUI.Button(new Rect(0 , 825 ,130, 20), "Rolling_Front"))
			anim.SetBool ("Rolling_Front", true);
		if(GUI.Button(new Rect(0 , 850 ,130, 20), "Rolling_Left"))
			anim.SetBool ("Rolling_Left", true);
		if(GUI.Button(new Rect(0 , 875 ,130, 20), "Rolling_Right"))
			anim.SetBool ("Rolling_Right", true);
		if(GUI.Button(new Rect(0 , 900 ,130, 20), "Step_Back"))
			anim.SetBool ("Step_Back", true);
		if(GUI.Button(new Rect(0 , 925 ,130, 20), "Step_Left"))
			anim.SetBool ("Step_Left", true);
		if(GUI.Button(new Rect(0 , 950 ,130, 20), "Step_Right"))
			anim.SetBool ("Step_Right", true);

		GUI.EndScrollView();
	}
}
