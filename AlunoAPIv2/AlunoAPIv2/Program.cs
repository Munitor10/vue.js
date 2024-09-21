using AlunoAPIv2.Repositores;
using Google.Protobuf.WellKnownTypes;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowVueClient",
        policy =>
        {
            policy.WithOrigins("http://localhost:8080").AllowAnyHeader().AllowAnyMethod();
        });
});

//Realisa a leitura da com o banco
builder.Services.AddSingleton<PessoaRepository>(prvider => new PessoaRepository(builder.Configuration.GetConnectionString("DefaulConnection")));

//Swagger part1
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.EnableAnnotations();
});

var app = builder.Build();

//Cors
app.UseCors("AllowClient");

//Swagger part2
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json","Crud api");
        c.RoutePrefix = string.Empty;
    });
}

// Configure the HTTP request pipeline.

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
