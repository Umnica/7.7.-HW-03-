namespace итоговое_задание7;

internal class Product
{
    public string Name;
    public decimal Price;

    public Product()
    {
        Console.WriteLine("Создание объекта Product");
        Name = "Undefined";
        Price = 0;
    }
    public Product(string name, decimal price)
    {
        Console.WriteLine("Создание объекта Product");
        Name = name;
        Price = price;
    }

    public string Print() => $"{Name}, цена = {Price}";
}

internal class Address
{
    public string City;
    public string Street;
    public int HouseNumber;


    public Address()
    {
        Console.WriteLine("Создание объекта Address");
        City = "Undefined";
        Street = "Undefined";
        HouseNumber = -1;
    }

    public Address(string city, string street, int houseNumber)
    {
        Console.WriteLine("Создание объекта Address");
        City = city;
        Street = street;
        HouseNumber = houseNumber;
    }

    public string Print()
    {
        return $"г. {City}, ул. {Street}, д. {HouseNumber}";
    }
}

internal abstract class Delivery
{
    // адрес куда доставить продукты
    public Address Address;

    protected Delivery(Address address)
    {
        Console.WriteLine("Создание объекта Delivery");
        Address = address;
    }
}

/*доставка на дом.
 Этот тип будет подразумевать наличие курьера или передачу курьерской компании, 
в нем будет располагаться своя, отдельная от прочих типов доставки логика.*/
internal class HomeDelivery : Delivery
{
    public string CourierName;

    public HomeDelivery(Address address, string courierName) : base(address)
    {
        Console.WriteLine("Создание объекта HomeDelivery");
        CourierName = courierName;
    }

    public void Deliver()
    {
        Console.WriteLine($"Доставка на дом по адресу {Address}. Кем {CourierName}");
    }
}

/*доставка в пункт выдачи.
 Здесь будет храниться какая-то ещё логика, 
необходимая для процесса доставки в пункт выдачи, 
например, хранение компании и точки выдачи, 
а также какой-то ещё информации.*/
internal class PickPointDelivery : Delivery
{
    public string CompanyName;
    public Address PointAddress;

    public PickPointDelivery(Address address, string companyName, Address pointAddress) : base(address)
    {
        Console.WriteLine("Создание объекта PickPointDelivery");
        CompanyName = companyName;
        PointAddress = pointAddress;
    }

    public void Deliver()
    {
        Console.WriteLine($"Доставка в пункт выдачи по адресу {PointAddress}. Кем {CompanyName}");
    }
}

/*доставка в розничный магазин.
 Эта доставка может выполняться внутренними средствами компании 
и совсем не требует работы с «внешними&raquo; элементами.*/
internal class ShopDelivery : Delivery
{
    public string ShopName;

    public ShopDelivery(Address address, string shopName) : base(address)
    {
        Console.WriteLine("Создание объекта ShopDelivery");
        ShopName = shopName;
    }

    public void Deliver()
    {
        Console.WriteLine($"Доставка в розничный магазин {ShopName}");
    }
}

internal class Order<TDelivery> where TDelivery : Delivery
{
    public TDelivery Delivery; // доставка
    public int Number; // номер заказа
    public string Description; // описание

    public List<Product> Products; // список продутов в заказе

    // общая стоимость
    public decimal TotalPrice
    {
        get
        {
            decimal total = 0;
            foreach (var product in Products) total += product.Price;
            return total;
        }
    }

    public Order(TDelivery delivery, string description, List<Product> products)
    {
        Console.WriteLine("Создание объекта Order");
        Delivery = delivery;
        Description = description;
        Products = products;
    }

    public void Print()
    {
        Console.WriteLine($"Номер заказа: {Number}\n" +
                          $"Описание: {Description}");
        Delivery.Address.Print();
        Console.Write("Продукты: ");

        foreach (var Product in Products) Console.Write(Product.Name + " ");
        Console.WriteLine('.');

        Console.WriteLine("Общая стоимость: " + TotalPrice);
    }

    public void AddProduct(Product product)
    {
        Products.Add(product);
    }
    // перегруженные операторы для задания 

    public static Order<TDelivery> operator +(Order<TDelivery> a, Product b)
    {
        a.AddProduct(b);
        return new Order<TDelivery>(a.Delivery, a.Description, a.Products);
    }
}

internal class OrderCollection<TDelivery> where TDelivery : Delivery
{
    private readonly List<Order<TDelivery>> collection;

    public OrderCollection()
    {
        Console.WriteLine("Создание объекта OrderCollection");
        collection = new List<Order<TDelivery>>();
    }

    public void Add(Order<TDelivery> value) { collection.Add(value); }
    public void Remove(Order<TDelivery> value) { collection.Remove(value); }
    public int Count => collection.Count;

