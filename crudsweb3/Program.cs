using Microsoft.EntityFrameworkCore;
using crudsweb3.Seeders;
//en Program.cs registramos el seeder y lo ejecutamos al iniciar la aplicacion.
//reset=false: solo inserta datos si la tabla esta vacia
// reset=true: borra todos los datos y vuelve a sembrar cada vez que arranca
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddDbContext<crudsweb3.data.TareaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IDbInitializer, TareaSeeder>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var seeder = scope.ServiceProvider.GetRequiredService<IDbInitializer>();
    seeder.Initialize(scope.ServiceProvider, reset: false); //aqui
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
