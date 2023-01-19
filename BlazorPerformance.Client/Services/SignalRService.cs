using Microsoft.AspNetCore.SignalR.Client;

namespace BlazorPerformance.Client.Services
{
    public class SignalRService
    {
        private HubConnection? _hubConnection;

        public bool IsConnected => _hubConnection?.State == HubConnectionState.Connected;

        public event EventHandler<ConferenceCountEventArgs>? CountChanged;

        public async Task InitConnectionAsync()
        {
            if (IsConnected)
            {
                return;
            }

            _hubConnection = new HubConnectionBuilder()
                    .WithUrl($"https://localhost:7231/count")
                    .WithAutomaticReconnect(new[] { TimeSpan.Zero, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10) })
                    .Build();

            _hubConnection.On("UpdateCount", (int id, int count) =>
            {
                CountChanged?.Invoke(this, new ConferenceCountEventArgs { Id = id, Count = count });
            });

            await _hubConnection.StartAsync();
        }
    }

    public class ConferenceCountEventArgs : EventArgs
    {
        public int Id { get; set; }
        public int Count { get; set; }
    }
}
