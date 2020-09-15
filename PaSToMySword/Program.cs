using CommandLine;
using Figgle;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MySwordTools.Commentaries;
using MySwordTools.Commentaries.Model;
using NLog.Extensions.Logging;
using PaSToMySword.Model.ExchangeFormat;
using PaSToMySword.Tools;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Console = Colorful.Console;

namespace PaSToMySword
{
    internal class CommandLineOptions
    {
        [Option('p', "path", Required = true, HelpText = "Input file to be processed.")]
        public string Path { get; set; }

        [Option('o', "output", Required = true, HelpText = "Output file to be generated.")]
        public string Output { get; set; }
    }

    internal class Program
    {
        static private ILogger<Program> _logger { get; set; }

        private static MySwordCommentariesExport _commentariesExport;

        private static void Main(string[] args)
        {
            Console.WriteLine(FiggleFonts.Standard.Render("PaS to MySword"), Color.Red);

            using var serviceProvider = new ServiceCollection()
                .AddSingleton<MySwordCommentariesExport>()
                .AddLogging(config =>
                {
                    config.ClearProviders().SetMinimumLevel(LogLevel.Trace);
                    config.AddNLog();
                }
                )
                .BuildServiceProvider();

            _logger = serviceProvider.GetService<ILoggerFactory>().CreateLogger<Program>();
            _commentariesExport = serviceProvider.GetService<MySwordCommentariesExport>();

            Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed(RunOptions);
        }

        private static void RunOptions(CommandLineOptions opts)
        {
            PasCommentaireToMySword(opts.Path, opts.Output);
        }

        private static RecueilExchange ReadJsonFile(string path)
        {
            _logger.LogDebug($"Read Json file {path}");

            using StreamReader streamReader = new StreamReader(path);
            string jsonContent = streamReader.ReadToEnd();
            RecueilExchange recueilExchange = JsonSerializer.Deserialize<RecueilExchange>(jsonContent, new JsonSerializerOptions() { PropertyNameCaseInsensitive = true, Converters = { new JsonStringEnumConverter() } });

            _logger.LogInformation($"Found {recueilExchange.Recueils.Count()} Recueils.");
            _logger.LogInformation($"Found {recueilExchange.Commentaires.Count()} Commentaires.");

            return recueilExchange;
        }

        private static void PasCommentaireToMySword(string inputJsonFile, string outputDbName)
        {
            RecueilExchange recueils = ReadJsonFile(inputJsonFile);

            _commentariesExport.SaveToDb(CommentairesToMySwordCommentaries(
                recueils.Commentaires), 
                new Details
                {
                    Abbreviation = "PaS",
                    Autor = "Plaire au Seigneur",
                    Comments = "Commentaire",
                    Description = "Recueil de commentaire biblique",
                    PublishDate = "2020",
                    Title = "Plaire au Seigneur",
                    Version = "0.1.0",
                    VersionDate = DateTime.UtcNow
                }, 
                outputDbName);
        }

        private static IEnumerable<Commentary> CommentairesToMySwordCommentaries(IEnumerable<Commentaire> commentaires)
        {
            List<Commentary> commentaries = new List<Commentary>();
            foreach (Commentaire commentaire in commentaires)
            {
                Commentary commentary = CommentaireToMySwordCommentary(commentaire);
                if (commentary is object)
                    commentaries.Add(commentary);
            }

            return commentaries;
        }

        private static Commentary CommentaireToMySwordCommentary(Commentaire commentaire)
        {
            Reference reference = ReferenceConverter.ConvertReference(commentaire.Reference);
            int bookIndex = ReferenceConverter.BookNumberFromAbbreviation(reference.Book);

            if (bookIndex >= 0)
            {
                return new Commentary
                {
                    Book = bookIndex,
                    Chapter = reference.Chapter,
                    FromVerse = reference.FromVerse,
                    ToVerse = reference.ToVerse,
                    Content = CommentaireFormatter.ToHtml(commentaire)
                };
            }
            else
            {
                _logger.LogError($"book {reference.Book} not found in BookList");
            }
            return null;
        }
    }
}