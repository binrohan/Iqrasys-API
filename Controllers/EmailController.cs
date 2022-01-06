using System;
using System.Threading.Tasks;
using iqrasys.api.Models;
using iqrasys.api.Services;
using Microsoft.AspNetCore.Mvc;

namespace iqrasys.api.Controllers
{
    public class EmailController : ApiController
    {

        private readonly IMailService mailService;
        public EmailController(IMailService mailService)
        {
            this.mailService = mailService;
        }

        [HttpPost("Send")]
        public async Task<IActionResult> Send([FromForm] MailRequest request)
        {
            try
            {
                await mailService.SendEmailAsync(request);
                return Ok();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }


    }
}