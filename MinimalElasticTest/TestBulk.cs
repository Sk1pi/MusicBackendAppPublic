using Elastic.Clients.Elasticsearch;

namespace MinimalElasticTest;

public async Task TestBulk()
{
    var client = new ElasticsearchClient(new Uri("http://localhost:9200"));

    var docs = new List<MyDoc> { new MyDoc { Id = "1", MyProperty = "Value1" } };

    var response = await client.BulkAsync(b => b
        .CreateMany(docs, (cd, model) => cd.Document(model).Id(model.Id))); // <--- ПРОВІРЯЄМО DOCUMENT

    // ... обробка відповіді
}