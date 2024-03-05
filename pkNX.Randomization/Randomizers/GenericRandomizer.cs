namespace pkNX.Randomization;

/// <summary> Cyclical Shuffled Randomizer </summary>
/// <remarks>
/// The shuffled list is iterated over, and reshuffled when exhausted.
/// The list does not repeat values until the list is exhausted.
/// </remarks>
public class GenericRandomizer<T>(T[] randomValues)
{
    private int ctr;
    public int Count => randomValues.Length;

    public void Reset()
    {
        ctr = 0;
        Util.Shuffle(randomValues);
    }

    public T Next()
    {
        if (ctr == 0)
            Util.Shuffle(randomValues);

        T value = randomValues[ctr++];
        ctr %= randomValues.Length;
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
