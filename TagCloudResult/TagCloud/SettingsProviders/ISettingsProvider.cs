namespace TagCloud.SettingsProviders;

public interface ISettingsProvider<out T> where T : new()
{
    T GetSettings();
}
