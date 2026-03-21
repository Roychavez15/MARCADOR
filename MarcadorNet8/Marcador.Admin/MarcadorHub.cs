using Microsoft.AspNetCore.SignalR;

namespace Marcador.Admin;

public class MarcadorHub : Hub
{
    public async Task NotifyActualizado()
    {
        await Clients.All.SendAsync("EstadoActualizado");
    }
}
