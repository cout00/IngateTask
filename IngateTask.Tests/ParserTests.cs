using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IngateTask.Core;
using IngateTask.Core.Crawler;
using IngateTask.Core.Interfaces;
using IngateTask.Core.Loggers;
using NUnit.Framework;
using IngateTask.Core.Parsers;
using IngateTask.Core.UserAgents;
using Rhino.Mocks;

namespace IngateTask.Tests
{
    [TestFixture]
    public class ParserTests
    {
        private IRequest fileStub;
        private string resursePath;
        private ILogProvider logStub;

        [SetUp]
        public void Init()
        {
            fileStub = MockRepository.GenerateStub<IRequest>();
            resursePath= Path.Combine(Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName, "Resources");
            logStub = MockRepository.GenerateStub<ILogProvider>();            
        }

        public List<string> GetFileFromDomain(string uri)
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
        public void RobotsFileTest()
        {
            string inpRightPath = Path.Combine(resursePath, "input Right.txt");
            //var test =
                //@"C:\Users\aizik\documentts\visual studio 2015\Projects\IngateTask\IngateTask.Tests\Resources\input Right.txt";
            File.OpenRead(inpRightPath);
            string inpWrongPath = Path.Combine(resursePath, "input Wrong.txt");
            fileStub.Expect(request => request.GetFileFromDomain(inpRightPath)).Return(GetFileFromDomain(inpRightPath));
            InputLocalFileParser fileParser=new InputLocalFileParser(inpRightPath, logStub);
            fileParser.GetParsedArray();
        }

        [Test]
        public void UriTest()
        {
            Uri uri1 = @"https://habrahabr.ru/company/tm/".ToUri();
            Uri uri2 = @"https://habrahabr.ru/".ToUri();
            Uri uri3 = @"https://habrahabr.ru".ToUri();
            string relPart1 = "/users/";
            string relPart2 = "./users/";
            Assert.That(uri1.GetBaseAdress(),Is.EqualTo(uri2.OriginalString));
            Assert.That(uri2.GetBaseAdress(), Is.EqualTo(uri2.OriginalString));
            Assert.That(uri3.GetBaseAdress(), Is.EqualTo(uri2.OriginalString));

            Assert.That(uri1.CombinePath(relPart1).OriginalString, Is.EqualTo(@"https://habrahabr.ru/users/"));
            Assert.That(uri2.CombinePath(relPart2).OriginalString, Is.EqualTo(@"https://habrahabr.ru/users/"));
            Assert.That(uri3.CombinePath(relPart2).OriginalString, Is.EqualTo(@"https://habrahabr.ru/users/"));
            Assert.That(uri1.CombinePath(relPart2).OriginalString, Is.EqualTo(@"https://habrahabr.ru/company/tm/users/"));
        }

        


    }
}