    public Order<TDelivery> this[int index]
    {
        get => collection[index];
        private set => collection[index] = value;
    }
}


internal class Customer<TDelivery> where TDelivery : Delivery// клиент интернет магазина
{
    public string Name;
    public string Phone;

    public Customer()
    {
        Console.WriteLine("Создание объекта Customer");
        Name = "Undefined";
        Phone = "Undefined";
    }

    public Customer(string name, string phone)
    {
        Console.WriteLine("Создание объекта Customer");
        Name = name;
        Phone = phone;
    }

    public OrderCollection<TDelivery> OrderCollection = new OrderCollection<TDelivery>();

    public static decimal Sum(OrderCollection<TDelivery> orderCollection)
    {
        decimal total = 0;
        for (int i = 0; i < orderCollection.Count; i++)
            total += orderCollection[i].TotalPrice;
        return total;
    }

    public decimal GetTotalPrice() => Sum(OrderCollection);
}

internal class CustomerCollection<TDelivery> where TDelivery : Delivery
{
    private readonly List<Customer<TDelivery>> collection;

    public CustomerCollection()
    {
        Console.WriteLine("Создание объекта CustomerCollection");
        collection = new List<Customer<TDelivery>>();
    }

    public void Add(Customer<TDelivery> value) { collection.Add(value); }
    public void Remove(Customer<TDelivery> value) { collection.Remove(value); }
    public int Count => collection.Count;

    public Customer<TDelivery> this[int index]
    {
        get => collection[index];
        private set => collection[index] = value;
    }
}

internal class Program
{
    private static void Main(string[] args)
    {
        // создаем список продуктов 
        Product p1 = new Product("продукт 1", 100);

        List<Product> products = new List<Product>();

        for (int i = 2; i < 20; i++)
        {
            products.Add(new Product($"продукт {i}", 100*i));
        }

        foreach (var product in products)
        {
            Console.WriteLine(product.Print());
        }

        // создаем список адресов
        List<Address> addresses = new List<Address>();
        for (int i = 2; i < 20; i++)
            addresses.Add(new Address("Саратов", "Московская", i));

        // создадим доставку (которую выбрал клиент)
        PickPointDelivery pickPointDelivery1 = 
            new PickPointDelivery(addresses[0], "OOO.xxx", addresses[10]);

        // список продуктов для заказа 1
        List<Product> productsForOrder1 = new List<Product>();
        productsForOrder1.Add(p1);
        productsForOrder1.Add(products[5]);
        productsForOrder1.Add(products[5]);

        // список продуктов для заказа 2
        List<Product> productsForOrder2 = new List<Product>();
        productsForOrder2.Add(products[6]);
        productsForOrder2.Add(products[7]);
        productsForOrder2.Add(products[8]);

        Order<PickPointDelivery> order1 =
            new Order<PickPointDelivery>(pickPointDelivery1, "маленькие пакеты", productsForOrder1);
        Order<PickPointDelivery> order2 =
            new Order<PickPointDelivery>(pickPointDelivery1, "коробка чего-то", productsForOrder2);

        HomeDelivery homeDelivery = new HomeDelivery(new Address(), "курьер1");
        Order<HomeDelivery> order3 = new Order<HomeDelivery>(homeDelivery, "пакет", productsForOrder1);

        order3 += products[2];
        order3 += products[2];
        order3 += products[2];
        order3 += products[2];
        order3 += products[2];

        Customer<PickPointDelivery> klient1 = new Customer<PickPointDelivery>("Кирилл", "88005553535");
        Customer<HomeDelivery> klient2 = new Customer<HomeDelivery>("Вася", "86848484841");

        klient1.OrderCollection.Add(order1);
        klient1.OrderCollection.Add(order2);
        klient2.OrderCollection.Add(order3);

        CustomerCollection<PickPointDelivery> customerCollection1 = new CustomerCollection<PickPointDelivery>();
        CustomerCollection<HomeDelivery> customerCollection2 = new CustomerCollection<HomeDelivery>();

        customerCollection1.Add(klient1);
        customerCollection2.Add(klient2);

        // вывод всего в customerCollection1

        for (int i = 0; i < customerCollection1.Count; i++)
        {
            var customer = customerCollection1[i];
            Console.WriteLine($"имя: {customer.Name}\n" +
                              $"телефон: {customer.Phone}\n" +
                              $"общая стоимость заказов: {customer.GetTotalPrice()}\n" +
                              $"заказы:");

            var orders = customer.OrderCollection;
            for (int j = 0; j < orders.Count; j++)
            {
                var order = orders[j];
                Console.WriteLine($"Заказ {j}");
                order.Print();
            }
        }
    }
}