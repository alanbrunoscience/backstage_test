using System.Reflection;

namespace Jazz.Application.Configuration;

public record ServiceInfo
{
    public ServiceInfo()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var fvi = System.Diagnostics.FileVersionInfo.GetVersionInfo(assembly.Location);
        Name = assembly.GetName().Name ?? throw new NotSupportedException("Undefined service name.");
        Version = fvi.FileVersion ?? throw new NotSupportedException("Undefined service version");
    }

    public string Name { get; }
    public string Version { get; }

    public void Deconstruct(out string name, out string? version)
    {
        name = Name;
        version = Version;
    }
}