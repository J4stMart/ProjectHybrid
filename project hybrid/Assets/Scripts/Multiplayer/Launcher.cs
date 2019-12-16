using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;


namespace Multiplayer
{

    public class Launcher : MonoBehaviourPunCallbacks
    {

        [SerializeField]
        private GameObject controlPanel;
        [SerializeField]
        private GameObject progressLabel;

        [SerializeField]
        private string gameVersion = "0.0.1";
        [SerializeField]
        private byte maxPlayerPerRoom = 4;

        private bool isConnected;

        private void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        // Start is called before the first frame update
        void Start()
        {
            controlPanel.SetActive(true);
            progressLabel.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public void Connect()
        {
            isConnected = true;

            controlPanel.SetActive(false);
            progressLabel.SetActive(true);

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                PhotonNetwork.GameVersion = this.gameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        #region MonoBehaviourPunCallBacks CallBacks

        public override void OnConnectedToMaster()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnConnectedToMaster() was called by PUN");

            if (isConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }   
        }

        public override void OnDisconnected(DisconnectCause cause)
        {   
            Debug.LogWarningFormat("PUN Basics Tutorial / Launcher: OnDisconnected() was called by PUN with reason { 0}", cause);

            controlPanel.SetActive(true);
            progressLabel.SetActive(false);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("PUN Basics Tutorial/Launcher:OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");
            Debug.Log(returnCode + " " + message);

            Debug.Log(PhotonNetwork.CountOfRooms);

            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = this.maxPlayerPerRoom });
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");

            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                PhotonNetwork.LoadLevel("ArMultiplayer");
            }
        }

        #endregion
    }
}
