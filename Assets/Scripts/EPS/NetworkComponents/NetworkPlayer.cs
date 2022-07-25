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
        public Animator animator;
        public LayerMask InvisibleMask; 
        public LayerMask VisibleMask;

        public GameObject characterModel;


        private static readonly int VelocityZ = Animator.StringToHash("VelocityZ");
        private static readonly int VelocityX = Animator.StringToHash("VelocityX");


        public override void OnNetworkSpawn()
        {
            fpp = GetComponent<FirstPersonController>();
            currentWeapon = GetComponent<WeaponSystem>();
            playerCamera = GetComponentInChildren<Camera>();
            cc = GetComponent<CharacterController>();
            characterModel = this.transform.Find("character").gameObject;
            animator = characterModel.GetComponent<Animator>();

            fpp.IsClient = IsClient;

            fpp.currentNetworkPlayer = this;
            SetMasks(IsLocalPlayer);
            EnableComponents(IsLocalPlayer);
        }

        void UpdateObjectLayers(Transform parent, int layer)
        {
            foreach (var childTransform in parent.GetComponentsInChildren<Transform>(true))
                childTransform.gameObject.layer = layer;
        }

        private void SetMasks(bool IsLocalPlayer)
        {
            int invisibleMask = LayerMask.NameToLayer("Invisible");
            int visibleMask = LayerMask.NameToLayer("Default");

            if (IsLocalPlayer)
            {
                UpdateObjectLayers(characterModel.transform, invisibleMask);
                UpdateObjectLayers(currentWeapon.gunPivot, visibleMask);
            }
            else
            {
                UpdateObjectLayers(characterModel.transform, visibleMask);
                UpdateObjectLayers(currentWeapon.gunPivot, invisibleMask);
            }
        }

        private void EnableComponents(bool enabled)
        {
            fpp.enabled = enabled;
            playerCamera.enabled = enabled;
            currentWeapon.enabled = enabled;
        }

        public  void SendInputs(Vector3 movement, Vector3 rotation, Quaternion aimOrentation, Vector3 inputDirection)
        {
            this.playerCamera.transform.localRotation = aimOrentation; // only pitch
            this.transform.Rotate(rotation); // only yaw
            cc.Move(movement);//player physics 
            animator.SetFloat(VelocityX, inputDirection.x);
            animator.SetFloat(VelocityZ, inputDirection.z); 
        }

        [ServerRpc]
        void CalculatePlayerMovementServerRpc(Vector3 movement, Vector3 rotation, Quaternion aimOrentation)
        {
            //Physics
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