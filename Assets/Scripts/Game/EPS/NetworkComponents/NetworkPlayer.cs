using Unity.Netcode;
using UnityEngine;

namespace EPS
{
    public class NetworkPlayer : NetworkBehaviour
    {
        public Transform spawnPoint;
        public float currentHealth = 100;
        
        public IPlayerControl currentControl;
        public WeaponSystem currentWeapon;
        public FirstPersonPlayer fpp;
        public UnityEngine.InputSystem.PlayerInput playerInput;

        //Network vars
        public NetworkVariable<Vector3> Position = new NetworkVariable<Vector3>();
        public NetworkVariable<bool> IsAlive = new NetworkVariable<bool>();
        public Camera cameraControl;
  

        public override void OnNetworkSpawn()
        {
            //MoveToSpawn(); // Move to spawn point
            PreparePlayer();
        }

        private void EnableLocalPlayerComponents(bool enable)
        {
            fpp.enabled = enable;
            cameraControl.enabled = enable;
            currentWeapon.enabled = enable;
            playerInput.enabled = enable;
            //transform.position = Position.Value;

          
               currentWeapon.OnBulletHit += ShootSomeOne; // args: hit, damage 
        }

        public void PreparePlayer()
        {
            if (IsOwner)
            {
                EnableLocalPlayerComponents(true);
            }
            else
            {
                EnableLocalPlayerComponents(false);
            }
        }

        public void MoveToSpawn()
        {
            transform.position = spawnPoint.transform.position;
            Position.Value =  transform.position;
        }

        public void DecreaseHealth(NetworkPlayer player, float value)
        {
            player.currentHealth -= value;
            player.UpdatePlayerHealthClientRpc(player.currentHealth);

            if (player.currentHealth <= 0)
            {
                //Disable fpp control
                //Show respawn screen (todo)
                Debug.Log("Player dead:" + player.OwnerClientId.ToString());
            }
        }

        public void ShootSomeOne(GameObject target, float damageToDeal){
            var networkPlayer = target.GetComponent<NetworkPlayer>();
            if (networkPlayer == null) return;

            Debug.Log("Player hit!: " + target.gameObject.name);
            if (IsClient){
                ShootServerRpc(networkPlayer, damageToDeal);
            }
            else{
                DecreaseHealth(networkPlayer, damageToDeal);
            }
        }

        [ServerRpc]
        void ShootServerRpc(NetworkBehaviourReference target,float damageToDeal)
        {
            if (target.TryGet(out NetworkPlayer targetObject))
            {
                DecreaseHealth(targetObject, damageToDeal);
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
            currentHealth = healthValue;
        }

        void Start()
        {
            PreparePlayer();
            currentWeapon = GetComponent<WeaponSystem>();
            //inputs = GetComponent<StarterAssets.StarterAssetsInputs>();
        }
    }
}