using UnityEngine;
using Photon;
using System.Collections;

public class RandomMatchmaker : Photon.PunBehaviour
{
    // Use this for initialization

    public TurretController[] turrets;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings("0.3");
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
    }

    public override void OnJoinedRoom()
    {
        if (!PhotonNetwork.isMasterClient)
        {

        }
    }
}
