using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using TMPro;

public class ChatManager : MonoBehaviourPunCallbacks
{
    public TextMeshProUGUI chatDisplay; // Text UI element to display chat messages
    public TMP_InputField chatInput; // Input field for player to type messages
    public static ChatManager Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    void Start()
    {
        // Ensure chat display is cleared at the start
        chatDisplay.text = "";
    }

    public void SendChatMessage()
    {
        if (!string.IsNullOrEmpty(chatInput.text))
        {
            string message = $"{PhotonNetwork.NickName}: {chatInput.text}";
            photonView.RPC("ReceiveChatMessage", RpcTarget.All, message);
            chatInput.text = ""; // Clear input field after sending
        }
        else
        {
            Debug.Log("String is emprty");
        }
    }

    [PunRPC]
    void ReceiveChatMessage(string message)
    {
        chatDisplay.text += message + "\n"; // Append new message to chat display
    }
}
