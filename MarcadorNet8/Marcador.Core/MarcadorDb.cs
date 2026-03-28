using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;

namespace Marcador.Core;

public class MarcadorDb
{
    private readonly string _path;

    public MarcadorDb(string dbPath)
    {
        _path = dbPath;
        EnsureSchema();
    }

    private string ConnectionString => $"Data Source={_path}";

    private void EnsureSchema()
    {
        using var cnn = new SqliteConnection(ConnectionString);
        cnn.Open();
        cnn.Execute(@"
            CREATE TABLE IF NOT EXISTS CatalogoEquipos (
                Id INTEGER PRIMARY KEY,
                Nombre TEXT,
                NombreCorto TEXT,
                Logo TEXT
            );
            CREATE TABLE IF NOT EXISTS CatalogoJugadores (
                Identificacion TEXT PRIMARY KEY,
                Nombres TEXT,
                Numero TEXT,
                Equipo INTEGER,
                Foto TEXT,
                CampeonatoSync TEXT
            );
            CREATE TABLE IF NOT EXISTS PartidoActual (
                Id INTEGER PRIMARY KEY CHECK(Id=1),
                Modo TEXT,
                IdEquipoLocal INTEGER,
                IdEquipoVisitante INTEGER,
                NombreLocal TEXT,
                NombreVisitante TEXT,
                LogoLocal TEXT,
                LogoVisitante TEXT
            );
            CREATE TABLE IF NOT EXISTS MarcadorEstado (
                Id INTEGER PRIMARY KEY CHECK(Id=1),
                GolesLocal INTEGER DEFAULT 0,
                GolesVisitante INTEGER DEFAULT 0,
                TextoMarquee TEXT
            );
            CREATE TABLE IF NOT EXISTS GolesPartido (
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                IdentificacionJugador TEXT,
                EquipoLocal INTEGER,
                Minuto TEXT
            );
            INSERT OR IGNORE INTO PartidoActual (Id, Modo) VALUES (1, 'Manual');
            INSERT OR IGNORE INTO MarcadorEstado (Id, GolesLocal, GolesVisitante) VALUES (1, 0, 0);
            CREATE TABLE IF NOT EXISTS Cronometro (
                Id INTEGER PRIMARY KEY CHECK(Id=1),
                TotalSeconds INTEGER DEFAULT 0,
                IsRunning INTEGER DEFAULT 0
            );
            INSERT OR IGNORE INTO Cronometro (Id, TotalSeconds, IsRunning) VALUES (1, 0, 0);
            CREATE TABLE IF NOT EXISTS CelebracionGol (
                Id INTEGER PRIMARY KEY CHECK(Id=1),
                Activa INTEGER DEFAULT 0,
                Identificacion TEXT,
                Nombres TEXT,
                Numero TEXT,
                FotoUrl TEXT,
                EscudoUrl TEXT,
                GolesPartido INTEGER DEFAULT 0,
                GolesCampeonato INTEGER DEFAULT 0,
                VisibleHastaUtc TEXT
            );
            INSERT OR IGNORE INTO CelebracionGol (Id, Activa) VALUES (1, 0);
            CREATE TABLE IF NOT EXISTS MarcadorLayout (
                Id INTEGER PRIMARY KEY CHECK(Id=1),
                Json TEXT NOT NULL
            );
        ");
        MigrateColumns(cnn);
        SeedMarcadorLayoutIfNeeded(cnn);
        MigrateMarcadorLayoutPresets(cnn);
    }

    private static void SeedMarcadorLayoutIfNeeded(SqliteConnection cnn)
    {
        var n = cnn.ExecuteScalar<long>("SELECT COUNT(*) FROM MarcadorLayout WHERE Id=1");
        if (n > 0) return;
        var json = MarcadorLayoutSnapshot.ToJson(MarcadorLayoutSnapshot.CreateDefault());
        cnn.Execute("INSERT INTO MarcadorLayout (Id, Json) VALUES (1, @J)", new { J = json });
    }

    /// <summary>Perfiles nombrados (GUAPULO, AMISTOSO, …) + diseño activo para Display/Admin.</summary>
    private static void MigrateMarcadorLayoutPresets(SqliteConnection cnn)
    {
        cnn.Execute(@"
            CREATE TABLE IF NOT EXISTS MarcadorLayoutPresets (
                Nombre TEXT PRIMARY KEY COLLATE NOCASE,
                Json TEXT NOT NULL
            );
            CREATE TABLE IF NOT EXISTS MarcadorLayoutSeleccion (
                Id INTEGER PRIMARY KEY CHECK(Id=1),
                NombrePreset TEXT NOT NULL
            );
        ");
        var count = cnn.ExecuteScalar<long>("SELECT COUNT(*) FROM MarcadorLayoutPresets");
        if (count == 0)
        {
            var jsonGuapulo = cnn.QueryFirstOrDefault<string>("SELECT Json FROM MarcadorLayout WHERE Id=1");
            if (string.IsNullOrWhiteSpace(jsonGuapulo))
                jsonGuapulo = MarcadorLayoutSnapshot.ToJson(MarcadorLayoutSnapshot.CreateDefault());
            var jsonAmistoso = MarcadorLayoutSnapshot.ToJson(MarcadorLayoutSnapshot.CreateDefault());
            cnn.Execute("INSERT INTO MarcadorLayoutPresets (Nombre, Json) VALUES (@N, @J)", new { N = "GUAPULO", J = jsonGuapulo });
            cnn.Execute("INSERT INTO MarcadorLayoutPresets (Nombre, Json) VALUES (@N, @J)", new { N = "AMISTOSO", J = jsonAmistoso });
            cnn.Execute("INSERT OR IGNORE INTO MarcadorLayoutSeleccion (Id, NombrePreset) VALUES (1, 'GUAPULO')");
        }
        EnsureMarcadorLayoutSeleccionValida(cnn);
    }

    private static void EnsureMarcadorLayoutSeleccionValida(SqliteConnection cnn)
    {
        var sel = cnn.ExecuteScalar<long>("SELECT COUNT(*) FROM MarcadorLayoutSeleccion WHERE Id=1");
        if (sel == 0)
        {
            var first = cnn.QueryFirstOrDefault<string>("SELECT Nombre FROM MarcadorLayoutPresets ORDER BY Nombre COLLATE NOCASE LIMIT 1");
            if (string.IsNullOrEmpty(first))
                first = "GUAPULO";
            cnn.Execute("INSERT INTO MarcadorLayoutSeleccion (Id, NombrePreset) VALUES (1, @N)", new { N = first });
        }
        var activo = cnn.QueryFirstOrDefault<string>("SELECT NombrePreset FROM MarcadorLayoutSeleccion WHERE Id=1");
        if (string.IsNullOrWhiteSpace(activo))
            activo = "GUAPULO";
        var ok = cnn.ExecuteScalar<long>("SELECT COUNT(*) FROM MarcadorLayoutPresets WHERE Nombre = @N", new { N = activo });
        if (ok == 0)
        {
            var fallback = cnn.QueryFirstOrDefault<string>("SELECT Nombre FROM MarcadorLayoutPresets ORDER BY Nombre COLLATE NOCASE LIMIT 1");
            if (!string.IsNullOrEmpty(fallback))
                cnn.Execute("UPDATE MarcadorLayoutSeleccion SET NombrePreset=@N WHERE Id=1", new { N = fallback });
        }
    }

    public IReadOnlyList<string> GetMarcadorLayoutPresetNames()
    {
        using var cnn = new SqliteConnection(ConnectionString);
        return cnn.Query<string>("SELECT Nombre FROM MarcadorLayoutPresets ORDER BY Nombre COLLATE NOCASE").AsList();
    }

    public string GetActiveMarcadorLayoutPresetName()
    {
        using var cnn = new SqliteConnection(ConnectionString);
        var n = cnn.QueryFirstOrDefault<string>("SELECT NombrePreset FROM MarcadorLayoutSeleccion WHERE Id=1");
        return string.IsNullOrWhiteSpace(n) ? "GUAPULO" : n;
    }

    public void SetActiveMarcadorLayoutPreset(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("Nombre de perfil vacío.", nameof(nombre));
        nombre = nombre.Trim();
        using var cnn = new SqliteConnection(ConnectionString);
        cnn.Open();
        var exists = cnn.ExecuteScalar<long>("SELECT COUNT(*) FROM MarcadorLayoutPresets WHERE Nombre = @N", new { N = nombre });
        if (exists == 0)
            throw new InvalidOperationException($"No existe el perfil «{nombre}».");
        cnn.Execute("UPDATE MarcadorLayoutSeleccion SET NombrePreset=@N WHERE Id=1", new { N = nombre });
    }

    public void SaveMarcadorLayoutPreset(string nombre, MarcadorLayoutSnapshot layout)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            throw new ArgumentException("Indique un nombre de perfil.", nameof(nombre));
        nombre = nombre.Trim();
        var json = MarcadorLayoutSnapshot.ToJson(layout);
        using var cnn = new SqliteConnection(ConnectionString);
        cnn.Execute("INSERT OR REPLACE INTO MarcadorLayoutPresets (Nombre, Json) VALUES (@N, @J)", new { N = nombre, J = json });
    }

    public void DeleteMarcadorLayoutPreset(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
            return;
        nombre = nombre.Trim();
        using var cnn = new SqliteConnection(ConnectionString);
        cnn.Open();
        var total = cnn.ExecuteScalar<long>("SELECT COUNT(*) FROM MarcadorLayoutPresets");
        if (total <= 1)
            throw new InvalidOperationException("Debe quedar al menos un perfil de diseño.");
        var activo = cnn.QueryFirstOrDefault<string>("SELECT NombrePreset FROM MarcadorLayoutSeleccion WHERE Id=1") ?? "";
        if (string.Equals(activo, nombre, StringComparison.OrdinalIgnoreCase))
        {
            var otro = cnn.QueryFirstOrDefault<string>(
                "SELECT Nombre FROM MarcadorLayoutPresets WHERE LOWER(Nombre) <> LOWER(@N) ORDER BY Nombre COLLATE NOCASE LIMIT 1",
                new { N = nombre });
            if (string.IsNullOrEmpty(otro))
                throw new InvalidOperationException("No se puede eliminar el único perfil.");
            cnn.Execute("UPDATE MarcadorLayoutSeleccion SET NombrePreset=@O WHERE Id=1", new { O = otro });
        }
        cnn.Execute("DELETE FROM MarcadorLayoutPresets WHERE Nombre = @N", new { N = nombre });
    }

    public MarcadorLayoutSnapshot GetMarcadorLayoutSnapshot()
    {
        using var cnn = new SqliteConnection(ConnectionString);
        cnn.Open();
        var nombre = cnn.QueryFirstOrDefault<string>("SELECT NombrePreset FROM MarcadorLayoutSeleccion WHERE Id=1");
        if (!string.IsNullOrWhiteSpace(nombre))
        {
            var jsonP = cnn.QueryFirstOrDefault<string>(
                "SELECT Json FROM MarcadorLayoutPresets WHERE Nombre = @N", new { N = nombre });
            var snap = MarcadorLayoutSnapshot.FromJson(jsonP);
            if (snap != null)
                return snap;
        }
        var json = cnn.QueryFirstOrDefault<string>("SELECT Json FROM MarcadorLayout WHERE Id=1");
        return MarcadorLayoutSnapshot.FromJson(json) ?? MarcadorLayoutSnapshot.CreateDefault();
    }

    public void SetMarcadorLayout(MarcadorLayoutSnapshot layout)
    {
        var json = MarcadorLayoutSnapshot.ToJson(layout);
        using var cnn = new SqliteConnection(ConnectionString);
        cnn.Open();
        var nombre = cnn.QueryFirstOrDefault<string>("SELECT NombrePreset FROM MarcadorLayoutSeleccion WHERE Id=1");
        if (!string.IsNullOrWhiteSpace(nombre))
        {
            var n = cnn.Execute(
                "UPDATE MarcadorLayoutPresets SET Json=@J WHERE Nombre = @N",
                new { J = json, N = nombre });
            if (n == 0)
                cnn.Execute("INSERT INTO MarcadorLayoutPresets (Nombre, Json) VALUES (@N, @J)", new { N = nombre.Trim(), J = json });
        }
        cnn.Execute("UPDATE MarcadorLayout SET Json=@J WHERE Id=1", new { J = json });
        if (cnn.ExecuteScalar<long>("SELECT changes()") == 0)
            cnn.Execute("INSERT INTO MarcadorLayout (Id, Json) VALUES (1, @J)", new { J = json });
    }

    private static void MigrateColumns(SqliteConnection cnn)
    {
        try { cnn.Execute("ALTER TABLE MarcadorEstado ADD COLUMN SubtituloPeriodo TEXT DEFAULT 'PRIMER TIEMPO'"); } catch { /* ya existe */ }
        try { cnn.Execute("ALTER TABLE MarcadorEstado ADD COLUMN TituloLiga TEXT DEFAULT 'LIGA GUAPULO'"); } catch { }
        try { cnn.Execute("ALTER TABLE CelebracionGol ADD COLUMN InicioUtc TEXT"); } catch { }
        try { cnn.Execute("ALTER TABLE CelebracionGol ADD COLUMN EsManual INTEGER DEFAULT 0"); } catch { }
        try { cnn.Execute("ALTER TABLE Cronometro ADD COLUMN StartedAtUtc TEXT"); } catch { }
    }

    public void SyncEquipos(IEnumerable<(int Id, string Nombre, string NombreCorto, string Logo)> equipos)
    {
        using var cnn = new SqliteConnection(ConnectionString);
        cnn.Open();
        using var tr = cnn.BeginTransaction();
        cnn.Execute("DELETE FROM CatalogoEquipos", transaction: tr);
        foreach (var e in equipos)
            cnn.Execute(
                "INSERT INTO CatalogoEquipos (Id, Nombre, NombreCorto, Logo) VALUES (@Id, @Nombre, @NombreCorto, @Logo)",
                new { e.Id, e.Nombre, e.NombreCorto, e.Logo }, tr);
        tr.Commit();
    }

    public void SyncJugadores(string campeonato, IEnumerable<(string Identificacion, string Nombres, string Numero, int Equipo, string Foto)> jugadores)
    {
        using var cnn = new SqliteConnection(ConnectionString);
        cnn.Open();
        using var tr = cnn.BeginTransaction();
        cnn.Execute("DELETE FROM CatalogoJugadores", transaction: tr);
        foreach (var j in jugadores)
            cnn.Execute(
                "INSERT INTO CatalogoJugadores (Identificacion, Nombres, Numero, Equipo, Foto, CampeonatoSync) VALUES (@Identificacion, @Nombres, @Numero, @Equipo, @Foto, @Campeonato)",
                new { j.Identificacion, j.Nombres, j.Numero, j.Equipo, j.Foto, Campeonato = campeonato }, tr);
        tr.Commit();
    }

    public List<EquipoRow> GetEquipos() =>
        new SqliteConnection(ConnectionString).Query<EquipoRow>("SELECT Id, Nombre, NombreCorto, Logo FROM CatalogoEquipos ORDER BY NombreCorto").ToList();

    public List<JugadorRow> GetJugadoresPorEquipo(long idEquipo) =>
        new SqliteConnection(ConnectionString).Query<JugadorRow>(
            "SELECT Identificacion, Nombres, Numero, Equipo, Foto FROM CatalogoJugadores WHERE Equipo = @Id ORDER BY Numero",
            new { Id = idEquipo }).ToList();

    public List<JugadorRow> GetTodosJugadores() =>
        new SqliteConnection(ConnectionString).Query<JugadorRow>(
            "SELECT Identificacion, Nombres, Numero, Equipo, Foto FROM CatalogoJugadores ORDER BY Equipo, Numero").ToList();

    public PartidoRow? GetPartidoActual()
    {
        var r = new SqliteConnection(ConnectionString).QueryFirstOrDefault<PartidoRow>(
            "SELECT Modo, IdEquipoLocal, IdEquipoVisitante, NombreLocal, NombreVisitante, LogoLocal, LogoVisitante FROM PartidoActual WHERE Id=1");
        return r;
    }

    public void SetPartidoEquipos(long? idLocal, long? idVisitante, string? nombreLocal, string? nombreVisitante, string? logoLocal, string? logoVisitante)
    {
        using var cnn = new SqliteConnection(ConnectionString);
        cnn.Execute(
            "UPDATE PartidoActual SET IdEquipoLocal=@IdL, IdEquipoVisitante=@IdV, NombreLocal=@NomL, NombreVisitante=@NomV, LogoLocal=@LogL, LogoVisitante=@LogV WHERE Id=1",
            new { IdL = idLocal ?? 0L, IdV = idVisitante ?? 0L, NomL = nombreLocal ?? "", NomV = nombreVisitante ?? "", LogL = logoLocal ?? "", LogV = logoVisitante ?? "" });
    }

    /// <summary>Reinicia estado del partido en curso: goles, cronómetro y celebración.</summary>
    public void ReiniciarPartidoEnJuego()
    {
        using var cnn = new SqliteConnection(ConnectionString);
        cnn.Open();
        using var tr = cnn.BeginTransaction();
        cnn.Execute("DELETE FROM GolesPartido", transaction: tr);
        cnn.Execute("UPDATE MarcadorEstado SET GolesLocal=0, GolesVisitante=0 WHERE Id=1", transaction: tr);
        cnn.Execute("UPDATE Cronometro SET TotalSeconds=0, IsRunning=0, StartedAtUtc=NULL WHERE Id=1", transaction: tr);
        cnn.Execute("UPDATE CelebracionGol SET Activa=0 WHERE Id=1", transaction: tr);
        tr.Commit();
    }

    public MarcadorEstadoRow GetEstado()
    {
        using var cnn = new SqliteConnection(ConnectionString);
        var r = cnn.QueryFirstOrDefault<MarcadorEstadoRow>(
            "SELECT GolesLocal, GolesVisitante, TextoMarquee, IFNULL(SubtituloPeriodo,'') AS SubtituloPeriodo, IFNULL(TituloLiga,'') AS TituloLiga FROM MarcadorEstado WHERE Id=1");
        return r ?? new MarcadorEstadoRow(0L, 0L, null, "PT", "LIGA GUAPULO");
    }

    public void SetTextosCabecera(string? tituloLiga, string? etapaMarquee, string? subtituloPeriodo)
    {
        using var cnn = new SqliteConnection(ConnectionString);
        cnn.Execute(
            "UPDATE MarcadorEstado SET TituloLiga=@T, TextoMarquee=@E, SubtituloPeriodo=@S WHERE Id=1",
            new { T = tituloLiga ?? "", E = etapaMarquee ?? "", S = subtituloPeriodo ?? "" });
    }

    public void SetGoles(long golesLocal, long golesVisitante)
    {
        using var cnn = new SqliteConnection(ConnectionString);
        cnn.Execute("UPDATE MarcadorEstado SET GolesLocal=@L, GolesVisitante=@V WHERE Id=1", new { L = golesLocal, V = golesVisitante });
    }

    public void AgregarGol(int equipoLocal, string? identificacionJugador, string? minuto)
    {
        using var cnn = new SqliteConnection(ConnectionString);
        cnn.Execute(
            "INSERT INTO GolesPartido (IdentificacionJugador, EquipoLocal, Minuto) VALUES (@IdJug, @EqL, @Min)",
            new { IdJug = identificacionJugador ?? "", EqL = equipoLocal, Min = minuto ?? "" });
    }

    public List<GolRow> GetGolesPartido() =>
        new SqliteConnection(ConnectionString).Query<GolRow>(
            "SELECT g.Id, g.IdentificacionJugador, g.EquipoLocal, g.Minuto, j.Nombres, j.Numero AS Numero FROM GolesPartido g LEFT JOIN CatalogoJugadores j ON j.Identificacion=g.IdentificacionJugador ORDER BY g.Id").ToList();

    /// <summary>Cuenta goles desde la tabla y actualiza MarcadorEstado (mantiene coherencia al borrar filas).</summary>
    public void RecalcularMarcadorDesdeGoles()
    {
        using var cnn = new SqliteConnection(ConnectionString);
        cnn.Open();
        var local = cnn.QuerySingle<long>("SELECT COUNT(*) FROM GolesPartido WHERE EquipoLocal = 1");
        var visit = cnn.QuerySingle<long>("SELECT COUNT(*) FROM GolesPartido WHERE EquipoLocal = 0");
        cnn.Execute("UPDATE MarcadorEstado SET GolesLocal=@L, GolesVisitante=@V WHERE Id=1", new { L = local, V = visit });
    }

    public void EliminarGolPorId(long id)
    {
        using var cnn = new SqliteConnection(ConnectionString);
        cnn.Open();
        var n = cnn.Execute("DELETE FROM GolesPartido WHERE Id=@Id", new { Id = id });
        if (n > 0)
            RecalcularMarcadorDesdeGoles();
    }

    public long? ObtenerUltimoGolIdEquipo(bool esLocal)
    {
        var eq = esLocal ? 1 : 0;
        var rows = new SqliteConnection(ConnectionString).Query<long>(
            "SELECT Id FROM GolesPartido WHERE EquipoLocal=@E ORDER BY Id DESC LIMIT 1",
            new { E = eq }).AsList();
        return rows.Count > 0 ? rows[0] : null;
    }

    public void SetModoManual(string nombreLocal, string nombreVisitante, string? logoLocal, string? logoVisitante)
    {
        using var cnn = new SqliteConnection(ConnectionString);
        cnn.Execute(
            "UPDATE PartidoActual SET Modo='Manual', IdEquipoLocal=0, IdEquipoVisitante=0, NombreLocal=@NomL, NombreVisitante=@NomV, LogoLocal=@LogL, LogoVisitante=@LogV WHERE Id=1",
            new { NomL = nombreLocal, NomV = nombreVisitante, LogL = logoLocal ?? "", LogV = logoVisitante ?? "" });
    }

    public JugadorRow? GetJugador(string identificacion) =>
        new SqliteConnection(ConnectionString).QueryFirstOrDefault<JugadorRow>(
            "SELECT Identificacion, Nombres, Numero, Equipo, Foto FROM CatalogoJugadores WHERE Identificacion = @Id",
            new { Id = identificacion });

    public long CountGolesJugadorEnPartido(string identificacion) =>
        new SqliteConnection(ConnectionString).QuerySingle<long>(
            "SELECT COUNT(*) FROM GolesPartido WHERE IdentificacionJugador = @Id",
            new { Id = identificacion });

    public CronometroRow GetCronometro()
    {
        var r = new SqliteConnection(ConnectionString).QueryFirstOrDefault<CronometroRow>(
            "SELECT TotalSeconds, IsRunning, IFNULL(StartedAtUtc,'') AS StartedAtUtc FROM Cronometro WHERE Id=1");
        return r ?? new CronometroRow(0L, 0L, "");
    }

    /// <summary>Segundos a mostrar (tiempo real si está corriendo; TotalSeconds si está pausado).</summary>
    public long GetCronometroElapsedSeconds()
    {
        var c = GetCronometro();
        if (c.IsRunning == 1 && !string.IsNullOrWhiteSpace(c.StartedAtUtc) &&
            DateTime.TryParse(c.StartedAtUtc, null, System.Globalization.DateTimeStyles.RoundtripKind, out var start))
        {
            var elapsed = (DateTime.UtcNow - start).TotalSeconds;
            return (long)Math.Max(0, Math.Floor(elapsed));
        }
        return c.TotalSeconds;
    }

    /// <summary>Inicia o reanuda el cronómetro. Guarda StartedAtUtc = now - totalSeconds para contar desde tiempo real.</summary>
    public void CronometroStart(long totalSecondsActuales)
    {
        var startedAt = DateTime.UtcNow.AddSeconds(-totalSecondsActuales).ToString("o");
        using var cnn = new SqliteConnection(ConnectionString);
        cnn.Execute("UPDATE Cronometro SET TotalSeconds=@T, IsRunning=1, StartedAtUtc=@S WHERE Id=1",
            new { T = totalSecondsActuales, S = startedAt });
    }

    /// <summary>Detiene y pone en cero el cronómetro.</summary>
    public void CronometroReset()
    {
        using var cnn = new SqliteConnection(ConnectionString);
        cnn.Execute("UPDATE Cronometro SET TotalSeconds=0, IsRunning=0, StartedAtUtc=NULL WHERE Id=1");
    }

    public CelebracionRow GetCelebracion()
    {
        var r = new SqliteConnection(ConnectionString).QueryFirstOrDefault<CelebracionRow>(
            @"SELECT Activa, Identificacion, Nombres, Numero, FotoUrl, EscudoUrl, GolesPartido, GolesCampeonato, VisibleHastaUtc,
                     IFNULL(InicioUtc,'') AS InicioUtc, IFNULL(EsManual,0) AS EsManual
              FROM CelebracionGol WHERE Id=1");
        return r ?? new CelebracionRow(0L, "", "", "", "", "", 0L, 0L, null, null, 0L);
    }

    /// <param name="esManual">true = solo escudo + nombre de equipo en el detalle (sin foto/número de jugador).</param>
    /// <param name="splashSegundos">Tiempo de la imagen intermedia (0 si no hay imagen configurada).</param>
    /// <param name="detalleSegundos">Tiempo del panel de jugador o de equipo.</param>
    public void ActivarCelebracionGol(string? identificacion, string nombres, string numero, string? fotoUrl, string? escudoUrl, long golesPartido, long golesCampeonato, bool esManual, int splashSegundos, int detalleSegundos)
    {
        var inicio = DateTime.UtcNow;
        var splash = Math.Max(0, splashSegundos);
        var detalle = Math.Max(1, detalleSegundos);
        var hasta = inicio.AddSeconds(splash + detalle).ToString("o");
        var inicioStr = inicio.ToString("o");
        using var cnn = new SqliteConnection(ConnectionString);
        cnn.Execute(
            @"UPDATE CelebracionGol SET Activa=1, Identificacion=@I, Nombres=@N, Numero=@Num, FotoUrl=@F, EscudoUrl=@E,
              GolesPartido=@GP, GolesCampeonato=@GC, VisibleHastaUtc=@H, InicioUtc=@Io, EsManual=@M WHERE Id=1",
            new
            {
                I = identificacion ?? "",
                N = nombres,
                Num = numero,
                F = fotoUrl ?? "",
                E = escudoUrl ?? "",
                GP = golesPartido,
                GC = golesCampeonato,
                H = hasta,
                Io = inicioStr,
                M = esManual ? 1L : 0L
            });
    }

    public void DesactivarCelebracionGol()
    {
        using var cnn = new SqliteConnection(ConnectionString);
        cnn.Execute("UPDATE CelebracionGol SET Activa=0 WHERE Id=1");
    }
}

/// <summary>Tipos numéricos como long: SQLite expone INTEGER como Int64 y Dapper exige constructor que coincida.</summary>
public record EquipoRow(long Id, string Nombre, string NombreCorto, string Logo);
public record JugadorRow(string Identificacion, string Nombres, string Numero, long Equipo, string? Foto);
public record PartidoRow(string Modo, long IdEquipoLocal, long IdEquipoVisitante, string? NombreLocal, string? NombreVisitante, string? LogoLocal, string? LogoVisitante);
public record MarcadorEstadoRow(long GolesLocal, long GolesVisitante, string? TextoMarquee, string? SubtituloPeriodo, string? TituloLiga);
public record GolRow(long Id, string IdentificacionJugador, long EquipoLocal, string? Minuto, string? Nombres, string? Numero);
public record CronometroRow(long TotalSeconds, long IsRunning, string? StartedAtUtc);
public record CelebracionRow(long Activa, string? Identificacion, string? Nombres, string? Numero, string? FotoUrl, string? EscudoUrl, long GolesPartido, long GolesCampeonato, string? VisibleHastaUtc, string? InicioUtc, long EsManual);
