using DesktopFox;
using Newtonsoft.Json;
using System;
using System.IO;
using Formatting = Newtonsoft.Json.Formatting;

namespace DesktopFox
{
    internal class DF_Json
    {

        public static string BaseDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        /// <summary>
        /// Speichert die Gallery oder Settings als JSON Datei ab
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool saveFile(object obj)
        {
            var option = Formatting.Indented;
            String saveFolder = BaseDir + "\\Saves\\";
            if (obj.GetType() == typeof(Gallery))
            {
                try
                {
                    var json = JsonConvert.SerializeObject(obj, option);
                    File.WriteAllText(saveFolder + "DF_Gallery.json", json);
                    return true;
                }
                catch
                {
                    Console.WriteLine("Save Error: Gallery");
                    return false;
                }
            }
            else if (obj.GetType() == typeof(Settings))
            {
                try
                {
                    var json = JsonConvert.SerializeObject(obj, option);
                    File.WriteAllText(saveFolder + "Settings.json", json);
                    return true;
                }
                catch
                {
                    Console.WriteLine("Save Error: Settings");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Save Error: Das Angegebene Objekt besitzt keinen gültigen Typ");
                return false;
            }
        }

        /// <summary>
        /// Läd die Gallery aus der JSON Datei
        /// </summary>
        /// <returns></returns>
        public static Gallery loadGallery()
        {
            if (!Directory.Exists(BaseDir + "\\Saves")) 
                Directory.CreateDirectory(BaseDir + "\\Saves");

            try
            {
                using (StreamReader reader = new StreamReader(BaseDir + "\\Saves\\DF_Gallery.json"))
                {
                    String json = reader.ReadToEnd();
                    var gal = JsonConvert.DeserializeObject<Gallery>(json);
                    return gal;
                }
            }
            catch (System.IO.FileNotFoundException)
            {
                Console.WriteLine("Fehler beim Lesen der Datei!!!");
                return null;
            }
        }

        /// <summary>
        /// Läd die Settings aus der JSON Datei
        /// </summary>
        /// <returns></returns>
        public static Settings loadSettings()
        {
            if (!Directory.Exists(BaseDir + "\\Saves"))
                Directory.CreateDirectory(BaseDir + "\\Saves");

            try
            {
                using (StreamReader reader = new StreamReader(BaseDir + "\\Saves\\Settings.json"))
                {
                    String json = reader.ReadToEnd();
                    var settings = JsonConvert.DeserializeObject<Settings>(json);
                    return settings;
                }
            }
            catch (System.IO.FileNotFoundException)
            {
                Console.WriteLine("Fehler beim Lesen der Datei!!!");
                return null;
            }
        }

        /// <summary>
        /// Kopiert ein Collection object und gibt die Kopie zurück
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static Collection objectCopy(Collection collection)
        {
            if (collection != null)
            {
                var option = Formatting.Indented;
                var json = JsonConvert.SerializeObject(collection, option);
                return JsonConvert.DeserializeObject<Collection>(json);
            }
            return null;
        }
    }
}