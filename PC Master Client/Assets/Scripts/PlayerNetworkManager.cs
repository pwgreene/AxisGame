using UnityEngine;
using Photon;

public class PlayerNetworkManager : PunBehaviour
{
    public GameObject gameController;

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings("0.3");
    }

    // Use this for initialization
    void Update()
    {

    }


    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinedRoom()
    {
        gameController.SetActive(true);
    }
}
