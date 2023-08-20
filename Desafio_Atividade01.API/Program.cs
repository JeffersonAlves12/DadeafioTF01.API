using Desafio_Atividade01.API.Models;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var produtos = new List<ProdutoModel>
{
    new ProdutoModel { Id = 1, Nome = "Mouse", Preco = 17m, Quantidade = 3 },
    new ProdutoModel { Id = 2, Nome = "Teclado", Preco = 35m, Quantidade = 5 },
    new ProdutoModel { Id = 3, Nome = "Monitor", Preco = 199m, Quantidade = 2 },
    new ProdutoModel { Id = 4, Nome = "Fone de Ouvido", Preco = 29.99m, Quantidade = 10 }
};



builder.Services.AddSingleton(produtos);
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Desafio.Atividade01.API", Version = "v1" });
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("/produtos", () =>
{
    var produtoService = app.Services.GetRequiredService<List<ProdutoModel>>();
    return Results.Ok(produtoService);
});


app.MapGet("produtos/{id}", (int id, HttpRequest resquest) =>
{
    var produtosService = app.Services.GetRequiredService<List<ProdutoModel>>();
    var produto = produtosService.FirstOrDefault(t => t.Id == id);

    if( produto == null)
    {
        return Results.NotFound();
    }

    return Results.Ok(produto);
});

app.MapPost("/produtos", (ProdutoModel produto) =>
{
    var produtoService = app.Services.GetRequiredService<List<ProdutoModel>>();
    produto.Id = produtoService.Max(t => t.Id) + 1;
    produtoService.Add(produto);
    return Results.Created($"/produtos/{produto.Id}", produto);

});

app.MapPut("/task/{id}", (int id, ProdutoModel produto) =>
{
    var produtoService = app.Services.GetRequiredService<List<ProdutoModel>>();
    var existingProduto = produtoService.FirstOrDefault(t => t.Id == id);

    if(existingProduto == null)
    {
        return Results.NotFound();

    }

    existingProduto.Nome = produto.Nome;
    existingProduto.Preco = produto.Preco;
    existingProduto.Quantidade = produto.Quantidade;

    return Results.NoContent();


});

app.MapDelete("/produtos/{id}", (int id) =>
{
    var produtoService = app.Services.GetRequiredService<List<ProdutoModel>>();
    var existingProduto = produtoService.FirstOrDefault(t => t.Id == id);
    if(existingProduto == null)
    {
        return Results.NotFound();

    }

    produtoService.Remove(existingProduto);

    return Results.NoContent();
});

// Configure the HTTP request pipeline.
/*if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
*/

//app.UseHttpsRedirection();

//app.UseAuthorization();

//app.MapControllers();

app.Run();
