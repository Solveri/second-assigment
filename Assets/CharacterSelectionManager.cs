using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

public class CharacterSelectionManager : MonoBehaviourPunCallbacks
{
    // Dictionary to store which player has selected which character index
    private Dictionary<int, int> selectedCharacters = new Dictionary<int, int>();

    public void RequestCharacterSelection(int characterIndex, int playerID)
    {
        photonView.RPC("RequestCharacterSelectionRPC", RpcTarget.MasterClient, characterIndex, playerID);
    }

    [PunRPC]
    void RequestCharacterSelectionRPC(int characterIndex, int playerID)
    {
        Debug.Log("Selection");
        if (IsCharacterAvailable(characterIndex))
        {
            selectedCharacters[playerID] = characterIndex; // Mark the character as selected
            // Notify the specific client who requested the character to instantiate it locally
            photonView.RPC("ConfirmCharacterSelectionRPC", GetPlayerByID(playerID), characterIndex);
        }
        else
        {
            photonView.RPC("RejectCharacterSelectionRPC", GetPlayerByID(playerID));
        }
    }

    [PunRPC]
    void ConfirmCharacterSelectionRPC(int characterIndex)
    {
        Debug.Log($"Character {characterIndex} selected successfully.");

        string prefabName = "Character_" + characterIndex; // Ensure the prefabs are named accordingly
        PhotonNetwork.Instantiate(prefabName, new Vector3(0, 0, 0), Quaternion.identity);
       
        // Ensure only the client that owns this view (the requesting client) instantiates the character
        if (photonView.IsMine)
        {
           
        }
    }

    [PunRPC]
    void RejectCharacterSelectionRPC()
    {
        Debug.Log("Character selection rejected. Please choose another character.");
    }

    private bool IsCharacterAvailable(int characterIndex)
    {
        return !selectedCharacters.ContainsValue(characterIndex);
    }

    private Player GetPlayerByID(int playerID)
    {
        foreach (Player player in PhotonNetwork.PlayerList)
        {
            if (player.ActorNumber == playerID)
            {
                return player;
            }
        }
        return null;
    }
}
