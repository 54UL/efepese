namespace EPS.Core
{
    public class EmptyMode : IGameMode
    {
        public void EndGame()
        {
            throw new System.NotImplementedException();
        }

        public void ForceEnd()
        {
            throw new System.NotImplementedException();
        }

        public void SetGameValue(string name, object value)
        {
            throw new System.NotImplementedException();
        }

        public object GetGameValue(string name)
        {
            throw new System.NotImplementedException();
        }

        public string ModeInfo()
        {
            throw new System.NotImplementedException();
        }

        public void OnCheckEndRules()
        {
            throw new System.NotImplementedException();
        }

        public void OnStartGame()
        {
            throw new System.NotImplementedException();
        }
    }
}