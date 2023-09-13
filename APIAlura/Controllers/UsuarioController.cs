

using APIAlura.Dto;
using APIAlura.Entity;
using APIAlura.Enums;
using APIAlura.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIAlura.Controllers
{
    [ApiController]
    [Route("Usuario")]
    public class UsuarioController : ControllerBase
    {


        private IUsuarioRepository _usuarioRepository;
        private readonly ILogger<UsuarioController> _logger;

        public UsuarioController(IUsuarioRepository usuarioRepository, ILogger<UsuarioController> logger)
        {
            _usuarioRepository = usuarioRepository;
            _logger = logger;

        }
        [Authorize]
        [Authorize(Roles = $"{Permissoes.Administrador},{Permissoes.Funcionario}" )]
        [HttpGet("obter-todos-com-pedidos/{id}")]
        public IActionResult ObterTodosComPedidos([FromRoute] int id)
        {
            return Ok(_usuarioRepository.ObterComPedidos(id));

        }
        [Authorize]
        [HttpGet("Obter-todos-os-usuarios")]
        public IActionResult ChamarTodosUsuarios()
        {

            try
            {
                return Ok(_usuarioRepository.ObterTodos());
            } catch (Exception ex)
            {
                _logger.LogError(ex, $"{DateTime.Now} -  Exception forcada");
                return BadRequest();
            }
        }
        [Authorize]
        [Authorize(Roles = $"{Permissoes.Administrador},{Permissoes.Funcionario}")]
        [HttpGet("Obter-usuario-por-id/{id}")]
        public IActionResult ChamarUsuarioID([FromRoute]int id)
        {
            return Ok(_usuarioRepository.ObterPorId(id));
        }

        [Authorize]
        [Authorize(Roles = $"{Permissoes.Administrador},{Permissoes.Funcionario}")]
        [HttpPost]
        public IActionResult CadastrarUsuario([FromBody]CadastrarUsuarioDto usuarioDto)
        {


            _usuarioRepository.Cadastrar(new Usuario(usuarioDto));
            var mensagem = $"Usuario criado com sucesso! | Nome: {usuarioDto.Nome}";
            _logger.LogWarning(mensagem);
            return Ok(mensagem);

        }
        [Authorize]
        [Authorize(Roles =$"{Permissoes.Administrador},{Permissoes.Funcionario}")]
        [HttpPut]
        public IActionResult EditarUsuario([FromBody]AlterarUsuarioDto usuarioDto)
        {
            _usuarioRepository.Editar(new Usuario(usuarioDto));
            return Ok("Usuario editado com sucesso!");
        }
        [Authorize]
        [Authorize(Roles = $"{Permissoes.Administrador}")]
        [HttpDelete]
        public IActionResult ExcluirUsuario(int id)
        {
            _usuarioRepository.Deletar(id);
            return Ok("Usuario deletado com sucesso!");
        }
    }
}
