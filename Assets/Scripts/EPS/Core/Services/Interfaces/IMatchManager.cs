namespace EPS.Core
{
    public interface IMatchManager
    {
        public void StartMatch(IGameMode gamemode);
        public void EndMatch();
        public void SpawnPlayer();
        public void KillPlayer(int playerId);
    }
}


