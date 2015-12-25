using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SocketIO;

public class NetManager : MonoBehaviour {
	public static NetManager instance;
	private SocketIOComponent socket;
	public InputField uiinput;
	public TankManager m_Tanks;
	public GameObject m_TankPrefab;             

	public static string myname;

	void Awake()
	{
		instance = this;
	}

	// Use this for initialization
	void Start () {
		GameObject go = GameObject.Find("SocketIO");
		socket = go.GetComponent<SocketIOComponent>();
		socket.On("new user",NewUserCallback);
		socket.On ("sys message", (SocketIOEvent obj) => Debug.Log (obj.data));
		ReciveOtherTank();
	}
	#region MyTankInfo
	public void SendMyInfo(Dictionary<string,string> data)
	{
		
		socket.Emit("move",new JSONObject(data));
	}

	public void SendMyTankRotato(Dictionary<string,string> data)
	{
		socket.Emit("rotation",new JSONObject(data));
	}

	public void SendFireInfo(Dictionary<string,string> data)
	{
		socket.Emit("SendFireInfo",new JSONObject(data));
	}
	#endregion

	#region othertank
	void ReciveOtherTank()
	{
		socket.On("PlayerMove",PlayerMove);

		socket.On("PlayerRotato",PlayerRotato);

		socket.On ("ReciveFirePos", (SocketIOEvent obj)=>Debug.Log("movemove"));
	}
	#endregion 

	void PlayerMove(SocketIOEvent data)
	{
		if(data.data["name"].str != myname)
		{
			string[] pos_array = data.data["postion"].str.Split(',');
			Vector3 pos = new Vector3(float.Parse(pos_array[0]) ,float.Parse(pos_array[1]),float.Parse(pos_array[2]));
			GameObject.Find("enemy").GetComponent<Complete.TankMovement>().enemyTankMove(pos);
			Debug.Log(data.data);
		}
	}

	void PlayerRotato(SocketIOEvent data)
	{
		if(data.data["name"].str != myname)
		{
			GameObject.Find("enemy").GetComponent<Complete.TankMovement>().enemyTankRotato(float.Parse(data.data["name"].str));
			Debug.Log(data.data);
		}
	}

	 void NewUserCallback(SocketIOEvent data)
	{
		string A = data.data["name"].str;
		Debug.Log(data.data +"+"+myname.ToString()+"+"+A );
		TankInfo info = new TankInfo(){name = data.data["name"].ToString(),color = data.data["color"].ToString()};
		if(A != myname)
		{
			Debug.Log("enter game");

			InitTank(info,false);
		}
		else
		{
			if(GameObject.Find(data.data["name"].str) == null)
			{
				InitTank(info,true);
			}
		}
	}

	const int i =0;
	void InitTank(TankInfo data,bool mytank)
	{
		Debug.Log("creat tank");
		Complete.GameManager.instance.SpawnAllTanks(mytank);
//		m_Tanks.m_Instance =
//                    Instantiate(m_TankPrefab, m_Tanks.m_SpawnPoint.position, m_Tanks.m_SpawnPoint.rotation) as GameObject;
//                m_Tanks.m_PlayerNumber = i + 1;
//                m_Tanks.Setup();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SureName()
	{
		if(string.IsNullOrEmpty(myname))
		{
			myname = uiinput.text;
			var data = new Dictionary<string, string> ();
				data["name"] =  uiinput.text;
				data ["color"] = "1";
			socket.Emit("create player",new JSONObject(data));
		}
	}
}

public class TankInfo
{
	public string name;
	public string color;
}
