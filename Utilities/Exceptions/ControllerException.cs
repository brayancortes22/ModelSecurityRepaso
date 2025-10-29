using System;

namespace ModelSecurityRepaso.Utilities.Exception
{
    /// <summary>
    /// Excepción personalizada para errores en la capa de controladores.
    /// </summary>
    public class ControllerException : System.Exception
    {
        public ControllerException() { }

        public ControllerException(string message) : base(message) { }

        public ControllerException(string message, Exception innerException) : base(message, innerException) { }
    }
}