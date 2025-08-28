using Docnet.Core;

string inputPath = @"C:\Temp\Facture 8 - 26 août 2025 - Docteur Client 2.pdf";
string outputPath = @"C:\Temp\pdfclean.pdf";


byte[] bytes = await File.ReadAllBytesAsync(inputPath);

var cleanBytes = DocLib.Instance.DeleteAttachment(bytes);

await File.WriteAllBytesAsync(outputPath, cleanBytes);

Console.WriteLine("PDF nettoyé avec succès.");