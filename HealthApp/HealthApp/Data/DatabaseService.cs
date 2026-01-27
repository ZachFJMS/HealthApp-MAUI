using SQLite;
using HealthApp.Models;

namespace HealthApp.Data
{
    public class DatabaseService
    {
        private SQLiteAsyncConnection _db;

        public async Task Init()
        {
            if (_db != null) return;

            var path = Path.Combine(FileSystem.AppDataDirectory, "health.db");
            _db = new SQLiteAsyncConnection(path);

            await _db.CreateTableAsync<User>();
            await _db.CreateTableAsync<HealthRecord>();
        }

        // User-related methods \\

        public async Task<int> AddUser(User user)
        {
            await Init();
            return await _db.InsertAsync(user);
        }

        public async Task<User> GetUser(string username, string passwordHash)
        {
            await Init();
            return await _db.Table<User>()
                .Where(u => u.Username == username && u.Password == passwordHash)
                .FirstOrDefaultAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            await Init();
            return await _db.Table<User>().Where(u => u.Id == id).FirstOrDefaultAsync();
        }

        // HealthRecord-related methods \\

        public async Task AddHealthRecord(HealthRecord record)
        {
            await Init();
            await _db.InsertAsync(record);
        }

        public async Task<HealthRecord> GetLatestHealthRecord(int userId)
        {
            await Init();

            return await _db.Table<HealthRecord>()
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.Date)
                .FirstOrDefaultAsync();
        }

        public async Task<List<HealthRecord>> GetAllHealthRecords(int userId)
        {
            await Init();

            return await _db.Table<HealthRecord>()
                .Where(r => r.UserId == userId)
                .OrderByDescending(r => r.Date)
                .ToListAsync();
        }
    }
}
