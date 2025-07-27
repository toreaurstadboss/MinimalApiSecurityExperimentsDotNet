namespace MinimalApiSecurityExperimentsDotNet
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            var app = builder.Build();

            // Configure the HTTP request pipeline.

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.MapGet("/guid", () =>
            {
                return GuidGenerator.CreateCryptographicallySecureGuid();
            });

            app.MapGet("/pseudonymize", (string? key, string? value) =>
            {
                var pseudonym = new Pseudonymizer(key).Pseudonymize(value, outputToHexString: true);
                return Results.Json(pseudonym);
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
