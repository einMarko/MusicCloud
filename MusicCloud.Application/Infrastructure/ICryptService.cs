using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MusicCloud.Application.Infrastructure
{
    public interface ICryptService
    {
        string GenerateHash(byte[] key, byte[] data);
        string GenerateHash(byte[] key, string data);
        string GenerateHash(string key, string data);
        string GenerateSecret(int bits = 256);
    }
}
