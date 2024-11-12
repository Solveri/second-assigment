using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class GameManager : MonoBehaviourPunCallbacks
{
    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            CheckForGameEnd();
        }
    }

    void CheckForGameEnd()
    {
        int activePlayers = 0;

        foreach (Player player in PhotonNetwork.PlayerList)
        {
            // Check if the player is still active (not destroyed)
            if (player.CustomProperties.ContainsKey("HP") && (int)player.CustomProperties["HP"] > 0)
            {
                activePlayers++;
            }
        }

        if (activePlayers <= 1)
        {
            photonView.RPC("EndGame", RpcTarget.All);
        }
    }

    [PunRPC]
    void EndGame()
    {
        Debug.Log("Game over. Only one or no players remain.");
        // Logic to display the game over screen and lock control
    }
}
