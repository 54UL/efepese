namespace EPS.Core
{
    public interface IGameSession
    {
        bool Host(string GameName);
        public bool Join(uint id);
    }
}


