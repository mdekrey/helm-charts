using CompressedStaticFiles;
using Microsoft.AspNetCore.HttpOverrides;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.All;
});
builder.Services.AddCompressedStaticFiles();

var app = builder.Build();

app.UseHttpsRedirection();
app.UseDefaultFiles();
app.UseCompressedStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.GetTypedHeaders().CacheControl =
            new Microsoft.Net.Http.Headers.CacheControlHeaderValue()
            {
                MustRevalidate = true,
                Public = true,
            };
    }
});

app.Run();
