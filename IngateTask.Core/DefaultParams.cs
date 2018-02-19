﻿using System.Collections.Generic;

namespace IngateTask.Core
{
    public static class DefaultParams
    {
        /// <summary>
        /// задержка подефолту
        /// </summary>
        public static int DefaulDelay = 100;

        /// <summary>
        /// список всех тестовых миме типов
        /// </summary>
        public static List<string> MEMETextList = new List<string>
        {
            "text/html",
            "text/plain",
            "text/php",
            "text/xml",
            "message/http"
        };

        /// <summary>
        ///  все типы которые самые распространенные их можно отсеч заранее
        /// </summary>
        public static List<string> MemeNonTextList = new List<string>
        {
            "image/gif",
            "image/jpeg",
            "image/pjpeg",
            "image/svg+xml",
            "image/tiff",
            "image/vnd.microsoft.icon",
            "image/vnd.wap.wbmp",
            "image/webp",
            "video/mpeg",
            "video/mp4",
            "video/ogg",
            "video/quicktime",
            "video/webm",
            "video/x-ms-wmv",
            "video/x-flv",
            "video/3gpp",
            "video/3gpp2",
            "audio/basic",
            "audio/mp4",
            "audio/aac",
            "audio/mpeg",
            "audio/ogg",
            "image/png"
        };
    }
}