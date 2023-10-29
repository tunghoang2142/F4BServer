using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using PdfSharpCore.Pdf;
using PdfSharpCore.Drawing;

namespace ImageToPdfConverter
{
    class PDFGenerator
    {
        static void GeneratePDF(string id, string fullName, string school, string hobbies, string motivation, bool level1Completed = false, bool level2Completed = false)
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string codeInput = id;
            string nameInput = fullName;
            string schoolNameInput = school;
            string hoobiesInput = hobbies;
            string whyInput = motivation;
            string achivementInput = "";
            if (level1Completed)
            {
                achivementInput += "Gym Level";
            }
            if (level2Completed)
            {
                if(achivementInput != "")
                {
                    achivementInput += ", ";
                }
                achivementInput += "Pool Level";
            }


            // Get Image name
            string imageName = $"{id}.png";
            string pdfName = $"{id}.pdf";

            // Paths
            string imagePath = System.IO.Path.Combine(currentDirectory, "input/pdf.png");
            string savedImagePath = System.IO.Path.Combine(currentDirectory, "results", imageName);
            string pdfFilePath = System.IO.Path.Combine(currentDirectory, "results", pdfName);

            // Load image
            Image img = Image.FromFile(imagePath);
            Bitmap bitmap = new Bitmap(img);


            // Add text on the image
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                string imageToInsertPath = System.IO.Path.Combine(currentDirectory, "input/input.png");
                Image imageToInsert = Image.FromFile(imageToInsertPath);
                Image resizedImageToInsert = ResizeImage(imageToInsert, 608, 620);  // Adjust size accordingly
                Rectangle blackBoxRect = new Rectangle(104, 232, 608, 620);  // Modify this rectangle based on the black box's coordinates and size
                graphics.DrawImage(resizedImageToInsert, blackBoxRect);

                Font superbigFont = new Font("Roboto", 60);
                Font bigFont = new Font("Roboto", 18);
                Font smlFont = new Font("Roboto", 14);

                DrawStringWrapped(graphics, codeInput, smlFont, Brushes.White, new Rectangle(74, 887, 600, 100), 10);
                DrawStringWrapped(graphics, nameInput, superbigFont, Brushes.Yellow, new Rectangle(74, 910, 500, 100), 10);
                DrawStringWrapped(graphics, schoolNameInput, smlFont, Brushes.Black, new Rectangle(1054, 468, 500, 100), 20);
                DrawStringWrapped(graphics, hoobiesInput, smlFont, Brushes.Black, new Rectangle(950, 605, 500, 100), 20);
                DrawStringWrapped(graphics, whyInput, smlFont, Brushes.Black, new Rectangle(950, 750, 500, 100), 30);
                DrawStringWrapped(graphics, "Achievement: " + achivementInput, bigFont, Brushes.White, new Rectangle(934, 211, 600, 100));

                // Save the modified image
                bitmap.Save(savedImagePath);
            }



            // Convert the image to PDF
            PdfDocument doc = new PdfDocument();

            PdfPage page = doc.AddPage();
            // Set the PDF page size to the size of the image
            page.Width = XUnit.FromPoint(bitmap.Width);
            page.Height = XUnit.FromPoint(bitmap.Height);

            XGraphics xgr = XGraphics.FromPdfPage(page);
            XImage ximg = XImage.FromFile(savedImagePath);
            xgr.DrawImage(ximg, 0, 0, page.Width, page.Height);

            // Save the PDF
            doc.Save(pdfFilePath);

            Console.WriteLine("Image and PDF created successfully!");
        }

        static void DrawStringWrapped(Graphics graphics, string text, Font font, Brush brush, Rectangle layoutRectangle, int maxWords = int.MaxValue)
        {
            string[] words = text.Split(' ');
            if (words.Length > maxWords)
            {
                Array.Resize(ref words, maxWords);
            }

            float lineHeight = font.GetHeight(graphics);
            float x = layoutRectangle.X;
            float y = layoutRectangle.Y;
            float maxWidth = layoutRectangle.Width;

            string line = "";
            foreach (var word in words)
            {
                var testLine = $"{line}{word} ";
                var testLineWidth = graphics.MeasureString(testLine, font).Width;
                if (testLineWidth > maxWidth)
                {
                    graphics.DrawString(line, font, brush, x, y);
                    y += lineHeight;
                    line = $"{word} ";
                }
                else
                {
                    line = testLine;
                }
            }
            graphics.DrawString(line, font, brush, x, y);
        }

        static Image ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

    }
}
