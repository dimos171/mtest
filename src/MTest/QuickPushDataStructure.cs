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

    // Push operation - O(1) complexity, always adds node to the front
    public void Push(T value)
    {
        lock (lockObject)
        {
            var newNode = new Node<T>(value);
            newNode.Next = head;
            head = newNode;
        }
    }

    // Pop operation - O(n), returns the maximum value
    public T Pop()
    {
        lock (lockObject)
        {
            if (head == null)
                throw new InvalidOperationException("The data structure is empty.");

            var maxNode = head;
            var maxNodePrev = (Node<T>)null;

            // Traverse all the linked list looking for the max element.
            var current = head;
            var prev = (Node<T>)null;
            while (current != null)
            {
                // Check if the current node has a larger value than maxNode
                if (comparer.Compare(current.Value, maxNode.Value) > 0)
                {
                    maxNode = current;
                    maxNodePrev = prev;
                }

                // Move pointers to the next node
                prev = current;
                current = current.Next;
            }

            // Remove the maximum node. Consider 2 scenarios:
            // 1. If Max Node is the 1st element: Move head to the next node
            // 2. Else: Update Previos-to-Max-Node with whole tail of Max-Node
            // Remove the maximum node
            if (ReferenceEquals(maxNode, head))
                head = head.Next; // 
            else
                // 
                maxNodePrev.Next = maxNode.Next;

            return maxNode.Value;
        }
    }
}