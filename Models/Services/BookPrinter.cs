using System.Collections.Generic;
using System.Linq;
using Lab3Books.Models.Interfaces;

namespace Lab3Books.Models.Services;

public class BookPrinter
{
    public string PrintInfo(IPrintable item)
    {
        return item.GetInfo();
    }

    public List<string> PrintAll(IEnumerable<IPrintable> items)
    {
        return items.Select(item => item.GetInfo()).ToList();
    }
}
