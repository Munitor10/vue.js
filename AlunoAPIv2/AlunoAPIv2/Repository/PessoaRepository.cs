using AlunosAPIv2.Models;
using Dapper;
using MySql.Data.MySqlClient;
using System.Data;

namespace AlunoAPIv2.Repositores
{
    public class PessoaRepository
    {
        private readonly string _connectionString;
       
        public PessoaRepository(string connectionString)
        {
            _connectionString = connectionString;
        }
        private IDbConnection Connection => new MySqlConnection(_connectionString);

        public async Task<IEnumerable<Pessoa>> ListarTodasPessoas(bool? ativo = null)
        {
            using (var conn = Connection)
            {
                    var sql = "SELECT * FROM tb_pessoas";

                if (ativo.HasValue)
                {
                    sql += " WHERE Ativo = @Ativo";
                    return await conn.QueryAsync<Pessoa>(sql, new {Ativo = ativo});
                }
                else
                {
                    return await conn.QueryAsync<Pessoa>(sql);
                }
            }
        }
        public async Task<Pessoa> BuscarPorId(int id)
        {
            var sql = "SELECT * FROM tb_pessoas WHERE Id = @Id";

            using (var conn = Connection)
            {
                return await conn.QueryFirstOrDefaultAsync<Pessoa>(sql,new {Id = id });
            }
        }
        public async Task<int> DeletarPorId(int id)
        {
            var sql = "DELETE FROM tb_pessoas WHERE Id = @Id";

            using (var conn = Connection)
            {
                return await conn.ExecuteAsync(sql, new { Id = id });
            }
        }
        public async Task<int> Criar(Pessoa dados)
        {
            var sql = "INSERT INTO tb_pessoas (Nome,Idade,Email) values (@Nome,@Idade,@Email);";

            using (var conn = Connection)
            {
                return await conn.ExecuteAsync(sql, new { Nome=dados.Nome, Idade=dados.Idade, Email=dados.Email});
            }
        }
        public async Task<int> Atualizar(Pessoa dados)
        {
            var sql = "update tb_pessoas set Nome = @Nome,Idade = @Idade,Email = @Email where Id = @id";

            using (var conn = Connection)
            {
                return await conn.ExecuteAsync(sql, dados);
            }
        }
        public async Task<bool> ValidaExistsEmail(string email)
        {
            var sql = "SELECT COUNT(*) FROM tb_pessoas WHERE Email = @Email";

            using (var conn = Connection)
            {
                return await conn.ExecuteScalarAsync<bool>(sql,
                    new { Email = email });
            }
        }
    }
}
