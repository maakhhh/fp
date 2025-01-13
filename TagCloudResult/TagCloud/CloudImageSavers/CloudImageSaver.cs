using System.Drawing;
using TagCloud.SettingsProviders;

namespace TagCloud.CloudImageSavers;

public class CloudImageSaver : ICloudImageSaver
{
    private readonly ISettingsProvider<SaveSettings> settingsProvider;

    public CloudImageSaver(ISettingsProvider<SaveSettings> settingsProvider)
    {
        this.settingsProvider = settingsProvider;
    }
    
    public string Save(Bitmap image)
    {
        var settings = settingsProvider.GetSettings();
        var filename = $"{settings.FileName}.{settings.Format.ToString().ToLower()}";
        image.Save(filename, settings.Format);
        return Path.Combine(Directory.GetCurrentDirectory(), filename);
    }
}
