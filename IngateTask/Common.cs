using System;

namespace IngateTask.Local
{
    public static class Common
    {
        public static void ShowHelp()
        {
            Console.WriteLine("-input input file with domens. structure see below");
            Console.WriteLine("-output output directory with logs and results");
            Console.WriteLine("-thread_count numbers of threads");
            Console.WriteLine("___________________________________________________");
            Console.WriteLine("input file looks like:");
            Console.WriteLine("<line>::=<domain><user_agent>|<crawl-delay>");
            Console.WriteLine("example:");
            Console.WriteLine(
                "https://stackoverflow.com yandex\r\nhttp://theory.phphtml.net google\r\nhttp://www.mkyong.com yandex\r\nhttp://2coders.ru 300\r\nhttps://habrahabr.ru google");
        }
    }
}