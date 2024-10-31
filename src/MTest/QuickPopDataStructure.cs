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

    public void Push(T value)
    {
        lock (lockObject)
        {
            var newNode = new Node<T>(value);

            if (head == null || comparer.Compare(value, head.Value) >= 0)
            {
                newNode.Next = head;
                head = newNode;
            }
            else
            {
                var current = head;
                while (current.Next != null && comparer.Compare(value, current.Next.Value) < 0)
                {
                    current = current.Next;
                }

                newNode.Next = current.Next;
                current.Next = newNode;
            }
        }
    }

    public T Pop()
    {
        lock (lockObject)
        {
            if (head == null)
                throw new InvalidOperationException("The data structure is empty.");

            T maxValue = head.Value;
            head = head.Next;

            return maxValue;
        }
    }
}