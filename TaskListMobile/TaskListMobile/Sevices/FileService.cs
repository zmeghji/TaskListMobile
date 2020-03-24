using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Essentials;

namespace TaskListMobile.Sevices
{
    public class FileService
    {
        public const string DbFileName = "TaskList.db";

        public static string GetDbFilePath()
        {
            return Path.Combine(FileSystem.AppDataDirectory, DbFileName);
        }
    }
}
