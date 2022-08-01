using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

namespace HelloWorld
{
    public class HelloWorldPlayer : NetworkBehaviour
    {
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
        public Rigidbody rb;
        public float valorX;
        public float valorY;
        public Vector3 positionLocal;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        public override void OnNetworkSpawn()
        {
            if (IsOwner)
            {
                Move();
            }
        }

        public void Move()
        {
            if (NetworkManager.Singleton.IsServer)
            {
                /*var randomPosition = GetRandomPositionOnPlane();
                transform.position = randomPosition;
                Position.Value = randomPosition;*/
            }
            else
            {
                //SubmitPositionRequestServerRpc();
            }
        }

        [ServerRpc]
        void SubmitPositionRequestServerRpc(Vector3 pos, ServerRpcParams rpcParams = default)
        {
            //Debug.Log("dentroRequest");
            //Position.Value = new Vector3(transform.position.x + (valorX * Time.fixedDeltaTime * 5), transform.position.y, transform.position.z + (valorY * Time.fixedDeltaTime * 5));
            Position.Value = GetRandomPositionOnPlane(pos);
        }

        Vector3 GetRandomPositionOnPlane(Vector3 pos)
        {
            return pos;
            //return new Vector3(Random.Range(-3f, 3f), 1f, Random.Range(-3f, 3f));
        }

        private void Update()
        {
            if (IsOwner)
            {
                SubmitPositionRequestServerRpc(transform.position);
            }
        }
        void FixedUpdate()
        {
            if (IsOwner)
            {
                valorX = Input.GetAxisRaw("Horizontal");
                valorY = Input.GetAxisRaw("Vertical");
                rb.MovePosition(new Vector3(transform.position.x + (valorX * Time.fixedDeltaTime * 5), transform.position.y, transform.position.z + (valorY * Time.fixedDeltaTime * 5)));
            }
        }
    }
}
