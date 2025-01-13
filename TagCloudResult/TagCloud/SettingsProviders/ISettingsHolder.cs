namespace TagCloud.SettingsProviders;

public interface ISettingsHolder<in T> where T : new()
{
    void SetSettings(T settings);
}
