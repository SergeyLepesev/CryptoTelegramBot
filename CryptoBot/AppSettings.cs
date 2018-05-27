namespace CryptoBot
{
    public class AppSettings
    {
        public DbSettings Db { get; set; }
    }

    public class DbSettings
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
    }
}