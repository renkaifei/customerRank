using Domain;
using System.Linq;

namespace CustomerRank.App;

public class Leaderboard
{
    private readonly Dictionary<decimal, Score> _dicScore;
    private readonly LinkedList<Score> _lstscore;

    public Leaderboard()
    {
        _dicScore = new Dictionary<decimal, Score>();
        _lstscore = new LinkedList<Score>();
    }

    public void AddScore(Score score)
    {
        if (_dicScore.TryAdd(score.Value, score))
        {
            LinkedListNode<Score>? header = _lstscore.First;
            if (header == null)
            {
                _lstscore.AddFirst(score);
            }
            else
            {
                LinkedListNode<Score> tempNode = header;
                while (tempNode.Value.Value < score.Value)
                {
                    if (tempNode.Next != null)
                    {
                        tempNode = tempNode.Next;
                    }
                    else
                    {
                        break;
                    }
                }
                if (tempNode.Value.Value < score.Value)
                {
                    _lstscore.AddAfter(tempNode, score);
                }
                else
                {
                    _lstscore.AddBefore(tempNode, score);
                }
            }
        }
    }

    public Score GetOneByScoreValue(decimal score)
    {
        if (_dicScore.TryGetValue(score, out var value))
        {
            return value;
        }
        else
        {
            value = new Score();
            value.Value = score;
            AddScore(value);
            return value;
        }
    }

    public void RemoveCustomer(Customer customer)
    {
        if (_dicScore.TryGetValue(customer.ScoreValue, out var score))
        {
            score.RemoveCustomer(customer);
        }
    }

    public void AddCustomer(Customer customer)
    {
        Score score = GetOneByScoreValue(customer.ScoreValue);
        score.AddCustomer(customer);
    }

    public List<Customer> GetCustomers(int start, int end)
    {
        List<Customer> customers = new List<Customer>(end - start + 1);
        int rank = 1;
        LinkedListNode<Score>? tempNode = _lstscore.First;
        int count = _lstscore.Count;
        while (tempNode != null)
        {
            LinkedListNode<Customer>? tempCustomerNode = tempNode.Value.GetFirstCustomerNode();
            while (tempCustomerNode != null)
            {
                if (rank >end)
                {
                    break;
                }
                else if (rank >= start && rank <= end)
                {
                    tempCustomerNode.Value.Rank = rank;
                    customers.Add(tempCustomerNode.Value);
                }
                tempCustomerNode = tempCustomerNode.Next;
                rank++;
            }
            if (rank > end) break;
            tempNode = tempNode.Next;
        }
        return customers;
    }

    public List<Customer> GetCustomers(Customer customer, int high, int low)
    {
        List<Customer> customers = new List<Customer>();
        LinkedListNode<Customer>? tempCustomerNode = null!;
        LinkedListNode<Score>? tempNode = _lstscore.First;
        int rank = 1;
        while (tempNode != null)
        {
            if (tempNode.Value.Value != customer.ScoreValue)
            {
                rank += tempNode.Value.GetCustomerCount();
                tempNode = tempNode.Next;
            }
            else
            {
                break;
            }
        }
        if (tempNode == null) return customers;
        tempCustomerNode = tempNode.Value.GetFirstCustomerNode();
        while (tempCustomerNode != null)
        {
            if (tempCustomerNode.Value.CustomerId != customer.CustomerId)
            {
                rank++;
                tempCustomerNode = tempCustomerNode.Next;
            }
            else
            {
                break;
            }
        }
        if (tempCustomerNode == null) return customers;
        tempCustomerNode.Value.Rank = rank;
        customers.Add(tempCustomerNode.Value);

        LinkedListNode<Score>? preScoreNode = tempNode.Previous;
        LinkedListNode<Customer>? preCustomerNode = tempCustomerNode.Previous;
        int previousRank = rank - 1;
        LinkedListNode<Score>? nextScoreNode = tempNode.Next;
        LinkedListNode<Customer>? nextCustomerNode = tempCustomerNode.Next;
        int nextRank = rank + 1;
        int preCount = 0;
        while (preCustomerNode != null)
        {
            if (preCount < low)
            {
                preCustomerNode.Value.Rank = previousRank;
                customers.Insert(0,preCustomerNode.Value);
                previousRank--;
                preCount++;
                if (preCustomerNode.Previous != null)
                {
                    preCustomerNode = preCustomerNode.Previous;
                }
                else
                {
                    if (preScoreNode == null)
                    {
                        break;
                    }
                    else
                    {
                        while (preScoreNode.Value.HasNoCustomers())
                        {
                            preScoreNode = preScoreNode.Previous;
                            if (preScoreNode == null)
                            {
                                break;
                            }
                        }
                        if (preScoreNode == null)
                        {
                            break;
                        }
                        else
                        {
                            preCustomerNode = preScoreNode.Value.GetLastCustomerNode();
                        }    
                    }
                }
            }
            else
            {
                break;
            }
        }

        int nextCount = 0;
        while (nextCustomerNode != null)
        {
            if (nextCount < high)
            {
                nextCustomerNode.Value.Rank = nextRank;
                customers.Add(nextCustomerNode.Value);
                nextRank--;
                nextCount++;
                if (nextCustomerNode.Next != null)
                {
                    nextCustomerNode = nextCustomerNode.Next;
                }
                else
                {
                    
                    if (nextScoreNode == null)
                    {
                        break;
                    }
                    else
                    {
                        while (nextScoreNode.Value.HasNoCustomers())
                        {
                            nextScoreNode = nextScoreNode.Next;
                            if (nextScoreNode == null)
                            {
                                break;
                            }                           
                        }
                        if (nextScoreNode == null)
                        {
                            break;
                        }
                        else
                        {
                            nextCustomerNode = nextScoreNode.Value.GetFirstCustomerNode();
                        }
                    }
                }
            }
            else
            {
                break;
            }
        }
        return customers;
    }
}
