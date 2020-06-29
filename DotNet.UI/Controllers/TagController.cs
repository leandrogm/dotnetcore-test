using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using DotNet.UI.Data.Repositories;
using DotNet.UI.Enums;
using DotNet.UI.Models;
using DotNet.UI.Settings;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using RestSharp;
using RestSharp.Authenticators;
using RestSharp.Extensions;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DotNet.UI.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TagController : ControllerBase
    {
        private readonly IRepository<Tag> _tagRepository;

        public TagController(
        IRepository<Tag> tagRepository)
        {
            _tagRepository = tagRepository;
        }

        // GET: api/<TagController>
        [HttpGet]
        public IActionResult Get()
        {
            var result = _tagRepository.GetAll();
            return Ok(result);
        }

    }
}
