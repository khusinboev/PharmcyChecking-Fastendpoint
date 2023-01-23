using PharmcyChecking.Utils;

namespace PharmcyChecking.Endpoints.Users.Get
{
    public class Endpoint : Endpoint<Request>
    {
        public override void Configure()
        {
            Get("user");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            MyBaseRespose<Response> response = new();
            await SendAsync(response, cancellation: ct);
        }
    }

    public class Request
    {
        public string Code { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class Response
    {
        public bool Status { get; set; }
        public string? Detail { get; set; }
        public string? Name { get; set; }
    }
}