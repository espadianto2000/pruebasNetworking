using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ballBehaviour : NetworkBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(IsOwner && IsClient)
        {
            GameObject[] ojos = GameObject.FindGameObjectsWithTag("ojo");
            foreach (GameObject ojo in ojos)
            {
                ojo.transform.LookAt(transform.position);
            }
        }
    }
}
