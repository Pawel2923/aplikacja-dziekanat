using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace aplikacja_dziekanat
{
    public interface ISecureStorageService
    {
        void Save(string key, string value);
        Task<string> Load(string key);
        void Delete(string key);
    }
}
