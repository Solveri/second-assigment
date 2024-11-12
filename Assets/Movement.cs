using UnityEngine;
using Photon.Pun;

public class Movement : MonoBehaviourPun, IPunObservable
{
    public float speed = 5f;
    private Vector3 networkPosition;
    private Quaternion networkRotation;
    private float smoothingSpeed = 10f; // Adjust for smoother interpolation

    void Update()
    {
        if (photonView.IsMine)
        {
            
            // Control only the local player
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");
            if (vertical>0.1f)
            {
                Debug.Log("");
            }
            Vector3 movement = new Vector3(horizontal, 0, vertical) * speed * Time.deltaTime;
            transform.Translate(movement);
        }
        else
        {
           

           transform.position = Vector3.Lerp(transform.position, networkPosition, Time.deltaTime * smoothingSpeed);
           transform.rotation = Quaternion.Lerp(transform.rotation, networkRotation, Time.deltaTime * smoothingSpeed);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
      
        
        if (stream.IsWriting)
        {
            if (photonView.IsMine)
            {
                Debug.Log("Sending position and rotation data");
                stream.SendNext(transform.position);
                stream.SendNext(transform.rotation);
            }
            // Local player sends data to the network

        }
        else
        {
            if (!photonView.IsMine)
            {
                networkPosition = (Vector3)stream.ReceiveNext();
                networkRotation = (Quaternion)stream.ReceiveNext();
                Debug.Log($"Received position: {networkPosition}, rotation: {networkRotation}");
            }
            // Non-local players receive data from the network
           
        }
    }
}
