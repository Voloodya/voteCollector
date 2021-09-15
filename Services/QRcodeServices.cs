using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using voteCollector.Models;
using ZXing;
using ZXing.Common;

namespace voteCollector.Services
{
    public class QRcodeServices
    {
        //Generates QR code files
        public static Byte [] GenerateQRcodeFile(string fio, string dateBirth, string QRtext, string typeFile, string wayPath)
        {
            //Generates QR code files
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(QRtext, QRCodeGenerator.ECCLevel.Q);

            Byte[] QRbytes = qrCodeData.GetRawData(QRCodeData.Compression.Uncompressed);

            //string fileName = Transliteration.Front(fio)+"_"+dateBirth.Replace('.','_').Replace(' ', '_').Replace(':', '_').Replace('/', '_');

            //qrCodeData.SaveRawData(wayPath + fileName + ".qrr", QRCodeData.Compression.Uncompressed);

            //SaveQRCodeDataInImageFile(qrCodeData, wayPath + fileName + "."+typeFile);

            return QRbytes;
        }

        public static Bitmap CreateBitmapFromBytes(Byte[] bytesQRcode)
        {
            QRCodeData qrCodeData = new QRCodeData(bytesQRcode, QRCodeData.Compression.Uncompressed);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            //var imgType = Base64QRCode.ImageType.Jpeg;
            //Base64QRCode qrCode64 = new Base64QRCode(qrCodeData);

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

        public static Bitmap ReadingQRcodeFromFile(string way,string fileName)
        {            
            // Чтение файла с сохраненным qr-кода
            QRCodeData qrCodeDataSave = new QRCodeData(way + fileName + ".qrr", QRCodeData.Compression.Uncompressed);

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

        public static string BytecodeQRinStringImageAsBase64(Byte[] bytesQRcode)
        {
            QRCodeData qrCodeData = new QRCodeData(bytesQRcode, QRCodeData.Compression.Uncompressed);
            var imgType = Base64QRCode.ImageType.Jpeg;
            Base64QRCode qrCode = new Base64QRCode(qrCodeData);
            string qrCodeImageAsBase64 = qrCode.GetGraphic(20, Color.Black, Color.White, true, imgType);

            return qrCodeImageAsBase64;
        }


    }
}
