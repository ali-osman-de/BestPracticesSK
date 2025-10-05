using Microsoft.SemanticKernel;
using System.ComponentModel;

public class WeatherPlugin
{
    [KernelFunction("get_weather")]
    [Description("Verilen şehir için hava durumu bilgisini döner.")]
    public string GetWeather(
        [Description("Şehrin adı")] string city)
    {

        return city.ToLower() switch
        {
            "İstanbul" => "Istanbul'da hava güneşli, 24°C.",
            "ankara" => "Ankara'da hava bulutlu, 20°C.",
            "izmir" => "İzmir'de hava sıcak, 28°C.",
            _ => $"{city} için hava durumu bilgisi bulunamadı."
        };
    }
}
