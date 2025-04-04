// -----------------------------------------------------------------------
// <copyright file="EmptyKeyValueStore<T>.cs" company="Asynkron AB">
//      Copyright (C) 2015-2024 Asynkron AB All rights reserved
// </copyright>
// -----------------------------------------------------------------------

using System.Threading;
using System.Threading.Tasks;

namespace Proto.Utils;

/// <summary>
///     Noop key value store.
/// </summary>
/// <typeparam name="T"></typeparam>
public class EmptyKeyValueStore<T> : IKeyValueStore<T>
{
    public Task<T> GetAsync(string id, CancellationToken ct) => Task.FromResult(default(T))!;

    public Task SetAsync(string id, T state, CancellationToken ct) => Task.CompletedTask;

    public Task ClearAsync(string id, CancellationToken ct) => Task.CompletedTask;
}