﻿using AuroraVisionLauncher.Models;

namespace AuroraVisionLauncher.Contracts.Services;

public interface IFileAssociationService
{
    void SetAssociationsToApp(string? mainAppExecutablePath = null);
    IEnumerable<FileAssociationStatus> CheckCurrentAssociations(string? mainAppExecutablePath = null);
}
