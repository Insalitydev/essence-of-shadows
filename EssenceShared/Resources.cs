using CocosSharp;

namespace EssenceShared {
    public class Resources {
        /** Добавляет в пути поиска ресурсов необходимые папки */

        public static void LoadContent(CCApplication application) {
            Log.Print("Loading resources");
            application.ContentRootDirectory = "Resource";
            application.ContentSearchPaths.Add("Font");
            application.ContentSearchPaths.Add("Icon");
            application.ContentSearchPaths.Add("Image");
            application.ContentSearchPaths.Add("Image/Boss");
            application.ContentSearchPaths.Add("Image/Class");
            application.ContentSearchPaths.Add("Image/Effects");
            application.ContentSearchPaths.Add("Image/GUI");
            application.ContentSearchPaths.Add("Image/GUI/Menu");
            application.ContentSearchPaths.Add("Image/Item");
            application.ContentSearchPaths.Add("Image/Projectile");
            application.ContentSearchPaths.Add("Image/Tile");
            application.ContentSearchPaths.Add("Image/Tile/Cave");
            application.ContentSearchPaths.Add("Image/Tile/City");
            application.ContentSearchPaths.Add("Image/Tile/Desert");
            application.ContentSearchPaths.Add("Music");
            application.ContentSearchPaths.Add("Sound");
        }
    }
}