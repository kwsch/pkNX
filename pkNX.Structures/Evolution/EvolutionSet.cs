using System;

namespace pkNX.Structures;

/// <summary>
/// Table of Evolution Branch Entries
/// </summary>
public abstract class EvolutionSet
{
    public EvolutionMethod[] PossibleEvolutions = [];
    public abstract byte[] Write();
}
