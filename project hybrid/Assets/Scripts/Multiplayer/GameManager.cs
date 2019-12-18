using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;
using UnityEngine.UI;

namespace Multiplayer
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        public static GameManager Instance
        {
            get
            {
                return instance;
            }
        }
        private static GameManager instance;

        public GameObject playerPrefab1;
        public GameObject playerPrefab2;
        public GameObject playerPrefab3;
        public GameObject playerPrefab4;

        public InputManager inputManager;
        public GameObject arCamera;
        public GameObject noMarker;
        public Text scoreText;

        public AudioClip chargingSound;
        public AudioClip reloadSound;

        public static bool respawn = true;
        private LayerMask raycastLayerMask;

        private GameObject RespawnText;

        [SerializeField] GameObject spawnTargetPrefab;
        Transform spawntarget;

        private int playersSpawned = 0;
        public int playerId;

        private bool startOnce = false;

        private const string CountdownStartTime = "StartTime";

        private bool isTimerRunning = false;
        private float startTime = 0f;

        public float gameLength = 180f;

        private int[] scores = new int[4];

        [SerializeField]
        private InputUi ui;

        private void Awake()
        {
            instance = this;
            playerId = PhotonNetwork.PlayerList.Length - 1;
        }

        // Start is called before the first frame update
        void Start()
        {
            raycastLayerMask = LayerMask.GetMask("Level");
            spawntarget = GameObject.Instantiate(spawnTargetPrefab, transform).transform;
            RespawnText = GameObject.FindGameObjectWithTag("RespawnTextTag");

            if (PhotonNetwork.IsMasterClient)
            {
                respawn = false;
                spawntarget.GetComponentInChildren<MeshRenderer>().enabled = false;

                arCamera.GetComponent<MarkerChecker>().enabled = false;
                inputManager.gameObject.SetActive(false);
                ui.gameObject.SetActive(false);
                noMarker.SetActive(false);
            }
        }

        // Update is called once per frame
        void Update()
        {


            if (respawn)
            {
                Respawn();
            }

            if (isTimerRunning)
            {
                float timer = (float)PhotonNetwork.Time - startTime;
                float countdown = gameLength - timer;

                if (!PhotonNetwork.IsMasterClient)
                    ui.setTime(countdown);

                if (countdown < 0)
                {
                    isTimerRunning = false;

                    LeaveRoom();

                    if (PhotonNetwork.IsMasterClient)
                    {
                        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 5 });
                    }
                    else
                    {
                        int highscore = 0, bestplayer = 0;
                        for (int i = 0; i < scores.Length; i++)
                        {
                            if(scores[i] > highscore)
                            {
                                highscore = scores[i];
                                bestplayer = i;
                            }
                        }

                        if (playerId == bestplayer)
                            SceneManager.LoadScene("VictoryScene");
                        else
                            SceneManager.LoadScene("LostScene");
                    }
                }
            }

            if (PhotonNetwork.IsMasterClient)
            {
                if (!startOnce && GameHasStarted)
                {
                    startOnce = true;
                    photonView.RPC("StartGame", RpcTarget.All);
                    Hashtable ht = new Hashtable { { "StartTime", PhotonNetwork.Time } };
                    PhotonNetwork.CurrentRoom.SetCustomProperties(ht);
                }
            }
        }

        public void Respawn()
        {
            RaycastHit hit;
            // Does the ray intersect any objects excluding the player layer
            if (Physics.Raycast(arCamera.transform.position, arCamera.transform.TransformDirection(Vector3.forward), out hit, Mathf.Infinity, raycastLayerMask))
            {
                spawntarget.GetComponentInChildren<MeshRenderer>().enabled = true;

                spawntarget.position = hit.point + (hit.normal / 100);
                spawntarget.rotation = Quaternion.FromToRotation(Vector3.up, hit.normal);

                if (Input.GetKey("v"))
                {
                    SpawnTank(hit.point + new Vector3(0, 20, 0));
                    respawn = false;
                    spawntarget.GetComponentInChildren<MeshRenderer>().enabled = false;
                    RespawnText.SetActive(false);

                    photonView.RPC("PlayerHasSpawned", RpcTarget.MasterClient);
                }

                if (Input.touches.Length == 0)
                {
                    return;
                }

                if (Input.touches[0].phase == TouchPhase.Began)
                {
                    SpawnTank(hit.point + new Vector3(0, 20, 0));
                    respawn = false;
                    spawntarget.GetComponentInChildren<MeshRenderer>().enabled = false;
                    RespawnText.SetActive(false);

                    photonView.RPC("PlayerHasSpawned", RpcTarget.MasterClient);
                }
            }
            else
            {
                spawntarget.GetComponentInChildren<MeshRenderer>().enabled = false;
            }
        }

        private void SpawnTank(Vector3 spawnpoint)
        {
            Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);

            GameObject instance;

            switch (playerId)
            {
                case 1:
                    instance = PhotonNetwork.Instantiate(this.playerPrefab1.name, spawnpoint, Quaternion.identity);
                    break;
                case 2:
                    instance = PhotonNetwork.Instantiate(this.playerPrefab2.name, spawnpoint, Quaternion.identity);
                    break;
                case 3:
                    instance = PhotonNetwork.Instantiate(this.playerPrefab3.name, spawnpoint, Quaternion.identity);
                    break;
                default:
                    instance = PhotonNetwork.Instantiate(this.playerPrefab4.name, spawnpoint, Quaternion.identity);
                    break;
            }

            instance.GetComponent<TankMultiplayer>().SetInputManager(inputManager);
            var turret = instance.GetComponentInChildren<TankTurret>();
            turret.SetVariables(arCamera.transform, inputManager);
            turret.SetAudio(chargingSound, reloadSound);

        }

        [PunRPC]
        private void PlayerHasSpawned()
        {
            playersSpawned += 1;
        }

        [PunRPC]
        private void StartGame()
        {
            inputManager.canShoot = true;
            inputManager.canDrive = true;
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
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", otherPlayer.NickName);
        }

        public override void OnRoomPropertiesUpdate(Hashtable propertiesThatChanged)
        {
            object startTimeFromProps;

            if (propertiesThatChanged.TryGetValue(CountdownStartTime, out startTimeFromProps))
            {
                isTimerRunning = true;
                startTime = (float)(double)startTimeFromProps;
            }
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("PUN Basics Tutorial/Launcher: OnJoinedRoom() called by PUN. Now this client is in a room.");

            if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
            {
                PhotonNetwork.LoadLevel("ArMultiplayer");
            }
        }

        public bool GameHasStarted
        {
            get { return playersSpawned >= 1; }
        }

        public void AddScore(int playerId)
        {
            scores[playerId] += 1;

            if (playerId == this.playerId)
            {
                scoreText.text = "Score: \n" + scores[playerId].ToString();
            }
        }
    }
}
