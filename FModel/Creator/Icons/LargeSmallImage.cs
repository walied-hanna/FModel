﻿using PakReader.Pak;
using PakReader.Parsers.Class;
using PakReader.Parsers.PropertyTagData;
using SkiaSharp;

namespace FModel.Creator.Icons
{
    static class LargeSmallImage
    {
        public static void GetPreviewImage(BaseIcon icon, StructProperty u)
        {
            if (u.Value is UObject o && o.TryGetValue("ResourceObject", out var v) && v is ObjectProperty resourceObject)
                icon.IconImage = Utils.GetObjectTexture(resourceObject);
        }
        public static void GetPreviewImage(BaseIcon icon, ObjectProperty o, string assetName) => GetPreviewImage(icon, o, assetName, true);
        public static void GetPreviewImage(BaseIcon icon, ObjectProperty o, string assetName, bool hightRes)
        {
            string path = o.Value.Resource.OuterIndex.Resource.ObjectName.String;
            if (path.Equals("/Game/Athena/Items/Weapons/WID_Harvest_Pickaxe_STWCosmetic_Tier"))
                path += "_" + assetName.Substring(assetName.LastIndexOf(".") - 1, 1);

            PakPackage p = Utils.GetPropertyPakPackage(path);
            if (p.HasExport() && !p.Equals(default))
            {
                if (GetPreviewImage(icon, p.GetIndexedExport<UObject>(0), hightRes))
                    return;
                else if (GetPreviewImage(icon, p.GetIndexedExport<UObject>(1), hightRes)) // FortniteGame/Content/Athena/Items/Cosmetics/Pickaxes/Pickaxe_ID_402_BlackKnightFemale1H.uasset
                    return;
            }
        }
        public static void GetPreviewImage(BaseIcon icon, SoftObjectProperty s) => icon.IconImage = Utils.GetSoftObjectTexture(s);

        private static bool GetPreviewImage(BaseIcon icon, UObject obj, bool hightRes)
        {
            if (obj != null)
            {
                if (hightRes && obj.TryGetValue("LargePreviewImage", out var sLarge) && sLarge is SoftObjectProperty largePreviewImage)
                {
                    GetPreviewImage(icon, largePreviewImage);
                    return true;
                }
                else if (obj.TryGetValue("SmallPreviewImage", out var sSmall) && sSmall is SoftObjectProperty smallPreviewImage)
                {
                    GetPreviewImage(icon, smallPreviewImage);
                    return true;
                }
            }
            return false;
        }

        public static void DrawPreviewImage(SKCanvas c, BaseIcon icon) =>
            c.DrawBitmap(icon.IconImage ?? icon.FallbackImage, new SKRect(icon.Margin, icon.Margin, icon.Size - icon.Margin, icon.Size - icon.Margin),
                            new SKPaint { FilterQuality = SKFilterQuality.High, IsAntialias = true });
    }
}
