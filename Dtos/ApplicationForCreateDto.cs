using Microsoft.AspNetCore.Http;

namespace iqrasys.api.Dtos
{
    public class ApplicationForCreateDto
    {
        public IFormFile File { get; set; }
    }
}