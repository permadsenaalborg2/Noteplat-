using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Noteplat;

public interface IRepository
{
    public bool Exists(string filename);
    public string ReadAllText(string filename);
    public void WriteAllText(string filename, string contents);
    public void Delete(String filename);

    Task<string> PickFileLoad();
    Task<string> PickFileSave();
}
