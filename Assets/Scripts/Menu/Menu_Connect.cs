using System.Linq;
using System.Net;
using TMPro;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Menu
{
    public class MenuConnect : MonoBehaviour
    {
        private UnityTransport _transport;

        [Header("Connection UI Elements")]
        [SerializeField] private Button startHostButton;
        [SerializeField] private Button startClientButton;
        [SerializeField] private TMP_InputField ipAddressInputField;
        [SerializeField] private TMP_InputField portInputField;
        [SerializeField] private TMP_InputField usernameInputField;
        [SerializeField] private TMP_Text localIPDisplayText;
    
        [Header("Public Variables")]
        public static string LocalPlayerUsername { get; private set; } = "Player";
    
        private void Awake()
        {
            if (FindAnyObjectByType<EventSystem>()) return;
            GameObject eventSystem = new("EventSystem", typeof(EventSystem), typeof(StandaloneInputModule));
            eventSystem.transform.SetParent(transform);
        }
    
        private void Start()
        {
            _transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            SetButtonListeners();
            DisplayLocalIPAddress();
        }

        private void SetButtonListeners()
        {
            startHostButton.onClick.AddListener(StartHost);
            startClientButton.onClick.AddListener(StartClient);
        }
    
        private void StartHost()
        {
            ApplyConnectionData();
            ApplyPlayerUsername();
            NetworkManager.Singleton.StartHost();
            ShowConnectingUI(false);
        }
        
        private void StartClient()
        {
            ApplyConnectionData();
            ApplyPlayerUsername();
            NetworkManager.Singleton.StartClient();
            ShowConnectingUI(false);
        }
        private void ApplyConnectionData()
        {
            string ip = string.IsNullOrEmpty(ipAddressInputField.text) ? "127.0.0.1" : ipAddressInputField.text;
            ushort port = ushort.TryParse(portInputField.text, out ushort p) ? p : (ushort)7777;
            _transport.SetConnectionData(ip, port);
        }

        private void ApplyPlayerUsername()
        {
            LocalPlayerUsername = string.IsNullOrEmpty(usernameInputField.text)
                ? "Player"
                : usernameInputField.text;
        }

        private void ShowConnectingUI(bool show)
        {
            startClientButton.gameObject.SetActive(show);
            startHostButton.gameObject.SetActive(show);
            ipAddressInputField.gameObject.SetActive(show);
            portInputField.gameObject.SetActive(show);
            localIPDisplayText.gameObject.SetActive(show);
            usernameInputField.gameObject.SetActive(show);
        }

        private void DisplayLocalIPAddress()
        {
            string ip = Dns.GetHostEntry(Dns.GetHostName())
                .AddressList.FirstOrDefault(x => x.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)?.ToString();
            localIPDisplayText.text = $"Your IP: {ip}\nRecommended port: 7777";
        }
    }
}
