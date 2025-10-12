using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using UnityEngine;

namespace HornetShermaSong
{
    internal class ModHelper
    {
        public static void Log(string msg)
        {
            HornetShermaSongPlugin.logSource.LogInfo(msg);
        }

        public static void LogError(string msg)
        {
            HornetShermaSongPlugin.logSource.LogError(msg);
        }

        public static AssetBundle LoadBundleFromAssembly(string resourceName)
        {
            var asm = Assembly.GetExecutingAssembly();

            using (Stream stream = asm.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    LogError("Couldnt load resoruce lol");
                    return null;
                }

                byte[] data = new byte[stream.Length];
                stream.Read(data, 0, data.Length);

                AssetBundle bundle = AssetBundle.LoadFromMemory(data);
                return bundle;
            }
        }

        public static Texture2D LoadTexFromAssembly(string resourceName)
        {
            var asm = Assembly.GetExecutingAssembly();

            using (Stream stream = asm.GetManifestResourceStream(resourceName))
            {
                if (stream == null)
                {
                    LogError("Couldnt load resoruce lol");
                    return null;
                }

                byte[] data = new byte[stream.Length];
                stream.Read(data, 0, data.Length);

                Texture2D tex = new(2, 2, TextureFormat.RG32, false);
                tex.LoadImage(data);
                return tex;
            }
        }
    }
}
