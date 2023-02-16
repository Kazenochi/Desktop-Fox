using Newtonsoft.Json;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Formatting = Newtonsoft.Json.Formatting;

namespace DesktopFox
{
    internal class DF_Json
    {
        /// <summary>
        /// Basisverzeichnis in dem sich die Dateien der Applikation befinden.
        /// </summary>
        public static string BaseDir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        /// <summary>
        /// Speichert die Gallery oder Settings als JSON Datei ab
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool saveFile(object obj, bool logging = false)
        {
            if (!Directory.Exists(BaseDir + "\\Saves"))
                Directory.CreateDirectory(BaseDir + "\\Saves");

            var option = Formatting.Indented;
            String saveFolder = BaseDir + "\\Saves\\";
            var json = JsonConvert.SerializeObject(obj, option);


            if (obj.GetType() == typeof(Gallery))
            {
                try
                {                
                    File.WriteAllText(saveFolder + "DF_Gallery.json", json);

                    if (logging)
                    {
                        File.WriteAllText(saveFolder + "DF_Gallery" + DateTime.Now.ToString() + ".json", json);
                    }                   

                    return true;
                }
                catch
                {
                    Debug.WriteLine("Save Error: Gallery");

                    if(logging)
                    {
                        if(!File.Exists(BaseDir + "\\Saves\\Error.log"))
                        {
                            File.CreateText("\\Saves\\Error.log");
                        }
                        using StreamWriter file = new("\\Saves\\Error.log", append: true);
                        file.WriteLineAsync(DateTime.Now.ToString() + ":\t Error Writing Gallery.");
                        file.Close();
                    }

                    return false;
                }
            }
            else if (obj.GetType() == typeof(Settings))
            {
                try
                {
                    File.WriteAllText(saveFolder + "Settings.json", json);
                    if (logging)
                    {
                        File.WriteAllText(saveFolder + "Settings" + DateTime.Now.ToString() + ".json", json);
                    }
                    return true;
                }
                catch
                {
                    Debug.WriteLine("Save Error: Settings");
                    if (logging)
                    {
                        if (!File.Exists(BaseDir + "\\Saves\\Error.log"))
                        {
                            File.CreateText("\\Saves\\Error.log");
                        }
                        using StreamWriter file = new("\\Saves\\Error.log", append: true);
                        file.WriteLineAsync(DateTime.Now.ToString() + ":\t Error Writing Settings.");
                        file.Close();
                    }
                    return false;
                }
            }
            else if (obj.GetType() == typeof(WallpaperSaves))
            {
                try
                {
                    File.WriteAllText(saveFolder + "Wallpapers.json", json);
                    return true;
                }
                catch
                {
                    Debug.WriteLine("Save Error: Wallpapers");
                    return false;
                }
            }
            else
            {
                Debug.WriteLine("Save Error: Das Angegebene Objekt besitzt keinen gültigen Typ");
                return false;
            }
        }

        /// <summary>
        /// Läd den Angegebenen Dateityp aus der Gespeicherten JSON Datei
        /// </summary>
        /// <param name="Type">"gallery" = Laden der Galerie; "settings" = Laden der Einstellungen</param>
        /// <returns>"Null" falls ein Fehler beim laden der Dateien stattfindet</returns>
        public static object loadFile(SaveFileType Type)
        {
            if (!Directory.Exists(BaseDir + "\\Saves")) return null;              

            try
            {
                switch (Type)
                {
                    case SaveFileType.Gallery:
                        using (StreamReader reader = new StreamReader(BaseDir + "\\Saves\\DF_Gallery.json"))
                        {
                            String json = reader.ReadToEnd();
                            var gal = JsonConvert.DeserializeObject<Gallery>(json);

                            //Wird benötigt um das Padding der Datei zu entfernen. Note: KP warum es nötig ist. Beim laden der datei sollte er keine extra Elemente in der Liste haben
                            if (gal.activeSetsList.Count > 3)
                            {
                                for (int i = 0; i < 3; i++)
                                {
                                    gal.activeSetsList.Remove(gal.activeSetsList.ElementAt(0));
                                }
                            }
                            return gal;
                        }
                       
                    case SaveFileType.Settings:
                        using (StreamReader reader = new StreamReader(BaseDir + "\\Saves\\Settings.json"))
                        {
                            String json = reader.ReadToEnd();
                            var settings = JsonConvert.DeserializeObject<Settings>(json);
                            return settings;
                        }             

                    case SaveFileType.Wallpaper:
                        using (StreamReader reader = new StreamReader(BaseDir + "\\Saves\\Wallpapers.json"))
                        {
                            String json = reader.ReadToEnd();
                            WallpaperSaves wallpapers = JsonConvert.DeserializeObject<WallpaperSaves>(json);
                            return wallpapers;
                        }
                    default:
                        Debug.WriteLine("Fehler bei der Auswahl der zu ladenden Datei");
                        break;
                }
                return null;
            }
            catch (System.IO.FileNotFoundException)
            {
                Console.WriteLine("Fehler beim Lesen der Datei!!!");
                return null;
            }

        }

        /// <summary>
        /// Löscht den angegebenen <see cref="SaveFileType"/> aus dem lokalen Verzeichniss
        /// </summary>
        /// <param name="Type"></param>
        public static void deleteFile(SaveFileType Type)
        {
            if (!Directory.Exists(BaseDir + "\\Saves")) return;

            try
            {
                switch (Type)
                {
                    case SaveFileType.Gallery:
                        if (File.Exists(BaseDir + "\\Saves\\DF_Gallery.json"))
                            File.Delete(BaseDir + "\\Saves\\DF_Gallery.json");
                        break;


                    case SaveFileType.Settings:
                        if (File.Exists(BaseDir + "\\Saves\\DF_Settings.json"))
                            File.Delete(BaseDir + "\\Saves\\DF_Settings.json");
                        break;

                    case SaveFileType.Wallpaper:
                        if (File.Exists(BaseDir + "\\Saves\\Wallpapers.json"))
                            File.Delete(BaseDir + "\\Saves\\Wallpapers.json");
                        break;
                    default:
                        Debug.WriteLine("Fehler bei der Auswahl der zu löschenden Datei");
                        break;
                }
                Debug.WriteLine("Löschen erfolgreich");
                return;
            }
            catch (System.IO.FileNotFoundException)
            {
                Console.WriteLine("Fehler beim Löschen der Datei!!!");
                return;
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

        /// <summary>
        /// Kopiert ein Collection object und gibt die Kopie zurück
        /// </summary>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static Wallpaper objectCopy(Wallpaper wallpaper)
        {
            if (wallpaper != null)
            {
                var option = Formatting.Indented;
                var json = JsonConvert.SerializeObject(wallpaper, option);
                return JsonConvert.DeserializeObject<Wallpaper>(json);
            }
            return null;
        }
    }
}