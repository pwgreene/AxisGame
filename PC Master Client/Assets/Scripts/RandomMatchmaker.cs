using UnityEngine;
using Photon;
using System.Collections;
using UnityEngine.SceneManagement;

public class RandomMatchmaker : Photon.PunBehaviour
{
	TextAsset animal_names;
    // Use this for initialization
    void Start()
    {
		

		//PhotonNetwork.logLevel = PhotonLogLevel.Full;
		if (PhotonNetwork.insideLobby) {
			PhotonNetwork.JoinRandomRoom();
		} else {
			
			PhotonConnect ();
		}
    }

	public void PhotonConnect (){
		PhotonNetwork.ConnectUsingSettings("0.2");
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
		//animal_names = Resources.Load("animal_names",typeof(TextAsset)) as TextAsset;

		//string[] dataLines = animal_names.text.Split('\n');
		//string name = dataLines [Mathf.RoundToInt (Random.Range (0, dataLines.Length - 1))];
		string name = "roomID" + Random.Range(0,100).ToString();
        Debug.Log("Can't join random room!" + "creating room: " + name);
        PhotonNetwork.CreateRoom(name);
    }

    public override void OnCreatedRoom()
    {
		
        PhotonNetwork.InstantiateSceneObject("rotating_core", Vector3.zero, Quaternion.identity, 0, null);
        PhotonNetwork.InstantiateSceneObject("WaveManager", Vector3.zero, Quaternion.identity, 0, null);
		//PhotonNetwork.InstantiateSceneObject("PowerupManager", Vector3.zero, Quaternion.identity, 0, null);

    }


    public override void OnJoinedRoom()
    {
        
		float radius = 6;
		float startingAngle = 0;

		GameObject player = PhotonNetwork.Instantiate("turret", new Vector3(radius* Mathf.Cos(startingAngle), radius * Mathf.Sin(startingAngle), 0), Quaternion.identity, 0);
		//TurretController controller = player.GetComponent<TurretController> ();
		//controller.playerColor =playerColors [PhotonNetwork.playerList.Length - 1];

		//if a player leaves and rejoins they get the same color
	
		player.SendMessage("setControllable", true);
		//photonView.RPC("SomeFunction", PhotonTargets.All, sender.gameObject.GetPhotonView().viewID, target.gameObject.GetPhotonView().viewID);
			
        
    }


	public override void OnLeftRoom()
	{
		SceneManager.LoadScene(0);
	}



}
