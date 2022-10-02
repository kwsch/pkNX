namespace pkNX.Randomization;

/// <summary> Cyclical Shuffled Randomizer </summary>
/// <remarks>
/// The shuffled list is iterated over, and reshuffled when exhausted.
/// The list does not repeat values until the list is exhausted.
/// </remarks>
public class GenericRandomizer<T>
{
    public GenericRandomizer(T[] randomValues)
    {
        RandomValues = randomValues;
    }

    private readonly T[] RandomValues;
    private int ctr;
    public int Count => RandomValues.Length;

    public void Reset()
    {
        ctr = 0;
        Util.Shuffle(RandomValues);
    }

    public T Next()
    {
        if (ctr == 0)
            Util.Shuffle(RandomValues);

        T value = RandomValues[ctr++];
        ctr %= RandomValues.Length;
        return value;
    }

    public T[] GetMany(int count)
    {
        var arr = new T[count];
        for (int i = 0; i < arr.Length; i++)
            arr[i] = Next();
        return arr;
    }
}
