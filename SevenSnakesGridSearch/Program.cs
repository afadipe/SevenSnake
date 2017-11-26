using SevenSnakesGridSearch.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace SevenSnakesGridSearch
{
    class Program
    {
        public static void Main(string[] args)
        {

            //Configuring log4net
            log4net.ILog log = LogManager.GetLogger("AppLogger");
            log.Warn("Tis is a warn message");
            log.Info("This is an Info message");

            try
            {
                var filePath = @"C:\Users\afadipe\Documents\Visual Studio 2015\Projects\AscentSevenSnake\SevenSnakesGridSearch\DocFolder\SnakeFormat.csv";
                if (!File.Exists(filePath))
                {
                    Console.WriteLine("::::: Can not find the specified file :::::");
                    return;
                }
                Tuple<SnakeHelper, SnakeHelper> showoutput;
                try
                {
                    showoutput = new GridLayout(new StreamReader(filePath)).GetMatchingPair();
                }
                catch (Exception e)
                {
                    Console.WriteLine("::::::: An error occurred processing cell value :::::::");
                    log.InfoFormat("Message:[{0}] InnerException:[{1}] Source:[{2}]", e.Message, e.InnerException, e.Source);
                    return;
                }
                if (showoutput == null)
                {
                    Console.WriteLine(":::: An Error occurred processing request  ::::");
                }
                else
                {
                    Console.WriteLine("Snake 1: " + showoutput.Item1.DisplayBoardFormatHelper());
                    Console.WriteLine("Snake 2: " + showoutput.Item2.DisplayBoardFormatHelper());
                }
            }
            catch (Exception e)
            {
                log.InfoFormat("Message:[{0}] InnerException:[{1}] Source:[{2}]", e.Message, e.InnerException, e.Source);
                Console.WriteLine("Message:[{0}] InnerException:[{1}] Source:[{2}]", e.Message, e.InnerException, e.Source);
            }
        }
    }
}
