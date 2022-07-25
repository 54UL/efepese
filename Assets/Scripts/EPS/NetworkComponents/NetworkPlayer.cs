using Unity.Netcode;
using UnityEngine;
using EPS.GamePhysics.Character;

namespace EPS
{
    public class NetworkPlayer : NetworkBehaviour
    {
        //Game systems
        public WeaponSystem currentWeapon;
        public FirstPersonController fpp;
        public Camera playerCamera;
        public CharacterController cc;

        public LayerMask InvisibleMask; 
        public LayerMask VisibleMask;

        public GameObject characterModel;

        public override void OnNetworkSpawn()
        {
            fpp = GetComponent<FirstPersonController>();
            currentWeapon = GetComponent<WeaponSystem>();
            playerCamera = GetComponentInChildren<Camera>();
            cc = GetComponent<CharacterController>();
            characterModel = gameObject.transform.Find("character").gameObject;
            fpp.IsClient = IsClient;

            fpp.currentNetworkPlayer = this;
            SetMasks(IsLocalPlayer);
            EnableComponents(IsLocalPlayer);
        }

        private void SetMasks(bool IsLocalPlayer)
        {
            if (!IsLocalPlayer)
            {
                characterModel.layer = InvisibleMask;
                currentWeapon.gunModel.gameObject.layer = VisibleMask;
            }
            else
            {
                characterModel.layer = VisibleMask;
                currentWeapon.gunPivot.gameObject.layer = InvisibleMask;
            }
        }

        private void EnableComponents(bool enabled)
        {
            fpp.enabled = enabled;
            playerCamera.enabled = enabled;
            currentWeapon.enabled = enabled;
        }

        public  void SendInputs(Vector3 movement, Vector3 rotation, Quaternion aimOrentation)
        {
            //DE MOMENTO SE APAGO POR QUE QUUEDA MEJOR EL CLIENTE AUTORITARIO DE SU PLAYER SMH NETCODE
            //if (IsClient)
            //{
            //    CalculatePlayerMovementServerRpc(movement, rotation, aimOrentation);
            //}
            //else
            //{
                this.playerCamera.transform.localRotation = aimOrentation; // only pitch
                this.transform.Rotate(rotation); // only yaw
                cc.Move(movement);//player physics 
            //}
        }


        [ServerRpc]
        void CalculatePlayerMovementServerRpc(Vector3 movement, Vector3 rotation, Quaternion aimOrentation)
        {
            this.playerCamera.transform.localRotation = aimOrentation; // only pitch
            this.transform.Rotate(rotation); // only yaw
            cc.Move(movement);//player physics 
        }

     
        [ClientRpc]
        //Refactor move this to network manager
        private void EnableLocalPlayerComponentsClientRpc(bool enable)
        {
            fpp.enabled = enable;
            playerCamera.enabled = enable;
            currentWeapon.enabled = enable;
        }

        //public void DecreaseHealth(NetworkPlayer player, float value)
        //{
        //    player.currentHealth -= value;
        //    player.UpdatePlayerHealthClientRpc(player.currentHealth);

        //    if (player.currentHealth <= 0)
        //    {
        //        //Disable fpp control
        //        //Show respawn screen (todo)
        //        Debug.Log("Player dead:" + player.OwnerClientId.ToString());
        //    }
        //}

        public void ShootSomeOne(GameObject target, float damageToDeal){
            var networkPlayer = target.GetComponent<NetworkPlayer>();
            if (networkPlayer == null) return;

            Debug.Log("Player hit!: " + target.gameObject.name);
            if (IsClient){
                ShootServerRpc(networkPlayer, damageToDeal);
            }
            else{
                //DecreaseHealth(networkPlayer, damageToDeal);
            }
        }

        [ServerRpc]
        void ShootServerRpc(NetworkBehaviourReference target,float damageToDeal)
        {
            if (target.TryGet(out NetworkPlayer targetObject))
            {
                //DecreaseHesalth(targetObject, damageToDeal);
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
            //currentHealth = healthValue;
        }
    }
}