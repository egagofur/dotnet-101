using Microsoft.EntityFrameworkCore;
using WarehouseApi.Data;
using WarehouseApi.Repositories;
using WarehouseApi.Services.Interface;
using WarehouseApi.Services.Implementations;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Repository and Service Layer
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRoleRepository, RoleRepository>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.Converters.Add(
            new System.Text.Json.Serialization.JsonStringEnumConverter(
                System.Text.Json.JsonNamingPolicy.CamelCase,
                allowIntegerValues: true
            )
        );
    });

builder.Services.Configure<Microsoft.AspNetCore.Mvc.ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value != null && e.Value.Errors.Count > 0)
            .ToDictionary(
                kvp =>
                {
                    var key = kvp.Key;
                    if (key.StartsWith("$.")) key = key.Substring(2);
                    return System.Text.Json.JsonNamingPolicy.CamelCase.ConvertName(key);
                },
                kvp => kvp.Value!.Errors.Select(er =>
                {
                    var msg = er.ErrorMessage;
                    if (string.IsNullOrEmpty(msg) && er.Exception != null)
                    {
                        msg = er.Exception.Message;
                    }

                    if (msg.Contains("could not be converted", StringComparison.OrdinalIgnoreCase))
                    {
                        if (msg.Contains("RolesNameEnum", StringComparison.OrdinalIgnoreCase))
                        {
                            return "Pilihan Role tidak valid. Gunakan salah satu dari: admin, supervisor, warehouse_operator.";
                        }
                        if (msg.Contains("Boolean", StringComparison.OrdinalIgnoreCase))
                        {
                            return "Nilai status tidak valid. Gunakan true atau false.";
                        }
                        return "Format nilai yang dikirim tidak sesuai.";
                    }
                    return msg;
                }).ToArray()
            );

        return new Microsoft.AspNetCore.Mvc.BadRequestObjectResult(new
        {
            success = false,
            message = "Validasi input gagal. Silakan periksa kembali data yang Anda kirim.",
            errors = errors
        });
    };
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseMiddleware<WarehouseApi.Middlewares.ExceptionHandlingMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
