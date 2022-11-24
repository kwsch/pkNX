/*
 * Copyright 2021 James Courtney
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

// FlatSharp attribute port of the reflection.fbs schema

using System;
using System.Collections.Generic;
using System.ComponentModel;
using FlatSharp;
using FlatSharp.Attributes;
// ReSharper disable ClassNeverInstantiated.Global
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable UnusedType.Global
#nullable disable
#pragma warning disable CS8632

namespace pkNX.Structures.FlatBuffers;

[FlatBufferTable(FileIdentifier = "BFBS"), TypeConverter(typeof(ExpandableObjectConverter))]
public class ReflectionSchema
{
    [FlatBufferItem(0, Required = true, SortedVector = true)]
    public virtual IList<FlatBufferObject> Objects { get; set; } = new List<FlatBufferObject>();

    [FlatBufferItem(1, Required = true, SortedVector = true)]
    public virtual IList<FlatBufferEnum> Enums { get; set; } = new List<FlatBufferEnum>();

    [FlatBufferItem(2)]
    public virtual string? FileIdentifier { get; set; }

    [FlatBufferItem(3)]
    public virtual string? FileExtension { get; set; }

    [FlatBufferItem(4)]
    public virtual FlatBufferObject? RootTable { get; set; }

    [FlatBufferItem(5)]
    public virtual IList<RpcService>? Services { get; set; }

    [FlatBufferItem(6)]
    public virtual AdvancedFeatures AdvancedFeatures { get; set; }

    [FlatBufferItem(7)]
    public virtual IIndexedVector<string, SchemaFile>? FbsFiles { get; set; }

    public (string Name, bool IsPrimitive) ToClr(FlatBufferType type, bool force = false) => type.ToClr(this, force);
}

[FlatBufferEnum(typeof(byte))]
public enum BaseType : byte
{
    None,
    UType,
    Bool,
    Byte,
    UByte,
    Short,
    UShort,
    Int,
    UInt,
    Long,
    ULong,
    Float,
    Double,
    String,
    Vector,
    Obj,     // Used for tables & structs.
    Union,
    Array,

    // Add any new type above this value.
    MaxBaseType,
}

#nullable disable
[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FlatBufferType
{
    [FlatBufferItem(0)]
    public virtual BaseType BaseType { get; set; }

    /// <summary>
    /// Only set if BaseType == Array or Vector.
    /// </summary>
    [FlatBufferItem(1, DefaultValue = BaseType.None)]
    public virtual BaseType ElementType { get; set; } = BaseType.None;

    [FlatBufferItem(2, DefaultValue = -1)]
    public virtual int Index { get; set; } = -1;

    [FlatBufferItem(3)]
    public virtual ushort FixedLength { get; set; }

    [FlatBufferItem(4, DefaultValue = 4u)]
    public uint BaseSize { get; set; } = 4;

    [FlatBufferItem(5, DefaultValue = 0u)]
    public uint ElementSize { get; set; } = 0;
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class KeyValue
{
    [FlatBufferItem(0, Required = true, Key = true)]
    public virtual string Key { get; set; } = string.Empty;

    [FlatBufferItem(1)]
    public virtual string? Value { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class EnumVal
{
    [FlatBufferItem(0, Required = true)]
    public virtual string Key { get; set; } = string.Empty;

    [FlatBufferItem(1, Key = true)]
    public virtual long Value { get; set; }

    [FlatBufferItem(2, Deprecated = true)]
    public bool Object { get; set; }

    [FlatBufferItem(3)]
    public virtual FlatBufferType? UnionType { get; set; }

    [FlatBufferItem(4)]
    public virtual IList<string>? Documentation { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class FlatBufferEnum
{
    [FlatBufferItem(0, Required = true, Key = true)]
    public virtual string Name { get; set; } = string.Empty;

    [FlatBufferItem(1, Required = true)]
    public virtual IIndexedVector<long, EnumVal> Values { get; set; } = new IndexedVector<long, EnumVal>();

    [FlatBufferItem(2)]
    public virtual bool IsUnion { get; set; }

    [FlatBufferItem(3, Required = true)]
    public virtual FlatBufferType UnderlyingType { get; set; } = new();

    [FlatBufferItem(4)]
    public virtual IIndexedVector<string, KeyValue>? Attributes { get; set; }

    [FlatBufferItem(5)]
    public virtual IList<string>? Documentation { get; set; }

    [FlatBufferItem(6)]
    public virtual string DeclarationFile { get; set; } = string.Empty;
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class Field
{
    [FlatBufferItem(0, Required = true, Key = true)]
    public virtual string Name { get; set; } = string.Empty;

    [FlatBufferItem(1, Required = true)]
    public virtual FlatBufferType Type { get; set; } = new();

    [FlatBufferItem(2)]
    public virtual ushort Id { get; set; }

    // Offset into the vtable for tables, or into the struct.
    [FlatBufferItem(3)]
    public virtual ushort Offset { get; set; }

    [FlatBufferItem(4)]
    public virtual long DefaultInteger { get; set; }

    [FlatBufferItem(5)]
    public virtual double DefaultDouble { get; set; }

    [FlatBufferItem(6, DefaultValue = false)]
    public bool Deprecated { get; set; } = false;

    [FlatBufferItem(7, DefaultValue = false)]
    public virtual bool Required { get; set; } = false;

    [FlatBufferItem(8, DefaultValue = false)]
    public virtual bool Key { get; set; } = false;

    [FlatBufferItem(9)]
    public virtual IIndexedVector<string, KeyValue>? Attributes { get; set; }

    [FlatBufferItem(10)]
    public virtual IList<string>? Documentation { get; set; }
}

[FlatBufferTable]
public class FlatBufferObject
{
    [FlatBufferItem(0, Required = true, Key = true)]
    public virtual string Name { get; set; } = string.Empty;

    [FlatBufferItem(1, Required = true)]
    public virtual IIndexedVector<string, Field> Fields { get; set; } = new IndexedVector<string, Field>();

    [FlatBufferItem(2)]
    public virtual bool IsStruct { get; set; }

    [FlatBufferItem(3)]
    public virtual int MinAlign { get; set; }

    // For structs, the size.
    [FlatBufferItem(4)]
    public virtual int ByteSize { get; set; }

    [FlatBufferItem(5)]
    public virtual IIndexedVector<string, KeyValue>? Attributes { get; set; }

    [FlatBufferItem(6)]
    public virtual IList<string>? Documentation { get; set; }

    [FlatBufferItem(7)]
    public virtual string? DeclarationFile { get; set; } = string.Empty;
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class RpcCall
{
    [FlatBufferItem(0, Required = true, Key = true)]
    public virtual string Name { get; set; } = string.Empty;

    // Must be a table.
    [FlatBufferItem(1, Required = true)]
    public virtual FlatBufferObject Request { get; set; } = new();

    // Must be a table.
    [FlatBufferItem(2, Required = true)]
    public virtual FlatBufferObject Response { get; set; } = new();

    [FlatBufferItem(3)]
    public virtual IIndexedVector<string, KeyValue>? Attributes { get; set; }

    [FlatBufferItem(4)]
    public virtual IList<string>? Documentation { get; set; }
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class RpcService
{
    [FlatBufferItem(0, Required = true, Key = true)]
    public virtual string Name { get; set; } = string.Empty;

    // Must be a table.
    [FlatBufferItem(1)]
    public virtual IList<RpcCall>? Calls { get; set; }

    [FlatBufferItem(2)]
    public virtual IIndexedVector<string, KeyValue>? Attributes { get; set; }

    [FlatBufferItem(3)]
    public virtual IList<string>? Documentation { get; set; }

    [FlatBufferItem(4, Required = true)]
    public virtual string DeclaringFile { get; set; } = string.Empty;
}

[FlatBufferEnum(typeof(ulong)), Flags]
public enum AdvancedFeatures : ulong
{
    None = 0,

    AdvancedArrayFeatures = 1,
    AdvancedUnionFeatures = 2,
    OptionalScalars = 4,
    DefaultVectorsAndStrings = 8,

    All = AdvancedArrayFeatures
          | AdvancedUnionFeatures
          | OptionalScalars
          | DefaultVectorsAndStrings,
}

[FlatBufferTable, TypeConverter(typeof(ExpandableObjectConverter))]
public class SchemaFile
{
    [FlatBufferItem(0, Required = true, Key = true)]
    public virtual string FileName { get; set; } = string.Empty;

    // Must be a table.
    [FlatBufferItem(1)]
    public virtual IList<string>? IncludedFileNames { get; set; }
}
