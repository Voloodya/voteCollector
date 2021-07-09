using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace voteCollector.Services
{
    public class QRcodeServices
    {
        //Generates QR code files
        public static string GenerateQRcodeFile(string fio, string dateBirth)
        {
            //Generates QR code files
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(fio+dateBirth, QRCodeGenerator.ECCLevel.Q);

            string fileName = Transliteration.Front(fio)+"_"+dateBirth.Replace('.','_').Replace(' ', '_').Replace(':', '_').Replace('/', '_');

            qrCodeData.SaveRawData("wwwroot/qr_codes/" + fileName + ".qrr", QRCodeData.Compression.Uncompressed);

            return fileName;
        }

        public static Bitmap ReadingQRcodeFromFile(string fileName)
        {
            // Чтение файла с сохраненным qr-кода
            QRCodeData qrCodeDataSave = new QRCodeData("wwwroot/qr_codes/" + fileName + ".qrr", QRCodeData.Compression.Uncompressed);

            QRCode qrCode = new QRCode(qrCodeDataSave);
            // Converting byte array qr-code in Bitmap
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            return qrCodeImage;
        }

        public static Byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }
}
