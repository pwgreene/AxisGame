using UnityEngine;
using Photon;
using System.Collections;

public class RandomMatchmaker : Photon.PunBehaviour
{

	Color[] playerColors = new Color[]{Color.blue, Color.red, Color.green, Color.magenta, Color.cyan};
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
		GameObject powerUps = PhotonNetwork.InstantiateSceneObject("PowerupManager", Vector3.zero, Quaternion.identity, 0, null);

    }


    public override void OnJoinedRoom()
    {
        
		float radius = 6;
		float startingAngle = 0;

		GameObject player = PhotonNetwork.Instantiate("turret", new Vector3(radius* Mathf.Cos(startingAngle), radius * Mathf.Sin(startingAngle), 0), Quaternion.identity, 0);
		//TurretController controller = player.GetComponent<TurretController> ();
		//controller.playerColor =playerColors [PhotonNetwork.playerList.Length - 1];

		player.GetComponent<PhotonView>().RPC("SetColor", playerColors[PhotonNetwork.playerList.Length -1]);
		player.SendMessage("setControllable", true);
		//photonView.RPC("SomeFunction", PhotonTargets.All, sender.gameObject.GetPhotonView().viewID, target.gameObject.GetPhotonView().viewID);
			
        
    }


}
