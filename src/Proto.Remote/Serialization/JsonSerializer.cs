// -----------------------------------------------------------------------
// <copyright file="JsonSerializer.cs" company="Asynkron AB">
//      Copyright (C) 2015-2024 Asynkron AB All rights reserved
// </copyright>
// -----------------------------------------------------------------------

using System;
using System.Collections.Concurrent;
using Google.Protobuf;

namespace Proto.Remote;

public class JsonSerializer : ISerializer
{
    private readonly ConcurrentDictionary<string, Type> _jsonTypes = new();
    private readonly Serialization _serialization;

    public JsonSerializer(Serialization serialization)
    {
        _serialization = serialization;
    }

    public ByteString Serialize(object obj) =>
        ByteString.CopyFromUtf8(
            System.Text.Json.JsonSerializer.Serialize(obj, _serialization.JsonSerializerOptions));

    public object Deserialize(ByteString bytes, string typeName)
    {
        var json = bytes.ToStringUtf8();

        var returnType = _jsonTypes.GetOrAdd(typeName,
            Type.GetType(typeName) ?? throw new Exception($"Type with the specified name {typeName} not found"));

        var message =
            System.Text.Json.JsonSerializer.Deserialize(json, returnType, _serialization.JsonSerializerOptions) ??
            throw new Exception($"Unable to deserialize message with type {typeName}");

        return message;
    }

    public string GetTypeName(object obj) =>
        obj.GetType().AssemblyQualifiedName ?? throw new ArgumentNullException(nameof(obj));

    public bool CanSerialize(object obj) =>
        obj switch
        {
            IMessage => false,
            _        => true
        };
}