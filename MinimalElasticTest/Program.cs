using Elastic.Clients.Elasticsearch;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

public async Task<List<MyDoc>> TestSearch()
{
    var client = new ElasticsearchClient(new Uri("http://localhost:9200")); // Просто для тесту

    var response = await client.SearchAsync<MyDoc>(s => s
        .Query(q => q
            .QueryString(qs => qs
                .Fields(f => f.Field(p => p.MyProperty)) // <--- ПРОВІРЯЄМО FIELD
                .Query("test"))));

    return response.Documents.ToList();
}

// Проста модель для тесту
public class MyDoc
{
    public string Id { get; set; }
    public string MyProperty { get; set; }
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();



app.Run();

