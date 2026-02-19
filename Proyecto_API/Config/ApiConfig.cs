using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_API.Config
{
    public static class ApiConfig
    {
        // Centralizamos la Key y la URL base aquí
        public const string ApiKey = "qJZtgLw3r2Ks34pjWLDezerlH8zfUhvYvBNQ2Dxr";
        public const string BaseUrl = "https://api.sportradar.com/motogp/trial/v2/en";

        // También podemos centralizar el ID de la temporada por si cambia
        public const string SeasonId = "sr:season:119671";
    }
}
