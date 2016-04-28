using UnityEngine;
using UnityEngine.SceneManagement;
using Photon;
using System.Collections.Generic;

public class RandomMatchmaker : PunBehaviour
{
    // Use this for initialization

    public TurretController[] turrets;

    Dictionary<int, TurretController> playerBindings;

    void Awake()
    {
        PhotonNetwork.OnEventCall += OnEvent;
    }

    void Start()
    {
        PhotonNetwork.ConnectUsingSettings("0.3");
        playerBindings = new Dictionary<int, TurretController>();
    }

    void OnGUI()
    {
        GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    void OnPhotonRandomJoinFailed()
    {
        Debug.Log("Can't join random room!");
        PhotonNetwork.CreateRoom("RoomToJoin");
    }

    public override void OnCreatedRoom()
    {
    }

    public override void OnJoinedRoom()
    {
        if (!PhotonNetwork.isMasterClient)
        {
            PhotonNetwork.Disconnect();
            SceneManager.LoadScene(2);
        }
    }

    public override void OnPhotonPlayerConnected(PhotonPlayer newPlayer)
    {
        base.OnPhotonPlayerConnected(newPlayer);

        if (PhotonNetwork.isMasterClient)
        {
            foreach (TurretController turret in turrets)
            {
                if (!playerBindings.ContainsValue(turret))
                {
                    playerBindings.Add(newPlayer.ID, turret);
                    turret.setControllable(true);
                    break;
                }
            }
        }
    }

    public override void OnPhotonPlayerDisconnected(PhotonPlayer otherPlayer)
    {
        base.OnPhotonPlayerDisconnected(otherPlayer);

        if (PhotonNetwork.isMasterClient)
        {
            TurretController otherPlayerTurret;

            if (playerBindings.TryGetValue(otherPlayer.ID, out otherPlayerTurret))
            {
                otherPlayerTurret.setControllable(false);
                playerBindings.Remove(otherPlayer.ID);
            }
        }
    }

    private void OnEvent(byte eventCode, object content, int senderID)
    {
        TurretController turretToControl;

        if (playerBindings.TryGetValue(senderID, out turretToControl))
        {
            if (eventCode == 0)
            {
                // Rotate the player's turret
                float horizontal = (float)content;
                turretToControl.SetHorizontal(horizontal);
            }
            else if (eventCode == 1)
            {
                // Move the player's turret up or down the shaft
                float vertical = (float)content;
                turretToControl.SetVertical(vertical);
            }
            else if (eventCode == 2)
            {
                // Fire the player's weapon
                int firing = Mathf.RoundToInt((float)content);
                turretToControl.SetFiring(firing);
            }
            else if (eventCode == 3)
            {
                // Change the player's weapon
                int ammoType = Mathf.RoundToInt((float)content);
                turretToControl.SetWeapon(ammoType);
            }
        }
        else
        {
            Debug.LogError("A player without a turret just sent an event.");
        }
    }
}
