namespace Domain;

public class Score
{
    private readonly HashSet<Customer> _customerSet = new HashSet<Customer>();
    private readonly LinkedList<Customer> _customers = new LinkedList<Customer>();

    public decimal Value { get; set; }

    public void RemoveCustomer(Customer customer)
    {
        _customers.Remove(customer);
    }

    public void AddCustomer(Customer customer) 
    {
        if (_customerSet.Add(customer))
        {
            LinkedListNode<Customer>? tempNode = _customers.First;
            if (tempNode == null)
            {
                _customers.AddFirst(customer);
            }
            else
            {
                while (tempNode != null)
                {
                    if (tempNode.Value.CustomerId < customer.CustomerId)
                    {
                        tempNode = tempNode.Next;
                        continue;
                    }
                    else
                    {
                        break;
                    }
                }
                if (tempNode == null)
                {
                    _customers.AddLast(customer);
                }
                else
                {
                    _customers.AddBefore(tempNode, customer);
                }
            }
        }
    }

    public LinkedListNode<Customer>? GetFirstCustomerNode()
    {
        return _customers.First;
    }

    public LinkedListNode<Customer>? GetLastCustomerNode()
    {
        return _customers.Last;
    }

    public int GetCustomerCount()
    {
        return _customerSet.Count;
    }

    public bool HasNoCustomers()
    {
        return _customerSet.Count == 0;
    }
}
