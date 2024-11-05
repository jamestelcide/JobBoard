using JobBoard.WebAPI.StartupExtensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.ConfigureServices(builder.Configuration);

var app = builder.Build();

app.UseHsts();
app.UseHttpsRedirection();
app.UseSwagger(); //creates endpoint for swagger.json
app.UseSwaggerUI();
app.UseRouting();
app.UseCors();
app.UseHttpLogging();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
