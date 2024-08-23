using System;

namespace pkNX.Structures;

[Flags]
public enum MoveCritStage : int
{
  // Valid for Gen4 onwards
  OneOverTwentyFour = 0, 
  OneOverEight = 1,
  Half = 2,
  Guaranteed = 6
}

