using System;
using SystemException = System.Exception;

namespace ModelSecurityRepaso.Utilities.Exceptions
{
    /// <summary>
    /// Excepción personalizada para errores en la capa de controladores.
    /// </summary>
    public class ControllerException : SystemException
    {
        public ControllerException() { }

        public ControllerException(string message) : base(message) { }

        public ControllerException(string message, SystemException innerException) : base(message, innerException) { }
    }
}