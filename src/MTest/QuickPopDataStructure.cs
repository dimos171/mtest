/// <summary>
/// Thread-safe Quick Pop Data Struct with 2 operation:
/// -Push, performed in O(n) time complexity
/// -Pop,  performed in O(1) time complexity
/// </summary>
/// <typeparam name="T">Any object</typeparam>
public class QuickPopDataStructure<T> : IDataStructure<T>
{
    private Node<T> head;
    private readonly IComparer<T> comparer;
    private readonly object lockObject = new();

    public QuickPopDataStructure(IComparer<T> customComparer = null)
    {
        comparer = customComparer ?? Comparer<T>.Default;
    }

    // Push operation - O(n)
    public void Push(T value)
    {
        lock (lockObject)
        {
            var newNode = new Node<T>(value);

            // Linked List is ordered (Desc) by Node value. Head always has node with a Max value. Consider 2 scenarios:
            // 1. If head's value is less the new value: we change the head with a new node keeping all nodes after
            // 2. Else: we are finding proper place to insert new node
            if (head == null || comparer.Compare(value, head.Value) >= 0)
            {
                newNode.Next = head;
                head = newNode;
            }
            else
            {
                // Find the correct position to insert for sorted order
                var current = head;
                while (current.Next != null && comparer.Compare(value, current.Next.Value) < 0)
                {
                    current = current.Next;
                }

                // Insert newNode after current:
                newNode.Next = current.Next;
                current.Next = newNode;
            }
        }
    }

    // Pop operation - O(1), returns the maximum value
    public T Pop()
    {
        lock (lockObject)
        {
            if (head == null)
                throw new InvalidOperationException("The data structure is empty.");

            // Maximum value is at the head
            T maxValue = head.Value;
            // Move head to the next node
            head = head.Next;

            return maxValue;
        }
    }
}