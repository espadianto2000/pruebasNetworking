using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class playerControls : NetworkBehaviour
{
    Rigidbody rb;
    NetworkObject no;

    [SerializeField]
    private NetworkVariable<float> vertAxis = new NetworkVariable<float>();

    [SerializeField]
    private NetworkVariable<float> HoriAxis = new NetworkVariable<float>();

    //client caching
    private float oldVertAxis;
    private float oldHoriAxis;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        no = GetComponent<NetworkObject>();
    }
    void FixedUpdate()
    {
        if (IsServer)
        {
            UpdateServer();
        }
        if (IsClient && IsOwner)
        {
            UpdateClient();
        }
    }
    private void UpdateServer()
    {
        rb.MovePosition(new Vector3(transform.position.x + (HoriAxis.Value*0.05f), transform.position.y, transform.position.z + (vertAxis.Value*0.05f)));
    }
    private void UpdateClient()
    {
        float hori = Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");
        if(oldHoriAxis != hori || oldVertAxis != vert)
        {
            oldHoriAxis = hori;
            oldVertAxis = vert;
        }
        updateClientPositionServerRpc(hori, vert);
    }
    [ServerRpc]
    private void updateClientPositionServerRpc(float h, float v)
    {
        HoriAxis.Value = h;
        vertAxis.Value = v;
    }
}
