using System;

namespace SmartDeliveryManagementSystem
{
    #region DeliveryAddress

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

    #endregion


    #region Shipment

    public class Shipment
    {
        private string _trackingCode;
        private string _description;
        private decimal _weight;
        private decimal _deliveryFee;

        public string TrackingCode
        {
            get { return _trackingCode; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Tracking code cannot be empty.");

                _trackingCode = value;
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Description cannot be empty.");

                _description = value;
            }
        }

        public decimal Weight
        {
            get { return _weight; }
            set
            {
                if (value <= 0)
                    throw new ArgumentException("Weight must be greater than 0.");

                _weight = value;
            }
        }

        public decimal DeliveryFee
        {
            get { return _deliveryFee; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Delivery fee cannot be negative.");

                _deliveryFee = value;
            }
        }

        public DeliveryAddress Destination { get; set; }

        public virtual decimal EstimatedCost
        {
            get
            {
                return DeliveryFee + (Weight * 5);
            }
        }

        public Shipment()
        {
        }

        public Shipment(
            string trackingCode,
            string description,
            decimal weight,
            decimal deliveryFee,
            DeliveryAddress destination)
        {
            TrackingCode = trackingCode;
            Description = description;
            Weight = weight;
            DeliveryFee = deliveryFee;
            Destination = destination;
        }

        public Shipment(
            string trackingCode,
            string description,
            decimal weight,
            decimal deliveryFee)
            : this(
                trackingCode,
                description,
                weight,
                deliveryFee,
                new DeliveryAddress("Unknown", "Unknown"))
        {
        }

        public void UpdateDeliveryFee(decimal newFee)
        {
            if (newFee < 0)
                throw new ArgumentException("Delivery fee cannot be negative.");

            DeliveryFee = newFee;
        }

        public virtual void PrintShipment()
        {
            Console.WriteLine($"Tracking Code: {TrackingCode}");
            Console.WriteLine($"Description: {Description}");
            Console.WriteLine($"Weight: {Weight}");
            Console.WriteLine($"Delivery Fee: {DeliveryFee}");
            Console.WriteLine($"Destination: {Destination}");
            Console.WriteLine($"Estimated Cost: {EstimatedCost}");
        }
    }

    #endregion


    #region StandardShipment

    public class StandardShipment : Shipment
    {
        public StandardShipment(
            string trackingCode,
            string description,
            decimal weight,
            decimal deliveryFee,
            DeliveryAddress destination)
            : base(
                trackingCode,
                description,
                weight,
                deliveryFee,
                destination)
        {
        }
    }

    #endregion


    #region ExpressShipment

    public class ExpressShipment : Shipment
    {
        private decimal _extraFee;

        public decimal ExtraFee
        {
            get { return _extraFee; }
            set
            {
                if (value < 0)
                    throw new ArgumentException("Extra fee cannot be negative.");

                _extraFee = value;
            }
        }

        public override decimal EstimatedCost
        {
            get
            {
                return DeliveryFee + (Weight * 5) + ExtraFee;
            }
        }

        public ExpressShipment(
            string trackingCode,
            string description,
            decimal weight,
            decimal deliveryFee,
            DeliveryAddress destination,
            decimal extraFee)
            : base(
                trackingCode,
                description,
                weight,
                deliveryFee,
                destination)
        {
            ExtraFee = extraFee;
        }

        public override void PrintShipment()
        {
            base.PrintShipment();
            Console.WriteLine($"Extra Fee: {ExtraFee}");
        }
    }

    #endregion


    #region InternationalShipment

    public class InternationalShipment : Shipment
    {
        private string _destinationCountry;
        private decimal _customsFee;

        public string DestinationCountry
        {
            get { return _destinationCountry; }
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException(
                        "Destination country cannot be empty.");

                _destinationCountry = value;
            }
        }

        public decimal CustomsFee
        {
            get { return _customsFee; }
            set
            {
                if (value < 0)
                    throw new ArgumentException(
                        "Customs fee cannot be negative.");

                _customsFee = value;
            }
        }

