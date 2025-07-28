using MinimalApiSecurityExperimentsDotNet.Extensions;
using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MinimalApiSecurityExperimentsDotNet
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            // Add Swagger services
            builder.Services.AddSwaggerGen();
            builder.Services.AddEndpointsApiExplorer();

            // Adding keyvault access via registering PseudonymizerHostedService 
            builder.Services.AddSingleton<PseudonymizerProvider>();
            builder.Services.AddHostedService<PseudonymizerHostedService>();  

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();

            app.MapControllers();

            // Enable Swagger middleware 
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.MapGet("/guid", () =>
            {
                return GuidGenerator.CreateCryptographicallySecureGuid();
            });

            app.MapGet("/generatestrongkey", () =>
            {
                return HmacSha256KeyGenerator.GenerateStrongKey();

            });

            app.MapGet("/pseudonymize", (string? key, string? value) =>
            {
                var pseudonym = new Pseudonymizer(key).Pseudonymize(value, outputToHexString: true);
                return Results.Json(pseudonym);
            }); 

            app.MapGet("/pseudonymize/verify", (string? key, string? value, string? pseudonym) =>
            {
                var pseudonymizer = new Pseudonymizer(key);
                var isValid = pseudonymizer.Verify(value, pseudonym);
                return Results.Json(isValid);
            });

            //Example using the injected singleton PseudonymizerProvider

            app.MapGet("/pseudo", (string? input, PseudonymizerProvider pseudonymizerProvider) => {
                var pseudonymizer = pseudonymizerProvider.Get();
                var result = pseudonymizer.Pseudonymize(input); //pseudonymize the input using the inject Pseudonymizer instance. This will use HMAC SHA256 under the hood.
                return Results.Ok(new { message = input, pseudonym = result });
            });

            app.MapGet("/getkeyvaultsecret", async (string secretName) =>
            {   
                var keyvaultRetriever = new KeyVaultSecretRetriever(builder.Configuration);
                var secret = await keyvaultRetriever.GetSecretAsync(secretName);
                return Results.Json(secret);               
            });

            app.MapGet("/", () =>
            {
                string html = """
                <h2>Welcome to the Security demos !</h2>
                <ul>
                    <li><a href="/guid">Generate a cryptographically secure GUID</a></li>
                    <li><a href="/otp">Generate a One Time Password (OTP)</a></li> 
                    <li><a href="/pseudonymize?key=your-secret-key&value=your-value">Pseudonymize a value</a></li>
                </ul>
                """;


                return Results.Content(html, "text/html");
            });


            app.MapGet("/otp", () =>
            {
                return OtpGenerator.GenerateOtp(10);
            });

            app.Run();
        }
    }


}
