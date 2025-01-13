namespace TagCloud.SettingsProviders;

public class SettingsProvider<T> : ISettingsProvider<T>, ISettingsHolder<T>
    where T : new()
{
    private T settings = new();
    public T GetSettings() => settings;

    public void SetSettings(T settings)
    {
        this.settings = settings;
    }
}
