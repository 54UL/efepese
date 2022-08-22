namespace EPS.Core.Services.Implementations
{
    public interface IGameMode 
    {
        void OnStartGame();
        void    OnCheckEndRules();
        string  ModeInfo();
        void    ForceEnd();
    }
}