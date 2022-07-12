using Unity.Netcode;
using UnityEngine;

namespace EPS
{
    public class NetworkPlayer : NetworkBehaviour
    {
        public Transform spawnPoint;
        public Transform lookAtPos;
        public float CurrentHealth;
        public float Damage = 10;

        public IPlayerControl currentControl;
        public FirstPersonController fpc;

        //Network vars
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
        public NetworkVariable<bool> IsAlive = new NetworkVariable<bool>();

        public override void OnNetworkSpawn()
        {
            MoveToSpawn(); // Move to spawn point
        }

        public void PreparePlayer()
        {
            // show fpp and camera controllers
            // hide fpp and camera controllers (just keep active the character)        
        }

        public void MoveToSpawn()
        {
           
            transform.position = spawnPoint.transform.position;
            Position.Value = Position;
        }

        public void DecreaseHealth(NetworkPlayer player, float value)
        {
            player.currentHealth -= value;
            player.UpdatePlayerHealthClientRpc(targetObject.currentHealth);
        }

        public void shootSomeOne(GameObject target){
            var networkPlayer = target.GetComponent<NetworkPlayer>();
            if (isClient){
                ShootServerRpc(networkPlayer)
            } else{
                DecreaseHealth(networkPlayer,Damage);
            }
        }

        [ServerRpc]
        void ShootServerRpc(NetworkBehaviourReference target)
        {
            if (target.TryGet(out NetworkPlayer targetObject))
            {
                DecreaseHealth(Damage);
            }
            else
            {
                Debug.Log("SERVER: TARGET NOT FOUND???");
            }
        }

        [ClientRpc]
        void UpdatePlayerHealthClientRpc(int healthValue)
        {
            Debug.Log("Updated damage: "+healthValue);
            currentHealth = healthValue;
            //If death just despawn the dude???
        }

        void Start()
        {
            transform.position = Position.Value;
        }

        void Update(){
            // fp.OnFire += shootServerRpc;// args: hit,time
            // fp.onReload += reloadGunServerRpc; //args: bullets
            // fp.onDamageTaken += damageTakenServerRpc;// arg: damage_taken, suspect
        }

    }
}