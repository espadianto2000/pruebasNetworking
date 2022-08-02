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

    [SerializeField]
    private NetworkVariable<bool> flagJump = new NetworkVariable<bool>();

    

    //client caching
    private float oldVertAxis;
    private float oldHoriAxis;
    public bool flagGround = false;
    public bool spawn = false;
    

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        no = GetComponent<NetworkObject>();
        flagJump.Value = false;

    }
    public override void OnNetworkSpawn()
    {
        if (IsOwner)
        {
            spawn = true;
        }
        
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
        if (spawn)
        {
            spawn = false;
            transform.position = new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        }
        rb.MovePosition(new Vector3(transform.position.x + (HoriAxis.Value*0.05f), transform.position.y, transform.position.z + (vertAxis.Value*0.05f)));
        if (flagJump.Value)
        {
            jump();
        }
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
        if(Input.GetKey(KeyCode.Space) && flagGround)
        {
            flagGround = false;
            jumpServerRpc();
        }

    }
    [ServerRpc]
    private void updateClientPositionServerRpc(float h, float v)
    {
        HoriAxis.Value = h;
        vertAxis.Value = v;
    }

    [ServerRpc]
    private void jumpServerRpc()
    {
        jump();
    }
    

    private void jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 7.5f, rb.velocity.z);
        //flagJump.Value = false;
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("suelo"))
        {
            flagGround = true;
        }
    }

}
