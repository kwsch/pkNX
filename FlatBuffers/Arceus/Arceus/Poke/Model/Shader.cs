using FlatSharp.Attributes;
using System;
using System.ComponentModel;

namespace pkNX.Structures.FlatBuffers.Arceus;

// *.trsha
public class TrinityShader
{
    /*
    table SlotMapping {
        string_value: string;
        uint_value: uint32;
    }

    table SlotMap {
        slot_name: string;
        slot_values: [SlotMapping];
        bool_1: uint8;
        bool_2: uint8;
        bool_3: uint8;
        slot_index: uint8;
        offset: uint32;
    }

    table TRSHA {
        name: string;
        file_name: string;
        shader_param: [SlotMap];
        global_param: [SlotMap];
        param_buffer: [uint32]; //Contains Offset and Value pairs of uint32 each
        has_shader_param: bool = false; //maybe contains shader_param
        has_global_param: bool = false; //maybe contains global_param
    }*/
}

/*
{
  "name": "haStandard",
  "file_name": "ha_standard.bnsh",
  "shader_param": [
    {
      "slot_name": "EnableBaseColorMap",
      "slot_values": [
        {
          "string_value": "False",
          "uint_value": 0
        },
        {
          "string_value": "True",
          "uint_value": 1
        }
      ],
      "bool_1": 1,
      "bool_2": 0,
      "bool_3": 0,
      "slot_index": 0,
      "offset": 1
    },
    {
      "slot_name": "EnableNormalMap",
      "slot_values": [
        {
          "string_value": "False",
          "uint_value": 0
        },
        {
          "string_value": "True",
          "uint_value": 1
        }
      ],
      "bool_1": 0,
      "bool_2": 0,
      "bool_3": 0,
      "slot_index": 1,
      "offset": 2
    },
    {
      "slot_name": "EnableMetallicMap",
      "slot_values": [
        {
          "string_value": "False",
          "uint_value": 0
        },
        {
          "string_value": "True",
          "uint_value": 1
        }
      ],
      "bool_1": 0,
      "bool_2": 0,
      "bool_3": 0,
      "slot_index": 2,
      "offset": 4
    },
    {
      "slot_name": "EnableRoughnessMap",
      "slot_values": [
        {
          "string_value": "False",
          "uint_value": 0
        },
        {
          "string_value": "True",
          "uint_value": 1
        }
      ],
      "bool_1": 0,
      "bool_2": 0,
      "bool_3": 0,
      "slot_index": 3,
      "offset": 8
    },
    {
      "slot_name": "EnableEmissionColorMap",
      "slot_values": [
        {
          "string_value": "False",
          "uint_value": 0
        },
        {
          "string_value": "True",
          "uint_value": 1
        }
      ],
      "bool_1": 0,
      "bool_2": 0,
      "bool_3": 0,
      "slot_index": 4,
      "offset": 16
    },
    {
      "slot_name": "EnableAOMap",
      "slot_values": [
        {
          "string_value": "False",
          "uint_value": 0
        },
        {
          "string_value": "True",
          "uint_value": 1
        }
      ],
      "bool_1": 0,
      "bool_2": 0,
      "bool_3": 0,
      "slot_index": 5,
      "offset": 32
    },
    {
      "slot_name": "EnableAlphaTest",
      "slot_values": [
        {
          "string_value": "False",
          "uint_value": 0
        },
        {
          "string_value": "True",
          "uint_value": 1
        }
      ],
      "bool_1": 0,
      "bool_2": 0,
      "bool_3": 0,
      "slot_index": 6,
      "offset": 64
    },
    {
      "slot_name": "NumMaterialLayer",
      "slot_values": [
        {
          "string_value": "1",
          "uint_value": 1
        },
        {
          "string_value": "2",
          "uint_value": 2
        },
        {
          "string_value": "3",
          "uint_value": 3
        },
        {
          "string_value": "4",
          "uint_value": 4
        },
        {
          "string_value": "5",
          "uint_value": 5
        }
      ],
      "bool_1": 0,
      "bool_2": 0,
      "bool_3": 0,
      "slot_index": 7,
      "offset": 896
    },
    {
      "slot_name": "BillboardType",
      "slot_values": [
        {
          "string_value": "Disable",
          "uint_value": 0
        },
        {
          "string_value": "AxisXYZ",
          "uint_value": 1
        },
        {
          "string_value": "AxisY",
          "uint_value": 2
        }
      ],
      "bool_1": 0,
      "bool_2": 0,
      "bool_3": 0,
      "slot_index": 10,
      "offset": 3072
    },
    {
      "slot_name": "WindReceiverType",
      "slot_values": [
        {
          "string_value": "Disable",
          "uint_value": 0
        },
        {
          "string_value": "Simple",
          "uint_value": 1
        },
        {
          "string_value": "Standard",
          "uint_value": 2
        },
        {
          "string_value": "SimpleLeaf",
          "uint_value": 3
        }
      ],
      "bool_1": 0,
      "bool_2": 0,
      "bool_3": 0,
      "slot_index": 12,
      "offset": 12288
    },
    {
      "slot_name": "EnableWindMaskMap",
      "slot_values": [
        {
          "string_value": "False",
          "uint_value": 0
        },
        {
          "string_value": "True",
          "uint_value": 1
        }
      ],
      "bool_1": 0,
      "bool_2": 0,
      "bool_3": 0,
      "slot_index": 14,
      "offset": 16384
    },
    {
      "slot_name": "EnableBaseColorMap1",
      "slot_values": [
        {
          "string_value": "False",
          "uint_value": 0
        },
        {
          "string_value": "True",
          "uint_value": 1
        }
      ],
      "bool_1": 0,
      "bool_2": 0,
      "bool_3": 0,
      "slot_index": 15,
      "offset": 32768
    },
    {
      "slot_name": "EnableNormalMap1",
      "slot_values": [
        {
          "string_value": "False",
          "uint_value": 0
        },
        {
          "string_value": "True",
          "uint_value": 1
        }
      ],
      "bool_1": 0,
      "bool_2": 0,
      "bool_3": 0,
      "slot_index": 16,
      "offset": 65536
    },
    {
      "slot_name": "EnableNormalMap2",
      "slot_values": [
        {
          "string_value": "False",
          "uint_value": 0
        },
        {
          "string_value": "True",
          "uint_value": 1
        }
      ],
      "bool_1": 0,
      "bool_2": 0,
      "bool_3": 0,
      "slot_index": 17,
      "offset": 131072
    },
    {
      "slot_name": "EnableAOMap1",
      "slot_values": [
        {
          "string_value": "False",
          "uint_value": 0
        },
        {
          "string_value": "True",
          "uint_value": 1
        }
      ],
      "bool_1": 0,
      "bool_2": 0,
      "bool_3": 0,
      "slot_index": 18,
      "offset": 262144
    },
    {
      "slot_name": "EnableAOMap2",
      "slot_values": [
        {
          "string_value": "False",
          "uint_value": 0
        },
        {
          "string_value": "True",
          "uint_value": 1
        }
      ],
      "bool_1": 0,
      "bool_2": 0,
      "bool_3": 0,
      "slot_index": 19,
      "offset": 524288
    },
    {
      "slot_name": "LayerMaskSource",
      "slot_values": [
        {
          "string_value": "Const",
          "uint_value": 0
        },
        {
          "string_value": "Texture",
          "uint_value": 1
        },
        {
          "string_value": "VertexColor",
          "uint_value": 2
        }
      ],
      "bool_1": 1,
      "bool_2": 0,
      "bool_3": 0,
      "slot_index": 20,
      "offset": 3145728
    },
    {
      "slot_name": "LayerMaskSwizzle",
      "slot_values": [
        {
          "string_value": "RGBA",
          "uint_value": 0
        },
        {
          "string_value": "R111",
          "uint_value": 1
        },
        {
          "string_value": "A111",
          "uint_value": 2
        }
      ],
      "bool_1": 0,
      "bool_2": 0,
      "bool_3": 0,
      "slot_index": 22,
      "offset": 12582912
    },
    {
      "slot_name": "LayerBaseMaskSource",
      "slot_values": [
        {
          "string_value": "One",
          "uint_value": 0
        },
        {
          "string_value": "OneMinusLayerMaskSum",
          "uint_value": 1
        }
      ],
      "bool_1": 1,
      "bool_2": 0,
      "bool_3": 0,
      "slot_index": 24,
      "offset": 16777216
    },
    {
      "slot_name": "EnableVertexBaseColor",
      "slot_values": [
        {
          "string_value": "False",
          "uint_value": 0
        },
        {
          "string_value": "True",
          "uint_value": 1
        }
      ],
      "bool_1": 0,
      "bool_2": 0,
      "bool_3": 0,
      "slot_index": 25,
      "offset": 33554432
    },
    {
      "slot_name": "WeatherLayerMaskSource",
      "slot_values": [
        {
          "string_value": "Const",
          "uint_value": 0
        },
        {
          "string_value": "Texture",
          "uint_value": 1
        }
      ],
      "bool_1": 0,
      "bool_2": 0,
      "bool_3": 0,
      "slot_index": 26,
      "offset": 67108864
    },
    {
      "slot_name": "EnableDepthFade",
      "slot_values": [
        {
          "string_value": "False",
          "uint_value": 0
        },
        {
          "string_value": "True",
          "uint_value": 1
        }
      ],
      "bool_1": 0,
      "bool_2": 0,
      "bool_3": 0,
      "slot_index": 27,
      "offset": 134217728
    },
    {
      "slot_name": "EnablePackedMap",
      "slot_values": [
        {
          "string_value": "False",
          "uint_value": 0
        },
        {
          "string_value": "True",
          "uint_value": 1
        }
      ],
      "bool_1": 0,
      "bool_2": 0,
      "bool_3": 0,
      "slot_index": 28,
      "offset": 268435456
    },
    {
      "slot_name": "EnableUVScaleOffsetNormal",
      "slot_values": [
        {
          "string_value": "False",
          "uint_value": 0
        },
        {
          "string_value": "True",
          "uint_value": 1
        }
      ],
      "bool_1": 0,
      "bool_2": 0,
      "bool_3": 0,
      "slot_index": 29,
      "offset": 536870912
    }
  ],
  "global_param": [
    {
      "slot_name": "EnableDeferredRendering",
      "slot_values": [
        {
          "string_value": "False",
          "uint_value": 0
        },
        {
          "string_value": "True",
          "uint_value": 1
        }
      ],
      "bool_1": 0,
      "bool_2": 1,
      "bool_3": 1,
      "slot_index": 0,
      "offset": 1
    },
    {
      "slot_name": "InstancingType",
      "slot_values": [
        {
          "string_value": "Disable",
          "uint_value": 0
        },
        {
          "string_value": "World",
          "uint_value": 1
        },
        {
          "string_value": "Detail",
          "uint_value": 2
        }
      ],
      "bool_1": 0,
      "bool_2": 1,
      "bool_3": 1,
      "slot_index": 1,
      "offset": 6
    },
    {
      "slot_name": "EnableGrassCollisionMap",
      "slot_values": [
        {
          "string_value": "False",
          "uint_value": 0
        },
        {
          "string_value": "True",
          "uint_value": 1
        }
      ],
      "bool_1": 0,
      "bool_2": 1,
      "bool_3": 1,
      "slot_index": 3,
      "offset": 8
    },
    {
      "slot_name": "NumRequiredUV",
      "slot_values": [
        {
          "string_value": "1",
          "uint_value": 1
        },
        {
          "string_value": "2",
          "uint_value": 2
        }
      ],
      "bool_1": 0,
      "bool_2": 1,
      "bool_3": 1,
      "slot_index": 4,
      "offset": 16
    },
    {
      "slot_name": "EnableWeatherLayer",
      "slot_values": [
        {
          "string_value": "False",
          "uint_value": 0
        },
        {
          "string_value": "True",
          "uint_value": 1
        }
      ],
      "bool_1": 0,
      "bool_2": 1,
      "bool_3": 1,
      "slot_index": 5,
      "offset": 32
    },
    {
      "slot_name": "EnableDisplacementMap",
      "slot_values": [
        {
          "string_value": "False",
          "uint_value": 0
        },
        {
          "string_value": "True",
          "uint_value": 1
        }
      ],
      "bool_1": 0,
      "bool_2": 1,
      "bool_3": 1,
      "slot_index": 6,
      "offset": 64
    },
    {
      "slot_name": "EnableLerpBaseColorEmission",
      "slot_values": [
        {
          "string_value": "False",
          "uint_value": 0
        },
        {
          "string_value": "True",
          "uint_value": 1
        }
      ],
      "bool_1": 0,
      "bool_2": 1,
      "bool_3": 1,
      "slot_index": 7,
      "offset": 128
    }
  ],
  "param_buffer": [],
  "has_shader_param": true,
  "has_global_param": true
}*/
