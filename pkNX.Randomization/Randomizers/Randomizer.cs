using System;

namespace pkNX.Randomization;

public abstract class Randomizer
{
    public abstract void Execute();

    protected readonly Random Rand = Util.Random;
}
