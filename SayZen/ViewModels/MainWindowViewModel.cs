using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Timers;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using SayZen.Models;

namespace SayZen.ViewModels
{
    public partial class MainWindowViewModel : ViewModelBase
    {
        [ObservableProperty]
        private string quoteText;
        [ObservableProperty]
        private string quoteAuthor;

        private Timer timer;
        private Dictionary<string, Quote> _quotes;
        private Random _random;

        public MainWindowViewModel()
        {
            _random = new Random();
            GetQuote();
            SetupTimer();
        }
        private async Task GetQuote()
        {
            string fileName = "Quote.json";

            string projectRoot = Directory.GetCurrentDirectory();
            string dataFolder = Path.Combine(projectRoot, "Data");
            string filePath = Path.Combine(dataFolder, fileName);

            //// Альтернативные пути для поиска
            //if (!File.Exists(filePath))
            //{
            //    // Если запускаем из bin/Debug/netX.0/
            //    filePath = Path.Combine(projectRoot, "..", "..", "..", "Data", fileName);
            //    filePath = Path.GetFullPath(filePath);
            //}

            if (!File.Exists(filePath))
            {
                QuoteText ="Error: Quote file not found.";
                QuoteAuthor = "";
                return;
            }

            var jsonText = File.ReadAllText(filePath, Encoding.UTF8);


            var options = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };


            _quotes = JsonSerializer.Deserialize<Dictionary<string, Quote>>(jsonText, options);
            ShowRandomQuote();
        }
        private void SetupTimer()
        {
            
            timer = new Timer(30000);
            timer.Elapsed += (s, e) =>
            {
                Dispatcher.UIThread.Post(() => ShowRandomQuote());
            };
            timer.AutoReset = true;
            timer.Start();
        }

        private void ShowRandomQuote()
        {
            if (_quotes != null && _quotes.Count > 0)
            {
                var keys = _quotes.Keys.ToList();
                var randomKey = keys[_random.Next(keys.Count)];
                var quote = _quotes[randomKey];

                QuoteText = quote.Text;
                QuoteAuthor = quote.Author;
            }
        }
    }
}
