using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Proyecto_API.Models
{
    /// <summary>
    /// Clase de respuesta para la lista de competidores de una temporada
    /// </summary>
    /// <remarks>
    /// Representa la respuesta JSON del endpoint /seasons/{id}/competitors.json
    /// </remarks>
    public class SeasonCompetitorsResponse
    {
        /// <summary>
        /// Lista de competidores de la temporada
        /// </summary>
        [JsonPropertyName("season_competitors")]
        public List<pilotosInfo> SeasonCompetitors { get; set; }
    }

    /// <summary>
    /// Modelo principal que representa un piloto con toda su información
    /// </summary>
    /// <remarks>
    /// Esta clase mapea la respuesta completa de la API para un piloto individual.
    /// Incluye información del competidor, equipos y datos extra.
    /// </remarks>
    public class pilotosModels
    {
        /// <summary>
        /// Información básica del competidor/piloto
        /// </summary>
        [JsonPropertyName("competitor")]
        public pilotosInfo Competitor { get; set; }

        /// <summary>
        /// Lista de equipos a los que pertenece el piloto
        /// </summary>
        [JsonPropertyName("teams")]
        public List<pilotosTeam> Teams { get; set; }

        /// <summary>
        /// Información adicional del piloto (fecha de nacimiento, etc.)
        /// </summary>
        [JsonPropertyName("info")]
        public pilotosInfoExtra Info { get; set; }

        /// <summary>
        /// Obtiene el nombre del primer equipo o "Sin Equipo"
        /// </summary>
        /// <value>Nombre del equipo principal del piloto</value>
        public string TeamName => Teams != null && Teams.Count > 0 ? Teams[0].Name : "Sin Equipo";

        /// <summary>
        /// Calcula el color hexadecimal según el nombre del equipo
        /// </summary>
        /// <value>Color en formato hexadecimal (#RRGGBB)</value>
        /// <remarks>
        /// Colores por equipo:
        /// - Ducati: Rojo (#D50000)
        /// - Fiat/Yamaha: Azul (#0055AA)
        /// - KTM/GasGas: Naranja (#FF6600)
        /// - Aprilia/Pramac: Morado (#6A0DAD)
        /// - Honda: Naranja/Dorado (#FF9900)
        /// - VR46: Amarillo Neon (#DFFF00)
        /// - Por defecto: Gris (#333333)
        /// </remarks>
        public string TeamColor
        {
            get
            {
                string t = TeamName.ToLower();
                if (t.Contains("ducati")) return "#D50000"; // Rojo
                if (t.Contains("fiat")) return "#0055AA"; // Azul
                if (t.Contains("ktm") || t.Contains("gasgas")) return "#FF6600"; // Naranja
                if (t.Contains("aprilia")) return "#6A0DAD"; // Morado
                if (t.Contains("honda")) return "#FF9900"; // Naranja/Dorado
                if (t.Contains("Prima") || t.Contains("Pramac")) return "#6A0DAD"; // Morado
                if (t.Contains("vr46")) return "#DFFF00"; // Amarillo Neon
                return "#333333"; // Gris por defecto
            }
        }
    }

    /// <summary>
    /// Información básica de un piloto/competidor
    /// </summary>
    /// <remarks>
    /// Contiene los datos principales del piloto según la API de Sportradar
    /// </remarks>
    public class pilotosInfo
    {
        /// <summary>
        /// Identificador único del piloto en formato Sportradar
        /// </summary>
        /// <example>sr:competitor:21999</example>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Nombre completo del piloto en formato "Apellido, Nombre"
        /// </summary>
        /// <example>Marquez, Marc</example>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Código de país del piloto (formato ISO 3166-1 alpha-3)
        /// </summary>
        /// <example>ESP</example>
        [JsonPropertyName("country_code")]
        public string CountryCode { get; set; }

        /// <summary>
        /// Abreviatura del nombre del piloto (generalmente 3 letras)
        /// </summary>
        /// <example>MAR</example>
        [JsonPropertyName("abbreviation")]
        public string Abbreviation { get; set; }

        /// <summary>
        /// Extrae solo el nombre del piloto (parte después de la coma)
        /// </summary>
        /// <value>Nombre del piloto</value>
        /// <example>Marc</example>
        public string FirstName => Name.Contains(",") ? Name.Split(',')[1].Trim() : "";
        
        /// <summary>
        /// Extrae solo el apellido del piloto en mayúsculas (parte antes de la coma)
        /// </summary>
        /// <value>Apellido en mayúsculas</value>
        /// <example>MARQUEZ</example>
        public string LastName => Name.Contains(",") ? Name.Split(',')[0].Trim().ToUpper() : Name;
    }

    /// <summary>
    /// Información de un equipo asociado a un piloto
    /// </summary>
    /// <remarks>
    /// Representa un equipo en la lista de equipos de un piloto
    /// </remarks>
    public class pilotosTeam
    {
        /// <summary>
        /// Nombre completo del equipo
        /// </summary>
        /// <example>Ducati Lenovo Team</example>
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }

    /// <summary>
    /// Información adicional del piloto
    /// </summary>
    /// <remarks>
    /// Contiene datos extra como fecha de nacimiento
    /// </remarks>
    public class pilotosInfoExtra
    {
        /// <summary>
        /// Fecha de nacimiento del piloto en formato ISO 8601
        /// </summary>
        /// <example>1993-02-17</example>
        [JsonPropertyName("date_of_birth")]
        public string DateOfBirth { get; set; }
    }
}