using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using NLog;
using Plus.Communication.Attributes;
using Plus.Communication.Packets.Incoming;
using Plus.HabboHotel.GameClients;

namespace Plus.Communication.Packets;

public sealed class PacketManager : IPacketManager
{
    private static readonly ILogger Log = LogManager.GetLogger("Plus.Communication.Packets");

    /// <summary>
    ///     The task factory which is used for running Asynchronous tasks, in this case we use it to execute packets.
    /// </summary>
    private readonly TaskFactory _eventDispatcher = new(TaskCreationOptions.PreferFairness, TaskContinuationOptions.None);

    /// <summary>
    ///     Testing the Task code
    /// </summary>
    private readonly bool _ignoreTasks = true;

    private readonly Dictionary<int, Type> _headerToPacketMapping = new();
    private readonly Dictionary<int, IPacketEvent> _incomingPackets = new();
    private readonly HashSet<Type> _handshakePackets = new();

    /// <summary>
    ///     The maximum time a task can run for before it is considered dead
    ///     (can be used for debugging any locking issues with certain areas of code)
    /// </summary>
    private readonly int _maximumRunTimeInSec = Debugger.IsAttached ? 300 : 30; // 5 minutes in debug. 30 seconds in release.
    private readonly Dictionary<int, string> _packetNames = new();

    /// <summary>
    ///     Currently running tasks to keep track of what the current load is
    /// </summary>
    private readonly ConcurrentDictionary<int, Task> _runningTasks = new();

    /// <summary>
    ///     Should the handler throw errors or log and continue.
    /// </summary>
    private readonly bool _throwUserErrors = false;

    public PacketManager(IEnumerable<IPacketEvent> incomingPackets)
    {
        foreach (var packet in incomingPackets)
        {
            var field = typeof(ClientPacketHeader).GetField(packet.GetType().Name, BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
            if (field == null)
            {
                Log.Warn("No incoming header defined for {packet}", packet.GetType().Name);
                continue;
            }
            var header = (int) field.GetValue(null);
            _incomingPackets.Add(header, packet);
            _packetNames.Add(header, packet.GetType().Name);
            if (packet.GetType().GetCustomAttribute<NoAuthenticationRequiredAttribute>() != null)
                _handshakePackets.Add(packet.GetType());
        }
    }

    public void TryExecutePacket(GameClient session, ClientPacket packet)
    {
        if (!_incomingPackets.TryGetValue(packet.Id, out var pak))
        {
            if (Debugger.IsAttached)
                Log.Debug("Unhandled Packet: " + packet);
            return;
        }

        if (Debugger.IsAttached)
        {
            if (_packetNames.ContainsKey(packet.Id))
                Log.Debug("Handled Packet: [" + packet.Id + "] " + _packetNames[packet.Id]);
            else
                Log.Debug("Handled Packet: [" + packet.Id + "] UnnamedPacketEvent");
        }

        if (!_handshakePackets.Contains(packet.GetType()) && session.GetHabbo() == null)
        {
            Log.Debug($"Session {session.ConnectionId} tried execute packet {packet.Id} but didn't handshake yet.");
            return;
        }

        if (!_ignoreTasks)
            ExecutePacketAsync(session, packet, pak);
        else
            pak.Parse(session, packet);
    }

    private void ExecutePacketAsync(GameClient session, ClientPacket packet, IPacketEvent pak)
    {
        var cancelSource = new CancellationTokenSource();
        var token = cancelSource.Token;
        var t = _eventDispatcher.StartNew(() =>
        {
            pak.Parse(session, packet);
            token.ThrowIfCancellationRequested();
        }, token);
        _runningTasks.TryAdd(t.Id, t);
        try
        {
            if (!t.Wait(_maximumRunTimeInSec * 1000, token)) cancelSource.Cancel();
        }
        catch (AggregateException ex)
        {
            foreach (var e in ex.Flatten().InnerExceptions)
            {
                if (_throwUserErrors)
                    throw e;
                else
                {
                    //log.Fatal("Unhandled Error: " + e.Message + " - " + e.StackTrace);
                    session.Disconnect();
                }
            }
        }
        catch (OperationCanceledException)
        {
            session.Disconnect();
        }
        finally
        {
            _runningTasks.TryRemove(t.Id, out var _);
            cancelSource.Dispose();

            //log.Debug("Event took " + (DateTime.Now - Start).Milliseconds + "ms to complete.");
        }
    }

    public void WaitForAllToComplete()
    {
        foreach (var t in _runningTasks.Values.ToList()) t.Wait();
    }

    public void UnregisterAll()
    {
        _headerToPacketMapping.Clear();
    }
}