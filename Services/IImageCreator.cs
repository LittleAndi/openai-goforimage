namespace Services;

public interface IImageCreator
{
    Task GenerateImage(string prompt);
}
public class ImageCreator(IOptions<OpenAIOptions> options) : IImageCreator
{
    private readonly OpenAIOptions options = options.Value;

    public async Task GenerateImage(string prompt)
    {
        OpenAIClient openAIClient = new(options.ApiKey);

        var client = openAIClient.GetImageClient(options.Model);

        ImageGenerationOptions imageGenerationOptions = new()
        {
            ResponseFormat = GeneratedImageFormat.Bytes,
            Size = GeneratedImageSize.W1024xH1024,
        };
        var generatedImage = await client.GenerateImageAsync(prompt, imageGenerationOptions);

        var filename = $"{DateTime.Now:yyyyMMdd_HHmmss}.png";

        using TextWriter tw = new StreamWriter("image-and-prompt.txt", append: true);
        System.Console.WriteLine(generatedImage.Value.RevisedPrompt);
        tw.WriteLine($"{filename}\t{generatedImage.Value.RevisedPrompt}");

        using FileStream fs = File.Create(filename);
        generatedImage.Value.ImageBytes.ToStream().CopyTo(fs);

        tw.Close();
        fs.Close();
    }
}

public sealed class OpenAIOptions
{
    public const string Section = "OpenAI";
    public string ApiKey { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
}