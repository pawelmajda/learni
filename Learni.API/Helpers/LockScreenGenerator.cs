using Learni.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Drawing;
using Nancy;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace Learni.API.Helpers
{
    public class LockScreenGenerator
    {
        private readonly IRootPathProvider _pathProvider;
        private Font _nameFont;
        private Font _definitionFont;

        public LockScreenGenerator(IRootPathProvider pathProvider)
        {
            _pathProvider = pathProvider;

            var fontCollection = new PrivateFontCollection();
            fontCollection.AddFontFile(Path.Combine(_pathProvider.GetRootPath(), "Content", "Fonts", "SegoeWP-Semilight.ttf"));

            _nameFont = new Font((FontFamily)fontCollection.Families[0], 42, FontStyle.Regular);
            _definitionFont = new Font((FontFamily)fontCollection.Families[0], 32, FontStyle.Regular);
        }

        public void GenerateLockScreen(Term term, string lockScreenTemplateName = "LockScreen_2.jpg")
        {
            InstalledFontCollection installedFontCollection = new InstalledFontCollection();

            var lockScreenTemplatePath = String.IsNullOrEmpty(term.ImagePath) 
                ? Path.Combine(_pathProvider.GetRootPath(), "Content", "LockScreens", lockScreenTemplateName)
                : Path.Combine(_pathProvider.GetRootPath(), term.ImagePath);
            
            var lockScreenPath = Path.Combine(_pathProvider.GetRootPath(), "Content", "Terms", term.Id + ".jpg");

            Bitmap bitMapImage = new Bitmap(lockScreenTemplatePath);
            Graphics graphicImage = Graphics.FromImage(bitMapImage);

            graphicImage.SmoothingMode = SmoothingMode.AntiAlias;

            LinearGradientBrush linGrBrush = new LinearGradientBrush(
                new Point(0, 0),
                new Point(0, 1280),
                Color.FromArgb(200, 0, 0, 0),
                Color.FromArgb(30, 0, 0, 0));

            graphicImage.FillRectangle(linGrBrush, 0, 0, 768, 1280);

            var nameStringFormat = new StringFormat
            { 
                Trimming = StringTrimming.EllipsisCharacter 
            };

            var definitionStringFormat = new StringFormat
            { 
                Trimming = StringTrimming.EllipsisWord 
            };

            var nameSize = graphicImage.MeasureString(term.Name, _nameFont);

            if (nameSize.Width > 560)
            {
                graphicImage.DrawString(term.Name,
                    _nameFont,
                    Brushes.White,
                    new Rectangle(34, 88, 560, 162),
                    nameStringFormat);

                graphicImage.DrawString(term.Definition,
                    _definitionFont,
                    Brushes.White,
                    new Rectangle(39, 260, 560, 290),
                    definitionStringFormat);
            }
            else
            {
                graphicImage.DrawString(term.Name,
                    _nameFont,
                    Brushes.White,
                    new Rectangle(34, 88, 560, 92),
                    nameStringFormat);

                graphicImage.DrawString(term.Definition,
                    _definitionFont,
                    Brushes.White,
                    new Rectangle(39, 180, 560, 290),
                    definitionStringFormat);
            }


            

            bitMapImage.Save(lockScreenPath, ImageFormat.Jpeg);

            graphicImage.Dispose();
            bitMapImage.Dispose();
        }


    }
}