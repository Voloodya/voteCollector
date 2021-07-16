using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using ZXing;
using ZXing.Common;

namespace voteCollector.Services
{
    public class QRcodeServices
    {
        //Generates QR code files
        public static string GenerateQRcodeFile(string fio, string dateBirth, string QRtext, string typeFile)
        {
            //Generates QR code files
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(QRtext, QRCodeGenerator.ECCLevel.Q);

            string fileName = Transliteration.Front(fio)+"_"+dateBirth.Replace('.','_').Replace(' ', '_').Replace(':', '_').Replace('/', '_');

            qrCodeData.SaveRawData("wwwroot/qr_codes/" + fileName + ".qrr", QRCodeData.Compression.Uncompressed);

            SaveQRCodeDataInImageFile(qrCodeData,"wwwroot/qr_codes/" + fileName + "."+typeFile);

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

        public static void SaveQRCodeDataInImageFile(QRCodeData qrCodeData, string nameFile)
        {
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);
            qrCodeImage.Save(nameFile);
        }

        public static Byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }

        public static string DecoderFromImage(Bitmap image)
        {
            //use gaussian filter to remove noise
            //var gFilter = new GaussianBlur(2);
            //image = gFilter.ProcessImage(image);

            var options = new DecodingOptions { PossibleFormats = new List<BarcodeFormat> { BarcodeFormat.QR_CODE }, TryHarder = true };

            using (image)
            {
                //use GlobalHistogramBinarizer for best result
                var reader = new BarcodeReader(null, null, ls => new GlobalHistogramBinarizer(ls)) { AutoRotate = false, TryInverted = false, Options = options };
                var result = reader.Decode(image);
                reader = null;

                return result.Text;
            }
        }

        
    }
}
