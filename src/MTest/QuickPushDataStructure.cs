/// <summary>
/// Thread-safe Quick Push Data Struct with 2 operation:
/// -Push, performed in O(1) time complexity
/// -Pop,  performed in O(n) time complexity
/// </summary>
/// <typeparam name="T">Any object</typeparam>
public class QuickPushDataStructure<T> : IDataStructure<T>
{
    private Node<T> head;
    private readonly IComparer<T> comparer;
    private readonly object lockObject = new();

    public QuickPushDataStructure(IComparer<T>? customComparer = null)
    {
        comparer = customComparer ?? Comparer<T>.Default;
    }

    public void Push(T value)
    {
        lock (lockObject)
        {
            var newNode = new Node<T>(value);
            newNode.Next = head;
            head = newNode;
        }
    }

    public T Pop()
    {
        lock (lockObject)
        {
            if (head == null)
                throw new InvalidOperationException("The data structure is empty.");

            var maxNode = head;
            var maxNodePrev = (Node<T>)null;

            var current = head;
            var prev = (Node<T>)null;
            while (current != null)
            {
                if (comparer.Compare(current.Value, maxNode.Value) > 0)
                {
                    maxNode = current;
                    maxNodePrev = prev;
                }

                prev = current;
                current = current.Next;
            }

            if (ReferenceEquals(maxNode, head))
                head = head.Next;
            else
                maxNodePrev.Next = maxNode.Next;

            return maxNode.Value;
        }
    }
}