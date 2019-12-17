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
        public GameObject respawnLine;
        private LineRenderer lineRenderer;
        private LayerMask raycastLayerMask;

        // Start is called before the first frame update
        void Start()
        {
            raycastLayerMask = LayerMask.GetMask("Level");
            lineRenderer = respawnLine.GetComponent<LineRenderer>();
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
            respawnLine.SetActive(true);
            Vector3 laserSpawn = respawnLine.transform.position - respawnLine.transform.up;
            Vector3 laserAim = respawnLine.transform.forward;

            lineRenderer.SetPosition(0, laserSpawn);

            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(respawnLine.transform.position, respawnLine.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, raycastLayerMask))
            {
                lineRenderer.SetPosition(1, hit.point);
                lineRenderer.endColor = Color.green;
                lineRenderer.startColor = Color.green;
                if (Input.GetKey("v") || Input.touchCount > 0)
                {
                    SpawnTank(hit.point + new Vector3(0, 20, 0));
                    respawn = false;
                    respawnLine.SetActive(false);
                }
            }
            else
            {
                lineRenderer.SetPosition(1, transform.TransformDirection(Vector3.forward) * 1000);
                lineRenderer.endColor = Color.red;
                lineRenderer.startColor = Color.red;
            }
        }

        private void SpawnTank(Vector3 spawnpoint)
        {
            if (playerPrefab == null)
            {
                Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
            }
            else
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);

                var instance = PhotonNetwork.Instantiate(this.playerPrefab.name, spawnpoint, Quaternion.identity);
                instance.GetComponent<TankMultiplayer>().SetInputManager(inputManager);
                var turret = instance.GetComponentInChildren<TankTurret>();
                turret.SetVariables(arCamera.transform, inputManager);
                turret.SetAudio(chargingSound, reloadSound);
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
