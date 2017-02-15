using SQLite;

namespace MIndFull
{
    public interface ISQLite
    {
        SQLiteConnection GetConnection();
    }
}
