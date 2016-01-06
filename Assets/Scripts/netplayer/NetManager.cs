using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using SocketIO;

// Analysis disable once CheckNamespace
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
	void Start ()
	{
		
		GameObject go =  GameObject.Find ("SocketIO");
		socket = go.GetComponent<SocketIOComponent> ();
		socket.On ("sys message", (SocketIOEvent obj) => Debug.Log (obj.data));

		//用户退出
		socket.On ("exit user", (obj) => Debug.Log (obj.data));


		socket.On("new user",(data) =>{
			Debug.Log (data.data["enemy"].b);
//			bool A = data.data ["enemy"].b;
			TankInfo info = new TankInfo (){ name = data.data ["name"].str, color = data.data ["color"].str };

			if (data.data["enemy"].b == false) {
				myname = info.name;
				GameData.instance.Mytankinfo = info;
				Debug.Log (GameData.instance.Mytankinfo.name + "+" + info.color);
				Application.LoadLevelAsync ("g1");
			
			} else {
				if (TankDataSet.instance != null) {
					TankDataSet.instance.InitTank (info, false);
				} else {
					GameData.instance.Emenytank = info;
				}
			}
		});
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

	public void SendFireInfo()
	{
		socket.Emit("SendFireInfo");
	}

	public void Sendtanklife (Dictionary<string,string> data)
	{
		socket.Emit ("ReciveLife",new JSONObject(data));
	}


	public void CreateMytank(Dictionary<string, string> data)
	{
		Debug.Log ("create player");
		socket.Emit("create player",new JSONObject(data));
	}

	#endregion

	#region othertank
	void ReciveOtherTank()
	{
		socket.On("PlayerMove",PlayerMove);

		socket.On("PlayerRotato",PlayerRotato);

		socket.On ("ReciveFire", (SocketIOEvent obj)=>{Debug.Log("shoot");
			GameObject.Find("enemytank").GetComponent<Complete.TankShooting>().EnemyFire();
		});

		socket.On ("enemylife",(SocketIOEvent obj)=>{Debug.Log("life");
			Debug.Log (obj.data["damagelife"].b);
			if(obj.data["damagelife"].b)
			{
				GameObject.Find(myname).GetComponent<Complete.TankHealth>().TakeDamage(float.Parse (obj.data["damagelife"].str));
			}
			else
			{
				GameObject.Find("enemytank").GetComponent<Complete.TankHealth>().TakeDamage(float.Parse (obj.data["damagelife"].str));
			}
		});
	}
	#endregion 

	void PlayerMove (SocketIOEvent data)
	{
		if (TankDataSet.instance == null) {
			return;
		}
		if(data.data["name"].str != myname)
		{
			string[] pos_array = data.data["Position"].str.Split(',');
			Vector3 pos = new Vector3(float.Parse(pos_array[0]) ,float.Parse(pos_array[1]),float.Parse(pos_array[2]));
			GameObject.Find("enemytank").GetComponent<Complete.TankMovement>().enemyTankMove(pos);
		}
	}

	void PlayerRotato(SocketIOEvent data)
	{
		if (TankDataSet.instance == null) {
			return;
		}

		if(data.data["name"].str != myname)
		{
			if(GameObject.Find("enemytank").transform.localRotation.y.ToString()!= data.data["Positiony"].str)
			{
				GameObject.Find("enemytank").GetComponent<Complete.TankMovement>().enemyTankRotato(float.Parse(data.data["Positiony"].str));
				Debug.Log(data.data);
			}
		}
	}

	 void NewUserCallback(SocketIOEvent data)
	{
		
		bool A = data.data ["enemy"].b;
		Debug.Log (data.data + "+" + myname.ToString () + "+" + A);
		TankInfo info = new TankInfo (){ name = data.data ["name"].str, color = data.data ["color"].str };

		if (A == false) {
			myname = info.name;
			GameData.instance.Mytankinfo = info;
			Debug.Log (GameData.instance.Mytankinfo.name + "+" + info.color);
			Application.LoadLevelAsync ("g1");
		
		} else {
			if (TankDataSet.instance != null) {
				TankDataSet.instance.InitTank (info, false);
			} else {
				GameData.instance.Emenytank = info;
			}
		}
	}

	const int i =0;


}

public class TankInfo
{
	public string name;
	public string color;
}
