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
        public GameObject characterModel;
        public double CurrentHealth = 100;
        public Transform SpawnPoint;
        private static readonly int VelocityZ = Animator.StringToHash("VelocityZ");
        private static readonly int VelocityX = Animator.StringToHash("VelocityX");
        public NetworkMatch Hud;

        public override void OnNetworkSpawn()
        {
            characterModel = this.transform.Find("character").gameObject;
            fpp = GetComponent<FirstPersonController>();
            currentWeapon = GetComponent<WeaponSystem>();
            playerCamera = GetComponentInChildren<Camera>();
            cc = GetComponent<CharacterController>();
            animator = characterModel.GetComponent<Animator>();
            currentWeapon.OnBulletHit += ShootSomeOne;
            Hud = FindObjectOfType<NetworkMatch>();
            Hud.SetUIHealth(CurrentHealth);

            fpp.IsClient = IsClient;
            fpp.currentNetworkPlayer = this;
            ConfigurePlayer(IsLocalPlayer);
            EnableComponents(IsLocalPlayer);
        }

        void UpdateObjectLayers(Transform parent, int layer)
        {
            foreach (var childTransform in parent.GetComponentsInChildren<Transform>(true))
                childTransform.gameObject.layer = layer;
        }

        private void SpawnPlayer()
        {
            if (IsClient || isHost)
            {
                PlacePlayerAtServerRpc(SpawnPoint.transform.position, Quaternion.identity);
            }
            else
            {
                PlacePlayerAtClientRpc(SpawnPoint.transform.position, Quaternion.identity);
            }
        }

        private void ConfigurePlayer(bool IsLocalPlayer)
        {
            int invisibleMask = LayerMask.NameToLayer("Invisible");
            int visibleMask = LayerMask.NameToLayer("Default");
            var audioListener = playerCamera.GetComponent<AudioListener>();

            if (IsLocalPlayer)
            {
                UpdateObjectLayers(characterModel.transform, invisibleMask);
                UpdateObjectLayers(currentWeapon.gunPivot, visibleMask);
                audioListener.enabled = true;
            }
            else
            {
                UpdateObjectLayers(characterModel.transform, visibleMask);
                UpdateObjectLayers(currentWeapon.gunPivot, invisibleMask);
                audioListener.enabled = false;
            }

            SpawnPlayer();
        }

        private void EnableComponents(bool enabled)
        {
            fpp.EnableLocalInput = enabled;
            playerCamera.enabled = enabled;
            currentWeapon.enabled = enabled;
        }
        
        //TODO: AQUI DEBERIA DE IR TODO EL FLUJO DE DATOS AUTORITARIO (SE ENVIA AL SERVIDOR Y DEBE DE REGRESAR EL FEEDBACK DEL TRANSFORM) (NO USAR ClientTransforms autorativos)
        public void SendInputs(Vector3 movement, Vector3 rotation, Quaternion aimOrentation, Vector3 inputDirection)
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

        public void DecreaseHealth(NetworkPlayer player, float value)
        {
            if (player.CurrentHealth > 0)
            {
                //Disable fpp control
                //Show respawn screen (todo)
                player.CurrentHealth -= value;
                player.UpdatePlayerHealthClientRpc(player.CurrentHealth);
            }
            else
            {
                player.UpdatePlayerHealthClientRpc(100.0f);
                player.PlacePlayerAtClientRpc(SpawnPoint.position, Quaternion.identity);
            }
        }

        public void ShootSomeOne(GameObject target, float damageToDeal)
        {
            var networkPlayer = target.GetComponent<NetworkPlayer>();
            if (networkPlayer == null) return;

            Debug.Log("Player hit!: " + target.gameObject.name);

            if (IsClient) {
                ShootServerRpc(networkPlayer, damageToDeal);
            }
            else {
                DecreaseHealth(networkPlayer, damageToDeal);
            }
        }

        [ServerRpc]
        void ShootServerRpc(NetworkBehaviourReference target, float damageToDeal)
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
        void UpdatePlayerHealthClientRpc(double healthValue)
        {
            Debug.Log("Updated damage: " + healthValue);
            CurrentHealth = healthValue;

            if (IsOwner)
                Hud.SetUIHealth(CurrentHealth);
        }

        [ClientRpc]
        void PlacePlayerAtClientRpc(Vector3 position, Quaternion orentation)
        {
            if (SpawnPoint != null)
            {
                this.transform.SetPositionAndRotation(position, orentation);
            }
            else
                Debug.LogError("No spawpoint specified to respawn player");
        }

        [ServerRpc]
        void PlacePlayerAtServerRpc(Vector3 position, Quaternion orentation)
        {
            if (SpawnPoint != null)
            {
                this.transform.SetPositionAndRotation(position, orentation);
                PlacePlayerAtClientRpc(position, orentation);
            }
            else
                Debug.LogError("No spawpoint specified to respawn player");
        }
    }
}