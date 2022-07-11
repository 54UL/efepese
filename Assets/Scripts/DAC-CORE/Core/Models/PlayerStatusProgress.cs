namespace DAC
{
    public class PlayerStatusProgress
    {
        public int PlayerId;
        public int VehicleLevelProgress;
        public PlayerStatusProgress(int playerId,int vehicleLevelProgress)
        {
            this.PlayerId = playerId;
            this.VehicleLevelProgress = vehicleLevelProgress;
        }
    }
}