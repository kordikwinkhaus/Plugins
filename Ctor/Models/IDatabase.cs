using System.Collections.Generic;

namespace Ctor.Models
{
    public interface IDatabase
    {
        WindowType GetWindowType(int id);

        bool IsVirtual(string nr_art, string table);

        List<int> GetCommonColors(IList<string> articles);
    }
}