using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Proyecto_API.Config
{
    /// <summary>
    /// Clase estática que centraliza la configuración de la API de Sportradar
    /// </summary>
    /// <remarks>
    /// Contiene las constantes necesarias para realizar peticiones a la API de MotoGP.
    /// Esta clase facilita el mantenimiento al centralizar todos los valores de configuración.
    /// </remarks>
    public static class ApiConfig
    {
        /// <summary>
        /// Clave de API para autenticación en Sportradar
        /// </summary>
        /// <remarks>
        /// Esta clave debe mantenerse privada y no debe compartirse públicamente.
        /// Se puede obtener en https://developer.sportradar.com/
        /// </remarks>
        public const string ApiKey = "qJZtgLw3r2Ks34pjWLDezerlH8zfUhvYvBNQ2Dxr";
        
        /// <summary>
        /// URL base de la API de MotoGP de Sportradar
        /// </summary>
        /// <remarks>
        /// Versión trial v2 de la API en inglés
        /// </remarks>
        public const string BaseUrl = "https://api.sportradar.com/motogp/trial/v2/en";

        /// <summary>
        /// Identificador de la temporada actual de MotoGP
        /// </summary>
        /// <remarks>
        /// Este ID puede cambiar cada año. Corresponde a la temporada 2024-2025
        /// </remarks>
        public const string SeasonId = "sr:season:119671";
    }
}
