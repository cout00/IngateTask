using System;
using IngateTask.Core;
using IngateTask.Core.Parsers;
using NUnit.Framework;
using Rhino.Mocks;
using System.IO;
using IngateTask.Core.Interfaces;
using System.Collections.Generic;
using IngateTask.PortableLibrary.Classes;
using IngateTask.PortableLibrary.Interfaces;
using IngateTask.PortableLibrary.UserAgents;

//using IngateTask.Core.UserAgents;

namespace IngateTask.Tests
{
    [TestFixture]
    public class ParserTests
    {
        private string resursePath;
        private IRequest robotsParserStub;
        ILogProvider logStub;

        [SetUp]
        public void Init()
        {
            robotsParserStub = MockRepository.GenerateStub<IRequest>();
            resursePath = Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName, "Resources");
            logStub = MockRepository.GenerateStub<ILogProvider>();
        }


        List<string> Load(string uri)
        {
            var list = new List<string>();
            using (var stream = new StreamReader(File.OpenRead(uri)))
            {
                foreach (var str in stream.ReadToEnd().Split('\r', '\n'))
                    list.Add(str.ToLower());
            }
            return list;
        }

        [Test]
        public void InputFileIsValidTest()
        {
            var path = Path.Combine(resursePath, "input right.txt");
            InputLocalFileParser fileParser=new InputLocalFileParser(path,logStub);
            var res = fileParser.GetParsedArray();
            Assert.That(res.Count==6);
            Assert.That(fileParser.FileIsValid);
            CollectionAssert.Contains(res,new InputFields() {Domain = "http://theory.phphtml.net/", UserAgent = "googlebot"});
        }

        [Test]
        public void InputFileNotValidTest()
        {
            var path = Path.Combine(resursePath, "input wrong.txt");
            InputLocalFileParser fileParser = new InputLocalFileParser(path, logStub);
            var res = fileParser.GetParsedArray();
            Assert.That(res.Count == 5);
            Assert.That(!fileParser.FileIsValid);
            CollectionAssert.Contains(res, new InputFields() { Domain = "http://2coders.ru/", UserAgent = 300 });
        }

        [Test]
        public void RobotsFileTestYandexBot()
        {
            InputFields input = new InputFields() {UserAgent = "yandexbot", Domain = "domain"};
            var path = Path.Combine(resursePath, "habrahabr robots file.txt");
            robotsParserStub.Expect(request => request.GetFileFromDomain("domain")).Return(Load(path));
            RobotsParser robotsParser = new RobotsParser(input, logStub, robotsParserStub);
            robotsParser.ParseFile();
            var res= robotsParser.GetResult();
            Assert.That(res.Value.GetType()==typeof(YandexBot));
            Assert.That(res.Value.GetCrawlDelay==10000);
        }
        [Test]
        public void RobotsFileTestCustom()
        {
            InputFields input = new InputFields() { UserAgent = 700, Domain = "domain" };
            var path = Path.Combine(resursePath, "2coders robots file.txt");
            robotsParserStub.Expect(request => request.GetFileFromDomain("domain")).Return(Load(path));
            RobotsParser robotsParser = new RobotsParser(input, logStub, robotsParserStub);
            robotsParser.ParseFile();
            var res = robotsParser.GetResult();
            Assert.That(res.Value.GetType() == typeof(CustomAgent));
            Assert.That(res.Value.GetCrawlDelay == 700);
        }


        [Test]
        public void UriTest()
        {
            Uri uri1 = @"https://habrahabr.ru/company/tm/".ToUri();
            Uri uri2 = @"https://habrahabr.ru/".ToUri();
            Uri uri3 = @"https://habrahabr.ru".ToUri();
            string relPart1 = "/users/";
            string relPart2 = "./users/";
            Assert.That(uri1.GetBaseAdress(), Is.EqualTo(uri2.OriginalString));
            Assert.That(uri2.GetBaseAdress(), Is.EqualTo(uri2.OriginalString));
            Assert.That(uri3.GetBaseAdress(), Is.EqualTo(uri2.OriginalString));

            Assert.That(uri1.CombinePath(relPart1).OriginalString, Is.EqualTo(@"https://habrahabr.ru/users"));
            Assert.That(uri2.CombinePath(relPart2).OriginalString, Is.EqualTo(@"https://habrahabr.ru/users"));
            Assert.That(uri3.CombinePath(relPart2).OriginalString, Is.EqualTo(@"https://habrahabr.ru/users"));
            Assert.That(uri1.CombinePath(relPart2).OriginalString,
                Is.EqualTo(@"https://habrahabr.ru/company/tm/users"));
        }
    }
}