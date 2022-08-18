namespace EPS.Core.Services.Implementations
{
    public interface GameMode 
    {
        void    OnStartGame();
        void    OnCheckEndRules();
        string  ModeInfo();
        void    ForceEnd();
    }
}