using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using QRCoder;
using voteCollector.Services;

namespace voteCollector.Controllers
{
    public class QRCoderController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string qrText)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrText,
            QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            return View(BitmapToBytes(qrCodeImage));
        }

        private static Byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }

        /// <summary>
        /// //////////////////////////////////////////////////////////////////////////////////
        /// </summary>
        /// <returns></returns>
        //Create QR Code File (.qrr) for any text and then save these files in the application
        [HttpGet]
        public IActionResult GenerateQRFile()
        {
            return View();
        }

        [HttpPost]
        public IActionResult GenerateQRFile(string qrText)
        {

            string fileGuid = GenerateQRcodeFile(qrText);
            Bitmap qrCodeImage = ReadingQRcodeFromFile(fileGuid);

            return View(BitmapToBytes(qrCodeImage));
        }

        //Generates QR code files
        private string GenerateQRcodeFile(string qrText)
        {
            //Generates QR code files
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(qrText, QRCodeGenerator.ECCLevel.Q);

            string fileName = Transliteration.Front(qrText);

            qrCodeData.SaveRawData("wwwroot/qr_codes/" + fileName + ".qrr", QRCodeData.Compression.Uncompressed);

            return fileName;
        }

        private Bitmap ReadingQRcodeFromFile(string fileName)
        {
            // Чтение файла с сохраненным qr-кода
            QRCodeData qrCodeDataSave = new QRCodeData("wwwroot/qr_codes/" + fileName + ".qrr", QRCodeData.Compression.Uncompressed);

            QRCode qrCode = new QRCode(qrCodeDataSave);
            // Converting byte array qr-code in Bitmap
            Bitmap qrCodeImage = qrCode.GetGraphic(20);

            return qrCodeImage;
        }

    }
}
