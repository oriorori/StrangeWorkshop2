using FishNet;
using FishNet.Component.Spawning;
using FishNet.Connection;
using FishNet.Managing.Server;
using TMPro;
using FishNet.Object;
using UnityEngine;
using UnityEngine.UI;
using Cysharp.Threading.Tasks;
using FishNet.Transporting;
using UnityEngine.Serialization;

public class MainMenuUIController : MonoBehaviour
{
    public GameObject NetworkGameManagerPrefab;
    
    public string ipAddress = "127.0.0.1";
    
    [Header("Main Menu UI Elements")] [SerializeField]
    private GameObject _mainMenuPanelObject;
    [SerializeField] private Button _makeRoomButton;
    [SerializeField] private Button _joinRoomButton;

    [Header("Make Room UI Elements")] 
    [SerializeField] private TextMeshProUGUI _playerCountText;

    [Header("Join Room UI Elements")] 
    [SerializeField] private GameObject _joinRoomPanelObject;
    [SerializeField] private TMP_InputField _hostIpInputField;
    [SerializeField] private Button _joinButton;
    
    private bool _isServerInitialized = false;

    void Start()
    {
        _makeRoomButton.onClick.AddListener(OnPressedMakeRoomButton);
        _joinRoomButton.onClick.AddListener(OnPressedJoinRoomButton);

        // 서버측에서 서버가 생성되었음을 확인
        InstanceFinder.ServerManager.OnServerConnectionState += (ServerConnectionStateArgs args) => OnServerConnectionState(args);
        // 서버측에서 클라가 연결되었음을 확인
        InstanceFinder.ServerManager.OnAuthenticationResult += (NetworkConnection conn, bool result) => OnAuthenticationResult(conn, result).Forget();
        // 클라측에서 서버에 연결되었음을 확인
        InstanceFinder.ClientManager.OnAuthenticated += OnClientInitialized; 
    }

    private void OnPressedMakeRoomButton()
    {
        Debug.Log("Pressed Make Room Button");
        InstanceFinder.ServerManager.StartConnection();
        InstanceFinder.ClientManager.StartConnection();
    }
    private void OnPressedJoinRoomButton()
    {
        Debug.Log("Pressed Join Room Button");
        InstanceFinder.ClientManager.StartConnection();
    }

    private void OnServerConnectionState(ServerConnectionStateArgs args)
    {
        // OnServerConnectionState는 서버 시작시, 서버 중단시 각각 2번씩 호출된다
        // starting -> started, stopping -> stopped
        switch (args.ConnectionState)
        {
            case LocalConnectionState.Started:
                var gameManager = Instantiate(NetworkGameManagerPrefab);
                InstanceFinder.ServerManager.Spawn(gameManager.gameObject);
                _isServerInitialized = true;
                break;
        }
    }

    private async UniTask OnAuthenticationResult(NetworkConnection conn, bool result)
    {
        await UniTask.WaitUntil(() => _isServerInitialized);
        
        Debug.Log("On Authentication Result");
        NetworkGameManager.Instance.SpawnPlayer(conn);
    }
    
    private void OnClientInitialized()
    {
        Debug.Log("Client initialized");
    }
}
