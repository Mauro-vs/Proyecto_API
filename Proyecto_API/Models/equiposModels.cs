using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Proyecto_API.Models
{
    /// <summary>
    /// Clase de respuesta para la lista de equipos de una temporada
    /// </summary>
    /// <remarks>
    /// Representa la respuesta JSON del endpoint /seasons/{id}/teams.json
    /// </remarks>
    public class equiposModelsResponse
    {
        /// <summary>
        /// Lista de equipos participantes en la temporada
        /// </summary>
        [JsonPropertyName("season_teams")]
        public List<equipoInfo> SeasonTeams { get; set; }
    }

    /// <summary>
    /// Modelo completo de un equipo de MotoGP
    /// </summary>
    /// <remarks>
    /// Esta clase mapea la respuesta completa de la API para un equipo individual.
    /// Incluye información del equipo y la lista de pilotos/competidores.
    /// </remarks>
    public class equiposModels
    {
        /// <summary>
        /// Información básica del equipo
        /// </summary>
        [JsonPropertyName("team")]
        public equipoInfo Team { get; set; }

        /// <summary>
        /// Lista de competidores/pilotos que pertenecen al equipo
        /// </summary>
        /// <remarks>
        /// Reutiliza la clase pilotosInfo del modelo de pilotos
        /// </remarks>
        [JsonPropertyName("competitors")]
        public List<pilotosInfo> Competitors { get; set; }

        /// <summary>
        /// Obtiene el color hexadecimal característico del equipo
        /// </summary>
        /// <value>Color en formato hexadecimal (#RRGGBB)</value>
        /// <remarks>
        /// Asigna colores dinámicos según la marca del equipo:
        /// - Ducati: Rojo (#D50000)
        /// - Yamaha/Monster: Azul (#0055AA)
        /// - KTM/GasGas: Naranja (#FF6600)
        /// - Aprilia: Morado (#6A0DAD)
        /// - Honda/LCR: Naranja/Dorado (#FF9900)
        /// - Pramac: Morado (#6A0DAD)
        /// - VR46: Amarillo Neon (#DFFF00)
        /// - Gresini: Azul claro (#87CEEB)
        /// - Por defecto: Gris (#333333)
        /// </remarks>
        public string TeamColor
        {
            get
            {
                if (Team == null || string.IsNullOrEmpty(Team.Name)) return "#333333";

                string t = Team.Name.ToLower();
                if (t.Contains("ducati")) return "#D50000"; // Rojo
                if (t.Contains("yamaha") || t.Contains("monster")) return "#0055AA"; // Azul
                if (t.Contains("ktm") || t.Contains("gasgas")) return "#FF6600"; // Naranja
                if (t.Contains("aprilia")) return "#6A0DAD"; // Morado
                if (t.Contains("honda") || t.Contains("lcr")) return "#FF9900"; // Naranja/Dorado
                if (t.Contains("pramac")) return "#6A0DAD"; // Morado/Rojo
                if (t.Contains("vr46")) return "#DFFF00"; // Amarillo Neon
                if (t.Contains("gresini")) return "#87CEEB"; // Azul claro

                return "#333333"; // Gris por defecto
            }
        }
    }

    /// <summary>
    /// Información básica de un equipo de MotoGP
    /// </summary>
    /// <remarks>
    /// Contiene los datos principales de identificación del equipo
    /// </remarks>
    public class equipoInfo
    {
        /// <summary>
        /// Identificador único del equipo en formato Sportradar
        /// </summary>
        /// <example>sr:competitor:4567</example>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Nombre completo del equipo
        /// </summary>
        /// <example>Ducati Lenovo Team</example>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Abreviatura del nombre del equipo
        /// </summary>
        /// <example>DUC</example>
        [JsonPropertyName("abbreviation")]
        public string Abbreviation { get; set; }
    }
}
