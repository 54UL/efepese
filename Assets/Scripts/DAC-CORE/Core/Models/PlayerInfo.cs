namespace DAC
{
    [System.Serializable]
    public class PlayerInfo
    {
        //Base data model
        public int PlayerID;
        public IPlayerControl AsignedControlOwner;
        public bool IsNPC;
        //Complementary (has to be setted later of the data base)
        public MouseOrbit AsignedCamera;
        public CarDynamics AsignedVehicle;
        //public PlayerHUD AsignedPlayerHud;
    }
}

