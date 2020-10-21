using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using FunctionAppProcessarAcoes.Models;
using FunctionAppProcessarAcoes.Documents;

namespace FunctionAppProcessarAcoes.Data
{
    public class AcoesRepository
    {
        private readonly MongoClient _client;
        private readonly IMongoDatabase _db;
        private readonly IMongoCollection<AcaoDocument> _collection;

        public AcoesRepository(IConfiguration configuration)
        {
            _client = new MongoClient(
                configuration["MongoDB:Connection"]);
            _db = _client.GetDatabase(
                configuration["MongoDB:Database"]);
            _collection = _db.GetCollection<AcaoDocument>(
                configuration["MongoDB:Collection"]);
        }

        public void Save(Acao acao)
        {
            _collection.InsertOne(new AcaoDocument()
            {
                HistLancamento = $"{acao.Codigo}-{acao.Data.ToString("yyyyMMdd-HHmmss")}",
                Sigla = acao.Codigo,
                Data = acao.Data.ToString("yyyy-MM-dd HH:mm:ss"),
                Valor = acao.Valor.Value,
                Corretora = new DadosCorretora()
                {
                    Codigo = acao.CodCorretora,
                    Nome = acao.NomeCorretora
                }
            });            
        }
    }
}