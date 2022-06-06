
using Project_Invoice_MAUI.Models;
using Syncfusion.Drawing;
using Syncfusion.Pdf;
using Syncfusion.Pdf.Graphics;
using Syncfusion.Pdf.Grid;


using System.Reflection;
using PointF = Syncfusion.Drawing.PointF;
using SizeF = Syncfusion.Drawing.SizeF;



namespace Project_Invoice_MAUI.SaveFileHelper
{
    public class SaveAsPdf : ISaveToFile
    {
        public SaveAsPdf(DocumentNumbering document_Numbering, Kontrahent seller, CompanyData buyer, BossData bossDataInfo,
            BankAccount bankAccountInfo, List<Goods> list_Of_Goods, DateTime dataWystawienia, string invoice_format, int invoice_Number)
        {
            Document_Numbering = document_Numbering;
            Seller = seller;
            Buyer = buyer;
            BossDataInfo = bossDataInfo;
            BankAccountInfo = bankAccountInfo;
            List_Of_Goods = list_Of_Goods;
            DataWystawienia = dataWystawienia;
            Invoice_format = invoice_format;
            Invoice_Number = invoice_Number;



        }

        private DocumentNumbering Document_Numbering { get; set; }
        private Kontrahent Seller { get; set; }
        private CompanyData Buyer { get; set; }
        private BossData BossDataInfo { get; set; }
        private BankAccount BankAccountInfo { get; set; }
        private List<Goods> List_Of_Goods { get; set; }
        private DateTime DataWystawienia { get; set; }
        private string Invoice_format { get; set; }
        private int Invoice_Number { get; set; }
        private List<BillsSum> billsSums { get; set; } = new();
        private int Index { get; set; }
        private bool done { get; set; }
        private double bill { get; set; }

