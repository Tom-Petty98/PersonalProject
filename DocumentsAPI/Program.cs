using DocumentsAPI.Extensions;
using DocumentsAPI.Providers;
using DocumentsAPI.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

//if (builder.Configuration["KeyVaultName"] != "IntergrationTest")
//{
//    builder.Configuration.AddAzureKeyVAlue(
//        new Uri($"https//{builder.Configuration["KeyvaultName"]}.value.azure.net"),
//        new DefaultAzureCredential().
//        new AzureKeyValueConfigurationOptions
//        {
//            ReloadInterval = TimeSpan.FromHours(12)
//        });
//}


builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<IAzureBlobStorageService, AzureBlobStorageService>();
builder.Services.AddTransient<IDocumentsService,  DocumentsService>();

builder.Services.AddDbContext<DocumentsDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DocumentsDB"),
        provider =>
        {
            provider.MigrationsAssembly(typeof(DocumentsDbContext).Assembly.FullName);
            provider.EnableRetryOnFailure();
        });

});

builder.Services.AddTransient<IDocumentsProvider, DocumentsProvider>();



var app = builder.Build();

app.UseMiddleware<ExceptionMiddleware>();
app.UseMiddleware<LoggerMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();


namespace DocumentsAPI
{
    public partial class Program
    {

    }
}
