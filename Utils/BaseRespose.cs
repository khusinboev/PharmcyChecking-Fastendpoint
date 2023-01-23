namespace PharmcyChecking.Utils
{
    public class MyBaseRespose<RVolue>
    {
        public RVolue? Data { get; set; }
        public string? Message { get; set; }
        public int? Status { get; set; } = 0;
    }
}
