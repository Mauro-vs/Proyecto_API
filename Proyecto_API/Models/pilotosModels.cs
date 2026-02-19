using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Proyecto_API.Models
{
    public class SeasonCompetitorsResponse
    {
        [JsonPropertyName("season_competitors")]
        public List<pilotosInfo> SeasonCompetitors { get; set; }
    }

    // 1. LA RESPUESTA QUE RECIBES DE LA API
    public class pilotosModels
    {
        [JsonPropertyName("competitor")]
        public pilotosInfo Competitor { get; set; }

        [JsonPropertyName("teams")]
        public List<pilotosTeam> Teams { get; set; }

        [JsonPropertyName("info")]
        public pilotosInfoExtra Info { get; set; }

        // Coge el primer equipo de la lista o pone "Sin Equipo"
        public string TeamName => Teams != null && Teams.Count > 0 ? Teams[0].Name : "Sin Equipo";

        // Calcula el color según el nombre del equipo
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

    // 2. DATOS DEL PILOTO
    public class pilotosInfo
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } // "Marquez, Marc"

        [JsonPropertyName("country_code")]
        public string CountryCode { get; set; } // "ESP"

        [JsonPropertyName("abbreviation")]
        public string Abbreviation { get; set; } // "MAR"

        // Helpers para separar nombre y apellido (Marc | MARQUEZ)
        public string FirstName => Name.Contains(",") ? Name.Split(',')[1].Trim() : "";
        public string LastName => Name.Contains(",") ? Name.Split(',')[0].Trim().ToUpper() : Name;
    }

    // 3. DATOS DEL EQUIPO
    public class pilotosTeam
    {
        [JsonPropertyName("name")]
        public string Name { get; set; } // "Ducati Lenovo Team"
    }

    // 4. INFO EXTRA
    public class pilotosInfoExtra
    {
        [JsonPropertyName("date_of_birth")]
        public string DateOfBirth { get; set; }
    }
}