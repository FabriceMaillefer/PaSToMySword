﻿using BU3Tools.Commentaries.Tools;
using CommandLine;
using Common.Commentaries.Model;
using CommonTools.Commentaries.Tools;
using Figgle;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MyBibleTools.Commentaries;
using MyBibleTools.Commentaries.Tools;
using MySwordTools.Commentaries;
using MySwordTools.Commentaries.Tools;
using NLog.Extensions.Logging;
using OSISTools.Commentaries.Tools;
using PaSToMySword.Tools;
using System.Drawing;
using System.IO;
using Console = Colorful.Console;

namespace PaSToMySword
{
    internal class CommandLineOptions
    {
        #region Properties

        [Option('i', "input", Required = true, HelpText = "Input file raw text to be processed.")]
        public string InputFile { get; set; }

        [Option('o', "output", Required = true, HelpText = "Output file name to be generated.")]
        public string Output { get; set; }

        #endregion Properties
    }

    internal class Program
    {
        #region Fields

        private static MySwordCommentariesSaver _mySwordCommentariesExport;
        private static MyBibleCommentariesSaver _myBibleCommentariesExport;
        private static OSISCommentariesSaver _osisCommentariesExport;
        private static BU3CommentariesSaver _BU3CommentariesSaver;
        private static ICommentaryFormater _commentaireHtmlFormater;
        private static BibleOnlineImporter _bibleOnlineImporter;

        #endregion Fields

        #region Methods

        private static void Main(string[] args)
        {
            Console.WriteLine(FiggleFonts.Standard.Render("PaS to MySword & MyBible"), Color.Red);

            using var serviceProvider = new ServiceCollection()
                .AddSingleton<MySwordCommentariesSaver>()
                .AddSingleton<MyBibleCommentariesSaver>()
                .AddSingleton<OSISCommentariesSaver>()
                .AddSingleton<BU3CommentariesSaver>()

                .AddSingleton<BibleOnlineImporter>()

                .AddSingleton<MyBibleReferenceConverter>()
                .AddSingleton<MySwordReferenceConverter>()
                .AddSingleton<OSISReferenceConverter>()
                .AddSingleton<BU3ReferenceConverter>()


                .AddSingleton<ICommentaryFormater<MyBibleReferenceConverter>, CommentaireHtmlFormater<MyBibleReferenceConverter>>()
                .AddSingleton<ICommentaryFormater<MySwordReferenceConverter>, CommentaireHtmlFormater<MySwordReferenceConverter>>()
                .AddSingleton<OSISFormater>()
                .AddSingleton<BU3Formater>()

                .AddLogging(config =>
                {
                    config.ClearProviders().SetMinimumLevel(LogLevel.Trace);
                    config.AddNLog();
                }
                )
                .BuildServiceProvider();

            _mySwordCommentariesExport = serviceProvider.GetService<MySwordCommentariesSaver>();
            _myBibleCommentariesExport = serviceProvider.GetService<MyBibleCommentariesSaver>();
            _osisCommentariesExport = serviceProvider.GetService<OSISCommentariesSaver>();
            _BU3CommentariesSaver = serviceProvider.GetService<BU3CommentariesSaver>();

            _commentaireHtmlFormater = serviceProvider.GetService<ICommentaryFormater<MySwordReferenceConverter>>();

            _bibleOnlineImporter = serviceProvider.GetService<BibleOnlineImporter>();

            Parser.Default.ParseArguments<CommandLineOptions>(args)
                .WithParsed(RunOptions);
        }

        private static void RunOptions(CommandLineOptions opts)
        {
            RecueilExchange recueils = _bibleOnlineImporter.ReadFile(opts.InputFile);

            //_mySwordCommentariesExport.Save(recueils.Commentaires, opts.Output);
            //_myBibleCommentariesExport.Save(recueils.Commentaires, opts.Output);
            //_osisCommentariesExport.Save(recueils.Commentaires, opts.Output);
            _BU3CommentariesSaver.Save(recueils.Commentaires, opts.Output);

            using StreamWriter file = new StreamWriter(@"output.html");

            foreach (var commentaire in recueils.Commentaires)
            {
                file.WriteLine(_commentaireHtmlFormater.ToString(commentaire));
            }

            Console.WriteLine(FiggleFonts.Swan.Render("Success !"), Color.Green);
        }

        #endregion Methods
    }
}