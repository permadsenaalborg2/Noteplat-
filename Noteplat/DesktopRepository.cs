using Avalonia.Platform.Storage;
using Noteplat.Views;
using System;
using System.IO;
using System.Threading.Tasks;
namespace Noteplat;

public class DesktopRepository : IRepository
{

    // wrapper for File functions
    public bool Exists(string filename)
    {
        return File.Exists(filename);
    }
    
    public string ReadAllText(string filename)
    {
        return File.ReadAllText(filename);
    }
    public void WriteAllText(string filename, string contents)
    {
        File.WriteAllText(filename, contents);
    }

    public void Delete(String filename)
    {
        File.Delete(filename);
    }

    // wrapper for OpenFilePicker
    public async Task<string> PickFileLoad()
    {
        var window = MainWindow.GetMainWindow() ?? throw new Exception("Unable to open file dialog");
        FilePickerOpenOptions options = new();
        options.AllowMultiple = false;
        var result = await window.StorageProvider.OpenFilePickerAsync(options);
        string path = "";
        if (result.Count == 1)
        {
            path = result[0].Path.LocalPath;
        }

        return path;
    }

    public async Task<string> PickFileSave()
    {
        var window = MainWindow.GetMainWindow() ?? throw new Exception("Unable to open file dialog");
        FilePickerSaveOptions options = new();
        options.ShowOverwritePrompt = true;
        var result = await window.StorageProvider.SaveFilePickerAsync(options);
        return result.Path.LocalPath;
    }
}
