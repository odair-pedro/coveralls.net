﻿using System.Collections.Generic;
using Coveralls.Net;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;

namespace Coveralls.Tests
{
    public class CommandLineOptionsTests
    {
        [Test]
        public void CommandLineOptions_UseOpenCoverSpecified_SetsParserToOpenCover()
        {
            var opts = new CommandLineOptions();
            opts.UseOpenCover = true;
            opts.Parser.Should().Be(ParserType.OpenCover);
        }

        [Test]
        public void CommandLineOptions_UseCoberturaSpecified_SetsParserToCobertura()
        {
            var opts = new CommandLineOptions();
            opts.UseCobertura = true;
            opts.Parser.Should().Be(ParserType.Cobertura);
        }

        [Test]
        public void CommandLineOptions_UseAutoDetectSpecified_SetsParserToAutoDetect()
        {
            var opts = new CommandLineOptions();
            opts.UseAutoDetect = true;
            opts.Parser.Should().Be(ParserType.AutoDetect);
        }

        [Test]
        public void CommandLineOptions_NoInputFileParameter_HasEmptyFileList()
        {
            var fileSystem = Substitute.For<IFileSystem>();
            var opts = new CommandLineOptions(fileSystem);
            opts.InputFile = "";

            opts.InputFiles.Should().BeEmpty();
        }

        [Test]
        public void CommandLineOptions_DirectoryNoSearchPattern_ReturnsNoFiles()
        {
            var fileSystem = Substitute.For<IFileSystem>();
            fileSystem.DirectoryExists("").ReturnsForAnyArgs(true);

            var opts = new CommandLineOptions(fileSystem);
            opts.InputFile = "./";

            opts.InputFiles.Should().BeEmpty();
        }

        [Test]
        public void CommandLineOptions_DirectoryAndSearchPattern_ReturnsMatchingFiles()
        {
            var fileSystem = Substitute.For<IFileSystem>();
            fileSystem.DirectoryExists("").ReturnsForAnyArgs(true);
            fileSystem.FileSearch("./", "*.xml", false).Returns(new List<string>
            {
                @"C:\src\test\coverage.xml"
            });

            var opts = new CommandLineOptions(fileSystem);
            opts.FileSearchPattern = "*.xml";
            opts.InputFile = "./";

            opts.InputFiles.Should().HaveCount(1);
        }
    }
}
