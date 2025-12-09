using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Shared.Response;

namespace Api.Domain.Controllers
{
    public interface IGenericController <T> where T : class
    {
        [HttpGet]
        public  Task<ActionResult<IEnumerable<T>>> GetAll();
       
        [HttpGet("{id}")]
        public  Task<ActionResult<T>> GetById(int id);
       

        [HttpGet("{{Controller}}/{uuid}")]
        public  Task<ActionResult<T>> GetByUUID(string uuid);
       

        [Authorize(Roles = "Admin,Developer")]
        [HttpPost]
        public  Task<ActionResult<GenericResponse>> Create([FromBody] T T);
       
        [Authorize(Roles = "Admin,Developer")]
        [HttpPut("update")]
        public  Task<ActionResult<GenericResponse>> Update([FromBody] T T);
        

        [Authorize(Roles = "Admin,Developer")]
        [HttpDelete("{id}")]
        public  Task<ActionResult<GenericResponse>> Delete(int id);
       
    }
}