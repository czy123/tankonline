using UnityEngine;
using System.Collections;
using System;

public class EasyTouchManager : MonoBehaviour
{
	public Transform Target;
	ETCJoystick etcJoy;
	// Use this for initialization
	void Start ()
	{
		etcJoy = GetComponent <ETCJoystick> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (null != Target && null != etcJoy
		    && (null == etcJoy.axisX.directTransform || null == etcJoy.axisY.directTransform)) {

			etcJoy.axisX.directTransform = Target;
			etcJoy.axisY.directTransform = etcJoy.axisX.directTransform;


		} else if (null != Target) {
			Complete.TankShooting tankShoot = Target.GetComponent <Complete.TankShooting> ();
			if (ETCInput.GetButtonDown ("Fire")) {
				tankShoot.Down ();
			}
			if (ETCInput.GetButton ("Fire") && !tankShoot.m_Fired) {
				tankShoot.Getbtn = true;
			} else {
				tankShoot.Getbtn = false;
			}
			if (ETCInput.GetButtonUp ("Fire") && !tankShoot.m_Fired) {
				tankShoot.Fire ();
			}
		} else {
			return;
		}
	}
}
