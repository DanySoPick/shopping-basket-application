namespace shopping.basket.shared.Cors;

public interface ICorsOptions
{
    string[]? Origins { get; set; }
}
public class CorsOptions : ICorsOptions
{
    public const string Section = "CorsOptions";

    public string[]? Origins { get; set; }
}