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

    [SerializeField]
    private NetworkVariable<float> MouseHoriAxis = new NetworkVariable<float>();
    

    //client caching
    private float oldVertAxis;
    private float oldHoriAxis;
    private float oldMouseHoriAxis;

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
            SpawnServerRpc();
            GameObject.Find("Main Camera").SetActive(false);
            transform.GetChild(0).gameObject.SetActive(true);
            Cursor.lockState = CursorLockMode.Locked;
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
        //rb.MovePosition(new Vector3(transform.position.x + (HoriAxis.Value*0.05f), transform.position.y, transform.position.z + (vertAxis.Value*0.05f)));
        rb.MovePosition(transform.position + (transform.forward * HoriAxis.Value * 0.05f) + (transform.right * vertAxis.Value * 0.05f));
        if (flagJump.Value)
        {
            jump();
        }
        var p = transform;
        p.Rotate(0, MouseHoriAxis.Value * 10f, 0);
    }
    private void UpdateClient()
    {
        float hori = -Input.GetAxisRaw("Horizontal");
        float vert = Input.GetAxisRaw("Vertical");

        float mHori = Input.GetAxis("Mouse X");

        if(oldHoriAxis != hori || oldVertAxis != vert)
        {
            oldHoriAxis = hori;
            oldVertAxis = vert;
        }
        if(oldMouseHoriAxis !=mHori)
        {
            oldMouseHoriAxis = mHori;
        }
        updateClientRotatioServerRpc(mHori);
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
    private void updateClientRotatioServerRpc(float h)
    {
        MouseHoriAxis.Value = h;
    }

    [ServerRpc]
    private void jumpServerRpc()
    {
        jump();
    }
    
    [ServerRpc]
    private void SpawnServerRpc()
    {
        Spawn();
    }

    private void Spawn()
    {
        transform.position = new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
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
