using UnityEngine;

public class CharacterSelectionUI : MonoBehaviour
{
    private CharacterSelectionManager selectionManager;

    void Start()
    {
        selectionManager = FindObjectOfType<CharacterSelectionManager>();
    }

    public void SelectCharacter(int characterIndex)
    {
        if (selectionManager != null)
        {
            selectionManager.RequestCharacterSelection(characterIndex, Photon.Pun.PhotonNetwork.LocalPlayer.ActorNumber);
        }
        else
        {
            Debug.LogError("CharacterSelectionManager not found in the scene.");
        }
    }
}
