# Marcador LBG .NET 8

Sistema de marcador electrónico que consume datos de WebApiLBG y los presenta en pantalla.

## Requisitos

- .NET 8 SDK
- WebApiLBG ejecutándose (puerto por defecto 52091)

## Proyectos

- **Marcador.Core**: biblioteca compartida (SQLite, cliente HTTP, DTOs)
- **Marcador.Admin**: aplicación de administración (sync, equipos, goles, SignalR)
- **Marcador.Display**: pantalla del marcador (tamaño por defecto 256×236; configurable en SQLite desde el Admin)

## Configuración

En `Marcador.Core/Config.cs`:
- `ApiBaseUrl`: URL base del WebApiLBG (ej. `http://localhost:52091`)
- `SignalRPort`: puerto del hub SignalR (58273)
- `DbPath`: ruta SQLite (`%LocalAppData%\MarcadorLBG\marcador.db`)

## Uso

1. Ejecutar WebApiLBG (asegurar que tenga conexión a SQL Server)
2. Ejecutar **Marcador.Admin**
3. Clic en **Sincronizar API** para cargar equipos y jugadores
4. Seleccionar equipo local y visitante → **Aplicar**
5. Usar **+1 Local** / **+1 Visitante** para goles (opcional: elegir jugador)
6. Ejecutar **Marcador.Display** por separado (misma carpeta que el Admin o acceso directo al `.exe`); debe compartir el mismo `marcador.db` que el Admin
7. Pestaña **Diseño marcador**: posición, tamaño, fuentes, **colores** (`#RRGGBB` o `#AARRGGBB`), **visibilidad de logos**, **PictureBoxSizeMode** (0–4), nombres de equipo (Y con logo / Y sin logo), opción **Cabecera_ApilarSiVacio** para apilar título y etapa sin dejar huecos vacíos → **Guardar diseño**; el Display actualiza al instante (SignalR + refresco)

El administrador usa **pestañas**: **Operación** (equipos, goles, cronómetro), **Textos pantalla** (título, etapa, periodo), **Diseño marcador** (PropertyGrid).

**Perfiles de diseño** (SQLite `MarcadorLayoutPresets` + `MarcadorLayoutSeleccion`): al actualizar la app se crean **GUAPULO** (copia del JSON que ya tenías en `MarcadorLayout`) y **AMISTOSO** (valores por defecto). El combo **Perfil activo** cambia qué diseño usa el Display; **Guardar diseño** escribe en el perfil activo; **Guardar como…** crea o sobrescribe otro perfil por nombre; **Eliminar perfil** deja siempre al menos uno.

### Modo manual

- Seleccionar "Manual": ingresar nombres y subir logos
- **Aplicar** para guardar

## APIs consumidas

- `GET api/Equipos` (Id, Nombre, NombreCorto, Logo)
- `GET api/JugadoresCampeonato` (Identificacion, Nombres, Numero, Equipo, Foto)

## Pantalla

- Doble clic para salir
- Fondo negro, logos y marcador con tipografía
- Actualización en tiempo real vía SignalR
- El diseño se guarda en `MarcadorLayout` (SQLite): rectángulos, fuentes, colores de texto, logos visibles u ocultos, modo de imagen de logos, etiquetas de **nombre de equipo** (encima del logo o, si el logo está oculto, encima del marcador numérico). Cabecera opcionalmente apilada sin franjas vacías (`Cabecera_ApilarSiVacio`).
