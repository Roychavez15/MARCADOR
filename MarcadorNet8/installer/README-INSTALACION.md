# Instalación del Marcador (PC sin .NET)

## Qué incluye

La publicación **self-contained** para **Windows 64 bits** empaqueta el runtime de **.NET 8** y **ASP.NET Core** (el Admin arranca SignalR embebido). La PC destino **no necesita** instalar el “Hosting Bundle” ni el runtime por separado.

Tamaño aproximado: **~190 MB** (carpeta o instalador).

## En tu máquina de desarrollo (generar el paquete)

1. Instala el [SDK .NET 8](https://dotnet.microsoft.com/download/dotnet/8.0) si aún no lo tienes.
2. Abre una consola en la carpeta `MarcadorNet8\installer`.
3. Ejecuta uno de estos:

```cmd
build-install.cmd
```

o

```powershell
powershell -ExecutionPolicy Bypass -File .\build-install.ps1
```

Opcional: además genera un ZIP para copiar por USB/red:

```powershell
.\build-install.ps1 -Zip
```

**Salida:**

- Carpeta lista para copiar: `installer\out\win-x64\` (contiene `Marcador.Admin.exe` y `Marcador.Display.exe` más todas las DLL).
- Si tienes **Inno Setup 6** instalado, el script intentará generar también `installer\dist\MarcadorLBG-Setup-1.0.0-win-x64.exe`.

### Instalador .exe (Inno Setup)

1. Descarga [Inno Setup 6](https://jrsoftware.org/isdl.php) (gratis).
2. Ejecuta `build-install.ps1` (crea `out\win-x64`).
3. Abre `Marcador.Inno.iss` en Inno Setup y **Compile**, o deja que `build-install.ps1` encuentre `ISCC.exe` y compile solo.

Para cambiar la versión que muestra el instalador, edita `#define MyAppVersion` en `Marcador.Inno.iss`.

## En la PC del estadio / sala

### Opción A — Carpeta (sin instalador)

1. Copia toda la carpeta `out\win-x64` (o el contenido del ZIP) a un directorio fijo, por ejemplo `C:\MarcadorLBG`.
2. Ejecuta primero **`Marcador.Admin.exe`** (crea la base de datos y el servidor local).
3. Luego **`Marcador.Display.exe`** en la pantalla del marcador.

### Opción B — Instalador Inno

1. Ejecuta el `.exe` generado y sigue el asistente.
2. Usa los accesos del menú Inicio o escritorio.

## Uso habitual

1. **Administración**: configuración, partido, diseño, goles.
2. **Pantalla**: solo lectura / proyección; debe poder conectar al mismo equipo donde corre Admin (SignalR en localhost, según tu `Config`).

## Notas

- Arquitectura: **win-x64**. PCs muy antiguas 32 bits no son compatibles con este paquete; habría que publicar con `-r win-x86` (no incluido por defecto).
- **Antivirus**: la primera ejecución puede tardar unos segundos.
- Actualizar versión: vuelve a ejecutar `build-install.ps1` y redistribuye carpeta o nuevo Setup.
