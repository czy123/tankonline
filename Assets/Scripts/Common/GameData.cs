using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameData : MonoBehaviour {


	void Awake()
	{
		DontDestroyOnLoad (this);
		instance = this;
	}

	public static GameData instance ;

	private TankInfo mytankinfo = new TankInfo();
	public TankInfo Mytankinfo {
		get {
			return mytankinfo;
		}
		set {
			mytankinfo = value;
		}
	}
}
