using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;

namespace Multiplayer
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public GameObject playerPrefab;
        public InputManager inputManager;
        public GameObject arCamera;

        public AudioClip chargingSound;
        public AudioClip reloadSound;

        public static bool respawn = true;
        public GameObject RespawnLine;
        private LayerMask raycastLayerMask;

        // Start is called before the first frame update
        void Start()
        {
            raycastLayerMask = LayerMask.GetMask("Level");
        }

        // Update is called once per frame
        void Update()
        {
            if (respawn)
            {
                Respawn();
            }
        }

        public void Respawn()
        {
            RespawnLine.SetActive(true);
            Vector3 laserSpawn = transform.position - transform.up;
            Vector3 laserAim = transform.forward;
            LineRenderer LaserLineRenderer = gameObject.GetComponent<LineRenderer>();

            LaserLineRenderer.SetPosition(0, laserSpawn);

            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, raycastLayerMask))
            {
                LaserLineRenderer.SetPosition(1, hit.point);
                LaserLineRenderer.endColor = Color.green;
                LaserLineRenderer.startColor = Color.green;
                if (Input.GetKey("v") || Input.touchCount > 0)
                {
                    SpawnTank();
                    respawn = false;
                    RespawnLine.SetActive(false);
                }
            }
            else
            {
                LaserLineRenderer.SetPosition(1, transform.TransformDirection(Vector3.forward) * 1000);
                LaserLineRenderer.endColor = Color.red;
                LaserLineRenderer.startColor = Color.red;
            }
        }

        private void SpawnTank()
        {
            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);

                Vector3 spawnpoint = new Vector3(Mathf.Cos(PhotonNetwork.CountOfPlayers * Mathf.PI) * 15, 2, 0);

                var instance = PhotonNetwork.Instantiate(this.playerPrefab.name, spawnpoint, Quaternion.identity);
                instance.GetComponent<TankMultiplayer>().SetInputManager(inputManager);
                var turret = instance.GetComponentInChildren<TankTurret>();
                turret.SetVariables(arCamera.transform, inputManager);
            }
        }

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }

        private void LoadArena()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to Load a level but we are not the master Client");
            }
            else
            {
                Debug.LogFormat("PhotonNetwork : Loading Level : {0}", PhotonNetwork.CurrentRoom.PlayerCount);
                PhotonNetwork.LoadLevel("MultiplayerGame");
            }
        }

        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            Debug.LogFormat("OnPlayerEnteredRoom() {0}", newPlayer.NickName);

            if (PhotonNetwork.IsMasterClient)
            {
                //LoadArena();
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", otherPlayer.NickName);

            if (PhotonNetwork.IsMasterClient)
            {
                //LoadArena();
            }
        }
    }
}
