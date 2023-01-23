using PharmcyChecking.Utils;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

namespace PharmcyChecking.Endpoints.Users.Post
{
    public class Endpoint : Endpoint<Request>
    {
        public override void Configure()
        {
            Post("user");
            AllowAnonymous();
        }

        public override async Task HandleAsync(Request req, CancellationToken ct)
        {
            MyBaseRespose<Response> response = new();

            HttpClient client = new HttpClient();
            Uri baseUri = new("https://api.osonapteka.uz/");
            client.BaseAddress = baseUri;
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.ConnectionClose = true;
            var authenticationString = $"checkstore:nMJO!p508H6s";
            var base64EncodedAuthenticationString = Convert.ToBase64String(Encoding.ASCII.GetBytes(authenticationString));
            var stringJson = @$"{{""code"": ""{req.Code}"",""password"": ""{req.Password}""}}";
            var stringContent = new StringContent(stringJson, Encoding.UTF8, "application/json");
            HttpRequestMessage requestMessage = new(System.Net.Http.HttpMethod.Post, "api/v1/check-store");
            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Basic", base64EncodedAuthenticationString);
            requestMessage.Content = stringContent;
            HttpResponseMessage result = await client.SendAsync(requestMessage, ct);
            string responseString = result.Content.ReadAsStringAsync(ct).Result;
            HttpResponseBody? responseBody = Newtonsoft.Json.JsonConvert.DeserializeObject<HttpResponseBody>(responseString);

            if (responseBody.Status is true)
            {
                response.Data = new()
                {
                    Status = responseBody.Status,
                    Detail = responseBody.Detail,
                    Name = responseBody.Name
                };
                response.Message = "User topildi, ma'lumotlar joylandi";
                response.Status = 1;

                string files = $"AllFolders\\{req.Code}";
                var FName = Directory.GetDirectories("AllFolders");

                DateTime nowT = DateTime.Now;
                string folder1 = nowT.ToString("yyyy-MM-d");
                string fileN = nowT.Ticks.ToString();

                if (FName.Contains(files))
                {
                    Directory.CreateDirectory(@$"AllFolders\{req.Code}\{folder1}");
                    string json = JsonSerializer.Serialize(responseBody);
                    File.WriteAllText(@$"AllFolders\{req.Code}\{folder1}\{fileN}.json", json);
                }
                else
                {
                    Directory.CreateDirectory(@$"AllFolders\{req.Code}");
                    Directory.CreateDirectory(@$"AllFolders\{req.Code}\{folder1}");
                    string json = JsonSerializer.Serialize(responseBody);
                    File.WriteAllText(@$"AllFolders\{req.Code}\{folder1}\{fileN}.json", json);
                }
            }
            else { response.Message = "User topilmadi"; response.Status = 0; }


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

    public class HttpResponseBody
    {
        public bool Status { get; set; }
        public string Detail { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
    }
}