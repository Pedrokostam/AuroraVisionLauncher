﻿using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Timers;
using AuroraVisionLauncher.Core.Models.Programs;

namespace AuroraVisionLauncher.Core.Models.Apps;
internal record Configuration
{
    public Configuration(AvAppType appType, IEnumerable<ProgramType> supportedPrograms)
    {
        AppType = appType;
        _supportedPrograms = new List<ProgramType>(supportedPrograms);
    }

    public AvAppType AppType { get; }
    private readonly List<ProgramType> _supportedPrograms;
    public IReadOnlyCollection<ProgramType> SupportedPrograms => _supportedPrograms.AsReadOnly();
}
public record AvApp : IAvApp
{
    private static readonly Dictionary<string, Configuration> AvAppConfigurations = new(StringComparer.OrdinalIgnoreCase)
    {
        {"aurora vision studio",    new Configuration(AvAppType.Professional,[ProgramType.AdaptiveVisionProject,ProgramType.AuroraVisionProject])},
        {"adaptive vision studio",  new Configuration(AvAppType.Professional,[ProgramType.AdaptiveVisionProject])},
        {"fabimage studio",         new Configuration(AvAppType.Professional,[ProgramType.FabImageProject])},
        {"aurora vision executor",  new Configuration(AvAppType.Runtime,[ProgramType.AdaptiveVisionProject,ProgramType.AuroraVisionProject,ProgramType.AuroraVisionRuntime])},
        {"adaptive vision executor",new Configuration(AvAppType.Runtime,[ProgramType.AdaptiveVisionProject])},
        {"fabimage runtime",        new Configuration(AvAppType.Runtime,[ProgramType.FabImageRuntime,ProgramType.FabImageProject])},
    };

    public string ExePath { get; }
    public Version Version { get; }
    public string Name { get; }
    public AvAppType AppType { get; }
    public bool IsDevelopmentBuild => Version.Build >= 1000;
    readonly private FileVersionInfo _originalInfo;
    private readonly List<ProgramType> _supportedPrograms;
    protected IReadOnlyCollection<ProgramType> SupportedProgramTypes => _supportedPrograms.AsReadOnly();
    internal AvApp(FileVersionInfo fvinfo, Configuration configuration)
    {
        ExePath = fvinfo.FileName;
        Version = ParseVersion(fvinfo);
        Name = fvinfo.ProductName ?? "N/A";
        _originalInfo = fvinfo;

        _supportedPrograms = new List<ProgramType>(configuration.SupportedPrograms);
        AppType = configuration.AppType;
    }
    /// <summary>
    /// Checks if any process associated with the executable is running.
    /// </summary>
    /// <returns><see langword="true"/> if the process is running; <see langword="false"/> if it is not running, or it could not be checked.</returns>
    public bool CheckIfProcessIsRunning()
    {
        try
        {
            var p = Process.GetProcessesByName(Path.GetFileNameWithoutExtension(_originalInfo.InternalName));

            return p.Any(x => string.Equals(x.MainModule?.FileName, ExePath, StringComparison.OrdinalIgnoreCase));
        }
        catch (InvalidOperationException)
        {
            return false;
        }
    }

    protected static Version ParseVersion(FileVersionInfo fvinfo)
    {
        string? productVersion = fvinfo.ProductVersion;
        if (string.IsNullOrWhiteSpace(productVersion))
        {
            throw new ArgumentException("Empty product version field.");
        }
        if (productVersion.Contains(' '))
        {
            productVersion = productVersion.Split(' ', StringSplitOptions.TrimEntries)[0];
        }
        var ver = Version.Parse(productVersion);
        return ver;
    }
    
    static FileVersionInfo? FindInfo(string folder)

    {
        if (string.IsNullOrWhiteSpace(folder))
        {
            return null;
        }
        var dir = new DirectoryInfo(folder);
        // environment variables for studio proffesional points to AVL libraries, which are in the SDK subfolder
        if (dir.Name.Contains("sdk", StringComparison.OrdinalIgnoreCase))
        {
            dir = dir.Parent!;
        }
        var exes = dir.GetFiles("*.exe");
        FileInfo? theExe = null;
        if (folder.Contains("professional", StringComparison.OrdinalIgnoreCase))
        {
            theExe = exes.FirstOrDefault(x => x.Name.Contains("studio", StringComparison.OrdinalIgnoreCase));
        }
        else if (folder.Contains("runtime", StringComparison.OrdinalIgnoreCase))
        {
            theExe = exes.FirstOrDefault(x => x.Name.Contains("executor", StringComparison.OrdinalIgnoreCase));
        }
        if (theExe is null)
        {
            return null;
        }
        return FileVersionInfo.GetVersionInfo(theExe.FullName);
    }
    public static AvApp Create(FileVersionInfo fvinfo)
    {
        if (AvAppConfigurations.TryGetValue(fvinfo.ProductName ?? "", out Configuration? configuration))
        {
            return new AvApp(fvinfo, configuration);

        }
        throw new InvalidOperationException($"Unsupported product type: {fvinfo.ProductName}");
    }

    public static bool TryCreate(string folder, [NotNullWhen(true)] out AvApp? app)
    {
        if (FindInfo(folder) is not FileVersionInfo fileVersionInfo)
        {
            app = null;
            return false;
        }
        app = Create(fileVersionInfo);
        return true;
    }

    public bool SupportsProgram(ProgramInformation information)
    {
        return SupportedProgramTypes.Contains(information.ProgramType);
    }
    public static int GetClosestApp(IEnumerable<IAvApp> apps, VisionProgram info)
    {
        var weights = new List<double>();
        foreach (IAvApp app in apps)
        {
            double weight = 0;
            bool isProgramRuntime = info.Type == ProgramType.AuroraVisionRuntime || info.Type == ProgramType.FabImageRuntime;
            if (isProgramRuntime && app.AppType!=AvAppType.Runtime || !isProgramRuntime && app.AppType == AvAppType.Runtime)
            {
                weight = -1e21;
            }
            double programVersionTransformed = info.Version.Major * 1e12 + info.Version.Minor * 1e9 + info.Version.Build * 1e6 + info.Version.Revision;
            double executableVersionTransformed = app.Version.Major * 1e12 + app.Version.Minor * 1e9 + app.Version.Build * 1e6 + app.Version.Revision;
            weight += executableVersionTransformed - programVersionTransformed;
            weights.Add(Math.Abs(weight));
        }
        if (weights.Count == 0)
        {
            return -1;
        }
        var min = weights.Min();
        return weights.IndexOf(min);
    }
    protected string ShortForm() => $"{Name} {Version}";
    public override string ToString() => ShortForm();
}