using DapperAPI.Models;
using DapperAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using GraphQL.Models;
using GraphQL.Schema.Mutations;
using GraphQL.Schema.Query;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<DapperRepository>();
builder.Services.AddSingleton<DapperContext>();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddGraphQLServer()
    .AddQueryType<Query>()
    .AddMutationType<Mutation>()
    .AddSorting()
    .AddFiltering();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}

app.MapGraphQL(); 

app.Run();