using System;

namespace SmartDeliveryManagementSystem
{
    // ============================================
    // DeliveryAddress Struct
    // ============================================
    public struct DeliveryAddress
    {
        public string Street { get; set; }
        public string City { get; set; }

        public DeliveryAddress(string street, string city)
        {
            if (string.IsNullOrWhiteSpace(street))
                throw new ArgumentException("Street cannot be empty.");

            if (string.IsNullOrWhiteSpace(city))
                throw new ArgumentException("City cannot be empty.");

            Street = street;
            City = city;
        }

        public override string ToString()
        {
            return $"{Street}, {City}";
        }
    }
