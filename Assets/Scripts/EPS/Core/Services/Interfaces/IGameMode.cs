
namespace EPS
{
    public interface IGameMode 
    {
        void    OnStartGame();
        void    OnCheckEndRules();
        string  ModeInfo();
        void    EndGame();
        void    ForceEnd();
        object  GetGameValue(string name);
        void    SetGameValue(string name, object value);
    }
}