using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AddressBook.DataAccess
{
    public static class AutoDirectory
    {
        public static DirectoryInfo CreateIfNotExists(this DirectoryInfo directory)
        {
            if (!directory.Exists)
                directory.Create();

            return directory;
        }
    }
}
