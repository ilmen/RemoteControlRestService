namespace RemoteControlRestService.Classes.RunnableTasks
{
    public interface IRunnableTask
    {
        enRunnableTaskStatus Status
        { get; }

        void Run();

        //void Stop();
    }
}
