<div align="center">

<img src="Proyecto_API/assets/motogp_logo.png" alt="MotoGP Logo" width="300"/>

# 🏍️ MotoGP Dashboard

**Aplicación de escritorio para consultar estadísticas oficiales de MotoGP en tiempo real**

[![.NET](https://img.shields.io/badge/.NET-WPF-512BD4?style=for-the-badge&logo=dotnet)](https://dotnet.microsoft.com/)
[![C#](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=csharp&logoColor=white)](https://learn.microsoft.com/en-us/dotnet/csharp/)
[![Sportradar API](https://img.shields.io/badge/Sportradar-API-D50000?style=for-the-badge)](https://developer.sportradar.com/)
[![Doxygen](https://img.shields.io/badge/Docs-Doxygen-2C4AA8?style=for-the-badge&logo=doxygen)](https://www.doxygen.nl/)

</div>

---

## 📋 Descripción

**MotoGP Dashboard** es una aplicación de escritorio desarrollada con **WPF (Windows Presentation Foundation)** y **C#** que consume la API oficial de **Sportradar** para mostrar información actualizada sobre la temporada de MotoGP.

La aplicación muestra tarjetas dinámicas de pilotos y equipos con **colores por equipo**, **carga asíncrona** para no bloquear la UI y un diseño oscuro inspirado en la estética de MotoGP.

---

## ✨ Características

- 🏍️ **Vista de Pilotos** — Tarjetas de los 10 primeros pilotos de la temporada con nombre, país, abreviatura y equipo
- 🏁 **Vista de Equipos** — Tarjetas de los equipos con sus pilotos y colores distintivos por marca
- 🎨 **Colores dinámicos por equipo** — Ducati 🔴, Yamaha 🔵, KTM/GasGas 🟠, Aprilia/Pramac 🟣, Honda 🟡, VR46 🟡
- ⚡ **Carga asíncrona** — Uso de `async/await` y `Task` para no bloquear la interfaz de usuario
- 💾 **Caché de respuestas** — Evita llamadas repetidas a la API para datos ya descargados
- 🛡️ **Manejo de errores HTTP** — Gestión de errores 404, 403, 5xx y fallos de red
- 🖥️ **Diseño oscuro** — Tema `#121212` inspirado en el look oficial de MotoGP
- 📚 **Documentación completa** — Generada con Doxygen para todo el código fuente

---

## 🏗️ Arquitectura

El proyecto sigue el patrón **MVC (Model-View-Controller)**:

```
Proyecto_API/
├── 📁 Config/
│   └── ApiConfig.cs          # Centraliza la API Key, URL base y Season ID
│
├── 📁 Models/
│   ├── pilotosModels.cs       # Modelos de datos para pilotos
│   └── equiposModels.cs       # Modelos de datos para equipos
│
├── 📁 Services/
│   ├── pilotoServices.cs      # Llamadas HTTP a la API de pilotos
│   └── equipoServices.cs      # Llamadas HTTP a la API de equipos
│
├── 📁 Controllers/
│   ├── MainController.cs      # Gestión de navegación entre vistas
│   ├── PilotosController.cs   # Lógica de negocio de pilotos
│   └── EquiposController.cs   # Lógica de negocio de equipos
│
└── 📁 View/
    ├── MainWindow.xaml        # Pantalla principal (menú)
    ├── ViewPilotos.xaml       # Vista de pilotos
    └── ViewEquipos.xaml       # Vista de equipos
```

---

## 🛠️ Tecnologías

| Tecnología | Uso |
|---|---|
| **C# / .NET** | Lenguaje y framework principal |
| **WPF** | Interfaz gráfica de escritorio |
| **HttpClient** | Consumo de la API REST |
| **System.Text.Json** | Deserialización de respuestas JSON |
| **async / await** | Programación asíncrona sin bloqueo |
| **Sportradar MotoGP API v2** | Fuente de datos oficial |
| **Doxygen** | Generación de documentación técnica |

---

## 📦 Requisitos previos

- **Windows 10/11**
- **Visual Studio 2022** (o superior) con soporte para WPF
- **.NET Framework 4.8** (configurado en el proyecto)
- Conexión a internet para consumir la API
- **Doxygen** (opcional, solo para generar documentación)

---

## 🚀 Instalación y uso

1. **Clona el repositorio:**
   ```bash
   git clone https://github.com/Mauro-vs/Proyecto_API.git
   cd Proyecto_API
   ```

2. **Abre la solución en Visual Studio:**
   ```
   Proyecto_API.sln
   ```

3. **Restaura los paquetes NuGet** (Visual Studio lo hace automáticamente al compilar).

4. **Ejecuta la aplicación** con `F5` o el botón ▶️ de Visual Studio.

> **⚠️ Nota:** Necesitas una API Key propia de Sportradar (plan *trial* gratuito disponible en [developer.sportradar.com](https://developer.sportradar.com/)). Una vez obtenida, reemplaza el valor de `ApiKey` en `Config/ApiConfig.cs`. **No compartas ni subas tu clave al repositorio.**

---

## 📚 Documentación

El proyecto incluye **documentación completa generada con Doxygen** para todo el código fuente.

### Generar la documentación

**Método 1: Script automático (PowerShell)**
```powershell
.\GenerarDocumentacion.ps1
```

**Método 2: Manual**
```bash
doxygen Doxyfile
```

La documentación se generará en `Documentation/html/index.html`

📖 **[Ver guía completa de documentación](DOCUMENTACION.md)**

### Contenido de la documentación

✅ Descripción completa de todas las clases y métodos  
✅ Parámetros, retornos y excepciones documentados  
✅ Ejemplos de uso con código  
✅ Diagramas de clases (requiere Graphviz)  
✅ Navegación interactiva con búsqueda  

---

## 🌐 API utilizada

La aplicación consume la **[Sportradar MotoGP API v2](https://developer.sportradar.com/)**:

| Endpoint | Descripción |
|---|---|
| `/seasons/{id}/competitors.json` | Lista de pilotos de la temporada |
| `/competitors/{id}/profile.json` | Perfil detallado de un piloto |
| `/teams/{id}/profile.json` | Perfil detallado de un equipo |

---

## 📸 Capturas de pantalla

| Menú Principal | Vista de Pilotos | Vista de Equipos |
|---|---|---|
| Pantalla de inicio con acceso a Pilotos y Equipos | Tarjetas de pilotos con colores por equipo | Tarjetas de equipos con lista de pilotos |

---

## 📁 Estructura del Proyecto

```
Proyecto_API/
├── Proyecto_API/              # Código fuente de la aplicación
│   ├── Config/                # Configuración de la API
│   ├── Controllers/           # Controladores (lógica de negocio)
│   ├── Models/                # Modelos de datos
│   ├── Services/              # Servicios HTTP
│   └── View/                  # Vistas XAML
│
├── Documentation/             # Documentación generada (Doxygen)
├── Doxyfile                   # Configuración de Doxygen
├── GenerarDocumentacion.ps1   # Script para generar docs
├── DOCUMENTACION.md           # Guía de documentación
└── README.md                  # Este archivo
```

---

## 🤝 Contribuir

Las contribuciones son bienvenidas. Por favor:

1. Haz fork del proyecto
2. Crea una rama para tu feature (`git checkout -b feature/NuevaCaracteristica`)
3. Documenta tu código siguiendo el estilo Doxygen existente
4. Haz commit de tus cambios (`git commit -m 'Agregar nueva característica'`)
5. Haz push a la rama (`git push origin feature/NuevaCaracteristica`)
6. Abre un Pull Request

---

## 📄 Licencia

Este proyecto es de código abierto y está disponible para uso educativo.

---

## 👨‍💻 Autor

Desarrollado por **Mauro-vs** como proyecto de consumo de APIs REST con WPF.

- GitHub: [@Mauro-vs](https://github.com/Mauro-vs)
- Repositorio: [Proyecto_API](https://github.com/Mauro-vs/Proyecto_API)

---

<div align="center">

*¡Que empiece la carrera! 🏁*

</div>
