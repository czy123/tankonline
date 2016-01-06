using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;

public class SetTankpProperty : MonoBehaviour {
	public ToggleGroup colorselect;
	public InputField nameinput;
	public Material tankcolor;
	public Text Name;

	public static string myname;

	private string nowcolor;
	// Use this for initialization
	void Start () 
	{
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

		NetManager.instance.CreateMytank (data);
		//TODO
	}
}