        public void SaveToFile()
        {
            using (PdfDocument document = new PdfDocument())
            {
                //Add a page to the document
                PdfPage page = document.Pages.Add();

                //Create PDF graphics for the page
                PdfGraphics graphics = page.Graphics;

                //Set the standard font
                //PdfFont font = new PdfStandardFont(PdfFontFamily.Helvetica, 20);

                ////Draw the text
                //graphics.DrawString("Hello World!!!", font, PdfBrushes.Black, new PointF(0, 0));


                //image
                Assembly assemblyImage = typeof(MainPage).GetTypeInfo().Assembly;
                string basePathImage = "Project_Invoice_MAUI.Resources.Images.";
                Stream ImageStream = assemblyImage.GetManifestResourceStream(basePathImage + "");
                PdfBitmap image = new PdfBitmap(ImageStream);

                RectangleF bounds = new RectangleF(10, 25, image.Width / 2, image.Height / 2);

                page.Graphics.DrawImage(image, bounds);

                //Get the font file stream from the assembly. 
                Assembly assembly = typeof(MainPage).GetTypeInfo().Assembly;
                string basePath = "Project_Invoice_MAUI.Resources.Fonts.";
                Stream fontStream = assembly.GetManifestResourceStream(basePath + "OpenSans-Regular.ttf");

                //Create a PdfTrueTypeFont from the stream with the different sizes. 
                PdfTrueTypeFont headerFont = new PdfTrueTypeFont(fontStream, 30, PdfFontStyle.Regular);
                PdfTrueTypeFont timesRoman = new PdfTrueTypeFont(fontStream, 18, PdfFontStyle.Regular); //arialRegularFont
                PdfTrueTypeFont arialBoldFont = new PdfTrueTypeFont(fontStream, 9, PdfFontStyle.Bold);

                //PdfFont timesRoman = new PdfTrueTypeFont(new Font("Times New Roman", 10), true);

                graphics.DrawString($"{Buyer.Full_Name}", timesRoman, PdfBrushes.Black, new PointF(bounds.Right + 30, bounds.Top));
                graphics.DrawString($"{Buyer.Street} {Seller.Company.House_Number}", timesRoman, PdfBrushes.Black, new PointF(bounds.Right + 30, bounds.Top + 10));
                graphics.DrawString($"{Buyer.ZIP_Code} {Seller.Company.Town}", timesRoman, PdfBrushes.Black, new PointF(bounds.Right + 30, bounds.Top + 20));
                graphics.DrawString($"{Buyer.NIP}", timesRoman, PdfBrushes.Black, new PointF(bounds.Right + 30, bounds.Top + 30));
                graphics.DrawString($"{BankAccountInfo.BankAccount_Name}", timesRoman, PdfBrushes.Black, new PointF(bounds.Right + 30, bounds.Top + 40));
                graphics.DrawString($"{BankAccountInfo.Account_Number}", timesRoman, PdfBrushes.Black, new PointF(bounds.Right + 30, bounds.Top + 50));


                PdfBrush solidBrush = new PdfSolidBrush(new PdfColor(126, 151, 173));
                bounds = new RectangleF(0, bounds.Bottom, graphics.ClientSize.Width, 30);
                //Draws a rectangle to place the heading in that region.
                graphics.DrawRectangle(solidBrush, bounds);

                PdfFont subHeadingFont = new PdfStandardFont(PdfFontFamily.TimesRoman, 14);

                PdfTextElement element = new PdfTextElement("Faktura VAT " + Invoice_format, subHeadingFont);

                element.Brush = PdfBrushes.White;

                PdfLayoutResult result = element.Draw(page, new PointF(10, bounds.Top + 8));

                string currentDate = "Dnia " + DataWystawienia.ToString("dd/MM/yyyy");

                SizeF textSize = subHeadingFont.MeasureString(currentDate);

                PointF textPosition = new PointF(graphics.ClientSize.Width - textSize.Width - 10, result.Bounds.Y);

                //Draws the date by using DrawString method
                graphics.DrawString(currentDate, subHeadingFont, element.Brush, textPosition);



                //Creates text elements to add the address and draw it to the page.

                PdfTextElement sprzedawca = new PdfTextElement("Sprzedwaca", timesRoman);

                sprzedawca.Brush = new PdfSolidBrush(new PdfColor(126, 155, 203));

                SizeF textSizeSprzedawca = timesRoman.MeasureString(sprzedawca.Text);

                PointF textPositionSprzedawca = new PointF(graphics.ClientSize.Width - textSizeSprzedawca.Width - 10, result.Bounds.Bottom + 25);

                sprzedawca.Draw(page, textPositionSprzedawca);

                //Dane nabywcy
                element = new PdfTextElement("Nabywca", timesRoman);

                element.Brush = new PdfSolidBrush(new PdfColor(126, 155, 203));

                result = element.Draw(page, new PointF(10, result.Bounds.Bottom + 25));

                PdfPen linePen = new PdfPen(new PdfColor(126, 151, 173), 0.7f);

                PointF startPointNabywca = new PointF(0, result.Bounds.Bottom + 10);
                //PointF endPointNabywca = new PointF(0, 10);

                graphics.DrawString($"{Seller.Company.Full_Name}", timesRoman, PdfBrushes.Black, startPointNabywca);
                graphics.DrawString($"{Seller.Company.Street} {Seller.Company.House_Number}", timesRoman, PdfBrushes.Black, startPointNabywca.X, startPointNabywca.Y + 10);
                graphics.DrawString($"{Seller.Company.ZIP_Code} {Seller.Company.Town}", timesRoman, PdfBrushes.Black, startPointNabywca.X, startPointNabywca.Y + 20);
                graphics.DrawString($"{Seller.Company.NIP}", timesRoman, PdfBrushes.Black, startPointNabywca.X, startPointNabywca.Y + 30);


                //Dane Sprzedawcy


                textSizeSprzedawca = timesRoman.MeasureString($"{Buyer.Full_Name}");
                textPositionSprzedawca = new PointF(graphics.ClientSize.Width - textSizeSprzedawca.Width - 10, result.Bounds.Bottom + 10);

                graphics.DrawString($"{Buyer.Full_Name}", timesRoman, PdfBrushes.Black, textPositionSprzedawca);

                textSizeSprzedawca = timesRoman.MeasureString($"{Buyer.Street} {Buyer.House_Number}");
                graphics.DrawString($"{Buyer.Street} {Buyer.House_Number}", timesRoman, PdfBrushes.Black, textPositionSprzedawca.X, textPositionSprzedawca.Y + 10);

                textSizeSprzedawca = timesRoman.MeasureString($"{Buyer.ZIP_Code} {Buyer.Town}");
                graphics.DrawString($"{Buyer.ZIP_Code} {Buyer.Town}", timesRoman, PdfBrushes.Black, textPositionSprzedawca.X, textPositionSprzedawca.Y + 20);

                textSizeSprzedawca = timesRoman.MeasureString($"{Buyer.NIP}");
                graphics.DrawString($"{Buyer.NIP}", timesRoman, PdfBrushes.Black, textPositionSprzedawca.X, textPositionSprzedawca.Y + 30);

                PointF startPoint = new PointF(0, result.Bounds.Bottom + 3);
                PointF endPoint = new PointF(graphics.ClientSize.Width, result.Bounds.Bottom + 3);

                //Draws a line at the bottom of the address
                graphics.DrawLine(linePen, startPoint, endPoint);


                PdfGrid pdfGrid = new();

                pdfGrid.Columns.Add(7);

                PdfGridRow[] headerRow = pdfGrid.Headers.Add(1);
                //DataTable dataTable = new();


                headerRow[0].Cells[0].Value = "Name";
                headerRow[0].Cells[1].Value = "ID";
                headerRow[0].Cells[2].Value = "Cena Netto";
                headerRow[0].Cells[3].Value = "Vat";
                headerRow[0].Cells[4].Value = "Cena Brutto";
                headerRow[0].Cells[5].Value = "Ilosc";
                headerRow[0].Cells[6].Value = "Suma kosztów";

                PdfGrid amountVATGrid = new();

                amountVATGrid.Columns.Add(4);
                PdfGridRow[] amountVatHeaderRow = amountVATGrid.Headers.Add(1);

                amountVatHeaderRow[0].Cells[0].Value = ("Według stawki VAT");
                amountVatHeaderRow[0].Cells[1].Value = ("Wartość netto");
                amountVatHeaderRow[0].Cells[2].Value = ("Kwota VAT");
                amountVatHeaderRow[0].Cells[3].Value = ("Wartość brutto");


                foreach (var item in List_Of_Goods)
                {
                    //jeśli nie bedzie działać weż poprostu dane z tej funkcji
                    AddProducts(item.Product_Name, item.Product_Code, item.Price_Netto,
                        item.VAT_String, item.Price_Brutto, item.Quantity, Math.Round(item.Sum, 2), pdfGrid);

                    Math.Round(bill += item.Price_Brutto, 2);

                    foreach (var vat_string in from vat_string in Vat_Helper.List_VAT_Strings
                                               where item.VAT_String == vat_string
                                               select vat_string)
                    {

                        //dodawanie pierwszego Vatu
                        if (billsSums.Count == 0)
                        {
                            billsSums.Add(new BillsSum());
                            billsSums[0].Sum_VAT = Math.Round(item.Price_Brutto - item.Price_Netto, 2);
                            billsSums[0].Sum_Netto = Math.Round(item.Price_Netto, 2);
                            billsSums[0].Sum_Brutto = Math.Round(item.Price_Brutto, 2);
                            billsSums[0].Vat_name = item.VAT_String;
                            billsSums[0].ID = 0;
                            done = true;

                        }

                        //edytowanie już powstałych
                        else
                        {
                            Index = 0;
                            foreach (var bills in billsSums)
                            {
                                bills.ID = Index;
                                if (bills.Vat_name == vat_string)
                                {
                                    Math.Round(billsSums[bills.ID].Sum_Netto += item.Price_Netto, 2);
                                    Math.Round(billsSums[bills.ID].Sum_VAT += (item.Price_Brutto - item.Price_Netto), 2);
                                    Math.Round(billsSums[bills.ID].Sum_Brutto += item.Price_Brutto, 2);
                                    done = true;
                                    break;
                                }
                                Index++;
                            }
                        }

                        //Jeśli nie ma tworzy nowy
                        if (!done)
                        {
                            billsSums.Add(new BillsSum());
                            billsSums[billsSums.Count - 1].Sum_VAT = Math.Round(item.Price_Brutto - item.Price_Netto, 2);
                            billsSums[billsSums.Count - 1].Sum_Netto = Math.Round(item.Price_Netto, 2);
                            billsSums[billsSums.Count - 1].Sum_Brutto = Math.Round(item.Price_Brutto, 2);
                            billsSums[billsSums.Count - 1].Vat_name = item.VAT_String;
                            billsSums[billsSums.Count - 1].ID = billsSums.Count - 1;
                            done = false;
                        }
                        done = false;
                    }



                }

                for (int i = 0; i < billsSums.Count; i++)
                {
                    PdfGridRow amountVATGridrow = amountVATGrid.Rows.Add();
                    amountVATGridrow.Cells[0].Value = $"Podatek {billsSums[i].Vat_name}";
                    amountVATGridrow.Cells[1].Value = $"{billsSums[i].Sum_Netto}";
                    amountVATGridrow.Cells[2].Value = $"{billsSums[i].Sum_VAT}";
                    amountVATGridrow.Cells[3].Value = $"{billsSums[i].Sum_Brutto}";


                }

                //pdfGrid.DataSource = dataTable;

                PdfGridCellStyle cellStyle = new PdfGridCellStyle();
                //cellStyle.Borders.All = PdfPens.White;
                cellStyle.Borders.Right = PdfPens.DarkGray;
                PdfGridRow header = pdfGrid.Headers[0];
                //Creates the header style



                //Po zmieniaj troche, bo się nie miesci
                //pdfGrid.Columns[0].Width = 100f;
                //pdfGrid.Columns[1].Width = 50f;
                //pdfGrid.Columns[2].Width = 150f;
                //pdfGrid.Columns[3].Width = 50f;
                //pdfGrid.Columns[4].Width = 150f;
                //pdfGrid.Columns[5].Width = 50f;
                //pdfGrid.Columns[6].Width = 150f;

                PdfGridCellStyle headerStyle = new PdfGridCellStyle();

                headerStyle.Borders.All = new PdfPen(new PdfColor(126, 151, 173));
                headerStyle.BackgroundBrush = new PdfSolidBrush(new PdfColor(126, 151, 173));
                headerStyle.TextBrush = PdfBrushes.White;
                headerStyle.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 12f, PdfFontStyle.Regular);

                //Adds cell customizations
                for (int i = 0; i < header.Cells.Count; i++)
                {
                    if (i == 0 || i == 1)
                        header.Cells[i].StringFormat = new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle);
                    else
                        header.Cells[i].StringFormat = new PdfStringFormat(PdfTextAlignment.Right, PdfVerticalAlignment.Middle);
                }


