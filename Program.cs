var builder = CoconaApp.CreateBuilder();
builder.Services.AddOptions<OpenAIOptions>().Bind(builder.Configuration.GetSection(OpenAIOptions.Section));
builder.Services.AddSingleton<IImageCreator, ImageCreator>();

var app = builder.Build();

app.AddCommand("image", async (string prompt, IImageCreator creator) =>
{
    await creator.GenerateImage(prompt);
});

await app.RunAsync();