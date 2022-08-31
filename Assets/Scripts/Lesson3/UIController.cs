using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace Lesson3
{
    public class UIController : MonoBehaviour
    {
        [SerializeField] private Button buttonStartServer;
        [SerializeField] private Button buttonShutDownServer;
        [SerializeField] private Button buttonConnectClient;
        [SerializeField] private Button buttonDisconnectClient;
        [SerializeField] private Button buttonSendMessage;
        [SerializeField] private TMP_InputField inputField;
        [SerializeField] private TextField textField;
        //[SerializeField] private TMP_InputField nameField;
        [SerializeField] private Server server;
        [SerializeField] private Client client;
        
        private void Start()
        {
            //nameField.gameObject.SetActive(false);
            buttonStartServer.onClick.AddListener(() => StartServer());
            buttonShutDownServer.onClick.AddListener(() => ShutDownServer());
            buttonConnectClient.onClick.AddListener(() => Connect());
            buttonDisconnectClient.onClick.AddListener(() => Disconnect());
            buttonSendMessage.onClick.AddListener(() => SendMessage());
            buttonSendMessage.onClick.AddListener(() => SendName());
            client.onMessageReceive += ReceiveMessage;
        }
        
        private void StartServer()
        {
            server.StartServer();
        }
        
        private void ShutDownServer()
        {
            server.ShutDownServer();
        }
        
        private void Connect()
        {
            client.Connect();
            //nameField.gameObject.SetActive(true);
            client.SendName(client.nameField.text);
        }
        
        private void Disconnect()
        {
            client.Disconnect();
        }
        
        private void SendMessage()
        {
            client.SendMessage(inputField.text);
            inputField.text = "";
        }

        private void ReceiveMessage(object message)
        {
            textField.ReceiveMessage(message);
        }
        
        private void SendName()
        {
            inputField.text = "";
        }
    }
}