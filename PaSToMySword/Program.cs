using CommandLine;
using Common.Commentaries.Model;
using CommonTools.Commentaries.Tools;
using Figgle;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyBibleTools.Commentaries;
using MyBibleTools.Commentaries.Tools;
using MySwordTools.Commentaries;
using MySwordTools.Commentaries.Model;
using MySwordTools.Commentaries.Tools;
using NLog.Extensions.Logging;
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
        [Option('i', "input", Required = true, HelpText = "Input file raw text to be processed.")]
        public string InputFile { get; set; }

        [Option('o', "output", Required = true, HelpText = "Output file name to be generated.")]
        public string Output { get; set; }
    }

    internal class Program
    {
        private static MySwordCommentariesSaver _mySwordCommentariesExport;
        private static MyBibleCommentariesSaver _myBibleCommentariesExport;
        private static ICommentaryFormater _commentaireHtmlFormater;
        private static BibleOnlineImporter _bibleOnlineImporter;

        private static void Main(string[] args)
        {
            Console.WriteLine(FiggleFonts.Standard.Render("PaS to MySword & MyBible"), Color.Red);

            using var serviceProvider = new ServiceCollection()
                .AddSingleton<MySwordCommentariesSaver>()
                .AddSingleton<MyBibleCommentariesSaver>()
                .AddSingleton<BibleOnlineImporter>()

                .AddSingleton<MyBibleReferenceConverter>()
                .AddSingleton<MySwordReferenceConverter>()

                .AddSingleton<ICommentaryFormater<MyBibleReferenceConverter>, CommentaireHtmlFormater<MyBibleReferenceConverter>>()
                .AddSingleton<ICommentaryFormater<MySwordReferenceConverter>, CommentaireHtmlFormater<MySwordReferenceConverter>>()

                .AddLogging(config =>
                {
                    config.ClearProviders().SetMinimumLevel(LogLevel.Trace);
                    config.AddNLog();
                }
                )
                .BuildServiceProvider();

            _mySwordCommentariesExport = serviceProvider.GetService<MySwordCommentariesSaver>();
            _myBibleCommentariesExport = serviceProvider.GetService<MyBibleCommentariesSaver>();
            _commentaireHtmlFormater = serviceProvider.GetService<ICommentaryFormater<MySwordReferenceConverter>>();

            _bibleOnlineImporter = serviceProvider.GetService<BibleOnlineImporter>();

            Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed(RunOptions);
        }

        private static void RunOptions(CommandLineOptions opts)
        {
            RecueilExchange recueils = _bibleOnlineImporter.ReadFile(opts.InputFile);

            _mySwordCommentariesExport.Save(recueils.Commentaires, opts.Output);

            _myBibleCommentariesExport.Save(recueils.Commentaires, opts.Output);

            using StreamWriter file = new StreamWriter(@"output.html");

            foreach (var commentaire in recueils.Commentaires)
            {
                file.WriteLine(_commentaireHtmlFormater.ToString(commentaire));
            }
        }
    }
}