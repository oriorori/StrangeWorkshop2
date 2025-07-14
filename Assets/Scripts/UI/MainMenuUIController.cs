using TMPro;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIController : MonoBehaviour
{
    public string ipAddress = "127.0.0.1";
    public GameObject myPlayerPrefab;
    
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

    void Start()
    {
        _makeRoomButton.onClick.AddListener(OnPressedMakeRoomButton);
        _joinRoomButton.onClick.AddListener(OnPressedJoinRoomButton);
        _joinButton.onClick.AddListener(OnPressedJoinButton);
        
        NetworkManager.Singleton.OnClientConnectedCallback += (clientId) =>
        {
            Debug.Log("연결 성공!");
            if (NetworkManager.Singleton.IsHost)
            {
                var player = Instantiate(myPlayerPrefab); // 프리팹 인스턴스화
                player.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId); // 소유자 설정 + Spawn
            }
        };
        
        NetworkManager.Singleton.OnClientDisconnectCallback += (id) =>
        {
            if (id == NetworkManager.Singleton.LocalClientId)
                Debug.Log("❌ 연결 실패 or 서버에서 강제 연결 종료");
        };
    }

    private void OnPressedMakeRoomButton()
    {
        NetworkManager.Singleton.StartHost();
        
        // CustomNetworkManager.Instance.StartHost();
    }

    private void OnPressedJoinRoomButton()
    {
        var transport = NetworkManager.Singleton.GetComponent<Unity.Netcode.Transports.UTP.UnityTransport>();
        transport.ConnectionData.Address = ipAddress;
        transport.ConnectionData.Port = 7777;
        NetworkManager.Singleton.StartClient();

        // _mainMenuPanelObject.SetActive(false);
        // _joinRoomPanelObject.SetActive(true);
    }

    private void OnPressedJoinButton()
    {
        if (_hostIpInputField.text == "") return;
        
        CustomNetworkManager.Instance.StartClient(_hostIpInputField.text);
    }
}
