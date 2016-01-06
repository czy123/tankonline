using UnityEngine;
using System.Collections;

/// <summary>
/// 初始化坦克，坦克数据接收
/// </summary>
public class TankDataSet : MonoBehaviour {
	public static TankDataSet instance;

	void Awake()
	{
		instance = this;
	}
	// Use this for initialization
	void Start () {
		//初始化坦克
		InitTank (GameData.instance.Mytankinfo, true);
		if (GameData.instance.Emenytank.name != null) {
			InitTank (GameData.instance.Emenytank, false);
		}

	}

	public void InitTank(TankInfo data,bool mytank)
	{
		Debug.Log("creat tank"+mytank+"+"+data+"+"+Complete.GameManager.instance);
		Complete.GameManager.instance.SpawnAllTanks(mytank,data);
	}

	// Update is called once per frame
	void Update () {
	
	}
}
