using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pkNX.Structures
{
    public enum AmxOpCodeType
    {
		Unknown = -1,
		NoParams,
		OneParam,
		TwoParams,
		ThreeParams,
		FourParams,
		FiveParams,
		Jump,
		Packed,
		CaseTable,
	}
}
