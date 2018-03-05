using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

/// <summary>
/// 初始化坦克，坦克数据接收
/// </summary>
public class TankDataSet : MonoBehaviour {
	public static TankDataSet instance;
	public Text showmessage;
	public InputField inputfied;
	public Text AllMessageFiled;


	void Awake()
	{
		instance = this;
	}
	// Use this for initialization
	void Start () {
		GameOver("Game Start");
		//初始化坦克
		InitTank (GameData.instance.Mytankinfo, true);
		if (GameData.instance.Emenytank.name != null) {
			InitTank (GameData.instance.Emenytank, false);
		}

	}

	public void SendMyMes ()
	{
		if (!string.IsNullOrEmpty (inputfied.text)) {
			var data = new Dictionary<string,string> ();
			data["name"] = NetManager.myname;
			data["message"] = inputfied.text;
			ReciveMessage(NetManager.myname,inputfied.text);
			inputfied.text = "";

		}
	}

	//send message recive system mes
	public void ReciveMessage (string name, string text)
	{
		Debug.Log ("system message"+text);
		if (!string.IsNullOrEmpty (name)) {
			AllMessageFiled.text = AllMessageFiled.text  + name + "said: " + text+ "\n";
		} else {
			AllMessageFiled.text = AllMessageFiled.text+  text + "\n" ;
		}
	}

	public void GameOver (string message)
	{
		showmessage.text = message;
		showmessage.gameObject.SetActive (true);
		StartCoroutine (hidemes ());
		//TODO
	}

	IEnumerator hidemes ()
	{
		yield return new WaitForSeconds (1f);
		showmessage.gameObject.SetActive (false);
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
