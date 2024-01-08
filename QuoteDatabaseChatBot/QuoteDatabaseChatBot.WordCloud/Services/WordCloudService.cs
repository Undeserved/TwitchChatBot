using KnowledgePicker.WordCloud;
using KnowledgePicker.WordCloud.Coloring;
using KnowledgePicker.WordCloud.Drawing;
using KnowledgePicker.WordCloud.Layouts;
using KnowledgePicker.WordCloud.Primitives;
using KnowledgePicker.WordCloud.Sizers;
using MediatR;
using Newtonsoft.Json;
using QuoteDatabaseChatBot.Application.Common.Dtos;
using QuoteDatabaseChatBot.Application.Quotes.Queries;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QuoteDatabaseChatBot.WordCloud.Services {
    internal class WordCloudService {
        private IMediator _mediator;
        private List<string> _stopWords;
        private char[] _punctuationMarkers = { ',', ';', ':', '!', '?', '.', '"' };

        public WordCloudService(IMediator mediator) {
            _mediator = mediator;
            _stopWords = new List<string> { "a", "able", "about", "across", "after", "all", "almost", "also", "am", "among", "an",
                                            "and", "any", "are", "as", "at", "be", "because", "been", "but", "by", "can", "cannot",
                                            "could", "did", "do", "does", "either", "else", "ever", "every", "for", "from", "get",
                                            "got", "had", "has", "have", "he", "her", "hers", "him", "his", "how", "however", "i",
                                            "if", "in", "into", "is", "it", "its", "just", "least", "let", "like", "likely", "may",
                                            "me", "might", "most", "must", "my", "neither", "no", "nor", "not", "of", "off", "often",
                                            "on", "only", "or", "other", "our", "own", "rather", "said", "say", "says", "she", "should",
                                            "since", "so", "some", "than", "that", "the", "their", "them", "then", "there", "these",
                                            "they", "this", "tis", "to", "too", "twas", "us", "wants", "was", "we", "were", "what",
                                            "when", "where", "which", "while", "who", "whom", "why", "will", "with", "would", "yet",
                                            "you", "your", "i", "i'm", "don't", "it's", "he's", "that's", "i'll"};
        }

        public async void ExportWordCloud(DateTime from, DateTime to) {
            GetQuoteQuery getQuoteQuery = new GetQuoteQuery { From = from, To = to };
            var quotes = await _mediator.Send(getQuoteQuery);
            IEnumerable<WordCloudEntry> wordFrequency = CalcWordFrequency(quotes);
            string wordFreqRawData = JsonConvert.SerializeObject(wordFrequency.OrderByDescending(x => x.Count));

            WordCloudInput wordCloud = new WordCloudInput(wordFrequency) {
                Width = 1024,
                Height = 256,
                MinFontSize = 16,
                MaxFontSize = 64
            };

            //IEnumerable<WordCloudEntry> uniqueWordFrequency = CalcUniqueQuoteFrequency(quotes);
            //string uniqueWordFreqRawData = JsonConvert.SerializeObject(uniqueWordFrequency.OrderByDescending(x => x.Count));

            var sizer = new LogSizer(wordCloud);
            using var engine = new SkGraphicEngine(sizer, wordCloud);
            var layout = new SpiralLayout(wordCloud);
            var colorizer = new RandomColorizer(); // optional
            var wcg = new WordCloudGenerator<SKBitmap>(wordCloud, engine, layout, colorizer);
            IEnumerable<(LayoutItem Item, double FontSize)> items = wcg.Arrange();

            using var final = new SKBitmap(wordCloud.Width, wordCloud.Height);
            using var canvas = new SKCanvas(final);

            // Draw on white background.
            canvas.Clear(SKColors.White);
            using var bitmap = wcg.Draw();
            canvas.DrawBitmap(bitmap, 0, 0);

            // Save to PNG.
            using var data = final.Encode(SKEncodedImageFormat.Png, 100);
            using var writer = File.Create("BoxFortWordCloud.png");
            data.SaveTo(writer);
        }

        private IEnumerable<WordCloudEntry> CalcWordFrequency(IEnumerable<QuoteDto> quotes) {
            Dictionary<string, int> wordFrequency = new Dictionary<string, int>();

            foreach (var quote in quotes) {
                string[] words = quote.QuoteContent.Split(' ');
                foreach (string word in words) {
                    string normalisedWord = removePunctuation(word.ToLower());
                    if (!_stopWords.Contains(normalisedWord)) {
                        if (wordFrequency.ContainsKey(normalisedWord)) {
                            wordFrequency[normalisedWord]++;
                        } else {
                            wordFrequency.Add(normalisedWord, 1);
                        }
                    }
                }
            }
            return wordFrequency.Where(x => x.Value > 3)
                .Select(x => new WordCloudEntry(x.Key, x.Value));
        }

        private IEnumerable<WordCloudEntry> CalcUniqueQuoteFrequency(IEnumerable<QuoteDto> quotes) {
            Dictionary<string, int> wordFrequency = new Dictionary<string, int>();

            foreach (var quote in quotes) {
                string[] words = quote.QuoteContent.Split(' ');
                words = words.Distinct().ToArray();
                foreach (string word in words) {
                    string normalisedWord = removePunctuation(word.ToLower());
                    if (!_stopWords.Contains(normalisedWord)) {
                        if (wordFrequency.ContainsKey(normalisedWord)) {
                            wordFrequency[normalisedWord]++;
                        } else {
                            wordFrequency.Add(normalisedWord, 1);
                        }
                    }
                }
            }
            return wordFrequency.Where(x => x.Value > 3)
                .Select(x => new WordCloudEntry(x.Key, x.Value));
        }

        private string removePunctuation(string word) {
            string pattern = "[" + Regex.Escape(new string(_punctuationMarkers)) + "]";
            return Regex.Replace(word, pattern, string.Empty);
        }
    }
}
