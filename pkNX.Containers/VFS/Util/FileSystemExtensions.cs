using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace pkNX.Containers.VFS;

public static class FileSystemExtensions
{
    public static IEnumerable<FileSystemPath> GetEntityPaths(this VirtualDirectory directory)
    {
        return directory.FileSystem.GetEntityPaths(directory.Path);
    }

    public static IEnumerable<FileSystemPath> GetEntitiesRecursive(this IFileSystem fileSystem, FileSystemPath path)
    {
        if (!path.IsDirectory)
            throw new ArgumentException("The specified path is not a directory.");

        foreach (var entity in fileSystem.GetEntityPaths(path))
        {
            yield return entity;

            if (!entity.IsDirectory)
                continue;

            foreach (var subEntity in fileSystem.GetEntitiesRecursive(entity))
                yield return subEntity;
        }
    }

    public static void CreateDirectoryRecursive(this IFileSystem fileSystem, FileSystemPath path)
    {
        if (!path.IsDirectory)
            throw new ArgumentException("The specified path is not a directory.");
        var currentDirectoryPath = FileSystemPath.Root;
        foreach (var dirName in path.GetDirectorySegments())
        {
            currentDirectoryPath = currentDirectoryPath.AppendDirectory(dirName);
            if (!fileSystem.Exists(currentDirectoryPath))
                fileSystem.CreateDirectory(currentDirectoryPath);
        }
    }

    /*#region Move Extensions
    public static void Move(this IFileSystem sourceFileSystem, FileSystemPath sourcePath, IFileSystem destinationFileSystem, FileSystemPath destinationPath)
    {
        if (!EntityMovers.Registration.TryGetSupported(sourceFileSystem.GetType(), destinationFileSystem.GetType(), out IEntityMover mover))
            throw new ArgumentException("The specified combination of file-systems is not supported.");
        mover.Move(sourceFileSystem, sourcePath, destinationFileSystem, destinationPath);
    }

    public static void MoveTo(this FileSystemEntity entity, IFileSystem destinationFileSystem, FileSystemPath destinationPath)
    {
        entity.FileSystem.Move(entity.Path, destinationFileSystem, destinationPath);
    }

    public static void MoveTo(this VirtualDirectory source, VirtualDirectory destination)
    {
        source.FileSystem.Move(source.Path, destination.FileSystem, destination.Path.AppendDirectory(source.Path.EntityName));
    }

    public static void MoveTo(this VirtualFile source, VirtualDirectory destination)
    {
        source.FileSystem.Move(source.Path, destination.FileSystem, destination.Path.AppendFile(source.Path.EntityName));
    }
    #endregion

    #region Copy Extensions
    public static void Copy(this IFileSystem sourceFileSystem, FileSystemPath sourcePath, IFileSystem destinationFileSystem, FileSystemPath destinationPath)
    {
        if (!EntityCopiers.Registration.TryGetSupported(sourceFileSystem.GetType(), destinationFileSystem.GetType(), out IEntityCopier copier))
            throw new ArgumentException("The specified combination of file-systems is not supported.");
        copier.Copy(sourceFileSystem, sourcePath, destinationFileSystem, destinationPath);
    }

    public static void CopyTo(this FileSystemEntity entity, IFileSystem destinationFileSystem, FileSystemPath destinationPath)
    {
        entity.FileSystem.Copy(entity.Path, destinationFileSystem, destinationPath);
    }

    public static void CopyTo(this VirtualDirectory source, VirtualDirectory destination)
    {
        source.FileSystem.Copy(source.Path, destination.FileSystem, destination.Path.AppendDirectory(source.Path.EntityName));
    }

    public static void CopyTo(this VirtualFile source, VirtualDirectory destination)
    {
        source.FileSystem.Copy(source.Path, destination.FileSystem, destination.Path.AppendFile(source.Path.EntityName));
    }
    #endregion*/
}
