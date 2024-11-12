
namespace Shared.Response
{
    public class InsertGenericResult
    {
        public string Message { get; set; } // Mensaje de la operación
        public int Pk { get; set; } // Clave principal del departamento (en caso de éxito)

        public InsertGenericResult()
        {
            Message = string.Empty;
            Pk = 0;
        }
    }
}