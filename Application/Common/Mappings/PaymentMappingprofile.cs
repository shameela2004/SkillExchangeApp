using AutoMapper;
using MyApp1.Application.DTOs.Payment;
using MyApp1.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp1.Application.Common.Mappings
{
    public class PaymentMappingprofile :Profile
    {
        public PaymentMappingprofile()
        {
            CreateMap<Payment, PaymentDto>()
         .ForMember(dest => dest.PaymentMethod, opt => opt.MapFrom(src => src.PaymentMethod))
         .ReverseMap();

            CreateMap<VerifyPaymentDto, Payment>()
                    .ForMember(dest => dest.RazorpayOrderId, opt => opt.MapFrom(src => src.RazorpayOrderId))
                    .ForMember(dest => dest.RazorpayPaymentId, opt => opt.MapFrom(src => src.RazorpayPaymentId))
                    .ForMember(dest => dest.RazorpaySignature, opt => opt.MapFrom(src => src.RazorpaySignature));
        }
 
    }
}
