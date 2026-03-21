using System.Net.Http.Json;
using System.Text.Json;
using Marcador.Core.Models;

namespace Marcador.Core;

public class ApiClient
{
    private static readonly JsonSerializerOptions JsonOpt = new() { PropertyNameCaseInsensitive = true };
    private readonly HttpClient _http;
    private readonly string _baseUrl;

    public ApiClient(string baseUrl)
    {
        _baseUrl = baseUrl.TrimEnd('/');
        _http = new HttpClient { BaseAddress = new Uri(_baseUrl), Timeout = TimeSpan.FromSeconds(15) };
    }

    public async Task<List<EquipoDto>> GetEquiposAsync(CancellationToken ct = default)
    {
        var list = await _http.GetFromJsonAsync<List<EquipoApi>>($"{_baseUrl}/api/Equipos", JsonOpt, ct) ?? [];
        return list.Select(e => new EquipoDto
        {
            Id = e.Id,
            Nombre = e.Nombre ?? "",
            NombreCorto = e.NombreCorto ?? "",
            Logo = e.Logo ?? ""
        }).ToList();
    }

    public async Task<List<JugadorPlantelDto>> GetJugadoresCampeonatoAsync(CancellationToken ct = default)
    {
        var list = await _http.GetFromJsonAsync<List<JugadorPlantelApi>>($"{_baseUrl}/api/JugadoresCampeonato", JsonOpt, ct) ?? [];
        return list.Select(j => new JugadorPlantelDto
        {
            Identificacion = j.Identificacion ?? "",
            Nombres = j.Nombres ?? "",
            Numero = j.Numero ?? "",
            Equipo = j.Equipo,
            Foto = j.Foto ?? ""
        }).ToList();
    }

    private class EquipoApi
    {
        public int Id { get; set; }
        public string? Nombre { get; set; }
        public string? NombreCorto { get; set; }
        public string? Logo { get; set; }
    }

    private class JugadorPlantelApi
    {
        public string? Identificacion { get; set; }
        public string? Nombres { get; set; }
        public string? Numero { get; set; }
        public int Equipo { get; set; }
        public string? Foto { get; set; }
    }
}
