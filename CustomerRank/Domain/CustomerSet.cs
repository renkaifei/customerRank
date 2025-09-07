namespace Domain;

public class CustomerSet
{
    public Dictionary<long, Customer> _dicCustomer = new Dictionary<long, Customer>();

    public bool AddCustomer(Customer customer)
    {
        return _dicCustomer.TryAdd(customer.CustomerId, customer);
    }

    public Customer? GetCustomer(long customerId)
    {
        if (_dicCustomer.TryGetValue(customerId, out var customer))
        {
            return customer;
        }
        else
        {
            return null;
        }
    }
}