        public override decimal EstimatedCost
        {
            get
            {
                return DeliveryFee + (Weight * 5) + CustomsFee;
            }
        }

        public InternationalShipment(
            string trackingCode,
            string description,
            decimal weight,
            decimal deliveryFee,
            DeliveryAddress destination,
            string destinationCountry,
            decimal customsFee)
            : base(
                trackingCode,
                description,
                weight,
                deliveryFee,
                destination)
        {
            DestinationCountry = destinationCountry;
            CustomsFee = customsFee;
        }

        public override void PrintShipment()
        {
            base.PrintShipment();
            Console.WriteLine($"Destination Country: {DestinationCountry}");
            Console.WriteLine($"Customs Fee: {CustomsFee}");
        }
    }

    #endregion


    #region DeliveryCenter

    public class DeliveryCenter
    {
        public string CenterName { get; set; }

        private Shipment[] _shipments;
        private int _count;

        public DeliveryCenter(string centerName)
        {
            if (string.IsNullOrWhiteSpace(centerName))
                throw new ArgumentException(
                    "Center name cannot be empty.");

            CenterName = centerName;

            _shipments = new Shipment[20];
            _count = 0;
        }

        public Shipment this[int index]
        {
            get
            {
                if (index < 0 || index >= _count)
                    throw new IndexOutOfRangeException();

                return _shipments[index];
            }
        }

        public Shipment this[string trackingCode]
        {
            get
            {
                for (int i = 0; i < _count; i++)
                {
                    if (_shipments[i].TrackingCode
                        .Equals(
                            trackingCode,
                            StringComparison.OrdinalIgnoreCase))
                    {
                        return _shipments[i];
                    }
                }

                return null;
            }
        }

        public void AddShipment(Shipment shipment)
        {
            if (shipment == null)
                throw new ArgumentNullException(nameof(shipment));

            if (_count >= 20)
                throw new InvalidOperationException(
                    "Delivery center is full.");

            _shipments[_count] = shipment;
            _count++;
        }

        public bool RemoveShipment(string trackingCode)
        {
            for (int i = 0; i < _count; i++)
            {
                if (_shipments[i].TrackingCode
                    .Equals(
                        trackingCode,
                        StringComparison.OrdinalIgnoreCase))
                {
                    for (int j = i; j < _count - 1; j++)
                    {
                        _shipments[j] = _shipments[j + 1];
                    }

                    _shipments[_count - 1] = null;
                    _count--;

                    return true;
                }
            }

            return false;
        }

        public void PrintAllShipments()
        {
            Console.WriteLine();
            Console.WriteLine($"Delivery Center: {CenterName}");
            Console.WriteLine("--------------------------------");

            for (int i = 0; i < _count; i++)
            {
                Console.WriteLine();
                Console.WriteLine($"Shipment #{i + 1}");

                _shipments[i].PrintShipment();

                Console.WriteLine("--------------------------------");
            }
        }
    }

    #endregion


    #region Program

    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Enter Delivery Center Name: ");
            string centerName = Console.ReadLine();

            DeliveryCenter center =
                new DeliveryCenter(centerName);


            #region Standard Shipment

            Console.WriteLine();
            Console.WriteLine("Enter Standard Shipment Data");

            Console.Write("Tracking Code: ");
            string standardTrackingCode =
                Console.ReadLine();

            Console.Write("Description: ");
            string standardDescription =
                Console.ReadLine();

            Console.Write("Weight: ");
            decimal standardWeight =
                decimal.Parse(Console.ReadLine());

            Console.Write("Delivery Fee: ");
            decimal standardDeliveryFee =
                decimal.Parse(Console.ReadLine());

            Console.Write("Street: ");
            string standardStreet =
                Console.ReadLine();

            Console.Write("City: ");
            string standardCity =
                Console.ReadLine();

            DeliveryAddress standardAddress =
                new DeliveryAddress(
                    standardStreet,
                    standardCity);

            StandardShipment standardShipment =
                new StandardShipment(
                    standardTrackingCode,
                    standardDescription,
                    standardWeight,
                    standardDeliveryFee,
                    standardAddress);

            #endregion


            #region Express Shipment

            Console.WriteLine();
            Console.WriteLine("Enter Express Shipment Data");

            Console.Write("Tracking Code: ");
            string expressTrackingCode =
                Console.ReadLine();

            Console.Write("Description: ");
            string expressDescription =
                Console.ReadLine();

            Console.Write("Weight: ");
            decimal expressWeight =
                decimal.Parse(Console.ReadLine());

            Console.Write("Delivery Fee: ");
            decimal expressDeliveryFee =
                decimal.Parse(Console.ReadLine());

            Console.Write("Street: ");
            string expressStreet =
                Console.ReadLine();

            Console.Write("City: ");
            string expressCity =
                Console.ReadLine();

            Console.Write("Extra Fee: ");
            decimal extraFee =
                decimal.Parse(Console.ReadLine());

            DeliveryAddress expressAddress =
                new DeliveryAddress(
                    expressStreet,
                    expressCity);

            ExpressShipment expressShipment =
                new ExpressShipment(
                    expressTrackingCode,
                    expressDescription,
                    expressWeight,
                    expressDeliveryFee,
                    expressAddress,
                    extraFee);

            #endregion


            #region International Shipment

            Console.WriteLine();
            Console.WriteLine("Enter International Shipment Data");

            Console.Write("Tracking Code: ");
            string internationalTrackingCode =
                Console.ReadLine();

            Console.Write("Description: ");
            string internationalDescription =
                Console.ReadLine();

            Console.Write("Weight: ");
            decimal internationalWeight =
                decimal.Parse(Console.ReadLine());

            Console.Write("Delivery Fee: ");
            decimal internationalDeliveryFee =
                decimal.Parse(Console.ReadLine());

            Console.Write("Street: ");
            string internationalStreet =
                Console.ReadLine();

            Console.Write("City: ");
            string internationalCity =
                Console.ReadLine();

            Console.Write("Destination Country: ");
            string destinationCountry =
                Console.ReadLine();

            Console.Write("Customs Fee: ");
            decimal customsFee =
                decimal.Parse(Console.ReadLine());

            DeliveryAddress internationalAddress =
                new DeliveryAddress(
                    internationalStreet,
                    internationalCity);

            InternationalShipment internationalShipment =
                new InternationalShipment(
                    internationalTrackingCode,
                    internationalDescription,
                    internationalWeight,
                    internationalDeliveryFee,
                    internationalAddress,
                    destinationCountry,
                    customsFee);

            #endregion


            #region Add Shipments

            center.AddShipment(standardShipment);
            center.AddShipment(expressShipment);
            center.AddShipment(internationalShipment);

            #endregion


            #region Print All Shipments

            center.PrintAllShipments();

            #endregion


            #region Search Using Tracking Code Indexer

            Console.WriteLine();
            Console.Write("Enter tracking code to search: ");

            string searchCode =
                Console.ReadLine();

            Shipment foundShipment =
                center[searchCode];

            if (foundShipment != null)
            {
                Console.WriteLine();
                Console.WriteLine("Shipment Found:");
                foundShipment.PrintShipment();
            }
            else
            {
                Console.WriteLine("Shipment not found.");
            }

            #endregion


            #region Remove Shipment

            Console.WriteLine();
            Console.Write("Enter tracking code to remove: ");

            string removeCode =
                Console.ReadLine();

            bool removed =
                center.RemoveShipment(removeCode);

            if (removed)
            {
                Console.WriteLine("Shipment removed successfully.");
            }
            else
            {
                Console.WriteLine("Shipment not found.");
            }

            #endregion


            #region Print Remaining Shipments

            center.PrintAllShipments();

            #endregion
        }
    }

    #endregion
}