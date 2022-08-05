using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class cameraControl : NetworkBehaviour
{
    public GameObject cuerpo;
    Ray RayOrigin;
    RaycastHit HitInfo;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner)
        {
            RaycastHit hit;

            if (Physics.Raycast(transform.position, transform.forward, out hit, 4f))
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Debug.Log("Hit: " + hit.transform.name);
                    
                }
            }
            /*if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(new Vector3(0.5f,0.5f,0));
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit))
                {
                    Debug.Log($"Hit {hit.transform.name}");
                }
            }*/
                /*if(Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out HitInfo, 100.0f))
                {
                    Debug.DrawRay(RayOrigin.direction, HitInfo.point, Color.yellow);
                }

                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 3))
                {
                    if (Input.GetKeyDown(KeyCode.Mouse0) && hit.transform.tag == "Player")
                    {
                        hit.transform.GetComponent<Rigidbody>().AddForce((new Vector3(hit.transform.position.x - cuerpo.transform.position.x, 3f, hit.transform.position.z - cuerpo.transform.position.z).normalized) * 10f);
                    }
                }*/
        }
    }
}
