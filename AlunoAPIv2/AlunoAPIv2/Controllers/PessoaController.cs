using AlunoAPIv2.Repositores;
using AlunosAPIv2.Models;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace AlunosAPIv2.Controllers
{
    [Route("api/pessoas")]
    [ApiController]
    public class PessoaController : ControllerBase
    {
        private readonly PessoaRepository _pessoaRepository;

        public PessoaController(PessoaRepository pessoaRepository)
        {
            _pessoaRepository = pessoaRepository;
        }

        // GET: api/<PessoaController>
        [HttpGet]
        [Route("listar")]
        [SwaggerOperation(Summary = "Listar todas as pessoas", Description = "Este endpoint retorna um listagem de pessoas cadastradas.")]
        public async Task<IActionResult> Listar([FromQuery] bool? ativo = null)
        {
            var dados = await _pessoaRepository.ListarTodasPessoas(ativo);

            if(dados == null)
            {
                return Ok("nao existe pesssoas cadastradas");
            }
            return Ok();
        }

        // GET api/<PessoaController>/5
        [HttpGet("detalhes/{id}")]
        [SwaggerOperation(
            Summary = "Obtém dados de uma pessoa pelo ID",
            Description = "Este endpoint retorna todos os dados de uma pessoa cadastrada filtrando pelo ID.")]
        public async Task<IActionResult> BuscarPorId(int id)
        {
            var pessoa =  await _pessoaRepository.BuscarPorId(id);

            if(pessoa== null)
            {
                return NotFound("Não existe pessoa cadastrada para o id informado");
            }

            return Ok(pessoa);
        }

        // POST api/<PessoaController>
        [HttpPost]
        [SwaggerOperation(
            Summary = "Cadastrar uma nova Pessoa",
            Description = "Este endpoint é responsavel por cadastrar uma nova pessoa no banco")]
        public async Task<IActionResult> Post([FromBody] Pessoa dados)
        {
            if(dados.Nome.Length < 5)
            {
                return BadRequest("O nome deve ter pelo menos 5 caracteres");
            }

            if(dados.Idade < 18)
            {
                return BadRequest("A idade deve ser maior ou igual a 18 anos");
            }

            var emailJaExiste = await _pessoaRepository.ValidaExistsEmail(dados.Email);

            if (emailJaExiste)
            {
                return BadRequest("O Email informado já está em usado, informe outro.");
            }

            await _pessoaRepository.Criar(dados);

            return Ok("Pessoa cadastrada com sucesso");
        }

        // PUT api/<PessoaController>/5
        [HttpPut("{id}")]
        [SwaggerOperation(
            Summary = "Atualizar os dados de Pessoa filtrando pelo ID.",
            Description = "Este endpoint é responsavel por atualizar os dados de uma pessoa no banco")]
        public async Task<IActionResult> Put(int id, [FromBody] Pessoa dados)
        {
            if (ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var emailJaExiste = await _pessoaRepository.ValidaExistsEmail(dados.Email);

            if (emailJaExiste)
            {
                return BadRequest("O Email informado já está em usado, informe outro.");
            }


            dados.Id = id;
            await _pessoaRepository.Atualizar(dados);
            return Ok();
        }

        // DELETE api/<PessoaController>/5
        [HttpDelete("{id}")]
        [SwaggerOperation(
            Summary = "Remover uma Pessoa filtrando pelo ID.",
            Description = "Este endpoint é responsavel por remover os dados de uma pessoa no banco")]
        public async Task<IActionResult> Delete(int id)
        {
            var pessoa = await _pessoaRepository.BuscarPorId(id);

            if (pessoa == null)
            {
                return NotFound("Não existe pessoa cadastrada para o id informado");
            }

            await _pessoaRepository.DeletarPorId(id);
            return Ok();
        }
    }

}
