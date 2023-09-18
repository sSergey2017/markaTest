namespace Products.Persistence;

public static class SD
{
    public static string MockyAPIBase { get; set; }
    public enum ApiType
    {
        GET,
        POST,
        PUT,
        DELETE
    }
}