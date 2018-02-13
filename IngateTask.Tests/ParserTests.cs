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
        private RobotsParser robotsParser;
        [SetUp]
        public void Init()
        {
            robotsParser = MockRepository.GenerateMock<RobotsParser>();
        }

        [Test]
        public void RobotsFileTest()
        {
            RobotsFileStub fileStub=new RobotsFileStub();
            InputFields inputFields = new InputFields() {Domain = Path.Combine(@"D:\", "mkyong.txt"), UserAgent = "yandexbot"};
            //RobotsParser robotsParser=new RobotsParser(inputFields,new LogMessanger(), fileStub);
            //robotsParser.ParseFile();
            //var tesRes = robotsParser.GetResult();
            YandexBot yandexBot=new YandexBot();
            yandexBot.GetCrawlDelay = 2;
            //robotsParser.Expect(parser => parser.GetResult())
            //    .Return(new KeyValuePair<string, IUserAgent>("\"http://www.mkyong.com\"", yandexBot));
            KeyValuePair<string, IUserAgent> test=new KeyValuePair<string, IUserAgent>("https://msdn.microsoft.com/en-us/library/windows/desktop/ms633545(v=vs.85).aspx", yandexBot);

            Uri test1 = new Uri("/sandbox/",UriKind.Relative);
            Uri test2 = test1.Merge("https://habrahabr.ru".ToUri());


            TextReader textReader = File.OpenText(Path.Combine(@"D:\", "1.txt"));
            GrammaHttpParser parser=new GrammaHttpParser();
            parser.GetNestedUri(textReader.ReadToEnd(), "https://habrahabr.ru".ToUri());

            //Uri uri=new Uri();
            //Crawler crawler=new Crawler(test,new LogMessanger());
            //Extensions.IsSubDomain("");
            //Uri uri=new Uri("http://cdndl.zaycev.net/960403/6692165/post_malone_feat._21_savage_-_rockstar_%28zaycev.net%29.mp3");

            //crawler.CrawAsync();
        }


        //static void test1()
        //{
        //    for (int i = 0; i < 100; i++)
        //    {
        //        if (i == 40)
        //        {
        //            _logMessanger.PostStatusMessage(LogMessages.Error, "Error tr1");
        //        }
        //        _logMessanger.PostMessage("i am thread 1");
        //        Task.Delay(40);
        //    }
        //}

        //static void test2()
        //{
        //    for (int i = 0; i < 100; i++)
        //    {
        //        _logMessanger.PostMessage("i am thread 2");
        //        Task.Delay(40);
        //    }
        //}

        //static void test3()
        //{
        //    for (int i = 0; i < 100; i++)
        //    {
        //        if (i == 40)
        //        {
        //            _logMessanger.PostStatusMessage(LogMessages.Error, "Error tr3");
        //        }
        //        else
        //        {
        //            _logMessanger.PostStatusMessage(LogMessages.Warning, "i am thread 2");
        //        }
        //        Task.Delay(40);
        //    }
        //}


    }
}
