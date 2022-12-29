using System.Net.Sockets;
using System.Net;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace server
{

    public class Ticker
    {
        public int Id { get; set; }
        public string? ticker { get; set; }

        public TodaysCondition? TodaysCondition { get; set; }
    }
    public class Prices
    {
        public int Id { get; set; }
        public double price { get; set; }
        public string date { get; set; }

        public int TickerId { get; set; }
        public Ticker Ticker { get; set; }

    }
    public class TodaysCondition
    {
        public int Id { get; set; }
        public double state { get; set; }

        public int TickerId { get; set; }
        public Ticker Ticker { get; set; }

    }
    public class ApplicationContext : DbContext
    {
        public DbSet<Ticker> Tickers { get; set; } = null!;
        public DbSet<Prices> Prices { get; set; } = null!;
        public DbSet<TodaysCondition> TodaysConditions { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=helloapp.db");
        }
    }

    internal class Program
    {
        static async Task Main(string[] args)
        {
            IPAddress localAddr = IPAddress.Parse("127.0.0.1");
            TcpListener tcpListener = new TcpListener(localAddr, 8888);

            try
            {
                tcpListener.Start();

                while (true)
                {
                    using var tcpClient = await tcpListener.AcceptTcpClientAsync();
                    var strm = tcpClient.GetStream();
                    byte[] responseData = new byte[1024];
                    int bytes = 0;

                    do
                    {
                        bytes = strm.Read(responseData, 0, responseData.Length);
                        var ticker = Encoding.UTF8.GetString(responseData, 0, bytes);

                        using (ApplicationContext data = new ApplicationContext())
                        {
                            Prices? price = data.Prices.Include(x => x.Ticker).ToList().Find(x => x.Ticker.ticker == ticker);

                            if (price != null)
                            {
                                var result = Encoding.UTF8.GetBytes(price.price.ToString());
                                strm.Write(result, 0, result.Length);
                            }
                            else
                            {
                                var result = Encoding.UTF8.GetBytes("No ticker");
                                strm.Write(result, 0, result.Length);
                            }
                        }
                    }
                    while (bytes > 0);
                }
            }
            finally
            {
                tcpListener.Stop();
            }
        }
    }
}