                //Applies the header style
                header.ApplyStyle(headerStyle);
                cellStyle.Borders.Bottom = new PdfPen(new PdfColor(0, 0, 0), 0.70f);
                cellStyle.Font = new PdfStandardFont(PdfFontFamily.TimesRoman, 8f);
                cellStyle.TextBrush = new PdfSolidBrush(new PdfColor(0, 0, 0)); // black
                //cellStyle.StringFormat = new PdfStringFormat(PdfTextAlignment.Right, PdfVerticalAlignment.Middle); 
                for (int j = 0; j < pdfGrid.Rows.Count; j++)
                {

                    pdfGrid.Rows[j].Cells[0].StringFormat = new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle);
                    pdfGrid.Rows[j].Cells[1].StringFormat = new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle);
                    pdfGrid.Rows[j].Cells[2].StringFormat = new PdfStringFormat(PdfTextAlignment.Right, PdfVerticalAlignment.Middle);
                    pdfGrid.Rows[j].Cells[3].StringFormat = new PdfStringFormat(PdfTextAlignment.Right, PdfVerticalAlignment.Middle);
                    pdfGrid.Rows[j].Cells[4].StringFormat = new PdfStringFormat(PdfTextAlignment.Right, PdfVerticalAlignment.Middle);
                    pdfGrid.Rows[j].Cells[5].StringFormat = new PdfStringFormat(PdfTextAlignment.Right, PdfVerticalAlignment.Middle);
                    pdfGrid.Rows[j].Cells[6].StringFormat = new PdfStringFormat(PdfTextAlignment.Right, PdfVerticalAlignment.Middle);
                    pdfGrid.Rows[j].ApplyStyle(cellStyle);

                }

                //Creates the layout format for grid
                PdfGridLayoutFormat layoutFormat = new PdfGridLayoutFormat();
                // Creates layout format settings to allow the table pagination
                layoutFormat.Layout = PdfLayoutType.Paginate;


                //Draws the grid to the PDF page.
                PdfGridLayoutResult gridResult = pdfGrid.Draw(page, new RectangleF(new PointF(0, result.Bounds.Bottom + 70),
                    new SizeF(graphics.ClientSize.Width, graphics.ClientSize.Height - 100)), layoutFormat);

                //Tabela z Sum Vat
                //pdfGrid.DataSource = amountVAT;

                for (int i = 0; i < amountVATGrid.Rows.Count; i++)
                {
                    pdfGrid.Rows[i].Cells[0].StringFormat = new PdfStringFormat(PdfTextAlignment.Left, PdfVerticalAlignment.Middle);
                    pdfGrid.Rows[i].Cells[1].StringFormat = new PdfStringFormat(PdfTextAlignment.Right, PdfVerticalAlignment.Middle);
                    pdfGrid.Rows[i].Cells[2].StringFormat = new PdfStringFormat(PdfTextAlignment.Right, PdfVerticalAlignment.Middle);
                    pdfGrid.Rows[i].Cells[3].StringFormat = new PdfStringFormat(PdfTextAlignment.Right, PdfVerticalAlignment.Middle);
                }

                //Creates the layout format for grid
                PdfGridLayoutFormat vatLayoutFormat = new PdfGridLayoutFormat();
                // Creates layout format settings to allow the table pagination
                vatLayoutFormat.Layout = PdfLayoutType.Paginate;



                //Draws the grid to the PDF page.
                PdfGridCellStyle amountVatheaderStyle = new PdfGridCellStyle();
                PdfGridRow VatSumheader = pdfGrid.Headers[0];
                amountVatheaderStyle.Borders.All = new PdfPen(new PdfColor(126, 151, 173));
                amountVatheaderStyle.BackgroundBrush = new PdfSolidBrush(new PdfColor(126, 151, 173));
                amountVatheaderStyle.TextBrush = PdfBrushes.White;
                amountVatheaderStyle.Font = timesRoman;

                VatSumheader.ApplyStyle(amountVatheaderStyle);

                PdfGridLayoutResult vatGridResult = pdfGrid.Draw(gridResult.Page, new RectangleF(new PointF(graphics.ClientSize.Width / 2 - 50f, gridResult.Bounds.Bottom + 30f),
                    new SizeF(graphics.ClientSize.Width / 2 + 50f, gridResult.Bounds.Right)), vatLayoutFormat);


                //Suma kosztów



                PointF pointSuma = new(graphics.ClientSize.Width / 2, vatGridResult.Bounds.Bottom + 10f);
                RectangleF CostSumRect = new RectangleF(pointSuma.X, pointSuma.Y, graphics.ClientSize.Width / 2, 20);
                graphics.DrawRectangle(solidBrush, CostSumRect);


                PdfTextElement NameOfSum = new PdfTextElement("Razem do zapłaty:", timesRoman);

                NameOfSum.Brush = PdfBrushes.White;

                PdfTextElement SumValue = new PdfTextElement($"{bill} PLN", timesRoman);

                SumValue.Brush = PdfBrushes.White;

                //SumValue.Brush = new PdfSolidBrush(new PdfColor(126, 155, 203));

                SizeF textSizeSum = timesRoman.MeasureString(SumValue.Text);



                PdfLayoutResult SumLayoutResult = NameOfSum.Draw(page, pointSuma.X, pointSuma.Y + 4f);

                PointF textPositionSum = new PointF(vatGridResult.Bounds.Right - textSizeSum.Width - 5f, SumLayoutResult.Bounds.Y);
                SumValue.Draw(page, textPositionSum);

                //Dodaj w parametrach walute
                PdfTextElement amountInValueText = new($"Słownie: {AmountInWords.KwotaSlownie(bill, "PLN")}", timesRoman);
                amountInValueText.Brush = PdfBrushes.Black;

                //format
                PdfStringFormat format = new PdfStringFormat();
                format.Alignment = PdfTextAlignment.Justify;
                format.LineAlignment = PdfVerticalAlignment.Top;
                //format.ParagraphIndent = 15f;
                format.WordWrap = PdfWordWrapType.Word;

                amountInValueText.StringFormat = format;

                PdfLayoutFormat AmountInValuelayoutFormat = new PdfLayoutFormat();
                AmountInValuelayoutFormat.Break = PdfLayoutBreakType.FitPage;
                AmountInValuelayoutFormat.Layout = PdfLayoutType.Paginate;


                PdfLayoutResult amountInValueTextLayout = amountInValueText.Draw(page, new PointF(SumLayoutResult.Bounds.Left, SumLayoutResult.Bounds.Bottom + 14f), CostSumRect.Width, AmountInValuelayoutFormat);
                //SumLayoutResult  = amountInValueText.Draw(page, new PointF(SumLayoutResult.Bounds.Left - 10f, SumLayoutResult.Bounds.Bottom + 14f));


                graphics.DrawLine(linePen, new PointF(pointSuma.X, amountInValueTextLayout.Bounds.Bottom + 10F),
                    new PointF(gridResult.Bounds.Right, amountInValueTextLayout.Bounds.Bottom + 10F));


                #region Podpis
                //RectangleF CostSumRect = new RectangleF(pointSuma.X, pointSuma.Y, graphics.ClientSize.Width / 2, 20);

                RectangleF signature = new RectangleF(10f, amountInValueTextLayout.Bounds.Bottom + 30f, 200f, 20f);
                RectangleF signatureWhite = new RectangleF(10f, amountInValueTextLayout.Bounds.Bottom + 30f, 200f, 75f);
                PdfPen borderPen = new PdfPen(new PdfColor(0, 0, 0));
                borderPen.Width = 1;

                PdfTextElement buyerTextSignature = new PdfTextElement("Wystawił(a):", timesRoman);

                SizeF buyerTextSignaturSize = timesRoman.MeasureString(buyerTextSignature.Text);

                buyerTextSignature.Brush = PdfBrushes.White;



                graphics.DrawRectangle(borderPen, signatureWhite);
                graphics.DrawRectangle(solidBrush, signature);

                PdfLayoutResult buyerSignatureLayoutResult = buyerTextSignature.Draw(page,
                    new PointF(signature.Width / 2 - buyerTextSignaturSize.Width / 2 + 10f, signature.Top + 4f));



                RectangleF signatureSecond = new RectangleF(graphics.ClientSize.Width - 200f, amountInValueTextLayout.Bounds.Bottom + 30f, 200f, 20f);
                RectangleF signatureWhiteSecond = new RectangleF(graphics.ClientSize.Width - 200f, amountInValueTextLayout.Bounds.Bottom + 30f, 200f, 75f);

                PdfTextElement SellerTextSignature = new PdfTextElement("Odebrał(a):", timesRoman);

                SizeF SellerTextSignatureSize = timesRoman.MeasureString(SellerTextSignature.Text);

                SellerTextSignature.Brush = PdfBrushes.White;



                graphics.DrawRectangle(borderPen, signatureWhiteSecond);
                graphics.DrawRectangle(solidBrush, signatureSecond);

                PdfLayoutResult SellerSignatureLayoutResult = SellerTextSignature.Draw(page,
                    new PointF(graphics.ClientSize.Width - 200f + signatureSecond.Width / 2 - SellerTextSignatureSize.Width / 2 + 10f, signatureSecond.Top + 4f));

                #endregion






                using MemoryStream ms = new();
                //Saves the presentation to the memory stream.
                document.Save(ms);
                ms.Position = 0;
                //Saves the memory stream as a file.
                //DependencyService.Get<ISave>().SaveAndView("Invoice.pdf", "application/pdf", ms);
                if (Document_Numbering != null)
                {
                    if (Document_Numbering.Broken_By_Year && Document_Numbering.Broken_By_Mounth)
                    {
                        DependencyService.Get<ISave>().SaveAndView($"Faktury/FS {Invoice_Number}_{DataWystawienia.ToString("MM")}_{DataWystawienia.ToString("yyyy")}.pdf", "application/pdf", ms);
                        //document.Save($"Faktury/FS {Invoice_Number}_{DataWystawienia.ToString("MM")}_{DataWystawienia.ToString("yyyy")}.pdf");
                    }
                    else if (Document_Numbering.Broken_By_Year)
                    {
                        DependencyService.Get<ISave>().SaveAndView($"Faktury/FS {Invoice_Number}_{DataWystawienia.ToString("yyyy")}.pdf", "application/pdf", ms);
                    }
                    else if (Document_Numbering.Broken_By_Mounth)
                    {
                        //document.Save($"Faktury/FS {Invoice_Number}_{DataWystawienia.ToString("MM")}.pdf");
                        DependencyService.Get<ISave>().SaveAndView($"Faktury/FS {Invoice_Number}_{DataWystawienia.ToString("MM")}.pdf", "application/pdf", ms);
                    }
                    else
                    {
                        DependencyService.Get<ISave>().SaveAndView("Faktury/FS {Invoice_Number}.pdf", "application/pdf", ms);
                        //document.Save($"Faktury/FS {Invoice_Number}.pdf");
                    }
                }
                else
                {
                    DependencyService.Get<ISave>().SaveAndView($"Faktury/FS {Invoice_Number}.pdf", "application/pdf", ms);
                }
                //document.Save($"Faktury/FS {Invoice_Number}_{dataWystawienia.ToString("MM")}_{dataWystawienia.ToString("yyyy")}.pdf");

                //Close the document
                document.Close(true);


                //Process.Start("Sample.pdf");

            }



        }

        /// <summary>
        /// Create and row for the grid.
        /// </summary>
        /// <param name="product_Name"></param>
        /// <param name="product_Code"></param>
        /// <param name="price_Netto"></param>
        /// <param name="vAT_String"></param>
        /// <param name="price_Brutto"></param>
        /// <param name="quantity"></param>
        /// <param name="sum"></param>
        /// <param name="grid"></param>
        private void AddProducts(string product_Name, string product_Code, double price_Netto, string vAT_String,
            double price_Brutto, int quantity, double sum, PdfGrid grid)
        {
            {
                //Add a new row and set the product value to the grid row cells. 
                PdfGridRow row = grid.Rows.Add();
                row.Cells[0].Value = product_Name;
                row.Cells[1].Value = product_Code;
                row.Cells[2].Value = price_Netto;
                row.Cells[3].Value = vAT_String;
                row.Cells[4].Value = price_Brutto;
                row.Cells[5].Value = quantity;
                row.Cells[6].Value = sum;
            }
        }
        #region Helper Methods
        //Create and row for the grid.

        // <summary>
        // Get the Total amount of the purcharsed items.
        // </summary>
        //private float GetTotalAmount(PdfGrid grid)
        //{
        //    float Total = 0f;
        //    for (int i = 0; i < grid.Rows.Count; i++)
        //    {
        //        string cellValue = grid.Rows[i].Cells[grid.Columns.Count - 1].Value.ToString();
        //        float result = float.Parse(cellValue, System.Globalization.CultureInfo.InvariantCulture);
        //        Total += result;
        //    }
        //    return Total;

        //}
        #endregion
    }
}
