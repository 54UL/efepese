using Unity.Netcode;
using UnityEngine;

namespace EPS
{
    public class NetworkPlayer : NetworkBehaviour
    {
        public Transform spawnPoint;
        
        public float CurrentHealth = 100;
        public float Damage = 10;

        public IPlayerControl currentControl;
        public FirstPersonPlayer fpp;

        //Network vars
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
        public NetworkVariable<bool> IsAlive = new NetworkVariable<bool>();
        public Camera cameraControl;
  

        public override void OnNetworkSpawn()
        {
            //MoveToSpawn(); // Move to spawn point
            PreparePlayer();
        }

        public void PreparePlayer()
        {
            if (IsOwner)
            {
                fpp.enabled = true;
                cameraControl.enabled = true;




            }
            else
            {
                fpp.enabled = false;
                cameraControl.enabled = false;
            }
        }

        public void MoveToSpawn()
        {
            transform.position = spawnPoint.transform.position;
            Position.Value =  transform.position;
        }

        public void DecreaseHealth(NetworkPlayer player, float value)
        {
            player.CurrentHealth -= value;
            player.UpdatePlayerHealthClientRpc(player.CurrentHealth);
        }

        public void ShootSomeOne(GameObject target){
            var networkPlayer = target.GetComponent<NetworkPlayer>();
            if (IsClient){
                ShootServerRpc(networkPlayer);
            }
            else{
                DecreaseHealth(networkPlayer,Damage);
            }
        }

        [ServerRpc]
        void ShootServerRpc(NetworkBehaviourReference target)
        {
            if (target.TryGet(out NetworkPlayer targetObject))
            {
                DecreaseHealth(targetObject, Damage);
            }
            else
            {
                Debug.Log("SERVER: TARGET NOT FOUND???");
            }
        }

        [ClientRpc]
        void UpdatePlayerHealthClientRpc(float healthValue)
        {
            Debug.Log("Updated damage: " + healthValue);
            CurrentHealth = healthValue;
            //If death just despawn the dude???
        }

        void Start()
        {
            transform.position = Position.Value;
        }

        void Update(){
            //fpp.OnBulletHit += ShootSomeOne;// args: hit,time(comming soon)
            // fp.onReload += reloadGunServerRpc; //args: bullets
            // fp.onDamageTaken += damageTakenServerRpc;// arg: damage_taken, suspect
        }

    }
}