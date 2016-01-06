using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using SocketIO;

public class SetTankpProperty : MonoBehaviour {
	public ToggleGroup colorselect;
	public InputField nameinput;
	public Material tankcolor;
	public Text Name;

	private SocketIOComponent socket;
	public static string myname;

	private string nowcolor;
	// Use this for initialization
	void Start () 
	{
		GameObject go = GameObject.Find("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();
		socket.On("new user",NewUserCallback);
	}
	
	// Update is called once per frame
	void Update ()
	{
		IEnumerator<Toggle> toggleEnum = colorselect.ActiveToggles ().GetEnumerator ();
		toggleEnum.MoveNext ();
		Toggle toggle = toggleEnum.Current;	
		switch (toggle.name) {
		case "OptionA":
			tankcolor.color = Color.red;
			nowcolor = "red";
		break;
		case "OptionB":
			tankcolor.color = Color.black;
			nowcolor = "black";
		break;
		case "OptionC":
			tankcolor.color = Color.blue;
			nowcolor = "blue";
		break;
		}
		Name.text = nameinput.text;
	}

	public void StartGame()
	{
		myname = nameinput.text;
		var data = new Dictionary<string, string> ();
		data["name"] =  nameinput.text;
		data ["color"] = nowcolor;
		socket.Emit("create player",new JSONObject(data));
		//TODO
	}

	void NewUserCallback(SocketIOEvent data)
	{
		
		bool A = data.data["enemy"].b;
		Debug.Log(data.data +"+"+myname.ToString()+"+"+A );
		TankInfo info = new TankInfo(){name = data.data["name"].ToString(),color = data.data["color"].str};

		if(A == false)
		{
			GameData.instance.Mytankinfo = info;
			Debug.Log (GameData.instance.Mytankinfo.name+"+"+info.color);
			Application.LoadLevelAsync ("g1");
		}
	}
}
