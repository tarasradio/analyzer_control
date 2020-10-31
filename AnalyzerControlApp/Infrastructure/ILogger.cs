namespace Infrastructure
{
    public interface ILogger
    {
        void Info(string message);
        void DemoInfo(string message);
        void ControllerInfo(string message);
    }
}
