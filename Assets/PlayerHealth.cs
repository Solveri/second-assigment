using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerHealth : MonoBehaviourPun
{
    public int maxHP = 100;
    private int currentHP;
    private bool isDestroyed = false;

    void Start()
    {
        currentHP = maxHP;
    }

    // Method to report damage to the player

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collision is with another player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the PhotonView component of the other player
            PhotonView otherPlayerPhotonView = collision.gameObject.GetComponent<PhotonView>();

            // Check if the other player has a PhotonView and is not the local player
            if (otherPlayerPhotonView != null && !otherPlayerPhotonView.IsMine)
            {
                // Call the TakeDamage RPC on the other player
                otherPlayerPhotonView.RPC("TakeDamage", RpcTarget.All, 5);
            }
        }
    }
    [PunRPC]
    public void TakeDamage(int damage)
    {
        if (!isDestroyed)
        {
            currentHP -= damage;
            Debug.Log($"Player {photonView.Owner.NickName} took {damage} damage. Current HP: {currentHP}");

            if (currentHP <= 0)
            {
                isDestroyed = true;
                photonView.RPC("DestroyPlayer", RpcTarget.All, photonView.Owner.ActorNumber);
            }
        }
    }

    // Method to destroy the player
    [PunRPC]
    public void DestroyPlayer(int playerID)
    {
        if (PhotonNetwork.LocalPlayer.ActorNumber == playerID)
        {
            // Logic to disable or "destroy" the player locally
            Debug.Log($"Player {PhotonNetwork.LocalPlayer.NickName} is destroyed.");
            gameObject.SetActive(false); // Example to "destroy" the player object
        }
    }
}
