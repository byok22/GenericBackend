
namespace Shared.Response
{
    public class GenericResponse
    {
        public bool IsSuccessful { get; set; }
        public string? Message { get; set; }
        public int? Id { get; set; }
    }
}