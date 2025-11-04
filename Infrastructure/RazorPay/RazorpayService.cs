using Microsoft.Extensions.Options;
using MyApp1.Application.DTOs.Payment;
using MyApp1.Infrastructure.Helpers;
using Razorpay.Api;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Infrastructure.RazorPay
{
    public class RazorpayService
    {
        private readonly string _key;
        private readonly string _secret;

        public RazorpayService(IOptions<RazorpaySettings> options)
        {
            _key = options.Value.KeyId;
            _secret = options.Value.KeySecret;
        }

        public Order CreateOrder(int amountInRupees, string currency = "INR")
        {
            var client = new RazorpayClient(_key, _secret);

            Dictionary<string, object> options = new Dictionary<string, object>
            {
                { "amount", amountInRupees * 100 },   // Amount in paise
                { "currency", currency },
                { "payment_capture", 1 }  // Auto capture
            };

            return client.Order.Create(options);
        }


        public bool VerifyPayment(VerifyPaymentDto dto)
        {
            string payload = dto.RazorpayOrderId + "|" + dto.RazorpayPaymentId;

            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(_secret));
            var hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));

            string generatedSignature = string.Concat(hash.Select(b => b.ToString("x2"))); // safe hex
            Console.WriteLine($"Razorpay Key: {_key}, Secret: {_secret}");

            Console.WriteLine("==== Razorpay Debug ====");
            Console.WriteLine($"OrderId    : {dto.RazorpayOrderId}");
            Console.WriteLine($"PaymentId  : {dto.RazorpayPaymentId}");
            Console.WriteLine($"Signature  : {dto.RazorpaySignature}");
            Console.WriteLine($"Generated  : {generatedSignature}");
            Console.WriteLine("========================");
            return string.Equals(generatedSignature, dto.RazorpaySignature, StringComparison.OrdinalIgnoreCase);


        }
    }
}
