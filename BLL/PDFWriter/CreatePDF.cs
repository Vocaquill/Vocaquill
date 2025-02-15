using BLL.PDFWriter.Model;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace BLL.PDFWriter
{
    public class CreatePDF
    {
        /*
       H1 - FontSize(24)
       H2 - FontSize(22)
       H3 - FontSize(20)
       H4 - FontSize(18)
       H5 - FontSize(16)
       H6 - FontSize(14)
       H7 - FontSize(12)
       H8 - FontSize(10)
       H9 - FontSize(8)

       P - Paragraph

       B - Bold 
       I - Italic
       U - Underline
       S - Strikethrough

       H1: Text\n
       H1P: Text\n
       H1BP: Text\n
       H1PB: Text\n
       H1BI: Text\n
       H1BIP: Text\n
       H1BIUS: Text\n
       H1BIUSP: Text\n
       */

        public static async Task TextToPDFAsync(string path, string text)
        {
            List<Paragraph> rows = await GetRows(text);

            await CreatePDFFile(path, rows);
        }

        private static async Task<List<Paragraph>> GetRows(string text)
        {
            var testList = text.Split('\n').ToList().Where(x => !String.IsNullOrWhiteSpace(x)).ToList();

            List<Paragraph> rows = new List<Paragraph>();

            foreach (var line in testList)
            {
                int st = line.IndexOf(' ');
                string param = line.Substring(0, st - 1);
                string lineText = line.Substring(st + 1);

                int res = param.IndexOf("P");

                if (res != -1)
                {
                    string tmp = "";
                    for (int i = 0; i < param.Length; i++)
                    {
                        if (i != res)
                            tmp += param[i];
                    }
                    param = tmp;

                    rows.Add(new Paragraph() { Pairs = new List<Pair>() });
                    rows[rows.Count - 1].Pairs.Add(new Pair() { Param = param, Text = lineText });
                }
                else
                {
                    if (rows.Count == 0)
                    {
                        rows.Add(new Paragraph() { Pairs = new List<Pair>() });
                    }
                    rows[rows.Count - 1].Pairs.Add(new Pair() { Param = param, Text = lineText });
                }
            }
            return rows;
        }

        private static async Task CreatePDFFile(string path, List<Paragraph> rows)
        {
            QuestPDF.Settings.License = LicenseType.Community;

            var document = Document.Create(container =>
            {
                container.Page(page =>
                {
                    page.Size(PageSizes.A4);
                    page.Margin(2, Unit.Centimetre);
                    page.PageColor(Colors.White);
                    page.DefaultTextStyle(x => x.FontSize(14));

                    page.Content()
                    .Column(column =>
                    {

                        foreach (var r in rows)
                        {
                            column.Item().Text(t =>
                            {
                                foreach (var pair in r.Pairs)
                                {
                                    SetParam(t, pair.Text, pair.Param);
                                }
                            });
                            column.Item().Text("").LineHeight(0.5f).FontSize(14);
                        }
                    });

                    page.Footer()
                        .AlignCenter()
                        .Text(x =>
                        {
                            x.CurrentPageNumber();
                            x.Span("/");
                            x.TotalPages();
                        });
                });
            });

            document.GeneratePdf(path);
        }

        public static void SetParam(TextDescriptor text, string content, string param)
        {
            var f = text.Span(content);
            for (int i = 0; i < param.Length; i++)
            {
                string p = "";
                if (param[i].ToString() == "H")
                {
                    p = param[i].ToString() + param[i + 1].ToString();
                    i++;
                }
                else
                    p = param[i].ToString();

                switch (p)
                {
                    case "H1": f.FontSize(24); break;
                    case "H2": f.FontSize(22); break;
                    case "H3": f.FontSize(20); break;
                    case "H4": f.FontSize(18); break;
                    case "H5": f.FontSize(16); break;
                    case "H6": f.FontSize(14); break;
                    case "H7": f.FontSize(12); break;
                    case "H8": f.FontSize(10); break;
                    case "H9": f.FontSize(8); break;
                    case "B": f.Bold(); break;
                    case "I": f.Italic(); break;
                    case "U": f.Underline(); break;
                    case "S": f.Strikethrough(); break;
                }
            }
        }

    }
}
