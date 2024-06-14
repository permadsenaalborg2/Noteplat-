using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noteplat.Tests;

public class UnitTestRepository : IRepository
{
    Dictionary<string, string> Files = new();

    public async Task<string> PickFileLoad()
    {
        return await Task.Run(() => {
            return Files.First().Key;
        });
    }

    public async Task<string> PickFileSave()
    {
        return await Task.Run(() => {
            return Files.First().Key;
        });
    }

    public bool Exists(string filename)
    {
        return Files.TryGetValue(filename, out _);
    }

    public string ReadAllText(string filename)
    {
        return Files[filename];
    }

    public void WriteAllText(string filename, string contents)
    {
        Files[filename] = contents;
    }

    void IRepository.Delete(string filename)
    {
        Files.Remove(filename);
    }
}
