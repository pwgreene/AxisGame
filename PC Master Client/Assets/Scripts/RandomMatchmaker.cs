using UnityEngine;
using Photon;
using System.Collections;

public class RandomMatchmaker : Photon.PunBehaviour
{
    // Use this for initialization
    void Start()
    {
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
        Debug.Log("Can't join random room!");
        PhotonNetwork.CreateRoom(null);
    }

    public override void OnCreatedRoom()
    {
        GameObject core = PhotonNetwork.InstantiateSceneObject("rotating_core", Vector3.zero, Quaternion.identity, 0, null);
        GameObject wave = PhotonNetwork.InstantiateSceneObject("WaveManager", Vector3.zero, Quaternion.identity, 0, null);
    }

    public override void OnJoinedRoom()
    {
        //if (!PhotonNetwork.isMasterClient)
        //{
		float radius = 6;
		float startingAngle = 0;
		if (PhotonNetwork.isMasterClient) {
			
			//disable the main camera
			int numPlayers = PhotonNetwork.playerList.Length + 1 ;
			float angle = 360f / (float) numPlayers;

			for(int i = 0; i < (numPlayers -1); i ++){
				Transform plTrans = PhotonView.Find (PhotonNetwork.playerList[i].ID).GetComponent < Transform> ();
				if (i == 0) {
					startingAngle = Mathf.Atan2 (plTrans.position.y, plTrans.position.x);
				} else {
					float newRadius = Mathf.Sqrt(Mathf.Pow(plTrans.position.x,2) + Mathf.Pow(plTrans.position.y, 2));
					startingAngle += angle;
					Vector3 newPos = new Vector3(newRadius * Mathf.Cos(startingAngle), newRadius * Mathf.Sin(startingAngle),0);
				}

			}
		}

			GameObject player = PhotonNetwork.Instantiate("turret", new Vector3(radius* Mathf.Cos(startingAngle), radius * Mathf.Sin(startingAngle), 0), Quaternion.identity, 0);
            player.SendMessage("setControllable", true);


        //}
    }
}
