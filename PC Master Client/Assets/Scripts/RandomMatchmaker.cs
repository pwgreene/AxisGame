using UnityEngine;
using Photon;
using System.Collections;

public class RandomMatchmaker : Photon.PunBehaviour {

	// Use this for initialization
	void Start()
	{
		PhotonNetwork.ConnectUsingSettings("0.1");
	}

	void OnGUI()
	{
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
	}

	public override void OnJoinedLobby()
	{
		PhotonNetwork.JoinRandomRoom();
	}

	void OnPhotonRandomJoinFailed()
	{
		Debug.Log("Can't join random room!");
		PhotonNetwork.CreateRoom(null);
	}
	public override void OnCreatedRoom ()
	{
		GameObject core= PhotonNetwork.InstantiateSceneObject("rotating_core", Vector3.zero, Quaternion.identity, 0,null);
		GameObject wave= PhotonNetwork.InstantiateSceneObject("WaveManager", Vector3.zero, Quaternion.identity, 0,null);
	}

	public override void OnJoinedRoom(){
		float angle = Random.Range (0, 360);
		GameObject player = PhotonNetwork.Instantiate("turret", new Vector3(6*Mathf.Cos(angle),6*Mathf.Sin(angle),0), Quaternion.identity, 0);
		player.SendMessage ("setControllable", true);
	}

}
