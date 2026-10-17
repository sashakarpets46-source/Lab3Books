using Lab3Books.Models;

namespace Lab3Books.Models.Interfaces;

public interface IComparableBook
{
    bool IsLongerThan(Book other);
}
