using System.Collections;
using System.Collections.Generic;
using Unity.Multiplayer.Samples.Utilities.ClientAuthority;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(NetworkObject))]
[RequireComponent(typeof(ClientNetworkTransform))]
public class playerControllerClient : NetworkBehaviour
{
    public bool flagGround = false;
    public bool spawn = false;
    public Rigidbody rb;
    public NetworkObject no;
    public GameObject cameraClient;
    public GameObject[] elementosAOcultar;
    float yRotate;
    public GameObject[] ojos;
    // Start is called before the first frame update
    void Start()
    {
    }
    public override void OnNetworkSpawn()
    {
        if(no.IsLocalPlayer && IsOwner && IsClient)
        {
            transform.position = new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
            GameObject.Find("Main Camera").tag = "Untagged";
            GameObject.Find("Main Camera").SetActive(false);
            cameraClient.SetActive(true);
            cameraClient.tag = "MainCamera";
            cameraClient.GetComponent<AudioListener>().enabled = true;
            Cursor.lockState = CursorLockMode.Locked;
            int LayerIgnoreRaycast = LayerMask.NameToLayer("notForMe");
            foreach(GameObject eo in elementosAOcultar)
            {
                eo.layer = LayerIgnoreRaycast;
                if(eo.tag == "cabeza")
                {
                    eo.GetComponent<ballBehaviour>().enabled = true;
                    eo.tag = "Untagged";
                }
            }
            foreach(GameObject ojo in ojos)
            {
                ojo.layer = LayerIgnoreRaycast;
                ojo.tag = "Untagged";
            }
            
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (no.IsLocalPlayer && IsOwner && IsClient)
        {
            float hori = -Input.GetAxisRaw("Horizontal");
            float vert = Input.GetAxisRaw("Vertical");

            float mHori = Input.GetAxisRaw("Mouse X");
            float mVert = -Input.GetAxisRaw("Mouse Y");

            rb.MovePosition(transform.position + (transform.forward * hori * 0.05f) + (transform.right * vert * 0.05f));
            if (Input.GetKey(KeyCode.Space) && flagGround)
            {
                flagGround = false;
                rb.velocity = new Vector3(rb.velocity.x, 7.5f, rb.velocity.z);
            }

            var p = transform;
            p.Rotate(0, mHori * 5f, 0);

            var c = transform.GetChild(0);

            yRotate += mVert * Time.fixedDeltaTime * 125f;
            yRotate = Mathf.Clamp(yRotate, -80f, 80f);
            c.eulerAngles = new Vector3(yRotate, c.eulerAngles.y, c.eulerAngles.z);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("suelo"))
        {
            flagGround = true;
        }
    }
}
