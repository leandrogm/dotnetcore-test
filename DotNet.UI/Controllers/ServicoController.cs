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
    public class ServicoController : ControllerBase
    {
        private readonly IRepository<Servico> _ServicoRepository;
        private readonly IRepository<Tipo> _tipoRepository;
        private readonly IRestClient _restClient;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly EmailSettings _emailSettings;

        public ServicoController(IRepository<Servico> ServicoRepository,
            IRepository<Tipo> tipoRepository,
            IRestClient restClient,
            UserManager<ApplicationUser> userManager,
            IOptions<EmailSettings> emailOptions)
        {
            _ServicoRepository = ServicoRepository;
            _restClient = restClient;
            _userManager = userManager;
            _tipoRepository = tipoRepository;
            _emailSettings = emailOptions.Value;
        }
        // GET: api/<ServicoController>
        [HttpGet]
        public IActionResult Get()
        {
            // todo mover lógica para projeto business e criar padrão CQRS para segmentar lógica de queries e commands
            // todo testes de integração
            var request = new RestRequest(@"http://virtserver.swaggerhub.com/finch/avaliacao/1.0.0/fila", Method.GET);
            var response = _restClient.Execute<List<Servico>>(request);
            var ServicoFromClient = response.Data;

            ServicoFromClient.ForEach(servico =>
            {
                var servicoExistente = _ServicoRepository.FirstOrDefault(x => x.Id == servico.Id);
                if (servicoExistente == null)
                {
                    servico.Status = StatusEnum.Pendente;
                    //Servico.UserId = _userManager.GetUserId(ClaimsPrincipal.Current); //todo pegar id do usuário logado
                    var tipoServico = _tipoRepository.SingleOrDefault(x => x.Id == servico.Tipo.Id);
                    if (tipoServico == null)
                    {
                        tipoServico = servico.Tipo;
                        _tipoRepository.Add(tipoServico);
                        _tipoRepository.CommitChanges();
                    }
                    servico.TipoId = tipoServico.Id;
                    _ServicoRepository.Add(servico);
                    _ServicoRepository.CommitChanges();
                }
            });

            var result = _ServicoRepository.Include(x => x.Tipo)
                .Where(x => x.Status == StatusEnum.Pendente)
                .ToList();

            return Ok(result);
        }

        // GET api/<ServicoController>/5
        [HttpGet("{id:guid}")]
        public IActionResult Get(Guid id)
        {
            // todo mover lógica para projeto business e criar padrão CQRS para segmentar lógica de queries e commands
            // todo testes de integração
            var result = _ServicoRepository.SingleOrDefault(x => x.Id == id);
            return Ok(result);
        }

        [HttpPatch("{id:guid}/status/{status}")]
        public IActionResult Patch(Guid id, [FromRoute] int status)
        {
            // todo mover lógica para projeto business e criar padrão CQRS para segmentar lógica de queries e commands
            // todo testes de integração
            var servico = _ServicoRepository.SingleOrDefault(x =>
                x.Id == id &&
                x.Status != StatusEnum.Finalizado //todo adicionar condicao se o id do usuario for igual ao local ou null
            );

            var newStatus = (StatusEnum)status;
            if (servico != null)
            {
                servico.Status = newStatus;
                _ServicoRepository.Update(servico);
                _ServicoRepository.CommitChanges();

                if (servico.Status == StatusEnum.Finalizado)
                {
                    //configurar o secret pras configurações do mailgun e descomentar para enviar o email de fato
                    //SendEmailFinalizacao(servico);
                }
                return Ok();
            }
            else
            {
                return NotFound();
            }
        }

        private Task<IRestResponse> SendEmailFinalizacao(Servico servico)
        {
            // todo mover lógica para projeto business e criar padrão CQRS para segmentar lógica de queries e commands
            _restClient.BaseUrl = new Uri(_emailSettings.ApiBaseUri);
            _restClient.Authenticator =
                    new HttpBasicAuthenticator("api",
                                                _emailSettings.ApiKey);
            RestRequest request = new RestRequest();
            request.AddParameter("domain", _emailSettings.Domain, ParameterType.UrlSegment);
            request.Resource = _emailSettings.RequestUri;
            request.AddParameter("from", _emailSettings.From);
            request.AddParameter("to", _emailSettings.DefaultTo);
            request.AddParameter("subject", "Serviço Finalizado");
            request.AddParameter("text",
                String.Format("Serviço {0} finalizado com sucesso. Usuário: {1}",
                    servico.Id, servico.User.UserName ?? "")
                );
            request.Method = Method.POST;
            return _restClient.ExecuteAsync(request);
        }

    }
}
