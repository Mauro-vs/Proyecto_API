using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Proyecto_API.Models
{
    // Clase para la respuesta de la lista de equipos de la temporada
    public class equiposModelsResponse
    {
        [JsonPropertyName("season_teams")]
        public List<equipoInfo> SeasonTeams { get; set; }
    }

    public class equiposModels
    {
        [JsonPropertyName("team")]
        public equipoInfo Team { get; set; }

        [JsonPropertyName("competitors")]
        public List<pilotosInfo> Competitors { get; set; } // Reutilizamos pilotosInfo

        // --- PROPIEDAD PARA EL COLOR DINÁMICO ---
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

    public class equipoInfo
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("abbreviation")]
        public string Abbreviation { get; set; }
    }
}
